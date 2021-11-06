using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace growth {
    public class Parameters {
        public float linkLength;
        public float springFactor;
        public float planarFactor;
        public float bulgeFactor;
        public float repulsionRange;
        public float repulsionFactor;

        public Mesh seedMesh;

        public static Parameters FromCellularForm(CellularForm cellularForm) {
            var p = new Parameters {
                linkLength = cellularForm.linkLength,
                springFactor = cellularForm.springFactor,
                planarFactor = cellularForm.planarFactor,
                bulgeFactor = cellularForm.bulgeFactor,
                repulsionRange = cellularForm.repulsionRange,
                repulsionFactor = cellularForm.repulsionFactor,
                seedMesh = cellularForm.seedMesh
            };
            Debug.Log("sf " + p.springFactor);
            return p;
        }

        public void Apply(CellularForm cellularForm) {
            cellularForm.linkLength = linkLength;
            cellularForm.springFactor = springFactor;
            cellularForm.planarFactor = planarFactor;
            cellularForm.bulgeFactor = bulgeFactor;
            cellularForm.repulsionRange = repulsionRange;
            cellularForm.repulsionFactor = repulsionFactor;
            cellularForm.seedMesh = seedMesh;
        }
    }
}
