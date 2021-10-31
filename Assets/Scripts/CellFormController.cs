using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace growth {


    public class CellFormController : MonoBehaviour {


        [Range(0, 1000)]
        public int initialCells = 10;

        public float surfaceRadius = 5f;

        [Range(0f, 1f)]
        public float linkStrength = 1f;

        [Range(0f, 1f)]
        public float surfaceStrength = 1f;

        [Range(0.5f, 2f)]
        public float cellScale = 1f;

        public CellObject cellPrefab;
        public GameObject cellParent;

        CellForm cellForm;

        private void Start() {
            GenerateInitialCells();

        }

        private void GenerateInitialCells() {
            cellForm = new CellForm(initialCells, surfaceRadius);
            if (cellPrefab != null) {
                foreach (var cell in cellForm.cells) {
                    CellObject cellObject = Instantiate(cellPrefab, cell.position, Quaternion.identity);
                    float scale = cell.radius * cellScale;
                    cellObject.transform.localScale = new Vector3(scale,scale,scale);
                    if (cellParent == null) {
                        cellObject.transform.parent = this.transform;
                    }
                    else {
                        cellObject.transform.parent = cellParent.transform;
                    }
                    cell.cellObject = cellObject;
                }
            }
        }
    }
}
