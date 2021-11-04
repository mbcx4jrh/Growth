using DataStructures.ViliWonka.KDTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace growth {
    public class Worker {

        KDQuery query = new KDQuery();
        List<int> queryResults = new List<int>();

        public int start;
        public int end;

        float link2, r2;

        CellularForm cellularForm;

        public Worker(CellularForm cellularForm, int start, int end, float link2, float r2) {
            this.cellularForm = cellularForm;
            this.start = start;
            this.end = end;
            this.link2 = link2;
            this.r2 = r2;
            //Debug.Log("work, start: " + this.start + " , end " + this.end);
        }

        public void CalculateForcesOnCells() {
            for (int i=start; i<end;i++) {
                //queryResults.Clear();
                cellularForm.CalculateForcesOnCell(query, queryResults,link2, r2, cellularForm.cells[i]);
            }
        }

    }
}
