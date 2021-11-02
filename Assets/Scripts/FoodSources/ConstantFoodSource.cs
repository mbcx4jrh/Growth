using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace growth {
    public class ConstantFoodSource : FoodSource {

        [Range(0f, 1f)]
        public float amount = 0.2f;

        public override void Feed(CellularForm cellularForm) {
            foreach (var cell in cellularForm.cells) {
                cell.food += amount * Time.deltaTime;
            }
        }
    }
}
