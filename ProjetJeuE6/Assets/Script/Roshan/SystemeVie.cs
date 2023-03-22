
//Programmeur Roshan
public class SystemeVie // class controlant la vie des personnages en combat
{

    private int vie; // un integer pour update la vie des personnages
    private int vieMax; // un integer pour limiter la vie maximale des personnages

    /* ---------------------------------------- cette classe permet d'initialiser des personnages --------------------------------------------- */
    public SystemeVie(int vieMax)
    {
        this.vieMax = vieMax; // on donne la variable vieMax global � celle donn� en param�tre
        vie = vieMax; // on donne � la variable vie globale les donn�es de la vieMax
    }

    public int initialiserVie() // cette variable permet de retourner la valeur de vie pour l'afficher dans le debug
    {
        return vie; // retourne la vie dans une variable integer
    }

    public float avoirVieEnPourcent() // cette variable permet de prendre la quantit� de vie pour le diviser avec la vieMax pour avoir
                                      // le % de vie afin de cr�er une barre de vie
    {
          return (float)vie / vieMax; // retourne le pourcentage de vie dans une variable float
    }

    /* Cette fonction de prendre une variable int pour stocker la quantit� de d�gats
     * et l'infliger au personnage adverse. Par la suite, cette fonction prends en 
     * param�tre une variable bool pour v�rifier si le joueur � cliqu� sur le bouton
     * d�fense et ainsi r�duire les d�gats caus�s � moiti�*/
    public void Degat(int quantiteDegat, bool estDefends)
    {
        if (estDefends) // si le joueur � cliqu� sur le bouton d�fense
        {
            vie -= quantiteDegat/2; // les d�gats sont r�duit � moiti�
        }
        else // sinon....
        {
            vie -= quantiteDegat; // le joueur ne se d�fends pas et il re�oit l'enti�ret� des d�gats
        }

        if (vie <= 0) // si la quantit� de vie des personnages est plus petit que zero on l'�gal � zero, pour controler lorsqu'un personnage meurt
        {
            vie = 0;
        }
    }

    /* Ce void permet au joueur de donnner du d�gat � un ennemi lorsqu'il utilise la potion d'attaque
     * Ce void prend en param�tre un int, qui est la quantit� de d�gat � infliger � l'ennemi */
    public void PotionAtk(int quantiteDegat) 
    {
        vie -= quantiteDegat; // inflige la quantit� de d�gat sur la vie de l'ennemi
        if(vie <= 0) // si la quantit� de vie ennemi est plus petit ou �gal � 0...
        {
            vie = 0; // la quantit� de vie ennemi revient � 0
        }
    }

    /* cette fonction permet au joueur de se soigner. Cette fonction prends en 
     * param�tre une variable int qui prends la quantit� de soin � appliquer au joueur */
    public void Soigner(int quantiteSoin)
    {
        vie += quantiteSoin; // on ajoute la quantit� de soin dans la vie du joueur
        if (vie >= vieMax) vie = vieMax; // si la vie est plus grande que la vie maximale on �gal la vie du joueur � la vie maximal initialis� au d�but
    }

}
