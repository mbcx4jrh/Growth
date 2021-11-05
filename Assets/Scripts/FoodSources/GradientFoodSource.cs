using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace growth
{
    public class GradientFoodSource : FoodSource {

        [Range(0f, 1f)]
        public float amount;

        public override void Feed(CellularForm cellularForm) {
            foreach (var cell in cellularForm.cells) {
                cell.food += Random.Range(0f, amount) / (Mathf.Abs(cell.position.y) + 1);
            }
        }
    }
}
