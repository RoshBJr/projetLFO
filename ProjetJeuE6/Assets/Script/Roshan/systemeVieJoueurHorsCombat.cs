using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Programmuer Roshan
public class systemeVieJoueurHorsCombat : MonoBehaviour
{

    public static SystemeVie vieHorsCombat;
    public static SystemeVie vieMax;
    public static bool sortiCombat = false;
    public static bool joueurSoigneHorsCombat = false;
    public GameObject barreDeVie;
    public static int degat;
    public static int viePersoHorsCombat;
    //public BarreVie barreVieJoueur;
    // Start is called before the first frame update
    void Start()
    {
        vieMax = new SystemeVie(200);
        //print(vieHorsCombat.initialiserVie());
        if (sortiCombat)
        {
            degat = 200 - vieHorsCombat.initialiserVie();
            vieMax.Degat(degat, false);
        }
        //sortiCombat = false;


    }
    private void FixedUpdate()
    {
        barreDeVie.transform.localScale = new Vector3(vieMax.avoirVieEnPourcent(), 1);
        //print("le joueur s'est soigné: " + joueurSoigneHorsCombat);
        
    }
    public void SoignerHorsCombat()
    {
        if(GestionInventairePerso.potionsSoin > 0 && joueurSoigneHorsCombat == false)
        {
            
            GestionInventairePerso.UtiliserPotionsSoin();
            vieMax.Soigner(60);
            print("valeur apres soin: "+vieMax.initialiserVie());
            viePersoHorsCombat = vieMax.initialiserVie();
            print("valeur à initialiser pour prochain combat: " + viePersoHorsCombat);
            vieHorsCombat = new SystemeVie(vieMax.initialiserVie());
            joueurSoigneHorsCombat = true;
            print("le joueur vient de se soigner: " + viePersoHorsCombat);
        }

        if (joueurSoigneHorsCombat)
        {
            joueurSoigneHorsCombat = false;
        }

    }
}
