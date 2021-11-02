using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace growth {
    public class CellObject : MonoBehaviour {
        public int n;
        public Cell cell;

        private bool debug = true;


        private void Update() {
            if (cell != null) {
                n = cell.neighbours.Count;
            }

         }

        private void OnDrawGizmos() {
            if (debug && cell != null) {
                foreach (var n in cell.neighbours) {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(cell.position, n.position);
                }
            }

        }
    }
}
