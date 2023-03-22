using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script pour gérer le menu pause

//programmeur Olivier
public class GestionMenuPause : MonoBehaviour
{
    public static bool Escape = true;
    public GameObject BoutonJouer;
    public GameObject BoutonQuitter;
    public GameObject BackgroundImgPause;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //si le mouvement est possible et on clique sur escape le menu pause apparait
        if (MouvementPersonnage.mouvementPossible == true && Escape == true && Input.GetKey(KeyCode.Escape) && GestionIntro.Intro == false) {

            //tout action devient impossible pendant que le jeu est sur pause
            GestionCoroutine.AchatPossible = false;
            MouvementPersonnage.mouvementPossible = false;
            //les boutons apparaissent
            BoutonJouer.SetActive(true);
            BoutonQuitter.SetActive(true);
            BackgroundImgPause.SetActive(true);
            Escape = false;
            Invoke("EscClicable", 0.2f);

        }

        //si le mouvement du joueur est impossible et on clique sur escape le menu pause disparait 
        if (MouvementPersonnage.mouvementPossible == false && Escape == true && Input.GetKey(KeyCode.Escape))
        {
            //le deplacemetn devient à nouveau possible
            MouvementPersonnage.mouvementPossible = true;
            // les boutons disparaissent
            BoutonJouer.SetActive(false);
            BoutonQuitter.SetActive(false);
            BackgroundImgPause.SetActive(false);
            Escape = false;
            Invoke("EscClicable", 0.2f);
        }




    }

    // si on clique sur bouton quitter ferme le jeu
    public void DetectionClicQuitter()
    {
        Invoke("fermerLeJeu", 1f);
    }

    //si on clique sur le bouton jouer on revient sur le jeu
    public void DetectionClicRetourJouer()
    {
        //GetComponent<AudioSource>().Play();
        Invoke("RetourJeu", 0f);
        print("bob");
    }

    void EscClicable()
    {
        Escape = true;
    }
    // void pour quitter le jeu
    void fermerLeJeu()
    {
        Application.Quit();
    }
    // void pour sortir du menu pause
    void RetourJeu()
    {
        MouvementPersonnage.mouvementPossible = true;
        BoutonJouer.SetActive(false);
        BoutonQuitter.SetActive(false);
        BackgroundImgPause.SetActive(false);

    }
}
