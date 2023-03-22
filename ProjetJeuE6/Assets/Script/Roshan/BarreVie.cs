using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Programmeur Roshan

/* cette classe nous permet de créer des barres de vie et d'update leur quantité au travers des combats */
public class BarreVie : MonoBehaviour
{
    private SystemeVie systemeVie; // créer une variable référence SystemeVie nommée systemeVie

    /* ce void nous permet d'initialiser une barre de vie pour pouvoir avoir la quantité de dégat et l'afficher en pourcent
     * Prend en paramètre une variable référence SystemeVie  */
    public void Initialiser(SystemeVie systemeVie)
    {
        this.systemeVie = systemeVie; // on prend la variable systemeVie de ce script pour égaler à celle donnée en paramètre
    }

    private void FixedUpdate()
    {
        transform.Find("barre").localScale = new Vector3(systemeVie.avoirVieEnPourcent(), 1); /* ici, nous allons chercher le gameObject nommé barre 
                                                                                               * et accéder à son localScale pour modifier la longueur 
                                                                                               * de la barre lorsque des dégats ou soin sont affligés */
    }
}
