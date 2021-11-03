using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace growth {
    public class CellGameObject : MonoBehaviour {

        public Cell cell;
        public bool drawGizmos = true;

        private void OnDrawGizmos() {
            if (!drawGizmos) return;
            Gizmos.color = Color.red;

            foreach (var n in cell.neighbours) {
                Gizmos.DrawLine(cell.position, n.position);
            }

            Gizmos.color = Color.green;
            Gizmos.DrawLine(cell.position, cell.position + cell.normal);
            //Debug.Log("cp: " + cell.position + " n: " + cell.normal);
            //Gizmos.DrawSphere(Vector3.zero, 2f);
        }

    }
}
