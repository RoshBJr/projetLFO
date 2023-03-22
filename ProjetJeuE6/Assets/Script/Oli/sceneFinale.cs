using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneFinale : MonoBehaviour
{
    public GameObject messageDeFin;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("textFin", 4.4f);
        Invoke("retourMenu", 15f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void textFin()
    {
        messageDeFin.SetActive(true);
    }
    void retourMenu()
    {
        SceneManager.LoadScene(0);
    }
}
