using DataStructures.ViliWonka.KDTree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace growth {

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class CellularForm : MonoBehaviour {

        public bool showMesh = true;
        //bool savePOVRayFile = false;
        [Range(1, 8)]
        public int threads = 1;

        public int maxCells = 1000;

        public int maxIterations = 100;

        [Range(0.1f, 2f)]
        public float linkLength = 1f;

        [Range(0f, 5f)]
        public float springFactor = 1f;

        [Range(0f, 1f)]
        public float planarFactor = 1f;

        [Range(0f, 1f)]
        public float bulgeFactor = 1f;

        [Range(0f, 10f)]
        public float repulsionRange = 2f;

        [Range(0f, 1f)]
        public float repulsionFactor = 1f;


        public Text textBox;

        public Mesh seedMesh;

        [Header("K-D Tree params")]
        public int maxPointsPerLeaf = 32;

        int iterations = 0;

        MeshBuilder meshBuilder;
        Mesh formMesh;
        MeshFilter meshFilter;

        [HideInInspector]
        public FoodSource[] foodSources;

        [HideInInspector]
        public List<Cell> cells;

        KDTree cellTree;
        int lastCellsCount = 0;

        bool writeFile = true;

        private void Start() {
            GenerateInitialCells();
            GenerateMesh();
            FindFoodSources();
            cellTree = new KDTree(maxPointsPerLeaf);
        }

        private void GenerateMesh() {
            formMesh = new Mesh();
            formMesh.name = "Cellular Mesh";
            formMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            meshBuilder = new MeshBuilder(formMesh);
            meshFilter = GetComponent<MeshFilter>();
            if (!meshFilter) Debug.LogWarning("Missing MeshFilter component");
            UpdateMesh();
        }

        private void FindFoodSources() {
            foodSources = GetComponents<FoodSource>();
        }

        private void UpdateAndBuildKDTree() {
            if (cellTree == null) {
                cellTree = new KDTree(ExtractPoints(0), maxPointsPerLeaf);
                lastCellsCount = cells.Count;
                return;
            }
            else {
                if (lastCellsCount != cells.Count) {
                    cellTree.Build(ExtractPoints(lastCellsCount), maxPointsPerLeaf);
                }
                for (int i=0; i< lastCellsCount; i++) {
                    cellTree.Points[i] = cells[i].position;
                }
                cellTree.Rebuild();

            }
        }

        private Vector3[] ExtractPoints(int startIndex) {
            Vector3[] points = new Vector3[cells.Count - startIndex];
            for (int i=0; i<cells.Count; i++) {
                points[i] = cells[i].position;
            }
            return points;
        }


        private void Update() {
            if (iterations++ < maxIterations) {
                FeedCells();
                CheckForSplits();
                CalculateForces();
                if (showMesh) {
                    UpdateMesh();
                }
                UpdateUI();
            }
            else if (writeFile) {
                FileWriter.WritePOVRaySpheres(cells, linkLength*1.5f);
                writeFile = false;
            }
        }

        private void UpdateMesh() {
            formMesh = meshBuilder.BuildMesh(cells);
            meshFilter.sharedMesh = formMesh;
        }

        private void UpdateUI() {
            textBox.text = "Iteration: " + iterations + " Cells: "+cells.Count;
        }

        private void CheckForSplits() {
            int n = cells.Count;
            if (n >= maxCells) return;
            for (int i = 0; i < n; i++) {
                if (cells[i].food >= 1f) {
                    var newCell = cells[i].Split();
                    cells.Add(newCell);
                    newCell.index = cells.Count - 1;
                    if (cells.Count == maxCells) {
                        Debug.Log("Maximum cells reached (" + maxCells + ")");
                    }
                }
            }
        }


        private void FeedCells() {
            foreach (var foodSource in foodSources) {
                foodSource.Feed(this);
            }
        }



        private void CalculateForces() {
            UpdateAndBuildKDTree();
            var link2 = linkLength * linkLength;
            var r2 = repulsionRange * repulsionRange;

            if (threads == 1) {
                KDQuery query = new KDQuery();
                var queryResults = new List<int>();
                foreach (var cell in cells) {
                    CalculateForcesOnCell(query, queryResults, link2, r2, cell);

                }
            }
            else {
                int n = cells.Count / threads;
                Worker[] workers = new Worker[threads];
                for (int i=0; i< threads; i++) {
                    workers[i] = new Worker(this, i * n, (i+1) * n, link2, r2);
                }
                if (workers[threads - 1].end < cells.Count) workers[threads - 1].end = cells.Count;
                Parallel.For(0, threads, i => {
                    workers[i].CalculateForcesOnCells();
                });
            }
            UpdateCellPositions();
        }

        private void UpdateCellPositions() {
            foreach (var cell in cells) {
                cell.position = cell.updatedPosition;
            }
        }

        public void CalculateForcesOnCell(KDQuery query, List<int> queryResults, float link2, float r2, Cell cell) {
            cell.NormalFromNeighbours();
            var d_spring_sum = Vector3.zero;
            var d_planar_sum = Vector3.zero;
            var d_bulge_sum = 0f;
            var d_collision_sum = Vector3.zero;
            int n = cell.neighbours.Count;
            foreach (var neighbour in cell.neighbours) {
                var vectorBetween = neighbour.position - cell.position;
                var distance2 = vectorBetween.sqrMagnitude;
                var distance = vectorBetween.magnitude;
                var t = neighbour.position + cell.normal * linkLength;
                float theta = Vector3.Angle(t - cell.position, vectorBetween) * Mathf.Deg2Rad;
                d_spring_sum += -cell.position + neighbour.position - vectorBetween.normalized * linkLength;
                d_planar_sum += vectorBetween;
                d_bulge_sum += Mathf.Sqrt(Mathf.Max(link2 + distance2 - 2 * linkLength * distance * Mathf.Cos(theta)));
                d_collision_sum += vectorBetween.normalized * ((r2 - distance2) / r2);
            }
            var d_spring = linkLength * d_spring_sum / n;
            var d_planar = d_planar_sum / n;
            var d_bulge = cell.normal * (d_bulge_sum / n);

            int nearby = cell.neighbours.Count;

            queryResults.Clear();
            query.Radius(cellTree, cell.position, repulsionRange, queryResults);

            foreach (int i in queryResults) {
                var other = cells[i];
                var between = cell.position - other.position;
                d_collision_sum += between * ((r2 - between.sqrMagnitude) / r2);
            }
            var d_collision = d_collision_sum / (queryResults.Count + nearby);
            //apply forces
            cell.updatedPosition = cell.position + (springFactor * d_spring
                                             + planarFactor * d_planar
                                             + bulgeFactor * d_bulge
                                             + repulsionFactor * d_collision);
        }

        private void GenerateInitialCells() {
            cells = MeshImporter.ImportMesh(seedMesh);
        }


        private void GenerateIcosphere() {
            cells = new List<Cell>();
            for (int i = 0; i < Icosphere.vertices.Length; i++) {
                cells.Add(Cell.CellOnSphere(Icosphere.vertices[i]));
            }
            for (int i = 0; i < 12; i++) {
                for (int j = 0; j < 5; j++) {
                    cells[i].neighbours.Add(cells[Icosphere.edges[i, j]]);
                }
            }
        }
    }
}
