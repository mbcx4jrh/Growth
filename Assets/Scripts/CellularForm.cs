using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace growth {
    public class CellularForm : MonoBehaviour {

        public CellGameObject cellPrefab;

        [Range(0.1f,2f)]
        public float linkLength = 1f;

        [Range(0f, 1f)]
        public float springFactor;

        [Range(0f, 1f)]
        public float planarFactor = 1f;

        [Range(0f, 1f)]
        public float bulgeFactor = 1f;

        Cell[] cells;

        private void Start() {
            GenerateInitialCells();
            GenerateGameObjectsForCells();
        }

        private void Update() {
            CalculateForces();
            UpdateCells();
        }

        private void UpdateCells() {
            foreach (var cell in cells) {
                cell.gameObject.transform.position = cell.position;
            }
        }

        private void CalculateForces() {
            var link2 = linkLength * linkLength;
            foreach (var cell in cells) {
                var d_spring_sum = Vector3.zero;
                var d_planar_sum = Vector3.zero;
                var d_bulge_sum = 0f;
                int n = cell.neighbours.Count;
                foreach (var neighbour in cell.neighbours) {
                    var vectorBetween = neighbour.position - cell.position;
                    var distance2 = vectorBetween.sqrMagnitude;
                    var distance = vectorBetween.magnitude;
                    var t = neighbour.position + cell.normal*linkLength;
                    float theta = Vector3.Angle(t - cell.position, vectorBetween)*Mathf.Deg2Rad;
                    d_spring_sum += -cell.position +neighbour.position - vectorBetween.normalized*linkLength;
                    d_planar_sum += neighbour.position;
                    d_bulge_sum += Mathf.Sqrt(Mathf.Max(link2 + distance2 - 2 * linkLength * distance * Mathf.Cos(theta)));
                }
                var d_spring = linkLength*d_spring_sum / n;
                var d_planar = d_planar_sum / n;
                var d_bulge = cell.normal * (d_bulge_sum / n);

                //apply forces
                cell.position += Time.deltaTime * (springFactor * d_spring + planarFactor * d_planar + bulgeFactor * d_bulge);
            }
        }

        private void GenerateGameObjectsForCells() {
            if (!cellPrefab) {
                Debug.LogWarning("No prefab for cells set");
                return;
            }
            foreach (var cell in cells) {
                GenerateGameObjectsForCell(cell);
            }
        }

        private void GenerateGameObjectsForCell(Cell cell) {
            cell.gameObject = Instantiate(cellPrefab, cell.position, Quaternion.identity);
            cell.gameObject.cell = cell;
        }

        private void GenerateInitialCells() {
            cells = new Cell[12];
            for (int i = 0; i < Icosphere.vertices.Length; i++) {
                cells[i] = Cell.CellOnSphere(Icosphere.vertices[i]);
            }
            for (int i = 0; i < 12; i++) {
                for (int j = 0; j < 5; j++) {
                    cells[i].neighbours.Add(cells[Icosphere.edges[i, j]]);
                }
            }
        }
    }
}
