using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace growth {
    public class FileWriter {

        public static string filename = "D:/GrowthOutput/lastout.pov";

        public static void WritePOVRaySpheres(List<Cell> cells, float radius) {
            if (File.Exists(filename)) {
                File.Delete(filename);
            }

            using (StreamWriter sw = File.AppendText(filename)) {
                foreach (var cell in cells) {
                    sw.WriteLine("sphere {");
                    sw.WriteLine("  <" + cell.position.x + ", " + cell.position.y + ", " + cell.position.z + ">, R_GROWTH");
                    sw.WriteLine("  texture {");
                    sw.WriteLine("    T_GROWTH");
                    sw.WriteLine("  }");
                    sw.WriteLine("}");
                }
            }
        }
    }
}
