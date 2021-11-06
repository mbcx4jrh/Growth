using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace growth {
    public class MainInterface : MonoBehaviour {

        

        public void Quit() {
            Application.Quit();
        }

        public void InvestigateForm() {
            SceneManager.LoadScene("InvestigateForm");
        }
    }
}
