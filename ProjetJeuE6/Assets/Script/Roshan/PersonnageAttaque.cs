using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Programmeur Roshan
public class PersonnageAttaque : MonoBehaviour
{
    public SpriteRenderer materielCouleur; // variable permettant d'attribuer un le mesh renderer d'un pfPersonnange de couleur au prefabs personnage
    private Animation attaque; // variable ref à l'animation d'attaque
    private Etat etatPersonnage; // variable pour stocker les différent état du personnage actif
    private Vector3 avancePositionCible; // V3 pour stocker la position de la cible à attaquer
    public Action avancementTerminer; // Action pour écouter lorsque l'attaque du joueur est terminé
    public static bool joueurDefend; // bool pour savoir si le joueur se défend
    public bool estVivant; // bool pour savoir si le personnage actif est vivant
    public static bool coroutineActif = true; // bool pour savoir si le joueur joue l'animation d'attaque
    //private Text texte;
    private static int nbTour = 0; // int pour savoir depuis combien de tour le joueur se défend lorsqu'il utilise la potion de défense
    public static bool tours; // bool pour savoir si le joueur a utiliser une potion de défense

    private enum Etat // l'énumération des différents états du joueur
    {
        idle, // idle lorsque le joueur est en repos
        avance, // avance lorsque le joueur fait une attaque
        occupe, // occupe lorsque le joueur fait une attaque ou prends une potion
    }


    private void Start()
    {
        materielCouleur = GetComponent<SpriteRenderer>(); // attribue le mesh renderer du pfPersonnage pour accéder et modifier le matériel
        attaque = GetComponent<Animation>(); // va chercher le component Animation du personnage
        etatPersonnage = Etat.idle; // au début le joueur est en état d'idle
        estVivant = true; // les personnages sont vivant
    }

    private void FixedUpdate()
    {

        switch (etatPersonnage) // cette fonction nous permet de changer des différents états et de faire une action si nécessaire
        {
            case Etat.idle: // ne fait rien dans l'état idle
                break;
            case Etat.occupe: // ne fait rien dans l'état occupé
                break;
            case Etat.avance: // fait quelque chose dans l'état avance
                float avanceVitesse = 2.5f; // ce float détermine la vitesse à laquelle le personnage va avancer vers la cible
                transform.position += (avancePositionCible - GetPosition()) * avanceVitesse * Time.deltaTime; // on fait avancer le personnage vers la cible avec une vitesse de 2f

                float distanceArrivee = 0.3f; // float permettant de comparer la distance entre le joueur et la cible
                if(Vector3.Distance(GetPosition(), avancePositionCible) < distanceArrivee) // si la distance entre la cible et le joueur est plus petit que 1f
                                                                                           // le joueur est arrivée à la position de la cible
                {
                    transform.position = avancePositionCible; // la position du joueur est maintenant égal à celle de la cible
                    if (UICombatAfficherDegatSoin.estActif == false) // lorsque le joueur à fini son animation d'attaque on active l'action suivante
                    {
                        avancementTerminer(); // cette action permet de surveiller lorsque le joueur est arrivé à la cible pour retourner à sa position de départ
                        
                    }
                    else if(SystemeCombat.ennemiPersonnageAttaque == SystemeCombat.actifPersonnageAttaque) // lorsque l'ennemi à fini son animation d'attaque on active l'action suivante
                    {
                        avancementTerminer(); // cette action permet de surveiller lorsque l'ennemi est arrivé à la cible pour retourner à sa position de départ
                    }
                    
                }

                break;
        }
    }

    /* -------------- Fonction permettant d'attribuer un matériel de couleur à un personnage au combat (rouge -> ennemi, vert -> joueur) ------------------------------- */
    public void personnageInstanceApparence(bool estJoueur) // bool permettant de déterminer le joueur de l'ennemi
    {
        Vector3 positionV3Texte;
        /* ----------------------------------------- si le joueur spawn, ce matériel de couleur sera utilisé -------------------------------------------------------- */
        if (estJoueur)
        {
            materielCouleur.sprite = SystemeCombat.ChoisirInstance().joueurMaterielCouleur;
            positionV3Texte = new Vector3(130, -50, 0);
            
        }
        /* ----------------------------------------- si l'ennemi spawn, ce matériel de couleur sera utilisé -------------------------------------------------------- */
        else
        {
            if (GestionCollisionPerso.fightBoss)
            {
                materielCouleur.sprite = SystemeCombat.ChoisirInstance().ennemiMaterielCouleur[2];
                positionV3Texte = new Vector3(-130, -50, 0);
            }
            else
            {
                int chiffrePourSpawnEnnemiRando = UnityEngine.Random.Range(0, 2); // va choisir un chiffre entre 0 et 1 qui contient les deux différents ennemis de bases
                materielCouleur.sprite = SystemeCombat.ChoisirInstance().ennemiMaterielCouleur[chiffrePourSpawnEnnemiRando]; // va prendre l'ennemiMaterielCouleur du chiffre généré en haut
                positionV3Texte = new Vector3(-130, -50, 0);
            }
            
        }


    }

    public Vector3 GetPosition() // ceci est un V3 nous permettant de conserver en paramètre la position d'un objet dans le monde
    {
        return (transform.position); // le V3 GetPosition() retourne la position vectorielle d'un objet
    }

   
    // si le joueur se soigne
    public void Soigner(SystemeVie systemeVieJ, Action actionCompleter, Action actionImpossible)
    {
        if (GestionInventairePerso.potionsSoin > 0) // si potion soin est supérieur à 0
        {
            int quantiteSoin = 80; // atrribue une quantité de soin de 60 points
            GestionInventairePerso.UtiliserPotionsSoin(); // on enlève une potion de soin dans notre inventaire
            systemeVieJ.Soigner(quantiteSoin); // on soigne le joueur de 60 points
            StartCoroutine(UICombatAfficherDegatSoin.ApparaitreImage("Soin: " + quantiteSoin)); // on affiche dans le UI de combat la quantité de soin
            StartCoroutine(UICombatAfficherDegatSoin.DisparaitreImage()); // on fait disparaitre la quantité de soin dans le UI combat
            actionCompleter(); // après que tout cela soit complèté, on active cette action pour passer au tour de l'ennemi
        }
        else // sinon...
        {
            StartCoroutine(UICombatAfficherDegatSoin.ApparaitreImage("Potion Soin Insuffisante")); // on affiche dans le UI de combat que la quantité de potion est insuffisante
            StartCoroutine(UICombatAfficherDegatSoin.DisparaitreImage()); // on fait disparaitre le message dans le UI combat
            actionImpossible();  // après que tout cela soit complèté, on active cette action pour retourner l'état du joueur en état d'attente de commande
        }
    }

    // si le joueur se défends
    public void Defense(bool boolDefend, Action actionCompleter)
    {
        StartCoroutine(UICombatAfficherDegatSoin.ApparaitreImage("Defends")); // on affiche dans le UI de combat le message que le joueur va se défendre contre la prochaine attaque de l'ennemi
        StartCoroutine(UICombatAfficherDegatSoin.DisparaitreImage()); // on fait disparaitre le message dans le UI combat
        actionCompleter(); // après que tout cela soit complèté, on active cette action pour passer au tour de l'ennemi
    }

    // si je joueur utilise la potion de défense
    public void PotionDefense(bool boolDefend, Action actionCompleter, Action actionImpossible)
    {
        if (GestionInventairePerso.potionsDefense > 0) // si potion de défense en inventaire est supérieur à 0
        {
            GestionInventairePerso.UtiliserPotionDefense(); // on enlève une potion de défense dans notre inventaire
            StartCoroutine(UICombatAfficherDegatSoin.ApparaitreImage("Defends pour 3 tours")); // on affiche dans le UI de combat le message que le joueur va prendre moins de dégat pour les trois prochains tours
            StartCoroutine(UICombatAfficherDegatSoin.DisparaitreImage()); // on fait disparaitre le message dans le UI combat 
            tours = true; // on active le bool que le joueur utilise la potion de défense pour calculer lorsque sa fait 3 tours pour le retourner à false
            actionCompleter(); // après que tout cela soit complèté, on active cette action pour passer au tour de l'ennemi
        }
        else
        {
            StartCoroutine(UICombatAfficherDegatSoin.ApparaitreImage("Potion Défense Insuffisante")); // on affiche dans le UI de combat que la quantité de potion est insuffisante
            StartCoroutine(UICombatAfficherDegatSoin.DisparaitreImage()); // on fait disparaitre le message dans le UI combat
            actionImpossible(); // après que tout cela soit complèté, on active cette action pour retourner l'état du joueur en état d'attente de commande
        }

    }

    // void si le joueur utilise la potion d'attaque
    public void PotionAtk(SystemeVie systemeVieEnnemi, Action actionCompleter, Action actionImpossible)
    {
        if(GestionInventairePerso.potionsAttaque > 0) // si potion d'attaque en inventaire est supérieur à 0
        {
            int degatPotionAtk = 30; // atrribue une quantité de dégat de 30 points
            GestionInventairePerso.UtiliserPotionsAttaque(); // on enlève une potion d'attaque dans notre inventaire
            systemeVieEnnemi.PotionAtk(degatPotionAtk); // on inflige des dégats sur l'ennemi de 30 points
            StartCoroutine(UICombatAfficherDegatSoin.ApparaitreImage("Dégat: " + degatPotionAtk)); // on affiche dans le UI de combat la quantité de dégat à infliger à l'ennemi
            StartCoroutine(UICombatAfficherDegatSoin.DisparaitreImage()); // on fait disparaitre la quantité de dégat à infliger sur l'ennemi dans le UI combat
            if (systemeVieEnnemi.initialiserVie() <= 0) // si la vie de l'ennemi est plus petit ou égal à 0
            {
                estVivant = false; // on retourne false au bool estVivant pour changer de scène
            }
            actionCompleter(); // après que tout cela soit complèté, on active cette action pour soit passer à l'ennemi ou le changement de scène si l'ennemi meurt
        }
        else
        {
            StartCoroutine(UICombatAfficherDegatSoin.ApparaitreImage("Potion d'atk Insuffisante")); // on affiche dans le UI de combat que la quantité de potion d'attaque est insuffisante
            StartCoroutine(UICombatAfficherDegatSoin.DisparaitreImage()); // on fait disparaitre le message dans le UI combat
            actionImpossible(); // après que tout cela soit complèté, on active cette action pour retourner l'état du joueur en état d'attente de commande
        }
        
    }

    /* ---------------------------------- Cette fonction permet au joueur de cliquer sur un bouton et de faire attaquer le personnage ----------------------------- */
    public void Attack(PersonnageAttaque ciblePersonnageAttaque, SystemeVie systemeViePersonnage, Action attaqueCompleter) // prends en paramètre la cible à attaquer
    {
        GetComponent<Animator>().SetBool("marche", true); // lorsque le joueur avance vers la position de l'ennemi l'animation de marche est true
        if(SystemeCombat.joueurPersonnageAttaque == SystemeCombat.actifPersonnageAttaque) // si le personnage actif est le joueur
        {
            StartCoroutine(UICombatAfficherDegatSoin.Attaquer(GetComponent<Animator>())); // on commence la coroutine d'attaque avec l'animator du joueur en paramètre
            StartCoroutine(UICombatAfficherDegatSoin.Attendre(coroutineActif)); // après, on lance la coroutine pour attendre sur l'ennemi et exécuter l'attaque
            UICombatAfficherDegatSoin.estActif = true; // après que les coroutines soit terminées on  retourne le bool estActif pour true lorsque l'anim d'attaque est fini
        }
        
        Vector3 positionDepartJoueur = GetPosition(); // position de départ du joueur
        Vector3 directionAttaque = (ciblePersonnageAttaque.GetPosition() - GetPosition()).normalized; // ici on veut déterminer la direction du personnage par rapport à
                                                                                                      // l'ennemi en question, donc on soustrait la position du joueur à
                                                                                                      // la cible et applique un normalized pour soit retourner un 1 ou -1

        AvancerALaPosition(ciblePersonnageAttaque.GetPosition(), () => { // lorsque que le joueur est arrivé à la cible il peut lancer l'attaque
            etatPersonnage = Etat.occupe; // l'état du joueur passe en occupé ce qui empêche le joueur de lancer d'autres commandes
            int quantiteDegatAtkJoueur = UnityEngine.Random.Range(20, 30); // quantité de dégat que le joueur inflige sur l'ennemi lorsqu'il fait l'attaque de base
            int quantiteDegatAtkEnnemi = UnityEngine.Random.Range(20, 50); // quantité que l'ennemi inflige sur le joueur lorsqu'il attaque
            if (directionAttaque.x == -1) // si la direction d'attaque est vers le joueur on joue l'animation d'attaque de l'ennemi
            {
                GetComponent<AudioSource>().Play();
                systemeViePersonnage.Degat(quantiteDegatAtkEnnemi, joueurDefend); // l'ennemi inflige les dégats au joueur
                
                if (joueurDefend && tours) // si le joueur défend et qu'il utilise la potion de défense
                {
                    nbTour++; // on augmente le nombre de tour plus 1
                    //print("tour : " + nbTour);
                    StartCoroutine(UICombatAfficherDegatSoin.ApparaitreImage("Dégat : " + quantiteDegatAtkEnnemi / 2)); // on affiche dans le UI de combat les dégats infligé réduit à moitié
                    StartCoroutine(UICombatAfficherDegatSoin.DisparaitreImage()); // on fait disparaitre le message dans le UI combat
                    if (nbTour == 3) // si le nombre de tour lorsque le joueur avait utilisé la potion de défense est égal à 3 tours
                    {
                        nbTour = 0; // le nombre de tour est retourné à 0 pour être en mesure de lancer une potion de défense
                        tours = false; // on retourne le bool permettant de savoir lorsqu'on avait utilisé la potion de défense à false
                    }
                }
                else if (joueurDefend) // sinon, si le joueur avait cliqué sur le bouton de défense
                {
                  
                    StartCoroutine(UICombatAfficherDegatSoin.ApparaitreImage("Dégat : " + quantiteDegatAtkEnnemi/2)); // on affiche dans le UI de combat les dégats infligé réduit à moitié
                    StartCoroutine(UICombatAfficherDegatSoin.DisparaitreImage()); // on fait disparaitre le message dans le UI combat
                }
                else // sinon, le joueur ne se défends pas alors il reçoit les dégats dans son entièreté
                {
                    
                    StartCoroutine(UICombatAfficherDegatSoin.ApparaitreImage("Dégat : " + quantiteDegatAtkEnnemi));  // on affiche dans le UI de combat les dégats infligé
                    StartCoroutine(UICombatAfficherDegatSoin.DisparaitreImage()); // on fait disparaitre le message dans le UI combat
                }
                
                if (systemeViePersonnage.initialiserVie() <= 0) // si la vie du joueur est plus petit ou égal à 0
                {
                    estVivant = false; // on retourne le bool estVivant à false pour changer de scène à celle de mort
                }

            }
            else // le joueur va infliger des dégats
            {
                GetComponent<AudioSource>().Play();
                systemeViePersonnage.Degat(quantiteDegatAtkJoueur, joueurDefend); // le joueur inflige des dégats sur l'ennemi
                StartCoroutine(UICombatAfficherDegatSoin.ApparaitreImage("Dégat: " + quantiteDegatAtkJoueur)); // on affiche dans le UI de combat les dégats infligé
                StartCoroutine(UICombatAfficherDegatSoin.DisparaitreImage()); // on fait disparaitre le message dans le UI combat
                if (systemeViePersonnage.initialiserVie() <= 0) // si la vie de l'ennemi est plus petit ou égal à 0
                {
                    estVivant = false; // on retourne le bool estVivant à false pour changer de scène à celle pour retourner à la map
                }
            }


            AvancerALaPosition(positionDepartJoueur, () => { // lorsque l'attaque est terminé on retourner l' état du joueur en repos
                etatPersonnage = Etat.idle; // on remet l'état du joueur en repos
                attaqueCompleter(); // Action permettant de déterminer lorsque l'animation d'attaque est terminée pour ainsi remettre l'etat du joueur à
                                    // attendsCommandeJoueur dans la class systemeCombat
                GetComponent<Animator>().SetBool("marche", false); // lorsque le joueur arrive à sa position de départ on désactive l'animation de marche

                if (joueurDefend && tours == false) // si le joueur se défend et et que la potion de défense n'est pas active
                {
                    joueurDefend = false; // on retourne le bool joueurDefend à false pour que le joueur puisse défendre au prochain tour
                }
            });
            
        });
    }


    /* Cette fonction permet de déterminer la position de la cible et de savoir lorsque l'avancement du joueur est fini  
     * et que tant que l'avancement n'est pas terminé l'état du joueur reste en avancement ---------- */
    private void AvancerALaPosition(Vector3 avancePositionCible, Action avancementTerminer)
    {
        this.avancePositionCible = avancePositionCible; // this.avancePositionCible est l'instance de la position de la cible déterminer dans le FixedUpdate,
                                                        // donc on attribue le this.avancePositionCible => ciblePersonnageAttaque.GetPosition()
        this.avancementTerminer = avancementTerminer; // tant que avancementTerminer est différent de true l'avancement continue
        etatPersonnage = Etat.avance; // on garde L'état du joueur à avance jusqu'a le joueur arrive sur la cible
    }

}
