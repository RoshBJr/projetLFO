using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Programmeur Roshan
public class UICombatAfficherDegatSoin : MonoBehaviour
{
    private static GameObject boiteTexte; // ce gameObject sert � stocker l'image pour afficher les actions en combat (soin, d�gat, d�fends, potion atk, potion d�fense)
    public static bool estActif = true; // bool nous permettant de savoir si le joueur est en train de joueur l'animation d'attaque au d�but il joue l'attaque
    public static Text textAAfficher; // variable text pour attribuer le texte � afficher dans la boite de texte

    
    void Start()
    {
        boiteTexte = GameObject.Find("boiteTexteAfficherDegat"); // attribue � la variable boiteTexte le gameobject nomm�e boiteTExteAfficherDegat dans la hierarchie
        textAAfficher = GameObject.Find("texteAAfficher").GetComponent<Text>(); // attribue � la variable texteAAfficher le gameObject nomm� texteAAficher dans la hierarchie
    }

    /* Ce IEnumerator nous permet d'attendre 3 secondes pour que le joueur reste sur l'ennemi afin de joueur l'animation d'attaque
     * ce IEnumerator prends en param�tre un bool nomm� boolCoroutine pour savoir si l'animation d'attaque du joueur est toujours active */
    public static IEnumerator Attendre(bool boolCoroutine)
    {
        yield return new WaitForSeconds(3); // permet d'attendre trois secondes avant d'�x�cuter le code qui suit
        estActif = boolCoroutine; // la variable estActif est �gal au bool donn� en param�tre
        estActif = false; // apr�s 3 secondes l'animation d'attaque du joueur est termin�e donc, on donne la valeur estActif false
    }

    /* Ce IEnumerator nous permet de modifier les animations pour que le joueur attaque lorsqu'il arrive sur la position de l'ennemi
     * Prend en param�tre l'animator du joueur pour modifier les bools d'animations */
    public static IEnumerator Attaquer(Animator animationJoueur)
    {
        yield return new WaitForSeconds(2); // attend 2 secondes avant de d�sactiver l'animation de marche et activer l'animation d'attaque
        animationJoueur.SetBool("marche", false); // apr�s 2 secondes l'animation de marche est d�sactiv�e
        animationJoueur.SetBool("attaque", true); // apr�s 2 secondes l'animation d'attaque est activ�e
        yield return new WaitForSeconds(1); // lorsque l'animation d'attaque joue on veut attendre une seconde avant de desactiver l'animation d'attaque et activer celle de marche
        animationJoueur.SetBool("marche", true); // active l'animation de marche
        animationJoueur.SetBool("attaque", false); // d�sactive l'animation d'attaque
    }

    /* Ce IEnumerator nous permet de faire disparaitre l'image de la boiteTexte apr�s de 2 secondes */
    public static IEnumerator DisparaitreImage()
    {
        yield return new WaitForSeconds(2); // attend 2 seconde avant de faire disparaitre l'image
        for (float i = 0.5f; i >= 0; i -= Time.deltaTime) // apr�s 2 secondes on veut tranquilement faire disparaitre l'image
        {
            boiteTexte.GetComponent<Image>().color = new Color(1, 1, 1, i); // on modifie l'opacit� de la boite texte pour qu'elle arrive � 0
            textAAfficher.color = new Color(0, 0, 0, i); // on modifie l'opacit� du texte � afficher pour qu'elle arrive � 0
            yield return null; // on retourne null a chaque fois parce qu'il faut toujours return quelque chose dans un Ienumerator
            
        }
    }

    /* Ce IEnumerator nous permet de faire apparaitre l'image de la boiteTexte 
     * prend en param�tre un string  pour l'afficher dans la variable textAAfficher */
    public static IEnumerator ApparaitreImage(string texteAffiche)
    {
        textAAfficher.text = texteAffiche; // on donne le string en param�tre � l'�l�ment texte de la variable textAAficher
        for (float i = 0.5f; i <= 1; i += Time.deltaTime) // on veut tranquilement faire apparaitre l'image boiteTexte
        {
            boiteTexte.GetComponent<Image>().color = new Color(1, 1, 1, i); // on modifie l'opacit� de la boite texte pour qu'elle arrive � 0
            textAAfficher.color = new Color(0, 0, 0, i); // on modifie l'opacit� du texte � afficher pour qu'elle arrive � 0
            yield return null; // on retourne null a chaque fois parce qu'il faut toujours return quelque chose dans un Ienumerator

        }
    }
}
