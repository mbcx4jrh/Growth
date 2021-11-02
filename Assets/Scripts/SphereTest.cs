using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace growth {
    public class SphereTest : MonoBehaviour {

        [Range(0f,10f)]
        public float sphereRadius = 2f;

        [Range(0f, 5f)]
        public float pointSize = 0.3f;

        public CellObject cellObject;

        private void Start() {
            var sphere = Sphere.CreateSphere(sphereRadius);
        }
    }
}
