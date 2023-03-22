using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Programmeur Roshan

/* cette classe nous permet de cr�er des barres de vie et d'update leur quantit� au travers des combats */
public class BarreVie : MonoBehaviour
{
    private SystemeVie systemeVie; // cr�er une variable r�f�rence SystemeVie nomm�e systemeVie

    /* ce void nous permet d'initialiser une barre de vie pour pouvoir avoir la quantit� de d�gat et l'afficher en pourcent
     * Prend en param�tre une variable r�f�rence SystemeVie  */
    public void Initialiser(SystemeVie systemeVie)
    {
        this.systemeVie = systemeVie; // on prend la variable systemeVie de ce script pour �galer � celle donn�e en param�tre
    }

    private void FixedUpdate()
    {
        transform.Find("barre").localScale = new Vector3(systemeVie.avoirVieEnPourcent(), 1); /* ici, nous allons chercher le gameObject nomm� barre 
                                                                                               * et acc�der � son localScale pour modifier la longueur 
                                                                                               * de la barre lorsque des d�gats ou soin sont afflig�s */
    }
}
