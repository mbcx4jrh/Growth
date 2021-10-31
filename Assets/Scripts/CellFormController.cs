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

        public GameObject cellPrefab;
        public GameObject cellParent;

        CellForm cellForm;

        private void Start() {
            GenerateInitialCells();

        }

        private void GenerateInitialCells() {
            cellForm = new CellForm(initialCells, surfaceRadius);
            if (cellPrefab != null) {
                foreach (var cell in cellForm.cells) {
                    GameObject cellObject = Instantiate(cellPrefab, cell.position, Quaternion.identity);
                    cellObject.transform.localScale = new Vector3(cell.radius, cell.radius, cell.radius);
                    if (cellParent == null) {
                        cellObject.transform.parent = this.transform;
                    }
                    else {
                        cellObject.transform.parent = cellParent.transform;
                    }
                }
            }
        }
    }
}
