using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace growth {
    public class Cell {

        public Vector3 position;
        public List<Cell> neighbours = new List<Cell>(5);

        public CellGameObject gameObject;
        
        public Cell(Vector3 position) {
            this.position = position;
        }
    }
}
