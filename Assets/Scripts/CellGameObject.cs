using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace growth {
    public class CellGameObject : MonoBehaviour {

        public Cell cell;

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;

            foreach (var n in cell.neighbours) {
                Gizmos.DrawLine(cell.position, n.position);
            }
        }

    }
}
