using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//script pour gérer les scenes de victoire 

//Programmeur Olivier
public class SceneWin : MonoBehaviour
{
    public static bool victoireTotal = false;

    public GameObject terre;
    public GameObject eau;
    public GameObject feu;
    public GameObject air;
    // Start is called before the first frame update
    private void Awake()
    {
        if (GestionInventairePerso.fragmentTerre == true && GestionInventairePerso.fragmentFeu == true && GestionInventairePerso.fragmentEau == true && GestionInventairePerso.fragmentAir == true)
        {
            SceneManager.LoadScene(10);
        }

        // si nous sommes dans la scene d'eau affiche que l'on vient de récolter le fragment d'eau
        if (MouvementPersonnage.eau == true)
        {
            terre.SetActive(false);
            eau.SetActive(true);
            feu.SetActive(false);
            air.SetActive(false);
        }//si nous sommes dans la scene de terre affiche que l'on vient de récolter le fragment de terre
        else if (MouvementPersonnage.earth == true)
        {
            terre.SetActive(true);
            eau.SetActive(false);
            feu.SetActive(false);
            air.SetActive(false);
        }// si nous sommes dans la scene de feu affiche que l'on vient de recolter le fragment de feu
        else if (MouvementPersonnage.feu == true)
        {
            terre.SetActive(false);
            eau.SetActive(false);
            feu.SetActive(true);
            air.SetActive(false);
        }
        //si nous sommes dans la scene d'air affiche que l'on vient de recolter le fragment d'air
        else if (MouvementPersonnage.air == true)
        {
            terre.SetActive(false);
            eau.SetActive(false);
            feu.SetActive(false);
            air.SetActive(true);
        }
    }
    // renouvelle la position du joueur
    void Start()
    {
        if (victoireTotal == false)
        {
            Invoke("RetourHub", 3f);
        }
        
        MouvementPersonnage.eau = false;
        MouvementPersonnage.feu = false;
        MouvementPersonnage.earth = false;
        MouvementPersonnage.air = false;
    }
    // retour au hub
    void RetourHub()
    {
        MouvementPersonnage.hubworld = true;
        MouvementPersonnage.earth = false;
        SceneManager.LoadScene(1);

    }
}
