
//Programmeur Roshan
public class SystemeVie // class controlant la vie des personnages en combat
{

    private int vie; // un integer pour update la vie des personnages
    private int vieMax; // un integer pour limiter la vie maximale des personnages

    /* ---------------------------------------- cette classe permet d'initialiser des personnages --------------------------------------------- */
    public SystemeVie(int vieMax)
    {
        this.vieMax = vieMax; // on donne la variable vieMax global à celle donné en paramètre
        vie = vieMax; // on donne à la variable vie globale les données de la vieMax
    }

    public int initialiserVie() // cette variable permet de retourner la valeur de vie pour l'afficher dans le debug
    {
        return vie; // retourne la vie dans une variable integer
    }

    public float avoirVieEnPourcent() // cette variable permet de prendre la quantité de vie pour le diviser avec la vieMax pour avoir
                                      // le % de vie afin de créer une barre de vie
    {
          return (float)vie / vieMax; // retourne le pourcentage de vie dans une variable float
    }

    /* Cette fonction de prendre une variable int pour stocker la quantité de dégats
     * et l'infliger au personnage adverse. Par la suite, cette fonction prends en 
     * paramètre une variable bool pour vérifier si le joueur à cliqué sur le bouton
     * défense et ainsi réduire les dégats causés à moitié*/
    public void Degat(int quantiteDegat, bool estDefends)
    {
        if (estDefends) // si le joueur à cliqué sur le bouton défense
        {
            vie -= quantiteDegat/2; // les dégats sont réduit à moitié
        }
        else // sinon....
        {
            vie -= quantiteDegat; // le joueur ne se défends pas et il reçoit l'entièreté des dégats
        }

        if (vie <= 0) // si la quantité de vie des personnages est plus petit que zero on l'égal à zero, pour controler lorsqu'un personnage meurt
        {
            vie = 0;
        }
    }

    /* Ce void permet au joueur de donnner du dégat à un ennemi lorsqu'il utilise la potion d'attaque
     * Ce void prend en paramètre un int, qui est la quantité de dégat à infliger à l'ennemi */
    public void PotionAtk(int quantiteDegat) 
    {
        vie -= quantiteDegat; // inflige la quantité de dégat sur la vie de l'ennemi
        if(vie <= 0) // si la quantité de vie ennemi est plus petit ou égal à 0...
        {
            vie = 0; // la quantité de vie ennemi revient à 0
        }
    }

    /* cette fonction permet au joueur de se soigner. Cette fonction prends en 
     * paramètre une variable int qui prends la quantité de soin à appliquer au joueur */
    public void Soigner(int quantiteSoin)
    {
        vie += quantiteSoin; // on ajoute la quantité de soin dans la vie du joueur
        if (vie >= vieMax) vie = vieMax; // si la vie est plus grande que la vie maximale on égal la vie du joueur à la vie maximal initialisé au début
    }

}
