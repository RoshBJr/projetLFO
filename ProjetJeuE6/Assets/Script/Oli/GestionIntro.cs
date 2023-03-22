using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script pour afficher le petit message d'introduction

//programmeur Olivier

public class GestionIntro : MonoBehaviour
{
    public static bool PlusDintro = false; //bool pour savoir si il y a encore un intro possible quand la scene reload
    public static bool Intro; //bool pour savoir si intro est true
    public GameObject textIntro; //text d'introduction
    // Start is called before the first frame update
    void Start()
    {
        //si PlusDintro est false affiche l'intro
        if (PlusDintro == false)
        {
            textIntro.SetActive(true);
            Invoke("finIntro", 25f);
            Intro = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //si PlusDintro est false affiche l'intro
        //si on appuie sur escape ou enter retire le message d'introduction
        if (PlusDintro == false)
        {
            if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Return))
            {
                PlusDintro = true;
                textIntro.SetActive(false);
                Intro = false;
            }
        }

    }
    //fait disparaitre d'intro si appelé
    void finIntro()
    {
        textIntro.SetActive(false);
    }
}
