using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// script pour gérer la scene de mort

//Programmeur Olivier
public class SceneMort : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("RetourHubWorld", 3f);
    }

    void RetourHubWorld()
    {
        SceneManager.LoadScene(1);
    }
}
