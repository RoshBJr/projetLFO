using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Programmeur Roshan
public class SystemeCombat : MonoBehaviour
{
    private static SystemeCombat instance; // variable static instance pour attribuer le bon matériel (joueurMaterielCouleur ou ennemiMaterielCouleur)
    public static SystemeCombat ChoisirInstance() // pseudo Fonction nous permmetant de choisir le bon matériel dans la méthode PersonnageAttaque --> personnageInstanceApparence
                                                  // et de la retourner dans la variable instance
    {
        return instance; // variable qui prends en paramètre soit (joueurMaterielCouleur ou ennemiMaterielCouleur)
    }
    public Transform pfabPersonnageCombat; // prefab des personnages ennemis en scène de combat
    public Transform pfabJoueurCombat; // prefab du joueur en scène de combat
    public Sprite joueurMaterielCouleur; // matériel de couleur pour le joueur
    public Sprite[] ennemiMaterielCouleur; // matériel de couleur pour l'ennemi
    public static PersonnageAttaque joueurPersonnageAttaque; // ref pour désigner le joueur et l'utiliser dans la class PersonnageAttaque
    public static PersonnageAttaque ennemiPersonnageAttaque; // ref pour désigner l'ennemi et l'utiliser dans la class PersonnageAttaque
    public static PersonnageAttaque actifPersonnageAttaque; // ref pour désigner à qui est le tour d'attaquer
    private Etat etatPersonnage; // variable pour stocker les différent état du personnage actif
    public BarreVie barreVieJoueur; // créer une variable BarreVie pour le Joueur
    public BarreVie barreVieEnnemi; // créer une variable BarreVie pour l'ennemi
    public static SystemeVie systemeVieJoueur; // créer un SystèmeVie pour le joueur afin de lui attribuer une quantité de vie
    private SystemeVie systemeVieEnnemi; // créer un SystèmeVie pour l'ennemi afin de lui attribuer une quantité de vie
    private static int viePersonnage = 200; // une variable int ayant la quantité de vie pour le joueur
    private int vieEnnemi; // une variable int ayant la quantité de vie pour les ennemis de bases
    private int vieBossTerre; // une variable int ayant la quantité de vie pour les boss
    private static GameObject boutonPotions; // GameObject qui contient les boutons affichant les potions
    private static GameObject containerBouton; // GameObject qui contient les boutons d'atk, défense et soin
    public static bool defendActive = false; // variable bool pour savoir si le joueur se défend


    private enum Etat // ceci est une énumération d'état pour controler les animations et les actions du joueur
    {
        attendsCommandeJoueur, // cet état permet au joueur de cliquer sur les boutons du UI pour lancer une animation ex: attaque défendre, etc
        Occupe, // cet état permet d'empêcher les personnages de lancer plusieurs animation en même temps, après qu'une animation sera terminé, l'état
                // occupé basculera à attendsCommandeJoueur et vice-versa, si le joueur est actif.
    }


    private void Awake()
    {
        containerBouton = GameObject.FindGameObjectWithTag("containerBouton"); // on va chercher le container des boutons dans la hierarchie pour controler son setActive
        boutonPotions = GameObject.FindGameObjectWithTag("potions"); // on va chercher le container des potions dans la hierarchie pour controler son setActive
        boutonPotions.SetActive(false); // au début, le container de potions est invisible
        instance = this; // applique la bonne instance de matériel dans l'awake
        GameObject refPourEnnemi = GameObject.Find("ennemiPourLesMap"); // ici on va chercher la référence des sprites ennemis dans la hierarchies
        ennemiMaterielCouleur = refPourEnnemi.GetComponent<Ennemis>().ennemiMaterielCouleur; // ici, on attribue à la variable ennemiMaterielCouleur à les sprites de l'ennemiMaterielCouleur
                                                                                             // de la refPourEnnemi
    }

    private void Start()
    {
        joueurPersonnageAttaque = SpawnPersonnage(true); // Spawn joueur
        ennemiPersonnageAttaque = SpawnPersonnage(false); // Spawn ennemi
        etatPersonnage = Etat.attendsCommandeJoueur; // Au début le joueur pourra lancer une animation / commande
        RendreActifLePersonnageAttaque(joueurPersonnageAttaque); // Le joueur a le premier tour en début de combat
        vieEnnemi = 50; // les ennemis de bases ont 50 points de vie
        vieBossTerre = 100; // les boss on 100 points de vie
        systemeVieJoueur = new SystemeVie(viePersonnage); // on initialise la quantité de vie pour le joueur
        systemeVieEnnemi = new SystemeVie(vieEnnemi); // on initialise la quantité de vie pour l'ennemi

        if (systemeVieJoueurHorsCombat.sortiCombat) // si le joueur sort d'un combat vivant...
        {
            systemeVieJoueur.Degat(systemeVieJoueurHorsCombat.degat, false); // on lui afflige des dégats pour avoir la meme quantité de vie lorsqu'il avait vaincu l'ennemi
            systemeVieJoueur.Soigner((systemeVieJoueurHorsCombat.vieHorsCombat.initialiserVie() - systemeVieJoueur.initialiserVie())); // ici, on initialise sa barre de vie pour l'affiche hors combat
            
        }

        barreVieJoueur.Initialiser(systemeVieJoueur); // ici on initialise la barre de vie du joueur pour l'affichage de la barre de vie
        barreVieEnnemi.Initialiser(systemeVieEnnemi); // ici on initialise la barre de vie de l'ennemi pour l'affichage de la barre de vie

        if (GestionCollisionPerso.fightBoss) // si le trigger combat du boss est true
        {
            systemeVieEnnemi = new SystemeVie(vieBossTerre); // on initialise la quantité de vie pour le boss
            barreVieEnnemi.Initialiser(systemeVieEnnemi); // ici on initialise la barre de vie du boss pour l'affichage de la barre de vie
        }


    }


    /* ------------------------------------------------- Fonction permettant de spawn les personnages  --------------------------------------------------------------- */
    private PersonnageAttaque SpawnPersonnage(bool estJoueur) // bool permettant de savoir quel type de personnage on veut spawn (true -> joueur, false -> ennemi)
    {
        Vector3 positionV3Perso; // positon V3 qui va être changé selon le personnage à spawn (joueur ou ennemis)
        Transform choisirBonPfPersonnage; // variable transform pour lui donner le bon prefab de personnage, soit ennemi ou joueur


        /* --------------------------------------------------- si le joueur spawn, cette position V3 sera utilisé ------------------------------------------------------- */
        if (estJoueur)
        {
            positionV3Perso = new Vector3(-6f, 2.05f, 0); // on donne cette position au joueur
            choisirBonPfPersonnage = pfabJoueurCombat; // donne le transform pfab du joueur
        }
        /* ------------------------------------------------- sinon le joueur est un ennemi alors cette position V3 sera utilisé  ----------------------------------+------ */
        else
        {
            positionV3Perso = new Vector3(6f, 2.05f, 0); // on donne cette position à l'ennemi
            choisirBonPfPersonnage = pfabPersonnageCombat; // donne le transform pfab des ennemis
        }

        Transform personnageTransform = Instantiate(choisirBonPfPersonnage, positionV3Perso, Quaternion.identity); // instance du Transform personnage à afficher
        PersonnageAttaque personnageAttaque = personnageTransform.GetComponent<PersonnageAttaque>(); // va chercher la ref du script PersonnageAttaque du pfPersonnage
        personnageAttaque.personnageInstanceApparence(estJoueur); // attribue une valeur true ou false à la fonction pesonnageInstanceApparence (true -> couleur vert, false -> couleur rouge)

        return personnageAttaque; // retourne les attribues du personnage qui à été spawn dans la variable personnageAttaque
    
    }

    /* ----------------------------------------------- si le bouton d'attaque du UI est cliqué le joueur attaquera  ----------------------------------------------------------- */
    public void PersonnageAttaqueBouton() // fonction servant au UI bouton d'attaque pour utiliser dans le on click
    {
        if(etatPersonnage == Etat.attendsCommandeJoueur) // si le joueur est en attente d'une commande, le joueur peut lancer
                                                         // une commande, dans ce cas-ci une attaque
        {
            etatPersonnage = Etat.Occupe; // lorsque l'attaque est lancé on bascule l'état du joueur à occupé


            joueurPersonnageAttaque.Attack(ennemiPersonnageAttaque, systemeVieEnnemi, () => { // le joueur attaque dans la direction de l'ennemi
                Invoke("ChoisirProchainPersonnageQuiAttaque", 1f);  // lorsque l'attaque du joueur est terminé on appelle cette fonction pour changer le tour à l'ennemi

            }); // appelle la fonction Attack et prends la direction de l'ennemi en paramètre et effectue une attaque vers cette direction, après que l'attaque soit terminé
                // on retourne l'état du joueur vers attendsCommandeJoueur grâce à l'Action attaqueCompleter()
        }
        
    }

    /* void à attribuer aux boutons pour afficher les potions */
    public void PersonnageContainerPotionBouton()
    {
        if (etatPersonnage == Etat.attendsCommandeJoueur) // si le joueur est en attente d'une commande, le joueur peut lancer
                                                          // une commande, dans ce cas-ci afficher les potions
        {
            if(boutonPotions.activeSelf == false) // si le container des potions invisible...
            {
                boutonPotions.SetActive(true); // on active le container de potion
                containerBouton.SetActive(false); // on désactive le container des boutons
            }
            else // sinon...
            {
                boutonPotions.SetActive(false); // on désactive le container des potions
                containerBouton.SetActive(true); // on active le container des boutons
            }


        }
    }

    /* void à attribuer au bouton potion d'atk */
    public void PersonnagePotionAtk()
    {
        if (etatPersonnage == Etat.attendsCommandeJoueur)// si le joueur est en attente d'une commande, le joueur peut lancer
                                                         // une commande, dans ce cas-ci potions atk
        {
            etatPersonnage = Etat.Occupe; // on bascule l'état du joueur à occupé pour l'empecher de lancer d'autre commande

            joueurPersonnageAttaque.PotionAtk(systemeVieEnnemi, () => { // on inflige les dégats du de la potion d'attaque sur l'ennemi
                Invoke("ChoisirProchainPersonnageQuiAttaque", 2f); // après que l'action soit complété on appelle ce void dans 2 seconde pour attendre que la boiteTexte fini de disparaitre et choisir le prochain personnage à attaquer
            }, () => { // sinon, si le joueur n'a pas une quantité suffisante de potion d'attaque
                etatPersonnage = Etat.attendsCommandeJoueur; // le joueur ne peut pas lancer l'attaque alors on retourne l'état du joueur en attente de commande
            });
        }
    }


    /* void à attribuer au bouton potion de soin et bouton soin */
    public void PersonnageSoignerBouton()
    {
        if(etatPersonnage == Etat.attendsCommandeJoueur) // si le joueur est en attente d'une commande, le joueur peut lancer
                                                         // une commande, dans ce cas-ci potion de soin
        {
            etatPersonnage = Etat.Occupe; // on bascule l'état du joueur à occupé pour l'empecher de lancer d'autre commande

            joueurPersonnageAttaque.Soigner(systemeVieJoueur, () => { // on soigne le joueur
                Invoke("ChoisirProchainPersonnageQuiAttaque", 2f); // après que l'action soit complété on appelle ce void dans 2 seconde pour attendre que la boiteTexte fini de disparaitre et choisir le prochain personnage à attaquer
            }, () => { // sinon, si le joueur n'a pas une quantité suffisante de potion de soin
                etatPersonnage = Etat.attendsCommandeJoueur; // le joueur ne peut pas se soigner alors on retourne l'état du joueur en attente de commande
            });
        }
    }

    /* void à attribuer au bouton potion de défense */
    public void PersonnagePotionDefenseBouton()
    {
        if (etatPersonnage == Etat.attendsCommandeJoueur) // si le joueur est en attente d'une commande, le joueur peut lancer
                                                          // une commande, dans ce cas-ci la potion de défense
        {
            etatPersonnage = Etat.Occupe; // on bascule l'état du joueur à occupé pour l'empecher de lancer d'autre commande
            PersonnageAttaque.joueurDefend = true; // on met ce bool a true pour dire que le joueur se défend
            joueurPersonnageAttaque.PotionDefense(defendActive, () => { // on appelle ce void pour que le joueur puisse prendre moins de dégat pendant 3 tours
                Invoke("ChoisirProchainPersonnageQuiAttaque", 2f); // après que l'action soit complété on appelle ce void dans 2 seconde pour attendre que la boiteTexte fini de disparaitre et choisir le prochain personnage à attaquer
            }, () => { // sinon, si le joueur n'a pas une quantité suffisante de potion de défense
                etatPersonnage = Etat.attendsCommandeJoueur; // le joueur ne peut pas se soigner alors on retourne l'état du joueur en attente de commande
            });

        }
    }

    /* void à attribuer au bouton de défense */
    public void PersonnageDefenseBouton()
    {
        if (etatPersonnage == Etat.attendsCommandeJoueur) // si le joueur est en attente d'une commande, le joueur peut lancer
                                                          // une commande, dans ce cas-ci le bouton de défense
        {
            etatPersonnage = Etat.Occupe; // on bascule l'état du joueur à occupé pour l'empecher de lancer d'autre commande
            PersonnageAttaque.joueurDefend = true; // on met ce bool a true pour dire que le joueur se défend
            joueurPersonnageAttaque.Defense(defendActive, () => { // on appelle ce void pour que le joueur puisse prendre moins de dégat pendant 1 tour
                Invoke("ChoisirProchainPersonnageQuiAttaque", 2f); // après que l'action soit complété on appelle ce void dans 2 seconde pour attendre que la boiteTexte fini de disparaitre et choisir le prochain personnage à attaquer
            });
            
        }
    }

    /* -------------------------------------------- cette fonction permet d'initialiser le personnage qui va attaque en premier ------------------------------------- */
    private void RendreActifLePersonnageAttaque(PersonnageAttaque personnageAttaque) // prends en paramètre une variable personnageAttaque ennemi ou joueur
    {
        //StartCoroutine(UICombatAfficherDegatSoin.Delai());
        if (actifPersonnageAttaque != null) // si le personnage actif est différent de null sa veux dire qu'il est actif
                                           // alors après son attaque on doit désactiver la boite de selection
        {
            //actifPersonnageAttaque.CacheBoiteDeSelection(); // désactive la boite de selection du personnage qui était actif lors de ce tour
        }

        actifPersonnageAttaque = personnageAttaque; // lorsque l'ennemi ou le joueur est pris en paramètre on lui attribu comme personnage actif à faire la première attaque
                                                    // ou commande
       // actifPersonnageAttaque.MontrerBoiteSelection(); // si le personnage actif est null sa veux dire qu'il n'est pas actif, alors il faut activer la boite de selection
                                                        // après que le personnage adverse termine son action
    }



    /* ---------------------------------------- cette fonction permet de controler les phases de tours ----------------------------------------------------------- */
    private void ChoisirProchainPersonnageQuiAttaque() 
    {
        if (actifPersonnageAttaque.estVivant) // si le personnage actif est vivant on rentre dans la condition
        {
            if (actifPersonnageAttaque == joueurPersonnageAttaque) // si le personnage actif est égal au joueur
            {
                RendreActifLePersonnageAttaque(ennemiPersonnageAttaque); // l'ennemi devient maintenant actif
                etatPersonnage = Etat.Occupe; // etat de l'ennemi devient maintenant occupé puisqu'il va directement attaquer
                ennemiPersonnageAttaque.Attack(joueurPersonnageAttaque, systemeVieJoueur, () => { // l'ennemi lorsqu'il est actif va attaquer le joueur
                    ChoisirProchainPersonnageQuiAttaque(); // lorsqu'il aura terminé d'attaquer on appelle la fonction pour choisir le prochain personnage à attaquer, dont le joueur
                });
            }
            else // sinon,  on sait que c'était le tour de l'ennemi alors..
            {
                RendreActifLePersonnageAttaque(joueurPersonnageAttaque); // le joueur devient maintenant actif
                etatPersonnage = Etat.attendsCommandeJoueur; // l'état du joueur devient maintenant en attente d'une commande
            }
        }
        else
        {
            if(actifPersonnageAttaque == joueurPersonnageAttaque  && GestionCollisionPerso.fightBoss == false) // si le joueur est actif, mais pas l'ennemi et que l'ennemi ne soit pas un boss
            {
                systemeVieJoueurHorsCombat.sortiCombat = true; // on donne au bool sortieCombat la valeur true
                systemeVieJoueurHorsCombat.vieHorsCombat = systemeVieJoueur; // on donne la valeur vieHorsCombat au systemeVieJoueur à l'intérieur du combat
                GestionInventairePerso.argents += 10; // le joueur gagne 10 pièces
                Invoke("RetourAMapApresCombat", 1f); // on retourne dans la map après 1 seconde
            }
            else if(actifPersonnageAttaque == joueurPersonnageAttaque && GestionCollisionPerso.fightBoss) // sinon, si le joueur est actif, et le boss est vaincu 
            {
                GestionInventairePerso.argents += 30;

                Permanent.musiqueCombatDetecteur = false; // la musique de combat est retourné à false

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
                Invoke("FinJeuTemporaire", 1f); // on lance la scène pour retourner dans le hubworld après 1 seconde
            }
            else // sinon, cela voudrait dire que le joueur est mort
            {
                GestionCollisionPerso.fightBoss = false; // on retourne le bool du trigger boss a false
                //print("joueur à péri ");
                Invoke("RecommencerJeuDebut", 1f); // On lance la scène du début après une seconde
            }


        }
        
    }


    /* void pour retourner dans la map après avoir battu un ennemi de base */
    void RetourAMapApresCombat()
    {
        Permanent.musiqueCombatDetecteur = false; // on désactive la musique de combat
        SceneManager.LoadScene(GestionCollisionPerso.chiffreScenePourSortirCombat); // on retourne dans la map ou le joueur étaient soit feu, eau, terre, ou air
    }

    /* void pour retourner au hubworld et aller recolter les autres fragments */
    void FinJeuTemporaire()
    {
        systemeVieJoueur = new SystemeVie(viePersonnage);
        GestionCollisionPerso.fightBoss = false; // on retourne la valeur de bool de boss à false
        MouvementPersonnage.hubworld = true; // on retourne la valeur de bool hubworld à true pour que le joueur spawn à la position par défaut du hubworld
        /*MouvementPersonnage.eau = false;
        MouvementPersonnage.feu = false;
        MouvementPersonnage.earth = false;
        MouvementPersonnage.air = false;*/
        MouvementPersonnage.estTriggerCombat = false; // le joueur n'est plus en combat alors, on retourne la valeur false à estTriggerCombat

        SceneManager.LoadScene(5); // on Lance la scene de pour afficher au joueur qu'il récolter un fragment
        //print("scene de fin");
    }

    // si le joueur meurt, on recommence le jeu du début
    void RecommencerJeuDebut()
    {
        GestionCollisionPerso.fightBoss = false; // on retourne la valeur de bool de boss à false
        MouvementPersonnage.estTriggerCombat = false; // le joueur n'est plus en combat alors, on retourne la valeur false à estTriggerCombat
        MouvementPersonnage.hubworld = true; // on retourne la valeur de bool hubworld à true pour que le joueur spawn à la position par défaut du hubworld
        GestionInventairePerso.argents = 100; // on réinitialise la quantité d'argent à 100
        GestionInventairePerso.potionsAttaque = 0; // on réinitialise la quantité de potion à 0
        GestionInventairePerso.potionsDefense = 0; // on réinitialise la quantité de potion à 0
        GestionInventairePerso.potionsSoin = 0; // on réinitialise la quantité de potion à 0
        SceneManager.LoadScene(4); // on lance la scène de mort

    }

}
