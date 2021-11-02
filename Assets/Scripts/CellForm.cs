
using System.Collections.Generic;
using UnityEngine;

namespace growth {
    public class CellForm {

        public List<Cell> cells;

        public int maxCells = 2000;

        public float linkLength = 1f;

        public float springFactor = 0.7f;
        public float planarFactor = 0.0f;
        public float bulgeFactor = 0.0f;

        public bool enableDivision = true;
        public float divisionShiftDistance = 0.1f;

        public CellRenderer cellRenderer;

        float surfaceRadius2;

        public CellForm() {
            cells = new List<Cell>();
        }

        public void LinkCells() {
            float d = linkLength;
            float d_sqr = d * d;
            foreach (var cell in cells) {
                foreach (var other in cells) {
                    if (other != cell) {
                        if ((cell.position-other.position).sqrMagnitude < d_sqr) {
                            cell.neighbours.Add(other);
                        }
                    }
                }
            }
        }

        public void CreateSingleCell() {
            cells.Add(new Cell(Vector3.zero, linkLength / 2f));
        }

        //ref: https://www.cmu.edu/biolphys/deserno/pdf/sphere_equi.pdf
        public void CreateCellsOnSphere(int numberOfCells, float surfaceRadius) {
            this.surfaceRadius2 = surfaceRadius *surfaceRadius;
            int count = 0;
            float a = 4f * Mathf.PI * surfaceRadius * surfaceRadius / numberOfCells;
            float d = Mathf.Sqrt(a);
            int Ma = Mathf.RoundToInt(2f * Mathf.PI / d);
            float da = 2f * Mathf.PI / Ma;
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
                    cells.Add(new Cell(position, linkLength)); //Debug.Log("d " + d);
                    count++;
                }
            }
            Debug.Log("Initialised " + count + " cells");
        }

        public void Evolve(float deltaTime) {
            if (enableDivision && cells.Count < maxCells) {
                for (int i = 0; i < cells.Count; i++) {
                    CheckForDivision(cells[i]);
                }
            }
            foreach (var cell in cells) {
                Move(cell, deltaTime);
            }
        }

        private void Move(Cell cell, float deltaTime) {
            var sumS = Vector3.zero;
            var sumP = Vector3.zero;
            float sumB = 0f;
            int n = cell.neighbours.Count;
            if (n == 0) return;
            float link2 = linkLength * linkLength;
            var surfaceNormal = (cell.position.sqrMagnitude > surfaceRadius2) ? cell.position.normalized : -cell.position.normalized;
            foreach (var nCell in cell.neighbours) {
                var D = nCell.position - cell.position;
                var Dn = D.normalized;
                sumS += nCell.position - Dn * linkLength;
                sumP += nCell.position;
                var length2 = D.sqrMagnitude;
                if (length2 < link2) {
                    float dotN = Vector3.Dot(D, surfaceNormal);
                    sumB += Mathf.Sqrt(link2 - D.sqrMagnitude + dotN * dotN) + dotN;
                }

                //sumS += nCell.position + linkLength * (cell.position - nCell.position).normalized;
                //sumP += nCell.position;
                //float link2 = linkLength * linkLength;
                //var dotN = Vector3.Dot((nCell.position - cell.position), cell.position.normalized);
                //sumB += Mathf.Sqrt(linkLength * linkLength - nCell.position.sqrMagnitude + dotN * dotN) + dotN;
            }
            var springTarget = sumS / n;
            var planarTarget = sumP / n;
            var bulgeTarget = cell.position + (sumB / n) * surfaceNormal;

            cell.position += (springFactor * (springTarget - cell.position)
                            + planarFactor * (planarTarget - cell.position)
                            + bulgeFactor * (bulgeTarget - cell.position))*deltaTime;

            
        }



        private void CheckForDivision(Cell cell) {
            if (cell.food >= 1f) {
                var daughter = Divide(cell, Random.onUnitSphere);
                cells.Add(daughter);
                cellRenderer.AddCell(daughter);
                if (cells.Count == maxCells) Debug.Log("Max number of cells reached (" + maxCells + ")");
            }
        }

        private Cell Divide(Cell cell, Vector3 divisionPlaneNormal) {
            var divisionPlane = new Plane(divisionPlaneNormal, cell.position);
            var daughter = new Cell(cell.position, cell.radius);
            var oldLinks = cell.neighbours;
            cell.neighbours = new List<Cell>();
            foreach (var n in oldLinks) {
                if (divisionPlane.GetSide(n.position)) {
                    cell.neighbours.Add(n);
                }
                else {
                    daughter.neighbours.Add(n);
                    n.neighbours.Remove(cell);
                    n.neighbours.Add(daughter);
                }
            }
            daughter.neighbours.Add(cell);
            cell.neighbours.Add(daughter);
            daughter.position -= divisionPlaneNormal * divisionShiftDistance;
            cell.position += divisionPlaneNormal * divisionShiftDistance;
            cell.food = 0f;
            Debug.Log("Divided: new cell has " + daughter.neighbours.Count + " neighbours and old cell has " + cell.neighbours.Count);
            return daughter;
        }
    }
}
