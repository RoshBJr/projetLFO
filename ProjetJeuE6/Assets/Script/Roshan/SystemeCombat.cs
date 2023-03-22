using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Programmeur Roshan
public class SystemeCombat : MonoBehaviour
{
    private static SystemeCombat instance; // variable static instance pour attribuer le bon mat�riel (joueurMaterielCouleur ou ennemiMaterielCouleur)
    public static SystemeCombat ChoisirInstance() // pseudo Fonction nous permmetant de choisir le bon mat�riel dans la m�thode PersonnageAttaque --> personnageInstanceApparence
                                                  // et de la retourner dans la variable instance
    {
        return instance; // variable qui prends en param�tre soit (joueurMaterielCouleur ou ennemiMaterielCouleur)
    }
    public Transform pfabPersonnageCombat; // prefab des personnages ennemis en sc�ne de combat
    public Transform pfabJoueurCombat; // prefab du joueur en sc�ne de combat
    public Sprite joueurMaterielCouleur; // mat�riel de couleur pour le joueur
    public Sprite[] ennemiMaterielCouleur; // mat�riel de couleur pour l'ennemi
    public static PersonnageAttaque joueurPersonnageAttaque; // ref pour d�signer le joueur et l'utiliser dans la class PersonnageAttaque
    public static PersonnageAttaque ennemiPersonnageAttaque; // ref pour d�signer l'ennemi et l'utiliser dans la class PersonnageAttaque
    public static PersonnageAttaque actifPersonnageAttaque; // ref pour d�signer � qui est le tour d'attaquer
    private Etat etatPersonnage; // variable pour stocker les diff�rent �tat du personnage actif
    public BarreVie barreVieJoueur; // cr�er une variable BarreVie pour le Joueur
    public BarreVie barreVieEnnemi; // cr�er une variable BarreVie pour l'ennemi
    public static SystemeVie systemeVieJoueur; // cr�er un Syst�meVie pour le joueur afin de lui attribuer une quantit� de vie
    private SystemeVie systemeVieEnnemi; // cr�er un Syst�meVie pour l'ennemi afin de lui attribuer une quantit� de vie
    private static int viePersonnage = 200; // une variable int ayant la quantit� de vie pour le joueur
    private int vieEnnemi; // une variable int ayant la quantit� de vie pour les ennemis de bases
    private int vieBossTerre; // une variable int ayant la quantit� de vie pour les boss
    private static GameObject boutonPotions; // GameObject qui contient les boutons affichant les potions
    private static GameObject containerBouton; // GameObject qui contient les boutons d'atk, d�fense et soin
    public static bool defendActive = false; // variable bool pour savoir si le joueur se d�fend


    private enum Etat // ceci est une �num�ration d'�tat pour controler les animations et les actions du joueur
    {
        attendsCommandeJoueur, // cet �tat permet au joueur de cliquer sur les boutons du UI pour lancer une animation ex: attaque d�fendre, etc
        Occupe, // cet �tat permet d'emp�cher les personnages de lancer plusieurs animation en m�me temps, apr�s qu'une animation sera termin�, l'�tat
                // occup� basculera � attendsCommandeJoueur et vice-versa, si le joueur est actif.
    }


    private void Awake()
    {
        containerBouton = GameObject.FindGameObjectWithTag("containerBouton"); // on va chercher le container des boutons dans la hierarchie pour controler son setActive
        boutonPotions = GameObject.FindGameObjectWithTag("potions"); // on va chercher le container des potions dans la hierarchie pour controler son setActive
        boutonPotions.SetActive(false); // au d�but, le container de potions est invisible
        instance = this; // applique la bonne instance de mat�riel dans l'awake
        GameObject refPourEnnemi = GameObject.Find("ennemiPourLesMap"); // ici on va chercher la r�f�rence des sprites ennemis dans la hierarchies
        ennemiMaterielCouleur = refPourEnnemi.GetComponent<Ennemis>().ennemiMaterielCouleur; // ici, on attribue � la variable ennemiMaterielCouleur � les sprites de l'ennemiMaterielCouleur
                                                                                             // de la refPourEnnemi
    }

    private void Start()
    {
        joueurPersonnageAttaque = SpawnPersonnage(true); // Spawn joueur
        ennemiPersonnageAttaque = SpawnPersonnage(false); // Spawn ennemi
        etatPersonnage = Etat.attendsCommandeJoueur; // Au d�but le joueur pourra lancer une animation / commande
        RendreActifLePersonnageAttaque(joueurPersonnageAttaque); // Le joueur a le premier tour en d�but de combat
        vieEnnemi = 50; // les ennemis de bases ont 50 points de vie
        vieBossTerre = 100; // les boss on 100 points de vie
        systemeVieJoueur = new SystemeVie(viePersonnage); // on initialise la quantit� de vie pour le joueur
        systemeVieEnnemi = new SystemeVie(vieEnnemi); // on initialise la quantit� de vie pour l'ennemi

        if (systemeVieJoueurHorsCombat.sortiCombat) // si le joueur sort d'un combat vivant...
        {
            systemeVieJoueur.Degat(systemeVieJoueurHorsCombat.degat, false); // on lui afflige des d�gats pour avoir la meme quantit� de vie lorsqu'il avait vaincu l'ennemi
            systemeVieJoueur.Soigner((systemeVieJoueurHorsCombat.vieHorsCombat.initialiserVie() - systemeVieJoueur.initialiserVie())); // ici, on initialise sa barre de vie pour l'affiche hors combat
            
        }

        barreVieJoueur.Initialiser(systemeVieJoueur); // ici on initialise la barre de vie du joueur pour l'affichage de la barre de vie
        barreVieEnnemi.Initialiser(systemeVieEnnemi); // ici on initialise la barre de vie de l'ennemi pour l'affichage de la barre de vie

        if (GestionCollisionPerso.fightBoss) // si le trigger combat du boss est true
        {
            systemeVieEnnemi = new SystemeVie(vieBossTerre); // on initialise la quantit� de vie pour le boss
            barreVieEnnemi.Initialiser(systemeVieEnnemi); // ici on initialise la barre de vie du boss pour l'affichage de la barre de vie
        }


    }


    /* ------------------------------------------------- Fonction permettant de spawn les personnages  --------------------------------------------------------------- */
    private PersonnageAttaque SpawnPersonnage(bool estJoueur) // bool permettant de savoir quel type de personnage on veut spawn (true -> joueur, false -> ennemi)
    {
        Vector3 positionV3Perso; // positon V3 qui va �tre chang� selon le personnage � spawn (joueur ou ennemis)
        Transform choisirBonPfPersonnage; // variable transform pour lui donner le bon prefab de personnage, soit ennemi ou joueur


        /* --------------------------------------------------- si le joueur spawn, cette position V3 sera utilis� ------------------------------------------------------- */
        if (estJoueur)
        {
            positionV3Perso = new Vector3(-6f, 2.05f, 0); // on donne cette position au joueur
            choisirBonPfPersonnage = pfabJoueurCombat; // donne le transform pfab du joueur
        }
        /* ------------------------------------------------- sinon le joueur est un ennemi alors cette position V3 sera utilis�  ----------------------------------+------ */
        else
        {
            positionV3Perso = new Vector3(6f, 2.05f, 0); // on donne cette position � l'ennemi
            choisirBonPfPersonnage = pfabPersonnageCombat; // donne le transform pfab des ennemis
        }

        Transform personnageTransform = Instantiate(choisirBonPfPersonnage, positionV3Perso, Quaternion.identity); // instance du Transform personnage � afficher
        PersonnageAttaque personnageAttaque = personnageTransform.GetComponent<PersonnageAttaque>(); // va chercher la ref du script PersonnageAttaque du pfPersonnage
        personnageAttaque.personnageInstanceApparence(estJoueur); // attribue une valeur true ou false � la fonction pesonnageInstanceApparence (true -> couleur vert, false -> couleur rouge)

        return personnageAttaque; // retourne les attribues du personnage qui � �t� spawn dans la variable personnageAttaque
    
    }

    /* ----------------------------------------------- si le bouton d'attaque du UI est cliqu� le joueur attaquera  ----------------------------------------------------------- */
    public void PersonnageAttaqueBouton() // fonction servant au UI bouton d'attaque pour utiliser dans le on click
    {
        if(etatPersonnage == Etat.attendsCommandeJoueur) // si le joueur est en attente d'une commande, le joueur peut lancer
                                                         // une commande, dans ce cas-ci une attaque
        {
            etatPersonnage = Etat.Occupe; // lorsque l'attaque est lanc� on bascule l'�tat du joueur � occup�


            joueurPersonnageAttaque.Attack(ennemiPersonnageAttaque, systemeVieEnnemi, () => { // le joueur attaque dans la direction de l'ennemi
                Invoke("ChoisirProchainPersonnageQuiAttaque", 1f);  // lorsque l'attaque du joueur est termin� on appelle cette fonction pour changer le tour � l'ennemi

            }); // appelle la fonction Attack et prends la direction de l'ennemi en param�tre et effectue une attaque vers cette direction, apr�s que l'attaque soit termin�
                // on retourne l'�tat du joueur vers attendsCommandeJoueur gr�ce � l'Action attaqueCompleter()
        }
        
    }

    /* void � attribuer aux boutons pour afficher les potions */
    public void PersonnageContainerPotionBouton()
    {
        if (etatPersonnage == Etat.attendsCommandeJoueur) // si le joueur est en attente d'une commande, le joueur peut lancer
                                                          // une commande, dans ce cas-ci afficher les potions
        {
            if(boutonPotions.activeSelf == false) // si le container des potions invisible...
            {
                boutonPotions.SetActive(true); // on active le container de potion
                containerBouton.SetActive(false); // on d�sactive le container des boutons
            }
            else // sinon...
            {
                boutonPotions.SetActive(false); // on d�sactive le container des potions
                containerBouton.SetActive(true); // on active le container des boutons
            }


        }
    }

    /* void � attribuer au bouton potion d'atk */
    public void PersonnagePotionAtk()
    {
        if (etatPersonnage == Etat.attendsCommandeJoueur)// si le joueur est en attente d'une commande, le joueur peut lancer
                                                         // une commande, dans ce cas-ci potions atk
        {
            etatPersonnage = Etat.Occupe; // on bascule l'�tat du joueur � occup� pour l'empecher de lancer d'autre commande

            joueurPersonnageAttaque.PotionAtk(systemeVieEnnemi, () => { // on inflige les d�gats du de la potion d'attaque sur l'ennemi
                Invoke("ChoisirProchainPersonnageQuiAttaque", 2f); // apr�s que l'action soit compl�t� on appelle ce void dans 2 seconde pour attendre que la boiteTexte fini de disparaitre et choisir le prochain personnage � attaquer
            }, () => { // sinon, si le joueur n'a pas une quantit� suffisante de potion d'attaque
                etatPersonnage = Etat.attendsCommandeJoueur; // le joueur ne peut pas lancer l'attaque alors on retourne l'�tat du joueur en attente de commande
            });
        }
    }


    /* void � attribuer au bouton potion de soin et bouton soin */
    public void PersonnageSoignerBouton()
    {
        if(etatPersonnage == Etat.attendsCommandeJoueur) // si le joueur est en attente d'une commande, le joueur peut lancer
                                                         // une commande, dans ce cas-ci potion de soin
        {
            etatPersonnage = Etat.Occupe; // on bascule l'�tat du joueur � occup� pour l'empecher de lancer d'autre commande

            joueurPersonnageAttaque.Soigner(systemeVieJoueur, () => { // on soigne le joueur
                Invoke("ChoisirProchainPersonnageQuiAttaque", 2f); // apr�s que l'action soit compl�t� on appelle ce void dans 2 seconde pour attendre que la boiteTexte fini de disparaitre et choisir le prochain personnage � attaquer
            }, () => { // sinon, si le joueur n'a pas une quantit� suffisante de potion de soin
                etatPersonnage = Etat.attendsCommandeJoueur; // le joueur ne peut pas se soigner alors on retourne l'�tat du joueur en attente de commande
            });
        }
    }

    /* void � attribuer au bouton potion de d�fense */
    public void PersonnagePotionDefenseBouton()
    {
        if (etatPersonnage == Etat.attendsCommandeJoueur) // si le joueur est en attente d'une commande, le joueur peut lancer
                                                          // une commande, dans ce cas-ci la potion de d�fense
        {
            etatPersonnage = Etat.Occupe; // on bascule l'�tat du joueur � occup� pour l'empecher de lancer d'autre commande
            PersonnageAttaque.joueurDefend = true; // on met ce bool a true pour dire que le joueur se d�fend
            joueurPersonnageAttaque.PotionDefense(defendActive, () => { // on appelle ce void pour que le joueur puisse prendre moins de d�gat pendant 3 tours
                Invoke("ChoisirProchainPersonnageQuiAttaque", 2f); // apr�s que l'action soit compl�t� on appelle ce void dans 2 seconde pour attendre que la boiteTexte fini de disparaitre et choisir le prochain personnage � attaquer
            }, () => { // sinon, si le joueur n'a pas une quantit� suffisante de potion de d�fense
                etatPersonnage = Etat.attendsCommandeJoueur; // le joueur ne peut pas se soigner alors on retourne l'�tat du joueur en attente de commande
            });

        }
    }

    /* void � attribuer au bouton de d�fense */
    public void PersonnageDefenseBouton()
    {
        if (etatPersonnage == Etat.attendsCommandeJoueur) // si le joueur est en attente d'une commande, le joueur peut lancer
                                                          // une commande, dans ce cas-ci le bouton de d�fense
        {
            etatPersonnage = Etat.Occupe; // on bascule l'�tat du joueur � occup� pour l'empecher de lancer d'autre commande
            PersonnageAttaque.joueurDefend = true; // on met ce bool a true pour dire que le joueur se d�fend
            joueurPersonnageAttaque.Defense(defendActive, () => { // on appelle ce void pour que le joueur puisse prendre moins de d�gat pendant 1 tour
                Invoke("ChoisirProchainPersonnageQuiAttaque", 2f); // apr�s que l'action soit compl�t� on appelle ce void dans 2 seconde pour attendre que la boiteTexte fini de disparaitre et choisir le prochain personnage � attaquer
            });
            
        }
    }

    /* -------------------------------------------- cette fonction permet d'initialiser le personnage qui va attaque en premier ------------------------------------- */
    private void RendreActifLePersonnageAttaque(PersonnageAttaque personnageAttaque) // prends en param�tre une variable personnageAttaque ennemi ou joueur
    {
        //StartCoroutine(UICombatAfficherDegatSoin.Delai());
        if (actifPersonnageAttaque != null) // si le personnage actif est diff�rent de null sa veux dire qu'il est actif
                                           // alors apr�s son attaque on doit d�sactiver la boite de selection
        {
            //actifPersonnageAttaque.CacheBoiteDeSelection(); // d�sactive la boite de selection du personnage qui �tait actif lors de ce tour
        }

        actifPersonnageAttaque = personnageAttaque; // lorsque l'ennemi ou le joueur est pris en param�tre on lui attribu comme personnage actif � faire la premi�re attaque
                                                    // ou commande
       // actifPersonnageAttaque.MontrerBoiteSelection(); // si le personnage actif est null sa veux dire qu'il n'est pas actif, alors il faut activer la boite de selection
                                                        // apr�s que le personnage adverse termine son action
    }



    /* ---------------------------------------- cette fonction permet de controler les phases de tours ----------------------------------------------------------- */
    private void ChoisirProchainPersonnageQuiAttaque() 
    {
        if (actifPersonnageAttaque.estVivant) // si le personnage actif est vivant on rentre dans la condition
        {
            if (actifPersonnageAttaque == joueurPersonnageAttaque) // si le personnage actif est �gal au joueur
            {
                RendreActifLePersonnageAttaque(ennemiPersonnageAttaque); // l'ennemi devient maintenant actif
                etatPersonnage = Etat.Occupe; // etat de l'ennemi devient maintenant occup� puisqu'il va directement attaquer
                ennemiPersonnageAttaque.Attack(joueurPersonnageAttaque, systemeVieJoueur, () => { // l'ennemi lorsqu'il est actif va attaquer le joueur
                    ChoisirProchainPersonnageQuiAttaque(); // lorsqu'il aura termin� d'attaquer on appelle la fonction pour choisir le prochain personnage � attaquer, dont le joueur
                });
            }
            else // sinon,  on sait que c'�tait le tour de l'ennemi alors..
            {
                RendreActifLePersonnageAttaque(joueurPersonnageAttaque); // le joueur devient maintenant actif
                etatPersonnage = Etat.attendsCommandeJoueur; // l'�tat du joueur devient maintenant en attente d'une commande
            }
        }
        else
        {
            if(actifPersonnageAttaque == joueurPersonnageAttaque  && GestionCollisionPerso.fightBoss == false) // si le joueur est actif, mais pas l'ennemi et que l'ennemi ne soit pas un boss
            {
                systemeVieJoueurHorsCombat.sortiCombat = true; // on donne au bool sortieCombat la valeur true
                systemeVieJoueurHorsCombat.vieHorsCombat = systemeVieJoueur; // on donne la valeur vieHorsCombat au systemeVieJoueur � l'int�rieur du combat
                GestionInventairePerso.argents += 10; // le joueur gagne 10 pi�ces
                Invoke("RetourAMapApresCombat", 1f); // on retourne dans la map apr�s 1 seconde
            }
            else if(actifPersonnageAttaque == joueurPersonnageAttaque && GestionCollisionPerso.fightBoss) // sinon, si le joueur est actif, et le boss est vaincu 
            {
                GestionInventairePerso.argents += 30;

                Permanent.musiqueCombatDetecteur = false; // la musique de combat est retourn� � false

                if (MouvementPersonnage.earth == true) // si on est sur la map de terre et que le boss est vaincu
                {
                    GestionInventairePerso.fragmentTerre = true; // on gagne le fragment de terre
                    
                }
                else if (MouvementPersonnage.air == true) // sinon si on est sur la map d'air et que le boss est vaincu
                {
                    GestionInventairePerso.fragmentAir = true; // on gagne le fragment d'air
                    
                }
                else if (MouvementPersonnage.feu == true) // sinon si on est sur la map de feu et que le boss est vaincu
                {
                    GestionInventairePerso.fragmentFeu = true; // on gagne le fragment de feu
                    
                }
                else if (MouvementPersonnage.eau == true) // sinon si on est sur la map d'eau et que le boss est vaincu
                {    
                    GestionInventairePerso.fragmentEau = true; // on gagne le fragment d'eau
                    
                }
                Invoke("FinJeuTemporaire", 1f); // on lance la sc�ne pour retourner dans le hubworld apr�s 1 seconde
            }
            else // sinon, cela voudrait dire que le joueur est mort
            {
                GestionCollisionPerso.fightBoss = false; // on retourne le bool du trigger boss a false
                //print("joueur � p�ri ");
                Invoke("RecommencerJeuDebut", 1f); // On lance la sc�ne du d�but apr�s une seconde
            }


        }
        
    }


    /* void pour retourner dans la map apr�s avoir battu un ennemi de base */
    void RetourAMapApresCombat()
    {
        Permanent.musiqueCombatDetecteur = false; // on d�sactive la musique de combat
        SceneManager.LoadScene(GestionCollisionPerso.chiffreScenePourSortirCombat); // on retourne dans la map ou le joueur �taient soit feu, eau, terre, ou air
    }

    /* void pour retourner au hubworld et aller recolter les autres fragments */
    void FinJeuTemporaire()
    {
        systemeVieJoueur = new SystemeVie(viePersonnage);
        GestionCollisionPerso.fightBoss = false; // on retourne la valeur de bool de boss � false
        MouvementPersonnage.hubworld = true; // on retourne la valeur de bool hubworld � true pour que le joueur spawn � la position par d�faut du hubworld
        /*MouvementPersonnage.eau = false;
        MouvementPersonnage.feu = false;
        MouvementPersonnage.earth = false;
        MouvementPersonnage.air = false;*/
        MouvementPersonnage.estTriggerCombat = false; // le joueur n'est plus en combat alors, on retourne la valeur false � estTriggerCombat

        SceneManager.LoadScene(5); // on Lance la scene de pour afficher au joueur qu'il r�colter un fragment
        //print("scene de fin");
    }

    // si le joueur meurt, on recommence le jeu du d�but
    void RecommencerJeuDebut()
    {
        GestionCollisionPerso.fightBoss = false; // on retourne la valeur de bool de boss � false
        MouvementPersonnage.estTriggerCombat = false; // le joueur n'est plus en combat alors, on retourne la valeur false � estTriggerCombat
        MouvementPersonnage.hubworld = true; // on retourne la valeur de bool hubworld � true pour que le joueur spawn � la position par d�faut du hubworld
        GestionInventairePerso.argents = 100; // on r�initialise la quantit� d'argent � 100
        GestionInventairePerso.potionsAttaque = 0; // on r�initialise la quantit� de potion � 0
        GestionInventairePerso.potionsDefense = 0; // on r�initialise la quantit� de potion � 0
        GestionInventairePerso.potionsSoin = 0; // on r�initialise la quantit� de potion � 0
        SceneManager.LoadScene(4); // on lance la sc�ne de mort

    }

}
