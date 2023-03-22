using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Script me permetant de créer une caméra de suivie*/

//Programmeur Danishcar

public class CameraDeSuivie : MonoBehaviour
{


	public GameObject cible;
	public Vector3 distaceCamera;
	public float amortissement;



	// Déplace la caméra pour suivre le joueur graduellement 
	void FixedUpdate()
	{
		//Gestion de la caméra de suivie
		var positionFinale = cible.transform.TransformPoint(distaceCamera);
		transform.position = Vector3.Lerp(transform.position, positionFinale, amortissement);

		transform.LookAt(cible.transform);
	}


}
