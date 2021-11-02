using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace growth {
    public class ConstantFoodSource : FoodSource {

        [Range(0f,1f)]
        public float amount = 0.1f;

        public override void Feed(CellForm cellForm) {
            foreach (var cell in cellForm.cells) {
                cell.food += amount*Time.deltaTime;
            }
        }
    }
}
