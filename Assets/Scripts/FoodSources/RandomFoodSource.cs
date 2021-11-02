using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace growth {
    public class RandomFoodSource : FoodSource {

        [Range(0f, 1f)]
        public float amount = 0.2f;

        [Range(0f,100f)]
        public float cellsPerSecond = 2f;

        float nextFeedTime;

        private void Start() {
            nextFeedTime = Time.time + 1f / cellsPerSecond;
        }

        public override void Feed(CellularForm cellularForm) {
            if (Time.time>nextFeedTime) {
                int i = Random.Range(0, cellularForm.cells.Count);
                cellularForm.cells[i].food += amount;
                nextFeedTime = Time.time + 1f / cellsPerSecond;
            }
        }
    }
}
