using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace growth {
    public class FileWriter {

        public static string filename1 = "D:/GrowthOutput/lastout1.pov";
        public static string filename2 = "D:/GrowthOutput/lastout2.pov";

        public static void WritePOVRaySpheres(List<Cell> cells, float radius) {
            EraseOldFile(filename1);

            using (StreamWriter sw = File.AppendText(filename1)) {
                foreach (var cell in cells) {
                    WriteSphere(sw, cell);
                }
            }
        }

        public static void WritePOVRaySpheresWithCut(List<Cell> cells, float radius, Plane cutPlane) {
            EraseOldFile(filename2);
            using (StreamWriter sw = File.AppendText(filename2)) {
                foreach (var cell in cells) {
                    if (cutPlane.GetSide(cell.position)) {
                        WriteSphere(sw, cell);
                    }
                }
            }
        }

        private static void EraseOldFile(string filename) {
            if (File.Exists(filename)) {
                File.Delete(filename);
            }
        }

        private static void WriteSphere(StreamWriter sw, Cell cell) {
            sw.WriteLine("sphere {");
            sw.WriteLine("  <" + cell.position.x + ", " + cell.position.y + ", " + cell.position.z + ">, R_GROWTH");
            sw.WriteLine("  texture {");
            sw.WriteLine("    T_GROWTH");
            sw.WriteLine("  }");
            sw.WriteLine("}");
        }
    }
}
