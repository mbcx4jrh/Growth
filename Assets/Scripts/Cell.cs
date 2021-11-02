using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace growth {
    public class Cell {

        public Vector3 position;
        public Vector3 normal;

        public List<Cell> neighbours = new List<Cell>(5);

        public CellGameObject gameObject;
        
        public Cell(Vector3 position, Vector3 normal) {
            this.position = position;
            this.normal = normal;
        }

        public static Cell CellOnSphere(Vector3 position) {
            return new Cell(position, position.normalized);
        }
    }
}
