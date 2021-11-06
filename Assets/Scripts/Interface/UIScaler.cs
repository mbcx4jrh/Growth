using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace growth {
    public class UIScaler : MonoBehaviour {
        public Slider sliderControl;

        float scale;

        public void Scale() {
            scale = sliderControl.value;
            //transform.localScale = Vector3.one * scale;
        } 
    }
}
