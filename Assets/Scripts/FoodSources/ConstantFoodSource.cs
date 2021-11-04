using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace growth {
    public class ConstantFoodSource : FoodSource {

        [Range(0f, 1f)]
        public float amount = 0.2f;

        public int iterations = 25;

        int countdown;

        private void Start() {
            countdown = iterations;
        }

        public override void Feed(CellularForm cellularForm) {
            if (countdown-- <= 1) {
                foreach (var cell in cellularForm.cells) {
                    cell.food += amount;

                }
                countdown = iterations;
            }
        }
    }
}
