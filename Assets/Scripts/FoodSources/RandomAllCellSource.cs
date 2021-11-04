using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace growth {
    public class RandomAllCellSource : FoodSource {

        [Range(0f,0.1f)]
        public float minAmount = 0f;
        [Range(0f,0.1f)]
        public float maxAmount = 0.1f;

        public override void Feed(CellularForm cellularForm) {
            foreach (var cell in cellularForm.cells) {
                cell.food += Random.Range(minAmount, maxAmount);
            }
        }
    }
}
