using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace growth {
    public class Cell {

        public Vector3 position;
        public Vector3 normal;
        public float food = 0f;

        public List<Cell> neighbours = new List<Cell>(5);

        public CellGameObject gameObject;

        public Cell(Vector3 position, Vector3 normal) {
            this.position = position;
            this.normal = normal;
        }

        public Cell(Vector3 position) {
            this.position = position;
        }

        public static Cell CellOnSphere(Vector3 position) {
            return new Cell(position, position.normalized);
        }

        public void ReplaceLink(Cell old, Cell neo) {
            int i = neighbours.IndexOf(old);
            neighbours[i] = neo;
        }

        public Cell Split() {
            var daughter = new Cell(position, normal);
            if (neighbours.Count == 0) return daughter;

            //find nearest neighbour
            float nearest_d = float.MaxValue;
            int nearest = -1;
            for (int i = 0; i < neighbours.Count; i++) {
                float d = (neighbours[i].position - position).sqrMagnitude;
                if (d < nearest_d) {
                    nearest_d = d;
                    nearest = i;
                }
            }

            var newLinks = new List<Cell>();
            var splitPlane = new Plane(position, neighbours[nearest].position, position + normal);
            daughter.neighbours.Add(this);
            daughter.neighbours.Add(neighbours[nearest]);
            neighbours[nearest].neighbours.Add(daughter);
            newLinks.Add(daughter);
            newLinks.Add(neighbours[nearest]);

            for (int i=0; i<neighbours.Count; i++) {

                if (i == nearest) continue;
                var n = neighbours[i];
             
                if (splitPlane.GetSide(n.position)) {
                    daughter.neighbours.Add(n);
                    n.ReplaceLink(this, daughter);
                }
                else {
                    newLinks.Add(n);
                }
            }
            neighbours = newLinks;
            food = 0f;
            return daughter;
        }

        public void NormalFromNeighbours() {
            var sum = Vector3.zero;
            for (int i=0; i<neighbours.Count; i++) {
                var a = neighbours[i].position - position;
                var b = neighbours[(i + 1) % neighbours.Count].position - position;
                sum += Vector3.Cross(a, b);
            }
            normal = sum.normalized;

            //Debug.Log("Normal " + normal);
        }

    }
}
