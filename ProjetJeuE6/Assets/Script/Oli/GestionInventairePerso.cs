
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script pour gérer l'inventaire du joueur 

//Programmeur Olivier

public class GestionInventairePerso : MonoBehaviour
{
    //quantité de potion de base
    public static float potionsSoin = 0;
    public static float potionsDefense = 0;
    public static float potionsAttaque = 0;
    public static float argents = 100;

    //public text
    public Text textePotionsSoin;
    public Text textePotionsAttaque;
    public Text textePotionsDefense;
    public Text texteArgents;

    //l'état des fragment recolté de base
    public static bool fragmentTerre = false;
    public static bool fragmentEau = false;
    public static bool fragmentAir = false;
    public static bool fragmentFeu = false;

    //audio d'achat
    public AudioClip sonAchat;

    public static bool sonAchatPossible;

    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        //Gestion de l'affichage des différentes parties de l'interface
        textePotionsSoin.text = " " + potionsSoin;
        textePotionsAttaque.text = " " + potionsAttaque;
        textePotionsDefense.text = " " + potionsDefense;

        texteArgents.text = argents + "";

        //Gestion de l'audio du marchand
        if(sonAchatPossible == true)
        {
            GetComponent<AudioSource>().PlayOneShot(sonAchat);

            sonAchatPossible = false;
        }
    }


    /* ---------------------------------------- si argent suffisant le joueur peut acheter une potion pour se soigner -------------------------------------------------- */
    public static void achatPotionsSoin()
    {
        if (argents > 0)
        {
            argents -= 10;
            potionsSoin += 1;
            sonAchatPossible = true;
        }

    }

    /* ---------------------------------------- si argent suffisant le joueur peut acheter une potion de défense -------------------------------------------------- */
    public static void achatPotionsDefense()
    {
        if (argents > 0)
        {
            argents -= 10;
            potionsAttaque += 1;
            sonAchatPossible = true;
        }

    }

    /* ---------------------------------------- si argent suffisant le joueur peut acheter une potion qui inflige des dégats -------------------------------------------------- */
    public static void achatPotionsAttaque()
    {
        if (argents > 0)
        {
            argents -= 10;
            potionsDefense += 1;
            sonAchatPossible = true;
        }

    }
    //gestion du bouton x du magasin
    public static void boutonX()
    {
        GestionCoroutine.AchatPossible = false;
    }
    //gestion du bouton pour la consommation de la potion d'attaque
    public static void UtiliserPotionsAttaque()
    {
        potionsAttaque--;
    }
    //gestion du bouton pour la consommation de la poition de soin
    public static void UtiliserPotionsSoin()
    {
        potionsSoin--;
    }
    //gestion du bouton pour la consommation de la poition de defense
    public static void UtiliserPotionDefense()
    {
        potionsDefense--;
    }

}
