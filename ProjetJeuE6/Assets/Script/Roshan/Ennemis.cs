using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Programmeur Roshan
public class Ennemis : MonoBehaviour
{
    public Sprite[] ennemiMaterielCouleur; // cr�er un tableau de type Sprite nomm� ennemiMaterielCouleur pour stocker les sprites ennemis de chacunes
                                           // des maps
    public GameObject[] gameObjectPourLesSprite; // ce tableau de type GameObject nomm� gameObjectPourLesSprite sert � garder les gameObjects poss�dant les sprites des ennemis
    public static bool existe; // bool permettant de savoir si le gameObject existe dans la hierarchie au d�but, il n'existe pas donc la valeur est false

    
    void Start()
    {
        DontDestroyOnLoad(gameObject); // lorsqu'on change de scene on ne veut pas d�truire le gameObject nomm� ennemiPourLesMap
        gameObjectPourLesSprite = GameObject.FindGameObjectsWithTag("spriteEnnemi"); // ici, on va chercher tous les gameObjects poss�dant le tag "spriteEnnemi"
        if ( GameObject.FindGameObjectsWithTag("spriteEnnemi").Length > 1) // s'il y a plus qu'un gameObject avec le tag "spriteEnnemi"
        {
            Destroy(gameObjectPourLesSprite[0]); // on veut d�truire celui � la position 0, car elle poss�de les sprites ennemis d'une autre map,
                                                 // alors qu'on veut celle � la positon 1
        }
        
    }
}
