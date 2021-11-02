using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace growth {

    public interface CellRenderer {
        public abstract void AddCell(Cell newCell);
        public abstract void UpdateCells();

    }
    public class Cell {
        public Vector3 position;
        public Vector3 idealPosition;
        public float radius;
        public List<Cell> neighbours = new List<Cell>();
        public float food = 0f;

        public CellObject cellObject;


        public Cell(Vector3 position, float radius) {
            this.position = position;
            this.radius = radius;
            this.idealPosition = position;
        }


    }
}
