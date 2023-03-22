using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script pour gérer les écrans de chargement

//Programmeur Olivier

public class LoadingScreen : MonoBehaviour
{
    public GameObject loading;
    public static bool notInLoadingScreen;
    // Start is called before the first frame update
    void Start()
    {
        // quand une scene charge le mouvement est mis impossible
        MouvementPersonnage.mouvementPossible = false;
        notInLoadingScreen = false;

        // apelle la fin du loading screen apres 2secs et rend l'aggro possible apres 4secs
        Invoke("finLoading", 2f);
        Invoke("AggroPossible", 4f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // void qui rend le mouvement du personnage à nouveau possible
    // enlève le loading screen
    void finLoading()
    {
        MouvementPersonnage.mouvementPossible = true;
        loading.SetActive(false);
    }
    // void qui rend l'aggro à nouveau possible en precisant que nous ne sommes plus dans le loading screen
    void AggroPossible()
    {
        
        notInLoadingScreen = true;
    }
}
