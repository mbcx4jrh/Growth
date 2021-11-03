using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace growth {
    public class MeshImporter {

        public static float maxDeltaForWelding = 0.01f;

        public static bool weldVertices = false;

        public static List<Cell> ImportMesh(Mesh mesh) {
            Vector3[] vertices;
            int[] triangles;

            if (weldVertices) {
                var welded = Weld(mesh);
                vertices = welded.Item1;
                triangles = welded.Item2;
            }
            else {
                vertices = mesh.vertices;
                triangles = mesh.triangles;
            }

            var cells = new List<Cell>(vertices.Length);
            List<Triangle>[] vertexTriangles = new List<Triangle>[vertices.Length];

            //create cells 
            for (int i = 0; i < vertices.Length; i++) {
                cells.Add(new Cell(vertices[i]));
                vertexTriangles[i] = new List<Triangle>();
            }

            //map triangles to vertices
            for (int i = 0; i < triangles.Length; i += 3) {
                var t = new Triangle(triangles, i);
                vertexTriangles[t.a].Add(t);
                vertexTriangles[t.b].Add(t);
                vertexTriangles[t.c].Add(t);
            }

            //sort triangles at each vertex to ccw
            for (int v = 0; v < vertexTriangles.Length; v++) {
                var verTris = vertexTriangles[v];
                for (int i = 1; i < verTris.Count; i++) {
                    var prev = verTris[i - 1].VertexBefore(v);
                    for (int j = i; j < verTris.Count; j++) {
                        if (verTris[j].VertexAfter(v) == prev) {
                            var temp = verTris[i];
                            verTris[i] = verTris[j];
                            verTris[j] = temp;
                            break;
                        }
                    }
                }

                Debug.Log("adding " + verTris.Count + " neighbours to cell " + v);
                //create neighbours in CCW
                foreach (var t in verTris) {
                    cells[v].neighbours.Add(cells[t.VertexAfter(v)]);
                    Debug.Log("n: " + t.VertexAfter(v));
                }
            }

            foreach (var cell in cells) {
                cell.NormalFromNeighbours();
            }

            Debug.Log("Mesh importer imported " + mesh.vertices.Length + " vertices, of which " + cells.Count + " were unique");
            return cells;
        }

        private static Tuple<Vector3[], int[]> Weld(Mesh mesh) {
            float d2 = maxDeltaForWelding * maxDeltaForWelding;
            var newVerts = new List<Vector3>();
            int[] map = new int[mesh.vertices.Length];
            for (int v = 0; v < mesh.vertices.Length; v++) {
                bool found = false;
                for (int nv = 0; nv < newVerts.Count; nv++) {
                    if ((mesh.vertices[v]-newVerts[nv]).sqrMagnitude < d2) {
                        //Debug.Log("found " + v + "," + nv);
                        map[v] = nv;
                        found = true;
                        break;
                    } 
                }
                if (!found) {
                    //Debug.Log("Added new vertex");
                    newVerts.Add(mesh.vertices[v]);
                }
            }

            int[] newTri = new int[mesh.triangles.Length];
            for (int i=0; i<mesh.triangles.Length; i++) {
                newTri[i] = map[mesh.triangles[i]];
            }
            //Debug.Log("Welded " + newVerts.Count + " vertices & " + newTri.Length/3 + " triangles");
            return new Tuple<Vector3[], int[]>(newVerts.ToArray(), newTri);

        }

        struct Triangle {
            public int a, b, c;
            public Triangle(int[] t, int i) {
                a = t[i];
                b = t[i + 1];
                c = t[i + 2];
            }

            public int VertexBefore(int p) {
                if (p == a) return c;
                else if (p == b) return a;
                else if (p == c) return b;
                else {
                    Debug.LogWarning("Vertex not found in VertexBefore");
                    return a;
                }
            }

            public int VertexAfter(int p) {
                if (p == a) return b;
                else if (p == b) return c;
                else if (p == c) return a;
                else {
                    Debug.LogWarning("Vertex not found in VertexAfer");
                    return a;
                }
            }
        }

    }
}
