using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Permet de rentre les musiques permanentes  */

//Programmeur Olivier

public class Permanent : MonoBehaviour
{
    public static bool musiqueCombatDetecteur = false;
    public static bool menu = true; 
    public GameObject musiqueCombat;
    public GameObject musiqueMenu;
    public GameObject musiqueHub;
    public AudioSource musiqueHubControle;
    public static bool existe; // false au début
    // Start is called before the first frame update
    void Start()
    {

        // existe est n'est pas false ne detruit pas le contenu
        if (!existe)
        {
            DontDestroyOnLoad(gameObject);
            existe = true;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    private void FixedUpdate()
    {
        // si nous ne sommes pas dans le menu
        if (menu == false)
        {
            //detruit la musique menu et active la musique du hub
            Destroy(musiqueMenu);          
            musiqueHub.SetActive(true);
            
            // si nous sommes en combat ferme la musique hub et active la musique de combate
            if (musiqueCombatDetecteur == true)
            {
                musiqueHub.SetActive(false);
                musiqueCombat.SetActive(true);

            } // si on sort d'un combat on remet la musique hub
            else if (musiqueCombatDetecteur == false)
            {
                musiqueCombat.SetActive(false);
                musiqueHub.SetActive(true);
            }
        }

    }
}
