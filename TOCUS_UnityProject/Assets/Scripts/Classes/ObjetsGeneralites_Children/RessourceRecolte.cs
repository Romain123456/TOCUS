using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Classe RessourceRecolte héritant de ObjetsGeneralites
public class RessourceRecolte : ObjetsGeneralites
{
    //=0 : blé, =1 : bois, =2 : fer, =3 : pierre 

    //Variables
    [HideInInspector] public int nbDispo;               //Nombre de ressources disponibles
    [HideInInspector] public int typeRessource;         //Int donnant le type de ressource

    //Component
    [HideInInspector] public Text nbRessourceText;          //Text pour l'affichage du nombre de ressources disponibles


    //Fonction constructeur de RessourceRecolte. Attribue les variables propres à la classe et détruit les composants du prefab inutiles.
    public RessourceRecolte(Sprite _spriteBaseObjet, string _NomObjet, bool _IsInteractable)
    {
        ConstructeursBatiment(_spriteBaseObjet, _NomObjet,_IsInteractable);        
        nbRessourceText = transformObjet.GetChild(0).GetChild(1).GetComponent<Text>();
        

        Destroy(imageFlagJoueur);
        Destroy(healthBarMax_GO);
        Destroy(batimentConstruit);
        Destroy(spritesUnitesFiles);

        //Attribution du type de ressource
        if (nomObjet == "ChampDeBle")
        {
            typeRessource = 0;
        } else if(nomObjet == "RecolteDeBois")
        {
            typeRessource = 1;
        } else if(nomObjet == "MineDeFer")
        {
            typeRessource = 2;
        } else if(nomObjet == "CarriereDePierre")
        {
            typeRessource = 3;
        }
    }

    //Affiche le nombre de ressources disponibles sur le lieu de récolte
    public void SetNbRessourceDispo()
    {
        nbRessourceText.text = nbDispo.ToString();
    }

    //Adapte l'échelle du texte d'affichage du nombre de ressources
    public void ScalingTextRessources()
    {
        nbTextRessources_GO.GetComponent<RectTransform>().localScale = new Vector3(nbTextRessources_GO.GetComponent<RectTransform>().localScale.x / nbTextRessources_GO.transform.parent.parent.localScale.x, nbTextRessources_GO.GetComponent<RectTransform>().localScale.y / nbTextRessources_GO.transform.parent.parent.localScale.y, 0)/2;
    }


    //Interaction avec les ressources : récupération dans l'inventaire du joueur
    public void RecuperationRessources()
    {
        if (nbDispo > 0)
        {
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[typeRessource] += nbDispo;       //Ajout des ressources dispo dans la réserve du joueur actif
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1].AfficheNbRessources(typeRessource);          //Affiche les ressources du joueur
            nbDispo = 0;                //Met le nombre de ressources disponible de l'objet à 0
            SetNbRessourceDispo();      //Met à jour l'affichage du nombre de ressources disponibles
            ChangeJoueurActif();        //Change le joueur actif
        }
        
    }


    //Production de ressource à chaque round
    public void ProductionRessourceRound()
    {
        nbDispo += levelManager.productionRecoltes[typeRessource];      //Augmente le nombre de ressources disponible de l'incrément du niveau
        SetNbRessourceDispo();                  //Met à jour l'affichage
    }

}
