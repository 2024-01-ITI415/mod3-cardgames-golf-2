using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Golf
{
    public class LoadScene : MonoBehaviour
    {
        public void loadGolf()
        {
            SceneManager.LoadScene("__Golf");
        }
    }
}
