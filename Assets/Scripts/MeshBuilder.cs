using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace growth {
    public class MeshBuilder {

        Mesh mesh;

        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<int> triangles = new List<int>();

        public MeshBuilder(Mesh mesh) {
            this.mesh = mesh;
        }

        public Mesh BuildMesh(List<Cell> cells) {
            vertices.Clear();
            triangles.Clear();
            normals.Clear();
            
            foreach (var cell in cells) {
                vertices.Add(cell.position);
                normals.Add(cell.normal);
                int n = cell.neighbours.Count;
                for (int i=0; i<n; i++) {
                    int j = (i + 1) % n;
                    triangles.Add(cell.index);
                    triangles.Add(cell.neighbours[i].index);
                    triangles.Add(cell.neighbours[j].index);
                    
                }    
                
            }

            mesh.Clear();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.normals = normals.ToArray();
            return mesh;
        }
    }
}
