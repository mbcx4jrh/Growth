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
            if (i == -1) {
                Debug.LogWarning("Bad link replacement");
                return;
            }
            neighbours[i] = neo;
        }

        public Cell Split() {
            //Debug.Log("Splitting start");
            var daughter = new Cell(position, normal);
            int n = neighbours.Count;
            if (n == 0) return daughter;

            //find nearest neighbour
            float nearest_d = float.MaxValue;
            int nearest = -1;
            for (int i = 0; i < n; i++) {
                float d = (neighbours[i].position - position).sqrMagnitude;
                if (d < nearest_d) {
                    nearest_d = d;
                    nearest = i;
                }
            }
            int opposite = (nearest + n/2) % n;
            //Debug.Log("Starting spinning nearest=" + nearest + ", opposite=" + opposite + ", n=" + n);
           
            //parent links
            var newLinks = new List<Cell>();
            for (int i=nearest; i!=(opposite+1)%n; i=(i+1)%n) {
                newLinks.Add(neighbours[i]);
            }
            newLinks.Add(daughter);
           // Debug.Log("Start daughter links");

            //daughter links
            daughter.neighbours.Add(neighbours[opposite]);
            for (int i=(opposite+1)%n; i != nearest; i=(i+1)%n) {
                daughter.neighbours.Add(neighbours[i]);
                //Debug.Log("replacing link " + i);
                neighbours[i].ReplaceLink(this, daughter);
            }
            neighbours[nearest].neighbours.Add(daughter);
            neighbours[opposite].neighbours.Add(daughter);
            daughter.neighbours.Add(neighbours[nearest]);
            daughter.neighbours.Add(this);

            neighbours = newLinks;
            food = 0f;
           // Debug.Log("Splitting end - Parent has "+neighbours.Count+" links, daughter has "+daughter.neighbours.Count);
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
