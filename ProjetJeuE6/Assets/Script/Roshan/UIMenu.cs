using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Programmeur Roshan
public class UIMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler // IPointerEnterHandler est un eventSystem permettant d'accèder au void OnPointerEnter,
                                                                               // IPointerExitHandler est un eventSystem permettant d'accèder au void OnPointerExit
{
    private bool estSelectionner; // un bool qui nous permet de savoir si un bouton est selectionner
    public GameObject bouttonASelectionner; // Élément GameObject pour stocker le bouton en question
    
    void Update()
    {
        if (estSelectionner) // si le bouton est selectionné
        {
            bouttonASelectionner.GetComponent<Animator>().SetBool("selected", true); // l'animation du bouton est actif
        }
        else // sinon...
        {
            bouttonASelectionner.GetComponent<Animator>().SetBool("selected", false); // l'animation du bouton est désactivée
        }
    }
    public void OnPointerEnter(PointerEventData eventData) // un void nous permettant de savoir si le curseur est par dessus le bouton
    {
        estSelectionner = true; // si c'est le cas, on donne une valeur true au bool estSelectioner
    }
    public void OnPointerExit(PointerEventData eventData) // un void nous permettant de savoir si le curseur n'est plus par dessus le bouton
    {
        estSelectionner = false; // si c'est le cas, on donne une valeur false au bool estSelectionner
    }
}
