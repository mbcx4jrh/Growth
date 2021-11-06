using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace growth {
    public class SingleFormController : MonoBehaviour {
        public Slider linkLengthSlider;
        public Slider springFactorSlider;
        public Slider planarFactorSlider;
        public Slider bulgeFactorSlider;
        public Slider repulsionRangeSlider;
        public Slider repulsionFactorSlider;

        Parameters parameters;
        CellularForm cellularForm;


        private void Start() {
            cellularForm = FindObjectOfType<CellularForm>();
            if (cellularForm) {
                parameters = Parameters.FromCellularForm(cellularForm);
            }
            else {
                Debug.LogWarning("Missing cellular form to apply paramters to. Whats the point?!");
                parameters = new Parameters();
            }
            SetSliders();
        }

        private void SetSliders() {
            linkLengthSlider.value = parameters.linkLength;
            Debug.Log("sett slider to sf " + parameters.springFactor);
            springFactorSlider.value = parameters.springFactor;
            planarFactorSlider.value = parameters.planarFactor;
            bulgeFactorSlider.value = parameters.bulgeFactor;
            repulsionRangeSlider.value = parameters.repulsionRange;
            repulsionFactorSlider.value = parameters.repulsionFactor;
        }

        public void LinkLengthChanged() {
            parameters.linkLength = linkLengthSlider.value;
            if (cellularForm) parameters.Apply(cellularForm);
        }

        public void SpringFactorChanged() {
            parameters.springFactor = springFactorSlider.value;
            if (cellularForm) parameters.Apply(cellularForm);
        }

        public void PlanarFactorChanged() {
            parameters.planarFactor = planarFactorSlider.value;
            if (cellularForm) parameters.Apply(cellularForm);
        }

        public void BulgeFactorChanged() {
            parameters.bulgeFactor = bulgeFactorSlider.value;
            if (cellularForm) parameters.Apply(cellularForm);
        }

        public void RepulsionRangeChanged() {
            parameters.repulsionRange = repulsionRangeSlider.value; 
            if (cellularForm) parameters.Apply(cellularForm);
        }

        public void RepulsionStrengthChanged() {
            parameters.repulsionFactor = repulsionFactorSlider.value;
            if (cellularForm) parameters.Apply(cellularForm);
        }

        public void StartGrowth() {
            cellularForm.Run();
        }

        public void StopGrowth() {
            cellularForm.Pause();
        }

        public void ResetGrowth() {
            cellularForm.Restart();
        }
    }
}
