using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace growth {
    public class CellularForm : MonoBehaviour {

        public CellGameObject cellPrefab;

        Cell[] cells;

        private void Start() {
            GenerateInitialCells();
            GenerateGameObjectsForCells();
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
                cells[i] = new Cell(Icosphere.vertices[i]);
            }
            for (int i = 0; i < 12; i++) {
                for (int j = 0; j < 5; j++) {
                    cells[i].neighbours.Add(cells[Icosphere.edges[i, j]]);
                }
            }
        }
    }
}
