using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Programmeur Roshan
public class UICombatAfficherDegatSoin : MonoBehaviour
{
    private static GameObject boiteTexte; // ce gameObject sert à stocker l'image pour afficher les actions en combat (soin, dégat, défends, potion atk, potion défense)
    public static bool estActif = true; // bool nous permettant de savoir si le joueur est en train de joueur l'animation d'attaque au début il joue l'attaque
    public static Text textAAfficher; // variable text pour attribuer le texte à afficher dans la boite de texte

    
    void Start()
    {
        boiteTexte = GameObject.Find("boiteTexteAfficherDegat"); // attribue à la variable boiteTexte le gameobject nommée boiteTExteAfficherDegat dans la hierarchie
        textAAfficher = GameObject.Find("texteAAfficher").GetComponent<Text>(); // attribue à la variable texteAAfficher le gameObject nommé texteAAficher dans la hierarchie
    }

    /* Ce IEnumerator nous permet d'attendre 3 secondes pour que le joueur reste sur l'ennemi afin de joueur l'animation d'attaque
     * ce IEnumerator prends en paramètre un bool nommé boolCoroutine pour savoir si l'animation d'attaque du joueur est toujours active */
    public static IEnumerator Attendre(bool boolCoroutine)
    {
        yield return new WaitForSeconds(3); // permet d'attendre trois secondes avant d'éxécuter le code qui suit
        estActif = boolCoroutine; // la variable estActif est égal au bool donné en paramètre
        estActif = false; // après 3 secondes l'animation d'attaque du joueur est terminée donc, on donne la valeur estActif false
    }

    /* Ce IEnumerator nous permet de modifier les animations pour que le joueur attaque lorsqu'il arrive sur la position de l'ennemi
     * Prend en paramètre l'animator du joueur pour modifier les bools d'animations */
    public static IEnumerator Attaquer(Animator animationJoueur)
    {
        yield return new WaitForSeconds(2); // attend 2 secondes avant de désactiver l'animation de marche et activer l'animation d'attaque
        animationJoueur.SetBool("marche", false); // après 2 secondes l'animation de marche est désactivée
        animationJoueur.SetBool("attaque", true); // après 2 secondes l'animation d'attaque est activée
        yield return new WaitForSeconds(1); // lorsque l'animation d'attaque joue on veut attendre une seconde avant de desactiver l'animation d'attaque et activer celle de marche
        animationJoueur.SetBool("marche", true); // active l'animation de marche
        animationJoueur.SetBool("attaque", false); // désactive l'animation d'attaque
    }

    /* Ce IEnumerator nous permet de faire disparaitre l'image de la boiteTexte après de 2 secondes */
    public static IEnumerator DisparaitreImage()
    {
        yield return new WaitForSeconds(2); // attend 2 seconde avant de faire disparaitre l'image
        for (float i = 0.5f; i >= 0; i -= Time.deltaTime) // après 2 secondes on veut tranquilement faire disparaitre l'image
        {
            boiteTexte.GetComponent<Image>().color = new Color(1, 1, 1, i); // on modifie l'opacité de la boite texte pour qu'elle arrive à 0
            textAAfficher.color = new Color(0, 0, 0, i); // on modifie l'opacité du texte à afficher pour qu'elle arrive à 0
            yield return null; // on retourne null a chaque fois parce qu'il faut toujours return quelque chose dans un Ienumerator
            
        }
    }

    /* Ce IEnumerator nous permet de faire apparaitre l'image de la boiteTexte 
     * prend en paramètre un string  pour l'afficher dans la variable textAAfficher */
    public static IEnumerator ApparaitreImage(string texteAffiche)
    {
        textAAfficher.text = texteAffiche; // on donne le string en paramètre à l'élément texte de la variable textAAficher
        for (float i = 0.5f; i <= 1; i += Time.deltaTime) // on veut tranquilement faire apparaitre l'image boiteTexte
        {
            boiteTexte.GetComponent<Image>().color = new Color(1, 1, 1, i); // on modifie l'opacité de la boite texte pour qu'elle arrive à 0
            textAAfficher.color = new Color(0, 0, 0, i); // on modifie l'opacité du texte à afficher pour qu'elle arrive à 0
            yield return null; // on retourne null a chaque fois parce qu'il faut toujours return quelque chose dans un Ienumerator

        }
    }
}
