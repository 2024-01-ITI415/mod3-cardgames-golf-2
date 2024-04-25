using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void loadProspector()
    {
        SceneManager.LoadScene("__Prospector");
    }

    public void loadGolf()
    {
        SceneManager.LoadScene("__Golf");
    }
}
