using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace growth {
    public class Cell {
        public Vector3 position;
        public Vector3 idealPosition;
        public float radius;

        public Cell(Vector3 position, float radius) {
            this.position = position;
            this.radius = radius;
            this.idealPosition = position;
        }
    }
}
