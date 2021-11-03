using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace growth {
    [ExecuteInEditMode]
    public class OneRingNeighbourhoodTest : MonoBehaviour {

        List<Cell> cells;
        Cell center;

        public bool showCenter = true;
        public bool showFirst = true;
        public bool showSecond = true;
        public bool showThird = true;
        public bool showFourth = true;
        public bool showDaughter = true;

        public bool split = false;
  

        private void Start() {
            GenerateOneRingN();
        }

        private void OnValidate() {
            GenerateOneRingN();
        }

        private void GenerateOneRingN() {
            cells = new List<Cell>();

            center = new Cell(Vector3.zero);

            var first = new Cell(new Vector3(-1f, -1f, 0f));
            var second = new Cell(new Vector3(1f, -1f, 0f));
            var third = new Cell(new Vector3(1f, 0.8f, 0f));
            var fourth = new Cell(new Vector3(-1f, 1f, 0f));

            center.neighbours.Add(first);
            center.neighbours.Add(second);
            center.neighbours.Add(third);
            center.neighbours.Add(fourth);

            first.neighbours.Add(second);
            first.neighbours.Add(center);
            first.neighbours.Add(fourth);

            second.neighbours.Add(third);
            second.neighbours.Add(center);
            second.neighbours.Add(first);

           third.neighbours.Add(fourth);
            third.neighbours.Add(center);
            third.neighbours.Add(second);

            fourth.neighbours.Add(first);
            fourth.neighbours.Add(center);
            fourth.neighbours.Add(third);

            cells.Add(center);
            cells.Add(first);
            cells.Add(second);
            cells.Add(third);
            cells.Add(fourth);
            if (split) {
                var d = center.Split();
                center.position += new Vector3(-0.2f, 0f, 0f);
                d.position += new Vector3(0.2f, 0f, 0f);
                cells.Add(d);
            }
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            if (showCenter) DrawGizmos(center);
            if (showFirst) DrawGizmos(cells[1]);
            if (showSecond) DrawGizmos(cells[2]);
            if (showThird) DrawGizmos(cells[3]);
            if (showFourth) DrawGizmos(cells[4]);
            if (showDaughter) {
                Gizmos.color = Color.green;
                DrawGizmos(cells[5]); ;
            }
        }

        private void DrawGizmos(Cell cell) {
            foreach (var n in cell.neighbours) {
                Gizmos.DrawLine(cell.position, n.position);
            }
        }
    }
}
