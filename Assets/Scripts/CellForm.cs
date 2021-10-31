using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace growth {
    public class CellForm {

        public List<Cell> cells;

        public CellForm(int numberOfCells, float surfaceRadius) {
            cells = new List<Cell>();
            CreateCellsOnSphere(numberOfCells, surfaceRadius);
        }


        //ref: https://www.cmu.edu/biolphys/deserno/pdf/sphere_equi.pdf
        private void CreateCellsOnSphere(int numberOfCells, float surfaceRadius) {
            int count = 0;
            float a = 4f * Mathf.PI * surfaceRadius * surfaceRadius / numberOfCells;
            float d = Mathf.Sqrt(a);
            int Ma = Mathf.RoundToInt(2f*Mathf.PI / d);
            float da = 2f*Mathf.PI / Ma;
            float db = a / da;
            Debug.Log("Ma " + Ma + " da " + da);
            for (int m = 0; m < Ma; m++) {
                ;
                float A = Mathf.PI * (m + 0.5f) / Ma;
                int Mb = Mathf.RoundToInt(4f * Mathf.PI * Mathf.Sin(A) / db);

                Debug.Log("Mb " + Mb);
                for (int n = 0; n < Mb; n++) {
                    float B = 2 * Mathf.PI * n / Mb;
                    float x = Mathf.Sin(A) * Mathf.Cos(B);
                    float y = Mathf.Sin(A) * Mathf.Sin(B);
                    float z = Mathf.Cos(A);
                    var position = new Vector3(x, y, z) * surfaceRadius;
                    cells.Add(new Cell(position, d));
                    count++;
                }
            }
            Debug.Log("Initialised " + count + " cells");
        }
    }
}
