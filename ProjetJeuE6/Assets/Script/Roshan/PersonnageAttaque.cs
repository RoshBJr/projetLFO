using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Programmeur Roshan
public class PersonnageAttaque : MonoBehaviour
{
    public SpriteRenderer materielCouleur; // variable permettant d'attribuer un le mesh renderer d'un pfPersonnange de couleur au prefabs personnage
    private Animation attaque; // variable ref � l'animation d'attaque
    private Etat etatPersonnage; // variable pour stocker les diff�rent �tat du personnage actif
    private Vector3 avancePositionCible; // V3 pour stocker la position de la cible � attaquer
    public Action avancementTerminer; // Action pour �couter lorsque l'attaque du joueur est termin�
    public static bool joueurDefend; // bool pour savoir si le joueur se d�fend
    public bool estVivant; // bool pour savoir si le personnage actif est vivant
    public static bool coroutineActif = true; // bool pour savoir si le joueur joue l'animation d'attaque
    //private Text texte;
    private static int nbTour = 0; // int pour savoir depuis combien de tour le joueur se d�fend lorsqu'il utilise la potion de d�fense
    public static bool tours; // bool pour savoir si le joueur a utiliser une potion de d�fense

    private enum Etat // l'�num�ration des diff�rents �tats du joueur
    {
        idle, // idle lorsque le joueur est en repos
        avance, // avance lorsque le joueur fait une attaque
        occupe, // occupe lorsque le joueur fait une attaque ou prends une potion
    }


    private void Start()
    {
        materielCouleur = GetComponent<SpriteRenderer>(); // attribue le mesh renderer du pfPersonnage pour acc�der et modifier le mat�riel
        attaque = GetComponent<Animation>(); // va chercher le component Animation du personnage
        etatPersonnage = Etat.idle; // au d�but le joueur est en �tat d'idle
        estVivant = true; // les personnages sont vivant
    }

    private void FixedUpdate()
    {

        switch (etatPersonnage) // cette fonction nous permet de changer des diff�rents �tats et de faire une action si n�cessaire
        {
            case Etat.idle: // ne fait rien dans l'�tat idle
                break;
            case Etat.occupe: // ne fait rien dans l'�tat occup�
                break;
            case Etat.avance: // fait quelque chose dans l'�tat avance
                float avanceVitesse = 2.5f; // ce float d�termine la vitesse � laquelle le personnage va avancer vers la cible
                transform.position += (avancePositionCible - GetPosition()) * avanceVitesse * Time.deltaTime; // on fait avancer le personnage vers la cible avec une vitesse de 2f

                float distanceArrivee = 0.3f; // float permettant de comparer la distance entre le joueur et la cible
                if(Vector3.Distance(GetPosition(), avancePositionCible) < distanceArrivee) // si la distance entre la cible et le joueur est plus petit que 1f
                                                                                           // le joueur est arriv�e � la position de la cible
                {
                    transform.position = avancePositionCible; // la position du joueur est maintenant �gal � celle de la cible
                    if (UICombatAfficherDegatSoin.estActif == false) // lorsque le joueur � fini son animation d'attaque on active l'action suivante
                    {
                        avancementTerminer(); // cette action permet de surveiller lorsque le joueur est arriv� � la cible pour retourner � sa position de d�part
                        
                    }
                    else if(SystemeCombat.ennemiPersonnageAttaque == SystemeCombat.actifPersonnageAttaque) // lorsque l'ennemi � fini son animation d'attaque on active l'action suivante
                    {
                        avancementTerminer(); // cette action permet de surveiller lorsque l'ennemi est arriv� � la cible pour retourner � sa position de d�part
                    }
                    
                }

                break;
        }
    }

    /* -------------- Fonction permettant d'attribuer un mat�riel de couleur � un personnage au combat (rouge -> ennemi, vert -> joueur) ------------------------------- */
    public void personnageInstanceApparence(bool estJoueur) // bool permettant de d�terminer le joueur de l'ennemi
    {
        Vector3 positionV3Texte;
        /* ----------------------------------------- si le joueur spawn, ce mat�riel de couleur sera utilis� -------------------------------------------------------- */
        if (estJoueur)
        {
            materielCouleur.sprite = SystemeCombat.ChoisirInstance().joueurMaterielCouleur;
            positionV3Texte = new Vector3(130, -50, 0);
            
        }
        /* ----------------------------------------- si l'ennemi spawn, ce mat�riel de couleur sera utilis� -------------------------------------------------------- */
        else
        {
            if (GestionCollisionPerso.fightBoss)
            {
                materielCouleur.sprite = SystemeCombat.ChoisirInstance().ennemiMaterielCouleur[2];
                positionV3Texte = new Vector3(-130, -50, 0);
            }
            else
            {
                int chiffrePourSpawnEnnemiRando = UnityEngine.Random.Range(0, 2); // va choisir un chiffre entre 0 et 1 qui contient les deux diff�rents ennemis de bases
                materielCouleur.sprite = SystemeCombat.ChoisirInstance().ennemiMaterielCouleur[chiffrePourSpawnEnnemiRando]; // va prendre l'ennemiMaterielCouleur du chiffre g�n�r� en haut
                positionV3Texte = new Vector3(-130, -50, 0);
            }
            
        }


    }

    public Vector3 GetPosition() // ceci est un V3 nous permettant de conserver en param�tre la position d'un objet dans le monde
    {
        return (transform.position); // le V3 GetPosition() retourne la position vectorielle d'un objet
    }

   
    // si le joueur se soigne
    public void Soigner(SystemeVie systemeVieJ, Action actionCompleter, Action actionImpossible)
    {
        if (GestionInventairePerso.potionsSoin > 0) // si potion soin est sup�rieur � 0
        {
            int quantiteSoin = 80; // atrribue une quantit� de soin de 60 points
            GestionInventairePerso.UtiliserPotionsSoin(); // on enl�ve une potion de soin dans notre inventaire
            systemeVieJ.Soigner(quantiteSoin); // on soigne le joueur de 60 points
            StartCoroutine(UICombatAfficherDegatSoin.ApparaitreImage("Soin: " + quantiteSoin)); // on affiche dans le UI de combat la quantit� de soin
            StartCoroutine(UICombatAfficherDegatSoin.DisparaitreImage()); // on fait disparaitre la quantit� de soin dans le UI combat
            actionCompleter(); // apr�s que tout cela soit compl�t�, on active cette action pour passer au tour de l'ennemi
        }
        else // sinon...
        {
            StartCoroutine(UICombatAfficherDegatSoin.ApparaitreImage("Potion Soin Insuffisante")); // on affiche dans le UI de combat que la quantit� de potion est insuffisante
            StartCoroutine(UICombatAfficherDegatSoin.DisparaitreImage()); // on fait disparaitre le message dans le UI combat
            actionImpossible();  // apr�s que tout cela soit compl�t�, on active cette action pour retourner l'�tat du joueur en �tat d'attente de commande
        }
    }

    // si le joueur se d�fends
    public void Defense(bool boolDefend, Action actionCompleter)
    {
        StartCoroutine(UICombatAfficherDegatSoin.ApparaitreImage("Defends")); // on affiche dans le UI de combat le message que le joueur va se d�fendre contre la prochaine attaque de l'ennemi
        StartCoroutine(UICombatAfficherDegatSoin.DisparaitreImage()); // on fait disparaitre le message dans le UI combat
        actionCompleter(); // apr�s que tout cela soit compl�t�, on active cette action pour passer au tour de l'ennemi
    }

    // si je joueur utilise la potion de d�fense
    public void PotionDefense(bool boolDefend, Action actionCompleter, Action actionImpossible)
    {
        if (GestionInventairePerso.potionsDefense > 0) // si potion de d�fense en inventaire est sup�rieur � 0
        {
            GestionInventairePerso.UtiliserPotionDefense(); // on enl�ve une potion de d�fense dans notre inventaire
            StartCoroutine(UICombatAfficherDegatSoin.ApparaitreImage("Defends pour 3 tours")); // on affiche dans le UI de combat le message que le joueur va prendre moins de d�gat pour les trois prochains tours
            StartCoroutine(UICombatAfficherDegatSoin.DisparaitreImage()); // on fait disparaitre le message dans le UI combat 
            tours = true; // on active le bool que le joueur utilise la potion de d�fense pour calculer lorsque sa fait 3 tours pour le retourner � false
            actionCompleter(); // apr�s que tout cela soit compl�t�, on active cette action pour passer au tour de l'ennemi
        }
        else
        {
            StartCoroutine(UICombatAfficherDegatSoin.ApparaitreImage("Potion D�fense Insuffisante")); // on affiche dans le UI de combat que la quantit� de potion est insuffisante
            StartCoroutine(UICombatAfficherDegatSoin.DisparaitreImage()); // on fait disparaitre le message dans le UI combat
            actionImpossible(); // apr�s que tout cela soit compl�t�, on active cette action pour retourner l'�tat du joueur en �tat d'attente de commande
        }

    }

    // void si le joueur utilise la potion d'attaque
    public void PotionAtk(SystemeVie systemeVieEnnemi, Action actionCompleter, Action actionImpossible)
    {
        if(GestionInventairePerso.potionsAttaque > 0) // si potion d'attaque en inventaire est sup�rieur � 0
        {
            int degatPotionAtk = 30; // atrribue une quantit� de d�gat de 30 points
            GestionInventairePerso.UtiliserPotionsAttaque(); // on enl�ve une potion d'attaque dans notre inventaire
            systemeVieEnnemi.PotionAtk(degatPotionAtk); // on inflige des d�gats sur l'ennemi de 30 points
            StartCoroutine(UICombatAfficherDegatSoin.ApparaitreImage("D�gat: " + degatPotionAtk)); // on affiche dans le UI de combat la quantit� de d�gat � infliger � l'ennemi
            StartCoroutine(UICombatAfficherDegatSoin.DisparaitreImage()); // on fait disparaitre la quantit� de d�gat � infliger sur l'ennemi dans le UI combat
            if (systemeVieEnnemi.initialiserVie() <= 0) // si la vie de l'ennemi est plus petit ou �gal � 0
            {
                estVivant = false; // on retourne false au bool estVivant pour changer de sc�ne
            }
            actionCompleter(); // apr�s que tout cela soit compl�t�, on active cette action pour soit passer � l'ennemi ou le changement de sc�ne si l'ennemi meurt
        }
        else
        {
            StartCoroutine(UICombatAfficherDegatSoin.ApparaitreImage("Potion d'atk Insuffisante")); // on affiche dans le UI de combat que la quantit� de potion d'attaque est insuffisante
            StartCoroutine(UICombatAfficherDegatSoin.DisparaitreImage()); // on fait disparaitre le message dans le UI combat
            actionImpossible(); // apr�s que tout cela soit compl�t�, on active cette action pour retourner l'�tat du joueur en �tat d'attente de commande
        }
        
    }

    /* ---------------------------------- Cette fonction permet au joueur de cliquer sur un bouton et de faire attaquer le personnage ----------------------------- */
    public void Attack(PersonnageAttaque ciblePersonnageAttaque, SystemeVie systemeViePersonnage, Action attaqueCompleter) // prends en param�tre la cible � attaquer
    {
        GetComponent<Animator>().SetBool("marche", true); // lorsque le joueur avance vers la position de l'ennemi l'animation de marche est true
        if(SystemeCombat.joueurPersonnageAttaque == SystemeCombat.actifPersonnageAttaque) // si le personnage actif est le joueur
        {
            StartCoroutine(UICombatAfficherDegatSoin.Attaquer(GetComponent<Animator>())); // on commence la coroutine d'attaque avec l'animator du joueur en param�tre
            StartCoroutine(UICombatAfficherDegatSoin.Attendre(coroutineActif)); // apr�s, on lance la coroutine pour attendre sur l'ennemi et ex�cuter l'attaque
            UICombatAfficherDegatSoin.estActif = true; // apr�s que les coroutines soit termin�es on  retourne le bool estActif pour true lorsque l'anim d'attaque est fini
        }
        
        Vector3 positionDepartJoueur = GetPosition(); // position de d�part du joueur
        Vector3 directionAttaque = (ciblePersonnageAttaque.GetPosition() - GetPosition()).normalized; // ici on veut d�terminer la direction du personnage par rapport �
                                                                                                      // l'ennemi en question, donc on soustrait la position du joueur �
                                                                                                      // la cible et applique un normalized pour soit retourner un 1 ou -1

        AvancerALaPosition(ciblePersonnageAttaque.GetPosition(), () => { // lorsque que le joueur est arriv� � la cible il peut lancer l'attaque
            etatPersonnage = Etat.occupe; // l'�tat du joueur passe en occup� ce qui emp�che le joueur de lancer d'autres commandes
            int quantiteDegatAtkJoueur = UnityEngine.Random.Range(20, 30); // quantit� de d�gat que le joueur inflige sur l'ennemi lorsqu'il fait l'attaque de base
            int quantiteDegatAtkEnnemi = UnityEngine.Random.Range(20, 50); // quantit� que l'ennemi inflige sur le joueur lorsqu'il attaque
            if (directionAttaque.x == -1) // si la direction d'attaque est vers le joueur on joue l'animation d'attaque de l'ennemi
            {
                GetComponent<AudioSource>().Play();
                systemeViePersonnage.Degat(quantiteDegatAtkEnnemi, joueurDefend); // l'ennemi inflige les d�gats au joueur
                
                if (joueurDefend && tours) // si le joueur d�fend et qu'il utilise la potion de d�fense
                {
                    nbTour++; // on augmente le nombre de tour plus 1
                    //print("tour : " + nbTour);
                    StartCoroutine(UICombatAfficherDegatSoin.ApparaitreImage("D�gat : " + quantiteDegatAtkEnnemi / 2)); // on affiche dans le UI de combat les d�gats inflig� r�duit � moiti�
                    StartCoroutine(UICombatAfficherDegatSoin.DisparaitreImage()); // on fait disparaitre le message dans le UI combat
                    if (nbTour == 3) // si le nombre de tour lorsque le joueur avait utilis� la potion de d�fense est �gal � 3 tours
                    {
                        nbTour = 0; // le nombre de tour est retourn� � 0 pour �tre en mesure de lancer une potion de d�fense
                        tours = false; // on retourne le bool permettant de savoir lorsqu'on avait utilis� la potion de d�fense � false
                    }
                }
                else if (joueurDefend) // sinon, si le joueur avait cliqu� sur le bouton de d�fense
                {
                  
                    StartCoroutine(UICombatAfficherDegatSoin.ApparaitreImage("D�gat : " + quantiteDegatAtkEnnemi/2)); // on affiche dans le UI de combat les d�gats inflig� r�duit � moiti�
                    StartCoroutine(UICombatAfficherDegatSoin.DisparaitreImage()); // on fait disparaitre le message dans le UI combat
                }
                else // sinon, le joueur ne se d�fends pas alors il re�oit les d�gats dans son enti�ret�
                {
                    
                    StartCoroutine(UICombatAfficherDegatSoin.ApparaitreImage("D�gat : " + quantiteDegatAtkEnnemi));  // on affiche dans le UI de combat les d�gats inflig�
                    StartCoroutine(UICombatAfficherDegatSoin.DisparaitreImage()); // on fait disparaitre le message dans le UI combat
                }
                
                if (systemeViePersonnage.initialiserVie() <= 0) // si la vie du joueur est plus petit ou �gal � 0
                {
                    estVivant = false; // on retourne le bool estVivant � false pour changer de sc�ne � celle de mort
                }

            }
            else // le joueur va infliger des d�gats
            {
                GetComponent<AudioSource>().Play();
                systemeViePersonnage.Degat(quantiteDegatAtkJoueur, joueurDefend); // le joueur inflige des d�gats sur l'ennemi
                StartCoroutine(UICombatAfficherDegatSoin.ApparaitreImage("D�gat: " + quantiteDegatAtkJoueur)); // on affiche dans le UI de combat les d�gats inflig�
                StartCoroutine(UICombatAfficherDegatSoin.DisparaitreImage()); // on fait disparaitre le message dans le UI combat
                if (systemeViePersonnage.initialiserVie() <= 0) // si la vie de l'ennemi est plus petit ou �gal � 0
                {
                    estVivant = false; // on retourne le bool estVivant � false pour changer de sc�ne � celle pour retourner � la map
                }
            }


            AvancerALaPosition(positionDepartJoueur, () => { // lorsque l'attaque est termin� on retourner l' �tat du joueur en repos
                etatPersonnage = Etat.idle; // on remet l'�tat du joueur en repos
                attaqueCompleter(); // Action permettant de d�terminer lorsque l'animation d'attaque est termin�e pour ainsi remettre l'etat du joueur �
                                    // attendsCommandeJoueur dans la class systemeCombat
                GetComponent<Animator>().SetBool("marche", false); // lorsque le joueur arrive � sa position de d�part on d�sactive l'animation de marche

                if (joueurDefend && tours == false) // si le joueur se d�fend et et que la potion de d�fense n'est pas active
                {
                    joueurDefend = false; // on retourne le bool joueurDefend � false pour que le joueur puisse d�fendre au prochain tour
                }
            });
            
        });
    }


    /* Cette fonction permet de d�terminer la position de la cible et de savoir lorsque l'avancement du joueur est fini  
     * et que tant que l'avancement n'est pas termin� l'�tat du joueur reste en avancement ---------- */
    private void AvancerALaPosition(Vector3 avancePositionCible, Action avancementTerminer)
    {
        this.avancePositionCible = avancePositionCible; // this.avancePositionCible est l'instance de la position de la cible d�terminer dans le FixedUpdate,
                                                        // donc on attribue le this.avancePositionCible => ciblePersonnageAttaque.GetPosition()
        this.avancementTerminer = avancementTerminer; // tant que avancementTerminer est diff�rent de true l'avancement continue
        etatPersonnage = Etat.avance; // on garde L'�tat du joueur � avance jusqu'a le joueur arrive sur la cible
    }

}
