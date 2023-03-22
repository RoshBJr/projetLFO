using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Script me permetant de cr�er une cam�ra de suivie*/

//Programmeur Danishcar

public class CameraDeSuivie : MonoBehaviour
{


	public GameObject cible;
	public Vector3 distaceCamera;
	public float amortissement;



	// D�place la cam�ra pour suivre le joueur graduellement 
	void FixedUpdate()
	{
		//Gestion de la cam�ra de suivie
		var positionFinale = cible.transform.TransformPoint(distaceCamera);
		transform.position = Vector3.Lerp(transform.position, positionFinale, amortissement);

		transform.LookAt(cible.transform);
	}


}
