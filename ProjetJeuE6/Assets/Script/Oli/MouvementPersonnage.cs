using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script pour gérer les mouvements du personnage et sa position dans le jeu

//Programmeur Olivier et Roshan

public class MouvementPersonnage : MonoBehaviour
{
    public float vitesse; // vitesse deplacement du perso
    public static bool mouvementPossible = true; // le mouvement est possible

    public AudioClip sonMarche; // audio de deplacement
    bool audioMarche = false;

    public static Vector3[] positionDebut = new Vector3[5] ; // liste pour gérer les positions d'apparition en fonction de la map
    public static Vector3 positionReappartion; //vecteur 3 pour appliquer la position de réaparation
    public static Vector3 positionReappartionFight; // vecteur 3 pour enregistrer la position d'un joueur sur la map quand il rentre en combat
    public static bool estTriggerCombat; // bool pour savoir si le joueur est en combat

    //bool pour savoir dans quel map le joueur est
    public static bool hubworld = true;
    public static bool earth;
    public static bool feu;
    public static bool air;
    public static bool eau;
    


    void Awake()
    { 
    }
        
    // Start is called before the first frame update joe
    void Start()
    {
        // les différents position d'apparition selon la map
        positionDebut[0] = new Vector3(109f, 475.1f, 83.5f); // hubworld
        positionDebut[1] = new Vector3(121.9f, 452.5f, 35f); // terre
        positionDebut[2] = new Vector3(24, 5.5f, 367.2526f); // air
        positionDebut[3] = new Vector3(21.2f, 701.9f, 12.3f); // feu
        positionDebut[4] = new Vector3(79.7f, 1.9f, 31.7f); // eau

        
        if (hubworld)// si nous sommes dans le hubWorld 
        {
            
            if (estTriggerCombat) //et qu'on sort d'un combat on réaparait où on est rentré en combat
            {
                positionReappartion = positionReappartionFight;
            }

            else //sinon on apparait au point d'apparition de cette zone
            {
                positionReappartion = positionDebut[0];
            }          
        }
        else if (earth) // si nous sommes dans la zone terre
        {
            if (estTriggerCombat) //et qu'on sort d'un combat on réaparait où on est rentré en combat
            {
                positionReappartion = positionReappartionFight;
            }
            else //sinon on apparait au point d'apparition de cette zone
            {
                positionReappartion = positionDebut[1];
            }
        }
        else if (air) // si nous sommes dans la zone air
        {
            if (estTriggerCombat) //et qu'on sort d'un combat on réaparait où on est rentré en combat
            {
                positionReappartion = positionReappartionFight; 
            }
            else //sinon on apparait au point d'apparition de cette zone
            {
                positionReappartion = positionDebut[2];
            }
        }
        else if (feu) // si nous sommes dans la zone feu
        {
            if (estTriggerCombat) //et qu'on sort d'un combat on réaparait où on est rentré en combat
            {
                positionReappartion = positionReappartionFight;
            }
            else //sinon on apparait au point d'apparition de cette zones
            {
                positionReappartion = positionDebut[3];
            }
        }
        else if (eau) // si nous sommes dans la zone eau
        {
            if (estTriggerCombat) //et qu'on sort d'un combat on réaparait où on est rentré en combat
            {
                positionReappartion = positionReappartionFight;
            }
            else //sinon on apparait au point d'apparition de cette zones
            {
                positionReappartion = positionDebut[4];
            }
        }
        transform.position = positionReappartion; // nous fait apparaitre au point de réaparition désigné

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //si deplacement est possible
        if (mouvementPossible == true) { 
        // D�placement du personnage
        //on va chercher les valeurs de l'axe vertical et horizontal.
        float axeHorizontal = Input.GetAxisRaw("Horizontal");
        float axeVertical = Input.GetAxisRaw("Vertical");

    
        //systeme pour empecher le personnage voler suite � un bug de collision
        //on va chercher sa velocity.y 
        float VelocityY = GetComponent<Rigidbody>().velocity.y;
        float VelocityX = GetComponent<Rigidbody>().velocity.x;
        float VelocityZ = GetComponent<Rigidbody>().velocity.z;

            //si sa velocity.y monte � 1 la ramene � 0
            if (VelocityY > 1)
        {
            VelocityY = 0;
        }

            if (VelocityX > 0 || VelocityZ > 0 || VelocityX < 0 || VelocityZ < 0)
            {
                if(audioMarche != true)
                {
                    Invoke("SonDeDeplacement", 0f);
                    Invoke("audioMarcheDevientFalse", 1f);
                }
                
                
            }

        //deplacement personnage
        GetComponent<Rigidbody>().velocity = new Vector3(axeHorizontal, VelocityY, axeVertical).normalized * vitesse;
        
        // gestion de toute les animations du personnage

        //idle cote
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {

            GetComponent<Animator>().SetBool("avant", false);
            GetComponent<Animator>().SetBool("cote", false);
            GetComponent<Animator>().SetBool("coteIdle", true);
            GetComponent<Animator>().SetBool("cul", false);
            GetComponent<SpriteRenderer>().flipX = false;

        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            
            GetComponent<Animator>().SetBool("cote", true);
            GetComponent<Animator>().SetBool("avant", false);
            GetComponent<Animator>().SetBool("cul", false);
            GetComponent<Animator>().SetBool("coteIdle", false);
            GetComponent<SpriteRenderer>().flipX = false;

        }
        //anim cote
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {

            GetComponent<Animator>().SetBool("avant", false);
            GetComponent<Animator>().SetBool("cote", false);
            GetComponent<Animator>().SetBool("cul", false);
            GetComponent<Animator>().SetBool("coteIdle", true);
            GetComponent<SpriteRenderer>().flipX = true;

        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {

            GetComponent<Animator>().SetBool("cote", true);
            GetComponent<Animator>().SetBool("avant", false);
            GetComponent<Animator>().SetBool("cul", false);
            GetComponent<Animator>().SetBool("coteIdle", false);
            GetComponent<SpriteRenderer>().flipX = true;
        }
        //anim back
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {

            GetComponent<Animator>().SetBool("cul", true);
            GetComponent<Animator>().SetBool("cote", false);
            GetComponent<Animator>().SetBool("avant", false);
            GetComponent<Animator>().SetBool("coteIdle", false);

            }
        //anim avant
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            
            GetComponent<Animator>().SetBool("avant", true);
            GetComponent<Animator>().SetBool("cote", false);
            GetComponent<Animator>().SetBool("cul", false);
            GetComponent<Animator>().SetBool("coteIdle", false);

            }


        }







        
}
    //audio du deplacement du personnage
    void SonDeDeplacement()
    {
        GetComponent<AudioSource>().PlayOneShot(sonMarche);
        audioMarche = true;
    }
    void audioMarcheDevientFalse()
    {
        audioMarche = false;
    }
}
