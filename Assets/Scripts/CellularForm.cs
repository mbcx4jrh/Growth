using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace growth {

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class CellularForm : MonoBehaviour {

        public int maxCells = 1000;

        public int maxIterations = 1000;

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

        int iterations = 0;
        Mesh formMesh;

        [HideInInspector]
        public FoodSource[] foodSources;

        [HideInInspector]
        public List<Cell> cells;

        private void Start() {
            GenerateInitialCells();
            GenerateMesh();
            FindFoodSources();
        }

        private void GenerateMesh() {
            Mesh mesh = new Mesh();
            mesh.name = "Cellular Mesh";
            UpdateMesh();
        }

        private void FindFoodSources() {
            foodSources = GetComponents<FoodSource>();
        }

        private void Update() {
            if (iterations++ < maxIterations) {
                FeedCells();
                CheckForSplits();
                CalculateForces();
                UpdateMesh();
                UpdateUI();
            }
        }

        private void UpdateMesh() {
            throw new NotImplementedException();
        }

        private void UpdateUI() {
            textBox.text = "Iteration: " + iterations;
        }

        private void CheckForSplits() {
            int n = cells.Count;
            if (n >= maxCells) return;
            for (int i = 0; i < n; i++) {
                if (cells[i].food >= 1f) {
                    var newCell = cells[i].Split();
                    cells.Add(newCell);
                    if (cells.Count == maxCells) Debug.Log("Maximum cells reached (" + maxCells + ")");
                }
            }
        }


        private void FeedCells() {
            foreach (var foodSource in foodSources) {
                foodSource.Feed(this);
            }
        }



        private void CalculateForces() {
            var link2 = linkLength * linkLength;
            var r2 = repulsionRange * repulsionRange;
            foreach (var cell in cells) {
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
                foreach (var other in cells) {
                    var between = cell.position - other.position;
                    if (between.sqrMagnitude < r2) {
                        d_collision_sum += between * ((r2 - between.sqrMagnitude) / r2);
                        nearby += 1;
                    }
                }
                var d_collision = d_collision_sum / nearby;
                //apply forces
                cell.position +=  (springFactor * d_spring
                                                 + planarFactor * d_planar
                                                 + bulgeFactor * d_bulge
                                                 + repulsionFactor * d_collision);

            }
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
