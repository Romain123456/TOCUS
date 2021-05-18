using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Gère les caractéristiques des joueurs
public class Joueur : MonoBehaviour
{
    [HideInInspector] public RepertoireDesSprites repertoireSprite;     //Repertoire des sprites générales
    [HideInInspector] public LevelManager levelManager;                 //Script gérant le déroulé de la partie

    

    //Variables de la classe Joueur
    //Caractéristiques globales
    public string _NomJoueur;           //Nom du joueur
    public Sprite _PortraitJoueur;      //Portrait de l'avatar
    public bool isIA;                   //Est-ce que le joueur est une IA et non une personne physique ?
    public int _NumeroJoueur;            //=1 ou =2
    public Sprite _ImagePionActif;      //Image du pion actif
    public GameObject ImageInterfaceActif;      //GO de l'image "actif" directement dans l'interface

    //Variables InGame
    public bool _ReserveRound;          //Est-ce que le joueur a réservé le round ?


    //Statistiques InGame
    public int[] _RessourcesPossedes = new int[4];      //=0 : blé, =1 : bois, =2 : fer, =3 : pierre 
    public int _VictoryPoints;              //Les points de Victoire : se gagnent au combat en éliminant ennemis, boss... ou dans certains batiments
    public int _SolidarityPoints;           //Les points de Solidarité : se gagnent en effectuant des actions prolifiques aux 2 joueurs
    public int _PrestigePoints;             //Les points de Prestige : se gagnent en réalisant des achievements
    public int _ActionStarsMax;
    public int _ActionStars;
    public float _TimeToPlay;               // = +Infini en général, valeur dans certains cas
    public bool isTimer;                    // Est-ce que le joueur a un temps imparti pour jouer ?
    public bool isActivePlayer;             // Est-ce que c'est à son tour de jouer ?
    public int[] _NbUnites;                 // =0 : soldats, =1 : chevaliers, =2 : barbares, =3 : cavaliers
    public int nbToursPossedees;            // Nombre de tours possédées par le joueur
    public bool[] _UpgradeRecolte;          //=0 : blé, =1 : bois, =2 : fer, =3 : pierre . Est-ce que le joueur a l'upgrade de la récolte
    public bool isFeuDeCamp;                // Est-ce que le joueur a le feu de camp ?
    public bool isPalissade;                // Est-ce que le joueur a la palissade ?
    public bool isEntrepot;                 // Est-ce que le joueur a l'entrepot ?
    public bool isGrue;                     // Est-ce que le joueur a la grue ?

    //GameObject relatifs
    public GameObject _panelInterfaceJoueur;        //Panel de l'interface du joueur
    public GameObject panelNbActionPerRound;            //Panel des actions par round
    public GameObject[] imagesAction;               //Tableau des images du nombre d'actions
    public Text victoirePointText;                  //Text du nombre de points de victoire
    public Text solidarityPointText;                //Text du nombre de points de solidarité
    public GameObject imagePrendreRound;            //Image indiquant que le joueur a pris la main pour le round


    //Compétences passives personnelles
    public int _NombreAutels;                       //Combien d'Autels possède le joueur ?
    public bool _HasFortin;                         //Est-ce que le joueur a le fortin ?
    public bool _HasEcoleDeMagie;                   //Est-ce que le joueur a l'école de magie ?
    public bool _HasFosse;                          //Est-ce que le joueur a la fosse ?
    public bool _HasMonumentAuxMorts;               //Est-ce que le joueur a le monument aux morts ?
    public bool _HasTenteSoins;                     //Est-ce que le joueur a la tente de soins ?


    //Nombre d'unités spéciales possédées
    public int _NbAssassinPossede;                  //Nombre d'Assassins possédés par le joueur


    #region Achievements
    //Gestion des Achievements
    //Achievement ponctuels et définitifs
    public bool _FullOwnConstructionArea;       //A rempli sa zone de construction le premier
    public int _NbOwnConstructionArea;          //Nombre total de zones de construction appartenant au joueur
    public int _ConstructionAreaBuild;          //Nombre de zones de construction construites dans la partie
    public bool _FirstBlood;                    //A versé le premier sang
    public bool _KillBoss;                      //A achevé le Boss

    //Achievements calculés à la fin du jeu et variables pendant la partie
    public bool _MoreRecolteRessources;         //A récolté le plus de ressources
    public int _NbRessourcesRecoltees;          //Nb de ressources récoltées
    public bool _FastestPlayer;                 //A joué le plus vite
    public float _TimeToMakeAction;             //Temps que mets le joueur à faire une action
    public bool _MoreVictoryPoints;             //A gagné le plus de points de victoire
    public bool _MoreSolidarityPoints;          //A gagné le plus de points de solidarité
    public bool _MoreBatimentsConstruits;       //A construit le plus de bâtiments
    public int _NbBatimentsConstruits;          //Nb de bâtiments construits
    public bool _MoreTowersConstruites;         //A construit le plus de tours
    public int _NbTowersConstruites;            //Nb de tours construites
    public bool _MoreTowersUpgrade;             //A amélioré le plus de tours
    public int _NbTowersUpgrade;                //Nb de tours améliorées
    public bool _MoreUnitesRecrutees;           //A recruté le plus d'unité
    public int _NbUnitesRecrutees;              //Nb d'unités recrutées
    public bool _MoreSuperUnitesRecrutees;      //A recruté le plus de supers unités
    public int _NbSuperUnitesRecrutees;         //Nb de super unités recrutées
    public bool _MoreEnnemisKilled;             //A tué le plus d'ennemis
    public int _NbEnnemisKilled;                //Nb d'ennemis tués
    public bool _MoreReparePorte;               //A le plus réparé la porte de la ville
    public float _NbPvReparePorte;              //Nb de pv réparés à la porte de la ville
    #endregion





    //Fonction Constructeur du Joueur. Attribue les valeurs aux variables depuis répertoire des sprites
    public Joueur(string nom,Sprite avatar,bool ia,int numJoueur,bool _isModeDebug, Sprite imagePionActif)
    {
        repertoireSprite = GameObject.Find("Main Camera").GetComponent<RepertoireDesSprites>();
        levelManager = GameObject.Find("Main Camera").GetComponent<LevelManager>();

        _NomJoueur = nom;
        _PortraitJoueur = avatar;
        isIA = ia;
        _NumeroJoueur = numJoueur;
        _panelInterfaceJoueur = GameObject.Find("PanelInterfaceJoueurs").transform.GetChild(numJoueur - 1).gameObject;
        _ImagePionActif = imagePionActif;


        //Interface Joueur Création
        _panelInterfaceJoueur.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = _PortraitJoueur;
        _panelInterfaceJoueur.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = _NomJoueur;

        imagePrendreRound = _panelInterfaceJoueur.transform.GetChild(9).gameObject;

        for (int ii = 0; ii < _RessourcesPossedes.Length; ii++)
        {
            if (_isModeDebug)
            {
                _RessourcesPossedes[ii] = 1000;
            } else
            {
                _RessourcesPossedes[ii] = 0;
            }
            
            AfficheNbRessources(ii);      //+2 pour prendre en compte l'ordre hiérarchique dans le panel
        }

        panelNbActionPerRound = _panelInterfaceJoueur.transform.Find("PanelNbActionPerRound").gameObject;
        imagesAction = new GameObject[panelNbActionPerRound.transform.childCount];
        for(int ii = 0; ii < imagesAction.Length; ii++)
        {
            imagesAction[ii] = panelNbActionPerRound.transform.GetChild(ii).GetChild(0).gameObject;
        }

        /*ImageInterfaceActif = _panelInterfaceJoueur.transform.Find("ImageActif").gameObject;
        _panelInterfaceJoueur.transform.Find("ImageContour").SetAsFirstSibling();
        ImageInterfaceActif.transform.SetAsFirstSibling();*/

        //Tableau du compte des types d'unités du joueur
        _NbUnites = new int[repertoireSprite.unitesJoueurData.Length];

        // Points de Victoire et Solidarité
        _VictoryPoints = 0;
        _SolidarityPoints = 0;
        victoirePointText = _panelInterfaceJoueur.transform.Find("ImagePointsVictoireSolid").Find("TextVictoire").GetChild(0).GetComponent<Text>();
        solidarityPointText = _panelInterfaceJoueur.transform.Find("ImagePointsVictoireSolid").Find("TextSolidarite").GetChild(0).GetComponent<Text>();
        AffichePointVictoireSolidarite(victoirePointText, _VictoryPoints);
        AffichePointVictoireSolidarite(solidarityPointText, _SolidarityPoints);

        //Compétences Passives
        _NombreAutels = 0;
        _UpgradeRecolte = new bool[4];

        //Initialisation abritraire du temps pour jouer 
        _TimeToMakeAction = 10000f;
    }

    //Affiche le nombre de ressources que possède le joueur
    public void AfficheNbRessources(int typeRessource)
    {
        _panelInterfaceJoueur.transform.GetChild(typeRessource + 1).GetChild(0).GetComponent<Text>().text = _RessourcesPossedes[typeRessource].ToString();      //+3 pour prendre en compte l'ordre hiérarchique dans le panel
    }


    //Désactive l'image de l'action effectuée par le joueur durant le round
    public void DesactiveImageActionRound(int nbAction)
    {
        if (nbAction < 2)
        {
            imagesAction[0].SetActive(false);
        }
        else
        {
            imagesAction[1].SetActive(false);
        }
    }

    //Active les images d'actions pendant un round du joueur
    public void ActiveImagesActionRound()
    {
        for(int ii = 0; ii < imagesAction.Length; ii++)
        {
            imagesAction[ii].SetActive(true);
        }
    }


    //Affiche points de solidarité et de victoire
    public void AffichePointVictoireSolidarite (Text _pointToAffiche,int _points)
    {
        _pointToAffiche.text = _points.ToString();
    }

}
