using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Script pour gérer les collisions

//Programmeur Olivier

public class GestionCollisionPerso : MonoBehaviour
{
    //bool pour savoir si le jeu roule les probabilit�s pour rentrer en combat
    public static bool engage;


    public static bool fightBoss = false;

    public GameObject AfficheE;

    public static int chiffreScenePourSortirCombat;




    // Start is called before the first frame update
    void Start()
    {
        engage = false; //bool permettant de controler les coroutines
    }
    // ------------------------------------- GESTION DES TRIGGERS ---------------------------------------
    private void OnTriggerStay(Collider autreObjet)
    {
        //si le joueur rentre en collision avec le trigger des ennemies
        //si engage est false appelle AppelerCoroutine
        if (autreObjet.gameObject.tag == "ennemies")
        {
            if(engage == false)
            {
                AppelerCoroutineAggro();
            }

        }

        //si le joueur rentre en collision avec le trigger du boss
        //si engage esst false appelle CouroutineAggroBoss
        if (autreObjet.gameObject.tag == "boss")
        {
            if (engage == false)
            {
                CoroutineAggroBoss();
            }

        }
        //si le joueur rentre en collision avec le trigger du marchands et clique sur e
        //active l'interaction avec le marchand
        if (autreObjet.gameObject.tag == "marchand" && Input.GetKey(KeyCode.E))
        {
            AfficheE.SetActive(false);
            if (engage == false)
            {
                AppelerCoroutineMarchand();
                
            }

        }
        if (autreObjet.gameObject.tag == "marchand")
        {
            if(GestionCoroutine.AchatPossible == false)
            {
                AfficheE.SetActive(true);
            }
            
        }
        //si le joueur est en transaction avec le marchand et souhaite sortir clique sur ESC 
        //rend le deplacement � nouveau possible et ferme le menu d'achat
        else if (autreObjet.gameObject.tag == "marchand" && Input.GetKey(KeyCode.Escape))
        {
            
            GestionCoroutine.AchatPossible = false;
        }

        //------------------------GESTION DE L'ACCES À TOUTES LES ZONES


        //------------------------Acces � la zone de terre 

        //si on rentre dans la zone terre toutes les autres zones tournent false
        if (autreObjet.gameObject.tag == "zoneTerre")
        {
            
            MouvementPersonnage.hubworld = false;
            MouvementPersonnage.earth = true;
            MouvementPersonnage.feu = false;
            MouvementPersonnage.air = false;
            MouvementPersonnage.eau = false;
            chiffreScenePourSortirCombat = 2;
            print("ZoneTerre" + MouvementPersonnage.earth);
            SceneManager.LoadScene(2);
            //-----------------------Acces � la zone de feu

        //si on rentre dans la zone feu toutes les autres zones tournent false
        }
        if (autreObjet.gameObject.tag == "zoneFeu")
        {
            MouvementPersonnage.hubworld = false;
            MouvementPersonnage.earth = false;
            MouvementPersonnage.feu = true;
            MouvementPersonnage.air = false;
            MouvementPersonnage.eau = false;
            chiffreScenePourSortirCombat = 6;
            SceneManager.LoadScene(6);
        }

        //---------------------Acces � la zone d'eau

        //si on rentre dans la zone d'eau toutes les autres zones tournent false
        if (autreObjet.gameObject.tag == "zoneEau")
        {

            MouvementPersonnage.hubworld = false;
            MouvementPersonnage.earth = false;
            MouvementPersonnage.feu = false;
            MouvementPersonnage.air = false;
            MouvementPersonnage.eau = true;
            chiffreScenePourSortirCombat = 7;
            SceneManager.LoadScene(7);
        }

        //----------------------Acces � la zone Air

        //si on rentre dans la zone d'air toutes les autres zones tournent false
        if (autreObjet.gameObject.tag == "zoneAir")
        {

            MouvementPersonnage.hubworld = false;
            MouvementPersonnage.earth = false;
            MouvementPersonnage.feu = false;
            MouvementPersonnage.air = true;
            MouvementPersonnage.eau = false;
            chiffreScenePourSortirCombat = 8;
            SceneManager.LoadScene(8);
        }

        //-----------------------Acces � la zone HubWorld

        //si on rentre dans le HubWorld toutes les autres zones tournent false
        if (autreObjet.gameObject.tag == "retourHubworld")
        {
            GestionIntro.PlusDintro = true;

            MouvementPersonnage.estTriggerCombat = false;
            MouvementPersonnage.hubworld = true;
            MouvementPersonnage.eau = false;
            MouvementPersonnage.feu = false;
            MouvementPersonnage.earth = false;
            MouvementPersonnage.air = false;
            chiffreScenePourSortirCombat = 1;
            SceneManager.LoadScene(1);
        }

    }
    // ------------------------------------- GESTION DES TRIGGERS EXIT---------------------------------------

    private void OnTriggerExit(Collider autreObjet)
    {
        //si le joueur quite la zone trigger du marchand ferme la transaction
        if (autreObjet.gameObject.tag == "marchand")
        {

            GestionCoroutine.AchatPossible = false;
            AfficheE.SetActive(false);
        }
    }


    public void AppelerCoroutineAggro()
    {
        //engage devient true
        //Appelle AggroCoroutine du script GestionCoroutine si le joueur n'est pas dans un loadingScreen
        
        if (LoadingScreen.notInLoadingScreen == true)
        {
            engage = true;
            StartCoroutine(GestionCoroutine.AggroCoroutine());
        }
        
        
        
    }

    public void CoroutineAggroBoss()
    {
        //engage devient true
        //Appelle AggroCoroutineBossTerre du script GestionCoroutine si le joueur n'est pas dans un loadingScreen


        if (LoadingScreen.notInLoadingScreen == true)
        {
            engage = true;

            fightBoss = true;
            print("fight boss");
            StartCoroutine(GestionCoroutine.AggroCoroutineBoss());
        }
        


    }
    public void AppelerCoroutineMarchand()
    {
        //engage devient true
        //Appelle MarchandCoroutine du script Gestion�coroutine
        engage = true;
        StartCoroutine(GestionCoroutine.MarchandCoroutine());
    }
}
