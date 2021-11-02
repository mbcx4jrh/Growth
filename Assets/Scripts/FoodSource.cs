using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace growth {
    public abstract class FoodSource : MonoBehaviour {

        public abstract void Feed( CellForm cellForm);

    }
}
