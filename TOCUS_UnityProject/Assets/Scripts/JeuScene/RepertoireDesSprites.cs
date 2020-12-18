using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepertoireDesSprites : MonoBehaviour
{
    //Sprites des Bâtiments
    public Sprite champBleSprite;
    public Sprite mineFerSprite;
    public Sprite recolteBoisSprite;
    public Sprite carrierePierreSprite;
    public Sprite campMilitaireSprite;
    public Sprite chantierTourSprite;
    public Sprite emptyTourSprite;
    public Sprite chantierBTPSprite;
    public Sprite emptyBTPSprite;
    public Sprite porteVilleSprite;
    public Sprite chateauSprite;
    public Sprite flagJ1;
    public Sprite flagJ2;


    #region Unités + Ennemis
    //Ennemis
    [HideInInspector] public EnnemiRepertoire[] ennemiData;
    [HideInInspector] public EnnemiObject[] listeEnnemiObject;
    public EnnemiObject[] stockTotalEnnemis;        //stock TOTAL de tous les ennemis du jeu
    //Création de ennemiData
    public void EnnemiDataCreate()
    {
        listeEnnemiObject = new EnnemiObject[ParametresCarteOpenning.ficOptionParam.objet_reglages.o_reglages.arr_s_ennemis_disponibles.Length];
        for(int ii = 0; ii < listeEnnemiObject.Length; ii++)
        {
            for(int jj = 0; jj < stockTotalEnnemis.Length; jj++)
            {
                if(ParametresCarteOpenning.ficOptionParam.objet_reglages.o_reglages.arr_s_ennemis_disponibles[ii] == stockTotalEnnemis[jj].name)
                {
                    listeEnnemiObject[ii] = stockTotalEnnemis[jj];
                }
            }
        }

        ennemiData = new EnnemiRepertoire[listeEnnemiObject.Length];
        for (int ii = 0; ii < ennemiData.Length; ii++)
        {
            ennemiData[ii] = new EnnemiRepertoire();
            ennemiData[ii].uniteNom = listeEnnemiObject[ii].uniteNom;
            ennemiData[ii].uniteBoxCollSize = listeEnnemiObject[ii].uniteBoxCollSize;
            ennemiData[ii].uniteScale = listeEnnemiObject[ii].uniteScale;
            ennemiData[ii].unitePositionCanvas = listeEnnemiObject[ii].unitePositionCanvas;
            ennemiData[ii].uniteScaleCanvas = listeEnnemiObject[ii].uniteScaleCanvas;
            ennemiData[ii].uniteTypeVitalite = listeEnnemiObject[ii].uniteTypeVitalite;
            ennemiData[ii].uniteTypeInitiative = listeEnnemiObject[ii].uniteTypeInitiative;
            ennemiData[ii].uniteTypeDegats = listeEnnemiObject[ii].uniteTypeDegats;
            ennemiData[ii].uniteTypeArmure = listeEnnemiObject[ii].uniteTypeArmure;
            ennemiData[ii].ennemiSpeedMove = listeEnnemiObject[ii].ennemiSpeedMove;
            ennemiData[ii].ennemiSprite = listeEnnemiObject[ii].ennemiSprite;
            ennemiData[ii].ennemiAnimatorController = listeEnnemiObject[ii].ennemiAnimatorController;
            ennemiData[ii].pointVictoireGain = JsonParametresGlobaux.ficParamGlobaux.objet_divers.o_divers.i_gain_victoire_par_ennemi_mort;
        }

    }


    //Unite
    [HideInInspector] public UnitesJoueurRepertoire[] unitesJoueurData;
    [HideInInspector] public UniteJoueurObject[] listeUniteJoueurObject;
    public UniteJoueurObject[] stockTotalUnite;     //Stock de TOUTES les unités

    //Unites Speciales
    [HideInInspector] public UnitesJoueurRepertoire[] unitesSpecialesData;
    [HideInInspector] public UniteJoueurObject[] listeUniteSpecialesObject;
    public UniteJoueurObject[] stockTotalUniteSpeciales;        //Stock de TOUTES les unites spéciales

    //Super Unite
    [HideInInspector] public SuperUnitesJoueurRepertoire[] superUnitesJoueurData;
    [HideInInspector] public SuperUniteJoueurObject[] listeSuperUniteJoueurObject;
    public SuperUniteJoueurObject[] stockTotalSuperUnites;      //Stock de TOUTES les SuperUnités

    public void UniteJoueurCreate()
    {
        listeUniteJoueurObject = new UniteJoueurObject[ParametresCarteOpenning.ficOptionParam.objet_reglages.o_reglages.arr_s_unites_disponibles.Length];
        for(int ii = 0; ii < listeUniteJoueurObject.Length; ii++)
        {
            for(int jj = 0; jj < stockTotalUnite.Length; jj++)
            {
                if(ParametresCarteOpenning.ficOptionParam.objet_reglages.o_reglages.arr_s_unites_disponibles[ii] == stockTotalUnite[jj].name)
                {
                    listeUniteJoueurObject[ii] = stockTotalUnite[jj];
                }
            }
        }

        unitesJoueurData = new UnitesJoueurRepertoire[listeUniteJoueurObject.Length];
        for (int ii = 0; ii < unitesJoueurData.Length; ii++)
        {
            unitesJoueurData[ii] = new UnitesJoueurRepertoire();
            unitesJoueurData[ii].uniteNom = listeUniteJoueurObject[ii].uniteNom;
            unitesJoueurData[ii].uniteBoxCollSize = listeUniteJoueurObject[ii].uniteBoxCollSize;
            unitesJoueurData[ii].uniteScale = listeUniteJoueurObject[ii].uniteScale;
            unitesJoueurData[ii].unitePositionCanvas = listeUniteJoueurObject[ii].unitePositionCanvas;
            unitesJoueurData[ii].uniteScaleCanvas = listeUniteJoueurObject[ii].uniteScaleCanvas;
            unitesJoueurData[ii].uniteTypeVitalite = listeUniteJoueurObject[ii].uniteTypeVitalite;
            unitesJoueurData[ii].uniteTypeInitiative = listeUniteJoueurObject[ii].uniteTypeInitiative;
            unitesJoueurData[ii].uniteTypeDegats = listeUniteJoueurObject[ii].uniteTypeDegats;
            unitesJoueurData[ii].uniteTypeArmure = listeUniteJoueurObject[ii].uniteTypeArmure;
            unitesJoueurData[ii].uniteSpriteBase = listeUniteJoueurObject[ii].uniteSpriteBase;
            unitesJoueurData[ii].unitePrixBle = listeUniteJoueurObject[ii].unitePrixBle;

            unitesJoueurData[ii].uniteAnimatorController = listeUniteJoueurObject[ii].uniteAnimatorController;

            unitesJoueurData[ii].uniteListeOrdre = listeUniteJoueurObject[ii].uniteListeOrdre;
        }


        listeUniteSpecialesObject = new UniteJoueurObject[ParametresCarteOpenning.ficOptionParam.objet_reglages.o_reglages.arr_s_unites_speciales_disponibles.Length];
        if (listeUniteSpecialesObject.Length > 0)
        {
            
            for(int ii = 0; ii < listeUniteSpecialesObject.Length; ii++)
            {
                for(int jj = 0; jj < stockTotalUniteSpeciales.Length; jj++)
                {
                    if(ParametresCarteOpenning.ficOptionParam.objet_reglages.o_reglages.arr_s_unites_speciales_disponibles[ii] == stockTotalUniteSpeciales[jj].name)
                    {
                        listeUniteSpecialesObject[ii] = stockTotalUniteSpeciales[jj];
                    }
                }
            }

            unitesSpecialesData = new UnitesJoueurRepertoire[listeUniteSpecialesObject.Length];
            for (int ii = 0; ii < unitesSpecialesData.Length; ii++)
            {
                unitesSpecialesData[ii] = new UnitesJoueurRepertoire();
                unitesSpecialesData[ii].uniteNom = listeUniteSpecialesObject[ii].uniteNom;
                unitesSpecialesData[ii].uniteBoxCollSize = listeUniteSpecialesObject[ii].uniteBoxCollSize;
                unitesSpecialesData[ii].uniteScale = listeUniteSpecialesObject[ii].uniteScale;
                unitesSpecialesData[ii].unitePositionCanvas = listeUniteSpecialesObject[ii].unitePositionCanvas;
                unitesSpecialesData[ii].uniteScaleCanvas = listeUniteSpecialesObject[ii].uniteScaleCanvas;
                unitesSpecialesData[ii].uniteTypeVitalite = listeUniteSpecialesObject[ii].uniteTypeVitalite;
                unitesSpecialesData[ii].uniteTypeInitiative = listeUniteSpecialesObject[ii].uniteTypeInitiative;
                unitesSpecialesData[ii].uniteTypeDegats = listeUniteSpecialesObject[ii].uniteTypeDegats;
                unitesSpecialesData[ii].uniteTypeArmure = listeUniteSpecialesObject[ii].uniteTypeArmure;
                unitesSpecialesData[ii].uniteSpriteBase = listeUniteSpecialesObject[ii].uniteSpriteBase;
                unitesSpecialesData[ii].unitePrixBle = listeUniteSpecialesObject[ii].unitePrixBle;

                unitesSpecialesData[ii].uniteAnimatorController = listeUniteSpecialesObject[ii].uniteAnimatorController;

                unitesSpecialesData[ii].uniteListeOrdre = listeUniteSpecialesObject[ii].uniteListeOrdre;
            }
        }

    }


    public void SuperUnitesJoueurCreate()
    {
        listeSuperUniteJoueurObject = new SuperUniteJoueurObject[ParametresCarteOpenning.ficOptionParam.objet_reglages.o_reglages.arr_s_superunites_disponibles.Length];
        for(int ii = 0; ii < listeSuperUniteJoueurObject.Length; ii++)
        {
            for(int jj = 0; jj < stockTotalSuperUnites.Length; jj++)
            {
                if(ParametresCarteOpenning.ficOptionParam.objet_reglages.o_reglages.arr_s_superunites_disponibles[ii] == stockTotalSuperUnites[jj].name)
                {
                    listeSuperUniteJoueurObject[ii] = stockTotalSuperUnites[jj];
                }
            }
        }

        superUnitesJoueurData = new SuperUnitesJoueurRepertoire[listeSuperUniteJoueurObject.Length];
        for (int ii = 0; ii < superUnitesJoueurData.Length; ii++)
        {
            superUnitesJoueurData[ii] = new SuperUnitesJoueurRepertoire();
            superUnitesJoueurData[ii].uniteNom = listeSuperUniteJoueurObject[ii].uniteNom;
            superUnitesJoueurData[ii].uniteBoxCollSize = listeSuperUniteJoueurObject[ii].uniteBoxCollSize;
            superUnitesJoueurData[ii].uniteScale = listeSuperUniteJoueurObject[ii].uniteScale;
            superUnitesJoueurData[ii].unitePositionCanvas = listeSuperUniteJoueurObject[ii].unitePositionCanvas;
            superUnitesJoueurData[ii].uniteScaleCanvas = listeSuperUniteJoueurObject[ii].uniteScaleCanvas;
            superUnitesJoueurData[ii].uniteTypeVitalite = listeSuperUniteJoueurObject[ii].uniteTypeVitalite;
            superUnitesJoueurData[ii].uniteTypeInitiative = listeSuperUniteJoueurObject[ii].uniteTypeInitiative;
            superUnitesJoueurData[ii].uniteTypeDegats = listeSuperUniteJoueurObject[ii].uniteTypeDegats;
            superUnitesJoueurData[ii].uniteTypeArmure = listeSuperUniteJoueurObject[ii].uniteTypeArmure;
            superUnitesJoueurData[ii].uniteSpriteBase = listeSuperUniteJoueurObject[ii].uniteSpriteBase;
            superUnitesJoueurData[ii].unitePrixBle = listeSuperUniteJoueurObject[ii].unitePrixBle;
            superUnitesJoueurData[ii].unitePrixFer = listeSuperUniteJoueurObject[ii].unitePrixFer;

            superUnitesJoueurData[ii].uniteAnimatorController = listeSuperUniteJoueurObject[ii].uniteAnimatorController;

            superUnitesJoueurData[ii].uniteListeOrdre = listeSuperUniteJoueurObject[ii].uniteListeOrdre;

        }
    }

    #endregion


    //Heros
    public HerosRepertoire[] herosData;

    #region Tours
    //Tour
    [HideInInspector] public List<ToursRepertoire> toursData;
    [HideInInspector] public TourObject[] listeToursObject;
    public TourObject[] stockTotalTours;        //stock de TOUTES les tours du jeux
    [HideInInspector] public int nbDataTours;
    public Sprite tourCirculaireVisee;

    public void ToursCreate()
    {
        listeToursObject = new TourObject[ParametresCarteOpenning.ficOptionParam.objet_reglages.o_reglages.arr_s_tours_disponibles.Length];
        for(int ii = 0; ii < listeToursObject.Length; ii++)
        {
            for(int jj = 0; jj < stockTotalTours.Length; jj++)
            {
                if(ParametresCarteOpenning.ficOptionParam.objet_reglages.o_reglages.arr_s_tours_disponibles[ii] == stockTotalTours[jj].name)
                {
                    listeToursObject[ii] = stockTotalTours[jj];
                }
            }
        }


        toursData = new List<ToursRepertoire>();
        List<int> listToursHasard = new List<int>();
        HasardListeObject(listeToursObject.Length, listToursHasard, true, nbDataTours);

        for (int ii = 0; ii < nbDataTours; ii++)
        {
            toursData.Add(new ToursRepertoire());
            toursData[ii].tourNom = listeToursObject[listToursHasard[ii]].tourNom;
            toursData[ii].tourSprite = listeToursObject[listToursHasard[ii]].tourSprite;
            toursData[ii].positionTour = listeToursObject[listToursHasard[ii]].positionTour;
            toursData[ii].scaleTour = listeToursObject[listToursHasard[ii]].scaleTour;
            toursData[ii].tourTypeVisee = listeToursObject[listToursHasard[ii]].tourTypeVisee.ToString();
            toursData[ii].tourTypeCadence = listeToursObject[listToursHasard[ii]].tourTypeCadence;
            toursData[ii].tourTypePuissance = listeToursObject[listToursHasard[ii]].tourTypePuissance;
            toursData[ii].tourTypePortee = listeToursObject[listToursHasard[ii]].tourTypePortee.ToString();
            toursData[ii].tourTypeDureeDegats = listeToursObject[listToursHasard[ii]].tourTypeDureeDegats.ToString();
            toursData[ii].tourTypeLargeurVisee = listeToursObject[listToursHasard[ii]].tourTypeLargeurVisee.ToString();
            toursData[ii].tourTypeImpact = listeToursObject[listToursHasard[ii]].tourTypeImpact.ToString();
        }
    }
    #endregion

    #region Batiments
    //Batiment
    [HideInInspector] public List<BatimentsRepertoire> batimentsData;         //Liste de l'architecte dont la taille est égale au nombre de batiments disponibles dans le jeu
    [HideInInspector] public BatimentObject[] listeBatiments;             //Tous les batiments du jeu possibles dans l'ordre de l'inspector de cette partie
    public BatimentObject[] stockTotalBatiment;         //Stock de TOUS les bâtiments du jeu

   
    public void BatimentsCreate()
    {
        listeBatiments = new BatimentObject[ParametresCarteOpenning.ficOptionParam.objet_reglages.o_reglages.arr_s_batiments_disponibles.Length];
        for(int ii = 0; ii < listeBatiments.Length; ii++)
        {
            for(int jj = 0; jj < stockTotalBatiment.Length; jj++)
            {
                if(ParametresCarteOpenning.ficOptionParam.objet_reglages.o_reglages.arr_s_batiments_disponibles[ii] == stockTotalBatiment[jj].name)
                {
                    listeBatiments[ii] = stockTotalBatiment[jj];
                }
            }
        }


        List<int> listBatimentHasard = new List<int>();
        HasardListeObject(listeBatiments.Length, listBatimentHasard,false, listeBatiments.Length);

        for (int ii = 0; ii < listeBatiments.Length; ii++)
        {
            batimentsData.Add(new BatimentsRepertoire());
            batimentsData[ii].batimentNom = listeBatiments[listBatimentHasard[ii]].batimentNom;
            batimentsData[ii].batimentSprite = listeBatiments[listBatimentHasard[ii]].batimentSprite;
            batimentsData[ii].batimentPrixBle = listeBatiments[listBatimentHasard[ii]].batimentPrixBle;
            batimentsData[ii].batimentPrixBois = listeBatiments[listBatimentHasard[ii]].batimentPrixBois;
            batimentsData[ii].batimentPrixFer = listeBatiments[listBatimentHasard[ii]].batimentPrixFer;
            batimentsData[ii].batimentPrixPierre = listeBatiments[listBatimentHasard[ii]].batimentPrixPierre;
            batimentsData[ii].scaleBatiment = listeBatiments[listBatimentHasard[ii]].scaleBatiment;
            batimentsData[ii].nombreBatimentMaxParJoueur = listeBatiments[listBatimentHasard[ii]].nombreBatimentMaxParJoueur;
            batimentsData[ii].isBatimentPublic = listeBatiments[listBatimentHasard[ii]].isBatimentPublic;
            batimentsData[ii].isBatimentPersonnel = listeBatiments[listBatimentHasard[ii]].isBatimentPersonnel;
        }
    }
    #endregion



    public void HasardListeObject(int _TailleMax, List<int> _MaListe,bool _IsRepetPossible,int _TailleListe)
    {
        List<int> _TableauIndex = new List<int>();
        int[] tableauRendu = new int[_TailleListe];

        for (int ii = 0; ii < _TailleMax; ii++)
        {
            _TableauIndex.Add(ii);
        }

        for (int ii = 0; ii < _TailleListe; ii++)
        {
            int choixTableau = Random.RandomRange(0, _TableauIndex.Count);
            tableauRendu[ii] = _TableauIndex[choixTableau];
            if (!_IsRepetPossible)
            {
                _TableauIndex.Remove(_TableauIndex[choixTableau]);
            }
            _MaListe.Add(tableauRendu[ii]);
        }

    }


}


#region Unités + Ennemis
//Classes des différents types de répertoire
[System.Serializable]
public class EnnemiRepertoire : UnitesRepertoire
{
    public float ennemiSpeedMove;               //Vitesse de déplacement de l'ennemi
    public Sprite ennemiSprite;         //Sprite principale
    public RuntimeAnimatorController ennemiAnimatorController;          //Animator Controller de l'ennemi
    public int pointVictoireGain;             //Nombre de points de victoire que donne l'ennemi
}


[System.Serializable]
public class UnitesRepertoire
{
    public string uniteNom;                 //Nom de l'unité

    //Variables des différents personnages (à renommer)
    public Vector2 uniteBoxCollSize;            //Taille de la boxCollider de l'unité
    public Vector2 uniteScale;                  //Scale de l'unité
    public Vector2 unitePositionCanvas;         //Position du Canvas de la barre de vie de l'unité
    public Vector2 uniteScaleCanvas;            //Scale du Canvas de l'unité


    //Caractéristiques
    public Unite_Objects.typeVitalite uniteTypeVitalite;      //Type de vitalité de l'unité
    public Unite_Objects.typeInitiative uniteTypeInitiative;              //Type de l'initiative de l'unité
    public Unite_Objects.typeDegats uniteTypeDegats;          //Type de dégats de l'unité
    public Unite_Objects.typeArmure uniteTypeArmure;            //Type d'armure de l'unité
}

[System.Serializable]
public class UnitesJoueurRepertoire : UnitesRepertoire
{
    public Sprite[] uniteSpriteBase;        //Sprite de base de l'unité (0 : joueur1, 1 : joueur2)
    public RuntimeAnimatorController[] uniteAnimatorController;         //Animator Cotroller de l'unité (0 : joueur1, 1 : joueur2)
    public int unitePrixBle;                //Prix en blé de l'unité
    public int uniteListeOrdre;             //Ordre dans la liste. Doit être égal à l'ordre donné dans RepertoireDesSprites
}


[System.Serializable]
public class SuperUnitesJoueurRepertoire : UnitesJoueurRepertoire
{
    public int unitePrixFer;                //Prix en fer de l'unité
}


#endregion






[System.Serializable]
public class HerosRepertoire
{
    public string herosNom;                 //Nom du héros
    public Sprite herosSprite;              //Sprite du héros
    public Sprite pionActif;                //Image du pion actif

}



#region Tours
[System.Serializable]
public class ToursRepertoire
{
    public string tourNom;                  //Nom de la tour
    public Sprite tourSprite;               //Sprite de la tour

    public Vector3 positionTour;            //Position de la tour par rapport à l'emplacement
    public Vector3 scaleTour;               //Scale de la tour

    public string tourTypeVisee;             //Type de visée de la tour

    public Vector2 offsetColliderVisee;              //Offset du Collider de la visée de la tour;
    public Vector2[] sizesColliderVisee;            //Tableau des positions déterminant la taille du collider de la visée de la tour. 
                                                    //Pour Ligne : Size = 1 
                                                    //Pour Cercle : Size = 1
    public Sprite viseeSprite;          //Sprite de la visée de la tour (Valable pour visée circulaire et Cone)

    public TourObject.typeCadence tourTypeCadence;             //Type de cadence de tir

    public TourObject.typePuissance tourTypePuissance;         //Type de puissance

    public string tourTypePortee;            //Type de portée

    public string tourTypeDureeDegats;      //Type de durée de dégâts

    public string tourTypeLargeurVisee;     //Type de largeur de visée

    public string tourTypeImpact;           //Type impact de la tour
}

#endregion



[System.Serializable]
public class BatimentsRepertoire
{
    public string batimentNom;          //Nom du batiment
    public Sprite batimentSprite;       //Sprite du batiment
    public int batimentPrixBle;         //Prix en blé du batiment
    public int batimentPrixBois;        //Prix en bois du batiment
    public int batimentPrixFer;         //Prix en fer du batiment
    public int batimentPrixPierre;      //Prix en pierre du batiment
    public Vector3 scaleBatiment;       //Scale du batiment
    public int nombreBatimentMaxParJoueur;      //Combien de bâtiments maximum le joueur peut construire ?
    public bool isBatimentPublic;           //Est-ce que le bâtiment est public ?
    public bool isBatimentPersonnel;        //Est-ce que c'est un bâtiment personnel ?
}


