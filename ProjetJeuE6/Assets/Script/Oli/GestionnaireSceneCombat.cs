using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script qui gere les décors du combat en fonction de la zone dans laquelle on se trouve

//Programmeur Olivier

public class GestionnaireSceneCombat : MonoBehaviour
{
    // gestionnaire de tout les décors
    public GameObject zoneTerre;
    public GameObject zoneAir;
    public GameObject zoneFeu;
    public GameObject zoneEau;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //si le joueur est dans la zone de terre fait apparaitre le decor de terre et disparaitre les autres décors
        if (MouvementPersonnage.earth == true)
        {
            zoneTerre.SetActive(true);
            zoneEau.SetActive(false);
            zoneFeu.SetActive(false);
            zoneAir.SetActive(false);
        }
        //si le joueur est dans la zone de feu fait apparaitre le decor de feu et disparaitre les autres décors
        if (MouvementPersonnage.feu == true)
        {
            zoneTerre.SetActive(false);
            zoneEau.SetActive(false);
            zoneFeu.SetActive(true);
            zoneAir.SetActive(false);
        }
        //si le joueur est dans la zone d'eau fait apparaitre le decor d'eau et disparaitre les autres décors
        if (MouvementPersonnage.eau == true)
        {
            zoneTerre.SetActive(false);
            zoneEau.SetActive(true);
            zoneFeu.SetActive(false);
            zoneAir.SetActive(false);
        }
        //si le joueur est dans la zone d'air fait apparaitre le decor d'air et disparaitre les autres décors
        if (MouvementPersonnage.air == true)
        {
            zoneTerre.SetActive(false);
            zoneEau.SetActive(false);
            zoneFeu.SetActive(false);
            zoneAir.SetActive(true);
        }

    }
}
