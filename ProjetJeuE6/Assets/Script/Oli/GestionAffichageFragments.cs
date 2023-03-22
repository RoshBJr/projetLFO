using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Programmeur Olivier

//Script permettant de faire apparaitre les fragments récupérés dans le hub world
public class GestionAffichageFragments : MonoBehaviour
{
    public GameObject terre;
    public GameObject eau;
    public GameObject air;
    public GameObject feu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*Si le fragment a été récupéré set active true*/
        if(GestionInventairePerso.fragmentFeu == true)
        {
            feu.SetActive(true);
        }
        if (GestionInventairePerso.fragmentEau == true)
        {
            eau.SetActive(true);
        }
        if (GestionInventairePerso.fragmentAir == true)
        {
            air.SetActive(true);
        }
        if (GestionInventairePerso.fragmentTerre == true)
        {
            terre.SetActive(true);
        }
    }
}
