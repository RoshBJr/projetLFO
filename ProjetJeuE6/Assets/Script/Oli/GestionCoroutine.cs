using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//script pour gérer les coroutines

//programmeur Olivier

public class GestionCoroutine : MonoBehaviour
{

    public GameObject buttonAchatDesPotionsSoin; // variable gameObject pour afficher le bouton d'achat des potions de soins
    public GameObject buttonAchatDesPotionsDefense; // variable gameObject pour afficher le bouton d'achat des potions de défense
    public GameObject buttonAchatDesPotionsAttaque; // variable gameObject pour afficher le bouton d'achat des potions pour augmenter les dégats
    public GameObject imgShop; // boite pour acheter items
    public GameObject boutonX;// bouton pour fermer le magasin

    public static bool AchatPossible = false;
    


    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        //si l'achat est possible affiche les boutons de soin, défense, attaque
        if (AchatPossible == true)
        {
            buttonAchatDesPotionsSoin.SetActive(true);
            buttonAchatDesPotionsDefense.SetActive(true);
            buttonAchatDesPotionsAttaque.SetActive(true);
            imgShop.SetActive(true);
            boutonX.SetActive(true);

        }
        //si l'achat n'est pas possible fait disparaitre les boutons de soin, défense, attaque
        if (AchatPossible == false)
        {
            buttonAchatDesPotionsSoin.SetActive(false);
            buttonAchatDesPotionsDefense.SetActive(false);
            buttonAchatDesPotionsAttaque.SetActive(false);
            imgShop.SetActive(false);
            boutonX.SetActive(false);
        }
    }

    
    public static IEnumerator AggroCoroutine()
    {
        
        //Attendre 1s secondes un fois que le trigger est trigger
        //ensuite roullement des probabilités de combat (si égale 1 le joueur rentre en combat)
        //et engage revient à false 
        yield return new WaitForSeconds(1);
        if(GestionCollisionPerso.engage == true)
        {
            var changementDeScene = Random.Range(0 , 2);

            if (changementDeScene == 1)
            {
                MouvementPersonnage.estTriggerCombat = true;

                Permanent.musiqueCombatDetecteur = true;
                //print(MouvementPersonnage.estTriggerCombat);
                MouvementPersonnage.positionReappartionFight = GameObject.Find("Joueur").transform.position;
                print(MouvementPersonnage.positionReappartionFight);
                //print("changement de scene Combat");
                SceneManager.LoadScene(3);
            }

            GestionCollisionPerso.engage = false;
        }

    }

    public static IEnumerator AggroCoroutineBoss()
    {

        //Attendre 0.1 secondes un fois que le trigger est trigger
        //ensuite le joueur rentre en combat avec le boss et la musique de combat commence
        //et engage revient à false 
        yield return new WaitForSeconds(0.1f);
        if (GestionCollisionPerso.engage == true)
        {
                Permanent.musiqueCombatDetecteur = true;
                MouvementPersonnage.estTriggerCombat = true;
                
                print(MouvementPersonnage.positionReappartionFight);
                
                GestionCollisionPerso.fightBoss = true;
                SceneManager.LoadScene(3);

        }

    }
    //Attendre 0.05 secondes un fois que le trigger est trigger
    //ensuite l'interaction avec le marchand s'active 
    //et engage revient à false 
    public static IEnumerator MarchandCoroutine()
    {
        yield return new WaitForSeconds(0.05f);
        if (GestionCollisionPerso.engage == true)
        {
            
            AchatPossible = true;

            GestionCollisionPerso.engage = false;
        }

    }



    
    
}
