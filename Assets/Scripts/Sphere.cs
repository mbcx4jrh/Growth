using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace growth {
    public struct Sphere {

        public List<Vertex> vertices;

        public static Sphere CreateSphere(float radius) {
            return new Sphere();
        }
    }

    public struct Vertex {
        public Vector3 position;
        List<int> connected;
    }
}
