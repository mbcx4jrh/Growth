using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace growth {



    public class CellFormController : MonoBehaviour, CellRenderer {

        public bool division = true;

        [Range(0, 1000)]
        public int initialCells = 10;

        public float surfaceRadius = 5f;

        [Range(0f, 3f)]
        public float linkLength = 1f;

        [Range(0f, 1f)]
        public float surfaceStrength = 1f;

        [Range(0.5f, 2f)]
        public float cellScale = 1f;

        public CellObject cellPrefab;
        public GameObject cellParent;

        CellForm cellForm;
        FoodSource[] foodSources;

        private void Start() {
            FindFoodSource();
            GenerateInitialCells();
        }

        private void Update() {
            foreach (var f in foodSources) f.Feed(cellForm);
            UpdateCellFormParams();
            cellForm.Evolve(Time.deltaTime);
            UpdateCells();
        }

        private void UpdateCellFormParams() {
            cellForm.linkLength = linkLength;
            cellForm.enableDivision = division;
        }

        private void FindFoodSource() {
            foodSources = GetComponents<FoodSource>();
            if (foodSources.Length <1) Debug.LogWarning("Missing FoodSource component");
        }

        private void GenerateInitialCells() {
            cellForm = new CellForm();
            cellForm.cellRenderer = this;
            cellForm.linkLength = linkLength;

            cellForm.CreateCellsOnSphere(initialCells, surfaceRadius);
            cellForm.LinkCells();
           // cellForm.CreateSingleCell();
            if (cellPrefab != null) {
                foreach (var cell in cellForm.cells) {
                    CreateCellObject(cell);
                }
            }
        }

        private CellObject CreateCellObject(Cell cell) {
            CellObject cellObject = Instantiate(cellPrefab, cell.position, Quaternion.identity);
            float scale = cell.radius * cellScale;
            cellObject.transform.localScale = new Vector3(scale, scale, scale);
            if (cellParent == null) {
                cellObject.transform.parent = this.transform;
            }
            else {
                cellObject.transform.parent = cellParent.transform;
            }
            cell.cellObject = cellObject;
            cellObject.cell = cell;
            return cellObject;
        }

        public void AddCell(Cell newCell) {
            newCell.cellObject = CreateCellObject(newCell);
        }

        public void UpdateCells() {
            foreach (var cell in cellForm.cells) {
                cell.cellObject.transform.position = cell.position;
            }
        }
    }
}
