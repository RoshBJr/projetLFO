using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Script pour gérer tout les menus sauf celui de pause

//programmeur Olivier

public class GestionMenu : MonoBehaviour
{
    public GameObject imgControl;
    public GameObject imgCredit;


    // si on clique sur jouer charge le jeu via le void ChargementJeu
    public void DetectionClicJouer()
    {      
        Invoke("ChargementJeu", 1f);
    }
    // si on clique sur quitter ferme le jeu via le void fermerLeJeu
    public void DetectionClicQuitter()
    {
        Invoke("fermerLeJeu", 1f);
    }
    // si on clique sur option affiche la scene d'option via le void menuOption
    public void DetectionClicOption()
    {
        Invoke("menuOption", 1f);
    }
    // si on clique sur le bouton commande affiche l'image des commandes via le void menuControl
    public void DetectionClicControl()
    {
        Invoke("menuControl", 0.1f);
    }
    // si on clique sur le bouton crédit affiche l'image des crédits via le void menuCrédit
    public void DetectionClicCredit()
    {
        Invoke("menuCredit", 0.1f);
    }
    // si on clique sur la fleche de retour on revient à la scene acceuil via le void retourMenu
    public void DetectionClicRetourMenu()
    {
        Invoke("retourMenu", 0.5f);
    }


    // Fonction pour le changement de scène.
    void ChargementJeu()
    {
        Permanent.menu = false;
        SceneManager.LoadScene(1);
    }
    // void pour ferme le jeu
    void fermerLeJeu()
    {
        Application.Quit();
    }
    // void pour charger la scene d'option
    void menuOption()
    {
        SceneManager.LoadScene(9);
    }
    // void pour afficher les control
    void menuControl()
    {
        imgControl.SetActive(true);
        imgCredit.SetActive(false);
    }
    //void pour afficher les crédits
    void menuCredit()
    {
        imgCredit.SetActive(true);
        imgControl.SetActive(false);
    }
    //void pour revenir au menu
    void retourMenu()
    {
        SceneManager.LoadScene(0);
    }
}