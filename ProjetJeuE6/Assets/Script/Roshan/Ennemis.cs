using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Programmeur Roshan
public class Ennemis : MonoBehaviour
{
    public Sprite[] ennemiMaterielCouleur; // créer un tableau de type Sprite nommé ennemiMaterielCouleur pour stocker les sprites ennemis de chacunes
                                           // des maps
    public GameObject[] gameObjectPourLesSprite; // ce tableau de type GameObject nommé gameObjectPourLesSprite sert à garder les gameObjects possédant les sprites des ennemis
    public static bool existe; // bool permettant de savoir si le gameObject existe dans la hierarchie au début, il n'existe pas donc la valeur est false

    
    void Start()
    {
        DontDestroyOnLoad(gameObject); // lorsqu'on change de scene on ne veut pas détruire le gameObject nommé ennemiPourLesMap
        gameObjectPourLesSprite = GameObject.FindGameObjectsWithTag("spriteEnnemi"); // ici, on va chercher tous les gameObjects possédant le tag "spriteEnnemi"
        if ( GameObject.FindGameObjectsWithTag("spriteEnnemi").Length > 1) // s'il y a plus qu'un gameObject avec le tag "spriteEnnemi"
        {
            Destroy(gameObjectPourLesSprite[0]); // on veut détruire celui à la position 0, car elle possède les sprites ennemis d'une autre map,
                                                 // alors qu'on veut celle à la positon 1
        }
        
    }
}
