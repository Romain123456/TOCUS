using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class LevelManager : MonoBehaviour
{
    #region Mode Débug
    //MODE DEBUG
    public bool isModeDebug;
    public int uniteRecrutableCaserne;
    #endregion

    #region Elements du niveau
    //Caracteristiques du Niveau
    public string nomNiveau;
    [HideInInspector] public JSonParameters fileJson;
    private float precisionFloat;

    [HideInInspector] public RepertoireDesSprites repertoireSprites;
    public EventSystem myEventSystem;
    [HideInInspector] public Camera maMainCamera;
    #endregion

    #region Création de la Map
    [HideInInspector] public ParametresCarteOpenning paramCarte;
    #endregion


    #region Batiments et interractions
    [HideInInspector] public RessourceRecolte _ChampBle;
    [HideInInspector] public RessourceRecolte _RecolteBois;
    [HideInInspector] public RessourceRecolte _MineFer;
    [HideInInspector] public RessourceRecolte _CarrierePierre;
    [HideInInspector] public int[] productionRecoltes;

    [HideInInspector] public BatimentConstructions _CampMilitaire;
    [HideInInspector] public BatimentConstructions _ChantierTours;
    [HideInInspector] public BatimentConstructions _ChantierBTP;

    [HideInInspector] public PorteVille _PorteVille;

    [HideInInspector] public Chateau _Chateau;

    //Panel d'interaction avec les bâtiments
    [HideInInspector] public Transform panelParentBatimentInterract;

    //Objets du niveau
    //public Image backgroundImage;
    //public Image mapImage;
    private int nbEmplacementBTP;
    private EmptyBatimentsConstruits[] batimentsBTP;
    [HideInInspector] public int nbTours;
    private EmptyBatimentsConstruits[] tours;
    [HideInInspector] public GameObject _GuizmoRotationTour;
    [HideInInspector] public Button _CloseGuizmoRotTourButton;
    #endregion

    #region Unités et ennemis
    public GameObject uniteBasePrefab;

    [HideInInspector] public int nbInstanceReserves;
    [HideInInspector] public int nbMiniInstancesInactiveReserves;                   //Nombre minimum d'instances inactives
    [HideInInspector] public Transform reservePrefabUnitesJoueur;
    [HideInInspector] public Transform reservePrefabEnnemis;
    [HideInInspector] public Transform reservePrefabSuperUnitesJoueur;

    //Unites
    //Chemins
    [HideInInspector] public Vector2[][] positionsChemin;           //Tableau de tableaux des positions des chemins //=0 : Libre, =1 : Unité joueur, =2 : Ennemi
    [HideInInspector] public Transform[][] cheminOccupantTransform; //Tableau de tableaux des occupants des chemins
    private JsonEnnemiPositions dataCheminsJson;             //Json des données des chemins

    //Emptys de réserve
    /*
    
    //Unites
    [HideInInspector] public Transform[] reserveTypePrefabUnitesJoueur;
    [HideInInspector] public Transform[,] prefabUnitesJoueur;
    //Ennemis
    [HideInInspector] public Transform[] reserveTypePrefabEnnemis;
    [HideInInspector] public Transform[,] prefabEnnemis;
    //Super Unités
    [HideInInspector] public Transform[] reserveTypePrefabSuperUnitesJoueur;
    [HideInInspector] public Transform[,] prefabSuperUnitesJoueur;*/
    #endregion

    #region Joueurs
    //Les Joueurs
    [HideInInspector] public Joueur joueur1;
    [HideInInspector] public Joueur joueur2;
    [HideInInspector] public Joueur[] tableauJoueurs = new Joueur[2];
    
    //Le "Pion" du joueur actif
    [HideInInspector] public GameObject _PionJoueurActif;
    [HideInInspector] public GameObject _ImageChangementJoueurActif;
    private RectTransform _PionJoueurActifTransform;
    private Image _PionJoueurActifImage;
    [HideInInspector] public int _JoueurActif;
    [HideInInspector] public int nbActionRealised;              //Compte le nombre d'action réalisée dans le round


    //Actions Joueur
    [HideInInspector] public Transform reservePionsUI;          //reserve de pions UI pour animations des actions joueur
    public GameObject prefabPionsUI;            //prefab pions action joueur
    [HideInInspector] public int nbRessourcesRecup;         //Nombre de ressources récupérées par le joueur
    [HideInInspector] public string typeActionJoueur;           //Quel type d'action le joueur a réalisé ?
    [HideInInspector] public List<GameObject> objetConstruit = new List<GameObject>();         //Liste d'objets construits par le joueur 
    public Sprite[] ressourcesSprites;                      //Tableau des sprites des ressources
    public Sprite[] pionOccupesSprites;                    //Tableau des sprites pour occuper le bâtiment
    [HideInInspector] public int[] indexSpriteRessources;       //Ressources utilisées par les sprites des pions UI
    [HideInInspector] public Vector3 scaleConstruction;         //Stocke l'échelle de la construction
    [HideInInspector] public List<GameObject> listUnitesAnimationActionJoueur = new List<GameObject>();         //Liste de GO des sprites des unités qui seront animées
    [HideInInspector] public List<Vector3> listUnitesAnimationActionPositions = new List<Vector3>();            //Liste des positions cibles des sprites des unités
    [HideInInspector] public GameObject imageChangementJoueur;      //GO de l'image animée lors du changement de joueur actif
    [HideInInspector] public GameObject[] pionsActionJoueursGO;     //GO pour stocker les pions d'actions utilisés par les joueurs
    #endregion

    #region Evolution du jeu
    //Evolution du jeu
    [HideInInspector] public bool isEndGame;                    //Est-ce que la partie est terminée
    [HideInInspector] public int numeroRound;
    [HideInInspector] public int nbMaxRounds;
    [HideInInspector] public Text textRounds;
    [HideInInspector] public int roundActivationDeploiementEnnemi;                  //Round auquel on lance de déploiement des ennemis (si =0, pas d'ennemis dans cette map) à adapter avec json
    [HideInInspector] public int[] periodeDeploiement;            //Variable temporaire, à adapter avec json
    //[HideInInspector] public int[,] ennemisPerRound;
    [HideInInspector] public GameObject panelVictoire;
    [HideInInspector] public GameObject panelDefaite;
    [HideInInspector] public bool isLose;       //Est-ce qu'on a perdu ?
    [HideInInspector] public bool isMaisonConstruite;            //Est-ce qu'on a construit la maison ?
    [HideInInspector] public int NbVictoryPointsPerAutel;                     //Nombre de points de victoire par autel
    [HideInInspector] public int NbAssassinMaxPerJoueur;                      //Nombre d'Assassins max par joueur
    [HideInInspector] public GameObject buttonSelectedTemp;             //GameObject Temporaire pour récupérer l'information d'un ancien objet sélectionné. Utile pour le mortier 
    [HideInInspector] public int _NbVictoryPointsFosse;                   //Nombre de points de victoire par ennemi tué si on a la fosse
    [HideInInspector] public bool isCabaneEclaireur;          //Est-ce qu'on a construit la cabane d'éclaireur ?
    public int _SecondesBeforeMonstres;                        //Temps (en secondes) avant arrivée de vague pour donner l'alerte
    [HideInInspector] public int[,] prixToursParToursJoueur;        //Prix des tours en fonction du nombre de tours possédées par le joueur

    //PopUps pour messages destinés aux joueurs
    [HideInInspector] public Text textPopUps;               //Text PopUps (Changements possibles par la suite, voir le GD !)
    #endregion


    #region Elements des Achievements
    public GameObject imageAchRessources;
    public GameObject imageAchNbUnites;
    public GameObject imageAchNbSuperUnites;
    public GameObject imageAchTours;
    public GameObject imageAchBatiments;
    public GameObject imageAchReparerPorte;
    public GameObject imageAchRapidite;

    private bool topChrono;
    #endregion



   

    // Start is called before the first frame update
    void Start()
    {
        #region Introdution du Jeu
        fileJson = new JSonParameters();
        fileJson = fileJson.OuvertureJson(nomNiveau);
        precisionFloat = (float)fileJson.precisionFloat;

        repertoireSprites = this.GetComponent<RepertoireDesSprites>();

        myEventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        maMainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        //Panels des états de la partie
        panelVictoire = GameObject.Find("PanelEtatPartie").transform.GetChild(0).gameObject;
        panelDefaite = GameObject.Find("PanelEtatPartie").transform.GetChild(1).gameObject;


        //Rounds de jeu
        pionsActionJoueursGO = new GameObject[4];
        numeroRound = 1;

        nbMaxRounds = ParametresCarteOpenning.ficOptionParam.i_nb_rounds_carte;

        textRounds = GameObject.Find("TextRound").GetComponent<Text>();
        textRounds.text = "Round : \n" + numeroRound.ToString() + "/" + nbMaxRounds.ToString();
        roundActivationDeploiementEnnemi = ParametresCarteOpenning.ficOptionParam.objet_vagues.o_vagues.i_nb_rounds_avant_premiere_vague;
        periodeDeploiement = new int[ParametresCarteOpenning.ficOptionParam.objet_vagues.o_vagues.o_structure_vagues.Length];
        for (int ii = 0; ii < periodeDeploiement.Length; ii++)
        {
            periodeDeploiement[ii] = ParametresCarteOpenning.ficOptionParam.objet_vagues.o_vagues.o_structure_vagues[ii].i_tps;
        }


        #region Unités Création
        #region PositionChemins
        dataCheminsJson = new JsonEnnemiPositions();
        dataCheminsJson = dataCheminsJson.OuvertureJson(nomNiveau + "_EnnemisPaths");
        positionsChemin = new Vector2[3][];
        cheminOccupantTransform = new Transform[3][];

        positionsChemin[0] = new Vector2[dataCheminsJson.ennemi1PosX.Length];
        cheminOccupantTransform[0] = new Transform[positionsChemin[0].Length];
        positionsChemin[1] = new Vector2[dataCheminsJson.ennemi2PosX.Length];
        cheminOccupantTransform[1] = new Transform[positionsChemin[1].Length];
        positionsChemin[2] = new Vector2[dataCheminsJson.ennemi3PosX.Length];
        cheminOccupantTransform[2] = new Transform[positionsChemin[2].Length];


        for (int jj = 0; jj < positionsChemin[0].Length; jj++)
        {
            positionsChemin[0][jj].x = (float)(dataCheminsJson.ennemi1PosX[jj]) / precisionFloat;
            positionsChemin[0][jj].y = (float)(dataCheminsJson.ennemi1PosY[jj]) / precisionFloat;
        }
        for (int jj = 0; jj < positionsChemin[1].Length; jj++)
        {
            positionsChemin[1][jj].x = (float)(dataCheminsJson.ennemi2PosX[jj]) / precisionFloat;
            positionsChemin[1][jj].y = (float)(dataCheminsJson.ennemi2PosY[jj]) / precisionFloat;
        }
        for (int jj = 0; jj < positionsChemin[2].Length; jj++)
        {
            positionsChemin[2][jj].x = (float)(dataCheminsJson.ennemi3PosX[jj]) / precisionFloat;
            positionsChemin[2][jj].y = (float)(dataCheminsJson.ennemi3PosY[jj]) / precisionFloat;
        }


        for (int ii = 0; ii < cheminOccupantTransform.Length; ii++)
        {
            for(int jj = 0; jj < cheminOccupantTransform[ii].Length; jj++)
            {
                cheminOccupantTransform[ii][jj] = null;
            }
        }
        #endregion

        nbInstanceReserves = 5;
        nbMiniInstancesInactiveReserves = 4;
        #region Unités Joueur
        reservePrefabUnitesJoueur = GameObject.Find("ReservePrefabUnitesJoueur").transform;
        repertoireSprites.UniteJoueurCreate();
        InstantiateReservesEmpty(repertoireSprites.unitesJoueurData.Length, "ReservePrefabUnitesJoueur_Type", reservePrefabUnitesJoueur,repertoireSprites.unitesJoueurData);         //On instantie les réserves d'unités

        for (int jj = 0; jj < nbInstanceReserves; jj++)
        {
            for (int ii = 0; ii < reservePrefabUnitesJoueur.childCount; ii++)
            {
                InstantiateUnite(ii);
            }
        }
        #endregion

        #region Super Unités
        reservePrefabSuperUnitesJoueur = GameObject.Find("ReservePrefabSuperUnitesJoueur").transform;
        repertoireSprites.SuperUnitesJoueurCreate();
        InstantiateReservesEmpty(repertoireSprites.superUnitesJoueurData.Length,"ReservePrefabSuperUnitesJoueur_Type",reservePrefabSuperUnitesJoueur,repertoireSprites.superUnitesJoueurData);

        for (int jj = 0; jj < nbInstanceReserves; jj++)
        {
            for (int ii = 0; ii < reservePrefabSuperUnitesJoueur.childCount; ii++)
            {
                InstantiateSuperUnite(ii);
            }
        }
        #endregion

        #region Ennemi
        reservePrefabEnnemis = GameObject.Find("ReservePrefabEnnemis").transform;
        repertoireSprites.EnnemiDataCreate();
        InstantiateReservesEmpty(repertoireSprites.ennemiData.Length,"ReservePrefabEnnemis_Type",reservePrefabEnnemis,repertoireSprites.ennemiData);

        for (int jj = 0; jj < nbInstanceReserves; jj++)
        {
            for (int ii = 0; ii < reservePrefabEnnemis.childCount; ii++)
            {
                InstantiateEnnemi(ii);
            }
        }
        #endregion



        #endregion

        //Gestion du Panel Interraction Batiment
        if (GameObject.Find("PanelBatimentInteract") != null)
        {
            panelParentBatimentInterract = GameObject.Find("PanelBatimentInteract").transform;
            panelParentBatimentInterract.gameObject.SetActive(false);
        }

        isMaisonConstruite = false;         //La maison n'est pas construite

        //Variables publiques des options
        NbVictoryPointsPerAutel = JsonParametresGlobaux.ficParamGlobaux.objet_divers.o_divers.i_gain_victoire_autel;
        _NbVictoryPointsFosse = JsonParametresGlobaux.ficParamGlobaux.objet_divers.o_divers.i_gain_victoire_de_la_fosse;
        NbAssassinMaxPerJoueur = JsonParametresGlobaux.ficParamGlobaux.objet_divers.o_divers.i_nb_assassin_max_par_joueur;
        #endregion


        #region     Initiations objets du plateau

        #region Création de la Map
        paramCarte = this.GetComponent<ParametresCarteOpenning>();
        paramCarte.backgroundImage.sprite = paramCarte._ReserveFondsCartes[paramCarte.level - 1];
        GameObject mursTransform = new GameObject("MursObject");
        mursTransform.AddComponent<SpriteRenderer>();
        mursTransform.GetComponent<SpriteRenderer>().sprite = paramCarte._ReserveMurs[paramCarte.level - 1];
        mursTransform.transform.position = new Vector3((float)fileJson.posMurVilleX / fileJson.precisionFloat,
            (float)fileJson.posMurVilleY / fileJson.precisionFloat,
            (float)fileJson.posMurVilleZ / fileJson.precisionFloat);
        mursTransform.transform.localScale = new Vector3((float)fileJson.scaleXMurVille / fileJson.precisionFloat,
            (float)fileJson.scaleYMurVille / fileJson.precisionFloat,
            (float)fileJson.scaleZMurVille / fileJson.precisionFloat);


        #region Arbres
        GameObject mesArbres = new GameObject("Arbres_Parent");
        mesArbres.transform.position = Vector3.zero;
        mesArbres.transform.localScale = Vector3.one;
        for (int ii = 0; ii < fileJson.posX_Arbre_Type1.Length; ii++)
        {
            GameObject monArbre = new GameObject("Arbre_Type1");
            monArbre.transform.parent = mesArbres.transform;
            SpriteRenderer arbreSprite = monArbre.AddComponent<SpriteRenderer>();
            arbreSprite.sprite = paramCarte._ReserveArbres[0];
            monArbre.transform.position = new Vector3(fileJson.posX_Arbre_Type1[ii] / fileJson.precisionFloat, (float)fileJson.posY_Arbre_Type1[ii] / fileJson.precisionFloat, (float)fileJson.posZ_Arbre_Type1[ii] / fileJson.precisionFloat);
            monArbre.transform.localScale = new Vector3((float)fileJson.scaleX_Arbre_Type1[ii]/fileJson.precisionFloat, (float)fileJson.scaleY_Arbre_Type1[ii] / fileJson.precisionFloat, (float)fileJson.scaleZ_Arbre_Type1[ii] / fileJson.precisionFloat);

        }

        for (int ii = 0; ii < fileJson.posX_Arbre_Type2.Length; ii++)
        {
            GameObject monArbre = new GameObject("Arbre_Type2");
            monArbre.transform.parent = mesArbres.transform;
            SpriteRenderer arbreSprite = monArbre.AddComponent<SpriteRenderer>();
            arbreSprite.sprite = paramCarte._ReserveArbres[1];
            monArbre.transform.position = new Vector3(fileJson.posX_Arbre_Type2[ii] / fileJson.precisionFloat, (float)fileJson.posY_Arbre_Type2[ii] / fileJson.precisionFloat, (float)fileJson.posZ_Arbre_Type2[ii] / fileJson.precisionFloat);
            monArbre.transform.localScale = new Vector3((float)fileJson.scaleX_Arbre_Type2[ii] / fileJson.precisionFloat, (float)fileJson.scaleY_Arbre_Type2[ii] / fileJson.precisionFloat, (float)fileJson.scaleZ_Arbre_Type2[ii] / fileJson.precisionFloat);

        }

        for (int ii = 0; ii < fileJson.posX_Arbre_Type3.Length; ii++)
        {
            GameObject monArbre = new GameObject("Arbre_Type3");
            monArbre.transform.parent = mesArbres.transform;
            SpriteRenderer arbreSprite = monArbre.AddComponent<SpriteRenderer>();
            arbreSprite.sprite = paramCarte._ReserveArbres[2];
            monArbre.transform.position = new Vector3(fileJson.posX_Arbre_Type3[ii] / fileJson.precisionFloat, (float)fileJson.posY_Arbre_Type3[ii] / fileJson.precisionFloat, (float)fileJson.posZ_Arbre_Type3[ii] / fileJson.precisionFloat);
            monArbre.transform.localScale = new Vector3((float)fileJson.scaleX_Arbre_Type3[ii] / fileJson.precisionFloat, (float)fileJson.scaleY_Arbre_Type3[ii] / fileJson.precisionFloat, (float)fileJson.scaleZ_Arbre_Type3[ii] / fileJson.precisionFloat);

        }

        for (int ii = 0; ii < fileJson.posX_Arbre_Type4.Length; ii++)
        {
            GameObject monArbre = new GameObject("Arbre_Type4");
            monArbre.transform.parent = mesArbres.transform;
            SpriteRenderer arbreSprite = monArbre.AddComponent<SpriteRenderer>();
            arbreSprite.sprite = paramCarte._ReserveArbres[3];
            monArbre.transform.position = new Vector3(fileJson.posX_Arbre_Type4[ii] / fileJson.precisionFloat, (float)fileJson.posY_Arbre_Type4[ii] / fileJson.precisionFloat, (float)fileJson.posZ_Arbre_Type4[ii] / fileJson.precisionFloat);
            monArbre.transform.localScale = new Vector3((float)fileJson.scaleX_Arbre_Type4[ii] / fileJson.precisionFloat, (float)fileJson.scaleY_Arbre_Type4[ii] / fileJson.precisionFloat, (float)fileJson.scaleZ_Arbre_Type4[ii] / fileJson.precisionFloat);

        }

        for (int ii = 0; ii < fileJson.posX_Arbre_Type5.Length; ii++)
        {
            GameObject monArbre = new GameObject("Arbre_Type5");
            monArbre.transform.parent = mesArbres.transform;
            SpriteRenderer arbreSprite = monArbre.AddComponent<SpriteRenderer>();
            arbreSprite.sprite = paramCarte._ReserveArbres[4];
            monArbre.transform.position = new Vector3(fileJson.posX_Arbre_Type5[ii] / fileJson.precisionFloat, (float)fileJson.posY_Arbre_Type5[ii] / fileJson.precisionFloat, (float)fileJson.posZ_Arbre_Type5[ii] / fileJson.precisionFloat);
            monArbre.transform.localScale = new Vector3((float)fileJson.scaleX_Arbre_Type5[ii] / fileJson.precisionFloat, (float)fileJson.scaleY_Arbre_Type5[ii] / fileJson.precisionFloat, (float)fileJson.scaleZ_Arbre_Type5[ii] / fileJson.precisionFloat);

        }
        #endregion


        #region Pierres
        GameObject mesPierres = new GameObject("Pierres_Parent");
        mesPierres.transform.position = Vector3.zero;
        mesPierres.transform.localScale = Vector3.one;
        for (int ii = 0; ii < fileJson.posX_Pierre_Type1.Length; ii++)
        {
            GameObject maPierre = new GameObject("Pierre_Type1");
            maPierre.transform.parent = mesPierres.transform;
            SpriteRenderer pierreSprite = maPierre.AddComponent<SpriteRenderer>();
            pierreSprite.sprite = paramCarte._ReservePierres[0];
            maPierre.transform.position = new Vector3(fileJson.posX_Pierre_Type1[ii] / fileJson.precisionFloat, (float)fileJson.posY_Pierre_Type1[ii] / fileJson.precisionFloat, (float)fileJson.posZ_Pierre_Type1[ii] / fileJson.precisionFloat);
            maPierre.transform.localScale = new Vector3((float)fileJson.scaleX_Pierre_Type1[ii] / fileJson.precisionFloat, (float)fileJson.scaleY_Pierre_Type1[ii] / fileJson.precisionFloat, (float)fileJson.scaleZ_Pierre_Type1[ii] / fileJson.precisionFloat);

        }

        #endregion
        #endregion

        #region Achievements
        imageAchRessources.SetActive(false);
        imageAchNbUnites.SetActive(false);
        imageAchNbSuperUnites.SetActive(false);
        imageAchTours.SetActive(false);
        imageAchBatiments.SetActive(false);
        imageAchReparerPorte.SetActive(false);
        imageAchRapidite.SetActive(false);
        #endregion


        #region Joueur
        //Joueur
        if (AvatarGestion.avatarChoisis[0] != null)
        {
            joueur1 = new Joueur(AvatarGestion.avatarChoisis[0].nomAvatar, AvatarGestion.avatarChoisis[0].portraitAvatar, false, 1, isModeDebug, repertoireSprites.pionActif[0]);
            joueur2 = new Joueur(AvatarGestion.avatarChoisis[1].nomAvatar, AvatarGestion.avatarChoisis[1].portraitAvatar, false, 2, isModeDebug, repertoireSprites.pionActif[1]);
        } 
        //Possibilité lors des tests de développement
        else
        {
            joueur1 = new Joueur("Testeur 1", null, false, 1, isModeDebug, repertoireSprites.pionActif[0]);
            joueur2 = new Joueur("Testeur 2", null, false, 2, isModeDebug, repertoireSprites.pionActif[1]);
        }

        Debug.Log("Si besoin d'activer les Palissades pour tests.");
        /*joueur1.isPalissade = true;
        joueur2.isPalissade = true;*/

        tableauJoueurs[0] = joueur1;
        tableauJoueurs[1] = joueur2;

        

        _JoueurActif = 1;
        imageChangementJoueur = GameObject.Find("ImageChangementJoueur");
        imageChangementJoueur.SetActive(false);
        _PionJoueurActif = GameObject.Find("ImagePionJoueurActif");
        _ImageChangementJoueurActif = GameObject.Find("ImageChangementJoueurActif");
        _ImageChangementJoueurActif.SetActive(false);
        reservePionsUI = GameObject.Find("ReservePionsUI").transform;
        _PionJoueurActifTransform = _PionJoueurActif.GetComponent<RectTransform>();
        _PionJoueurActifImage = _PionJoueurActifTransform.GetComponent<Image>();
        
        SetJoueurActif();
        #endregion



        #region Les Ressources
        //Instanciation d'un champ de blé
        _ChampBle = new RessourceRecolte(repertoireSprites.champBleSprite, "ChampDeBle", true);
        _ChampBle.scaleObjet = new Vector2((float)fileJson.scaleXRecoltes[0] / precisionFloat, (float)fileJson.scaleYRecoltes[0] / precisionFloat);
        _ChampBle.transformObjet.localScale = _ChampBle.scaleObjet;
        _ChampBle.transformObjet.name = _ChampBle.nomObjet;
        _ChampBle.nbDispo = 3;
        _ChampBle.SetNbRessourceDispo();
        _ChampBle.transformObjet.position = new Vector2((float)fileJson.positionXRecoltes[0] / precisionFloat, (float)fileJson.positionYRecoltes[0] / precisionFloat);
        _ChampBle.ScalingChildElement(_ChampBle.nbTextRessources_GO);
        _ChampBle.buttonObjet.onClick.AddListener(delegate { _ChampBle.RecuperationRessources(); });
        _ChampBle.imageOccupe.GetComponent<RectTransform>().localScale = new Vector3(
            1f / _ChampBle.imageOccupe.transform.parent.parent.transform.localScale.x,
            1f / _ChampBle.imageOccupe.transform.parent.parent.transform.localScale.y,1);


        //Instanciation d'une récolte de bois
        _RecolteBois = new RessourceRecolte(repertoireSprites.recolteBoisSprite, "RecolteDeBois", true);
        _RecolteBois.scaleObjet = new Vector2((float)fileJson.scaleXRecoltes[1] / precisionFloat, (float)fileJson.scaleYRecoltes[1] / precisionFloat);
        _RecolteBois.transformObjet.localScale = _RecolteBois.scaleObjet;
        _RecolteBois.transformObjet.name = _RecolteBois.nomObjet;
        _RecolteBois.nbDispo = 2;
        _RecolteBois.SetNbRessourceDispo();
        _RecolteBois.transformObjet.position = new Vector2((float)fileJson.positionXRecoltes[1] / precisionFloat, (float)fileJson.positionYRecoltes[1] / precisionFloat);
        _RecolteBois.ScalingChildElement(_RecolteBois.nbTextRessources_GO);
        _RecolteBois.buttonObjet.onClick.AddListener(delegate { _RecolteBois.RecuperationRessources(); });
        _RecolteBois.imageOccupe.GetComponent<RectTransform>().localScale = new Vector3(
            1f / _RecolteBois.imageOccupe.transform.parent.parent.transform.localScale.x,
            1f / _RecolteBois.imageOccupe.transform.parent.parent.transform.localScale.y, 1);


        //Instanciation d'une mine de fer
        _MineFer = new RessourceRecolte(repertoireSprites.mineFerSprite, "MineDeFer", true);
        _MineFer.scaleObjet = new Vector2((float)fileJson.scaleXRecoltes[2] / precisionFloat, (float)fileJson.scaleYRecoltes[2] / precisionFloat);
        _MineFer.transformObjet.localScale = _MineFer.scaleObjet;
        _MineFer.transformObjet.name = _MineFer.nomObjet;
        _MineFer.nbDispo = 3;
        _MineFer.SetNbRessourceDispo();
        _MineFer.transformObjet.position = new Vector2((float)fileJson.positionXRecoltes[2] / precisionFloat, (float)fileJson.positionYRecoltes[2] / precisionFloat);
        _MineFer.ScalingChildElement(_MineFer.nbTextRessources_GO);
        _MineFer.buttonObjet.onClick.AddListener(delegate { _MineFer.RecuperationRessources(); });
        _MineFer.imageOccupe.GetComponent<RectTransform>().localScale = new Vector3(1f / _MineFer.imageOccupe.transform.parent.parent.transform.localScale.x,1f / _MineFer.imageOccupe.transform.parent.parent.transform.localScale.y, 1);


        //Instanciation de carriere de pierre
        _CarrierePierre = new RessourceRecolte(repertoireSprites.carrierePierreSprite, "CarriereDePierre", true);
        _CarrierePierre.scaleObjet = new Vector2((float)fileJson.scaleXRecoltes[3] / precisionFloat, (float)fileJson.scaleYRecoltes[3] / precisionFloat);
        _CarrierePierre.transformObjet.localScale = _CarrierePierre.scaleObjet;
        _CarrierePierre.transformObjet.name = _CarrierePierre.nomObjet;
        _CarrierePierre.nbDispo = 2;
        _CarrierePierre.SetNbRessourceDispo();
        _CarrierePierre.transformObjet.position = new Vector2((float)fileJson.positionXRecoltes[3] / precisionFloat, (float)fileJson.positionYRecoltes[3] / precisionFloat);
        _CarrierePierre.ScalingChildElement(_CarrierePierre.nbTextRessources_GO);
        _CarrierePierre.buttonObjet.onClick.AddListener(delegate { _CarrierePierre.RecuperationRessources(); });
        _CarrierePierre.imageOccupe.GetComponent<RectTransform>().localScale = new Vector3(1f / _CarrierePierre.imageOccupe.transform.parent.parent.transform.localScale.x,1f / _CarrierePierre.imageOccupe.transform.parent.parent.transform.localScale.y, 1);


        // Production des récoltes
        productionRecoltes = new int[4];
        for(int ii = 0; ii < productionRecoltes.Length; ii++)
        {
            productionRecoltes[ii] = fileJson.productionRecoltes[ii];
        }
        #endregion


        #region Camp Militaire
        //Instanciation du camp Militaire
        _CampMilitaire = new BatimentConstructions(repertoireSprites.campMilitaireSprite, "CampMilitaire", true, 2);
        _CampMilitaire.scaleObjet = new Vector2((float)fileJson.scaleXCampMilitaire / precisionFloat, (float)fileJson.scaleYCampMilitaire / precisionFloat);
        _CampMilitaire.transformObjet.localScale = _CampMilitaire.scaleObjet;
        _CampMilitaire.transformObjet.name = _CampMilitaire.nomObjet;
        _CampMilitaire.transformObjet.position = new Vector2((float)fileJson.positionXCampMilitaire / precisionFloat, (float)fileJson.positionYCampMilitaire / precisionFloat);
        _CampMilitaire.buttonObjet.onClick.AddListener(delegate { _CampMilitaire.OuverturePanelInterractBatiment(_CampMilitaire.indicePanelInterractBatiment); });
        _CampMilitaire.panelInteractText.text = "Quelle Unité choisir ?";
        _CampMilitaire.CreationBouton();
        _CampMilitaire.imageOccupe.GetComponent<RectTransform>().localScale = new Vector3(
            1f / _CampMilitaire.imageOccupe.transform.parent.parent.transform.localScale.x,
            1f / _CampMilitaire.imageOccupe.transform.parent.parent.transform.localScale.y, 1);

        //Bouton Recruter Unité assignation
        _CampMilitaire._InterractionsDisponibles[0].GetComponent<Button>().onClick.AddListener(delegate { _CampMilitaire.SelectionCreationConstruire(); });

        //Bouton Améliorer Unité assignation (Mettre Fonction activer boutons d'amélioration si on a plus d'1 type d'unités
        if (reservePrefabUnitesJoueur.childCount <= 1)
        {
            _CampMilitaire._InterractionsDisponibles[1].GetComponent<Button>().onClick.AddListener(delegate { _CampMilitaire.AmeliorerUnite(); });
        } else
        {
            _CampMilitaire._InterractionsDisponibles[1].GetComponent<Button>().onClick.AddListener(delegate { _CampMilitaire.ActiveAmeliorationUnite(); });
            StartCoroutine(FonctionsBoutonsAmeliorationUnite());
        }

        //}

        //Scale des unités en attente
        _CampMilitaire.spritesUnitesFiles.transform.localScale = new Vector2(_CampMilitaire.spritesUnitesFiles.transform.localScale.x / _CampMilitaire.transformObjet.localScale.x, _CampMilitaire.spritesUnitesFiles.transform.localScale.y / _CampMilitaire.transformObjet.localScale.y) * 0.4f;

        #endregion


        #region Les Tours
        nbTours = fileJson.nbTours;
        tours = new EmptyBatimentsConstruits[nbTours];

        //Prix des tours en fonction du nombre de tours possédées par le joueur 
        prixToursParToursJoueur = JsonParametresGlobaux.ficParamGlobaux.objet_cout_achat_tours_selon_nombre.o_cout_achat_tours_selon_nombre.arr_cout_achat_tour_joueur_possede_2D;

        for (int ii = 0; ii < nbTours; ii++)
        {
            tours[ii] = new EmptyBatimentsConstruits(repertoireSprites.emptyTourSprite, "PlacementTour", true, "EmplacementTour");
            tours[ii].scaleObjet = new Vector2((float)fileJson.scaleXTours[ii] / precisionFloat, (float)fileJson.scaleYTours[ii] / precisionFloat);
            tours[ii].transformObjet.GetChild(0).localScale = tours[ii].scaleObjet;
            tours[ii].transformObjet.name = tours[ii].nomObjet;
            tours[ii].transformObjet.position = new Vector2((float)fileJson.positionXTours[ii] / precisionFloat, (float)fileJson.positionYTours[ii] / precisionFloat);
            tours[ii].modifAccesBat = fileJson.modifAccessTours[ii];
            tours[ii].ScalingChildElement(tours[ii].imageFlagJoueur);
            tours[ii].FlagBatimentAcces();
            StartCoroutine(FonctionsBoutonsPanelBatiment(tours[ii].buttonObjet,tours[ii]));
            Destroy(tours[ii].imageOccupe.gameObject);
        }

        //Chargement des types de tours
        repertoireSprites.nbDataTours = nbTours;
        repertoireSprites.ToursCreate();


        //Instanciation du batiment construction Tours
        _ChantierTours = new BatimentConstructions(repertoireSprites.chantierTourSprite, "ChantierTours", true,0);
        _ChantierTours.scaleObjet = new Vector2((float)fileJson.scaleXConstructTours / precisionFloat, (float)fileJson.scaleYConstructTours / precisionFloat);
        _ChantierTours.transformObjet.localScale = _ChantierTours.scaleObjet;
        _ChantierTours.transformObjet.name = _ChantierTours.nomObjet;
        _ChantierTours.transformObjet.position = new Vector2((float)fileJson.positionXConstructTours / precisionFloat, (float)fileJson.positionYConstructTours / precisionFloat);
        _ChantierTours.buttonObjet.onClick.AddListener(delegate { _ChantierTours.OuverturePanelInterractBatiment(_ChantierTours.indicePanelInterractBatiment); });
        _ChantierTours.panelInteractText.text = "Tour à construire ?";
        _ChantierTours.CreationBouton();
        _ChantierTours.imageOccupe.GetComponent<RectTransform>().localScale = new Vector3(1f / _ChantierTours.imageOccupe.transform.parent.parent.transform.localScale.x, 1f / _ChantierTours.imageOccupe.transform.parent.parent.transform.localScale.y, 1);

        //Activation/Désactivation des boutons/surplus de boutons
        for (int ii=0;ii< panelParentBatimentInterract.GetChild(0).GetChild(1).childCount; ii++)
        {
            if(ii >= _ChantierTours._InterractionsDisponibles.Length)
            {
                panelParentBatimentInterract.GetChild(0).GetChild(1).GetChild(ii).gameObject.SetActive(false);
            }
        }

        for (int ii = 0; ii < _ChantierTours._InterractionsDisponibles.Length; ii++)
        {
            //Sélectionne la tour à construire
            _ChantierTours._InterractionsDisponibles[ii].GetComponent<Button>().onClick.AddListener(delegate { _ChantierTours.SelectionCreationConstruire(); });
        }
        _ChantierTours._EmplacementsCreations = new EmptyBatimentsConstruits[tours.Length];
        for(int ii = 0; ii < tours.Length; ii++)
        {
            _ChantierTours._EmplacementsCreations[ii] = tours[ii];
            tours[ii].batimentGestion = _ChantierTours;
        }
        #endregion

        #region Les Batiments Constructibles
        //Emplacements BTP
        nbEmplacementBTP = fileJson.nbEmplacementBTP;
        batimentsBTP = new EmptyBatimentsConstruits[nbEmplacementBTP];
        for (int ii = 0; ii < nbEmplacementBTP; ii++)
        {
            batimentsBTP[ii] = new EmptyBatimentsConstruits(repertoireSprites.emptyBTPSprite, "PlacementBatiment", true, "EmplacementBTP");
            batimentsBTP[ii].scaleObjet = new Vector2((float)fileJson.scaleXBTP[ii] / precisionFloat, (float)fileJson.scaleYBTP[ii] / precisionFloat);
            batimentsBTP[ii].transformObjet.localScale = batimentsBTP[ii].scaleObjet;
            batimentsBTP[ii].transformObjet.name = batimentsBTP[ii].nomObjet;
            batimentsBTP[ii].transformObjet.position = new Vector2((float)fileJson.positionXBTP[ii] / precisionFloat, (float)fileJson.positionYBTP[ii] / precisionFloat);
            batimentsBTP[ii].modifAccesBat = fileJson.modifAccesBTP[ii];
            batimentsBTP[ii].FlagBatimentAcces();
            batimentsBTP[ii].ScalingChildElement(batimentsBTP[ii].imageFlagJoueur);
            StartCoroutine(FonctionsBoutonsPanelBatiment(batimentsBTP[ii].buttonObjet, batimentsBTP[ii]));
            Destroy(batimentsBTP[ii].imageOccupe.gameObject);
        }

        //Chargement des Batiments
        //repertoireSprites.nbDataBatiments = nbEmplacementBTP;
        repertoireSprites.BatimentsCreate();

        //Instanciation du batiment chantierBTP
        _ChantierBTP = new BatimentConstructions(repertoireSprites.chantierBTPSprite, "ChantierBTP", true,1);
        _ChantierBTP.scaleObjet = new Vector2((float)fileJson.scaleXConstructBTP / precisionFloat, (float)fileJson.scaleYConstructBTP / precisionFloat);
        _ChantierBTP.transformObjet.localScale = _ChantierBTP.scaleObjet;
        _ChantierBTP.transformObjet.name = _ChantierBTP.nomObjet;
        _ChantierBTP.transformObjet.position = new Vector2((float)fileJson.positionXConstructBTP / precisionFloat, (float)fileJson.positionYConstructBTP / precisionFloat);
        _ChantierBTP.buttonObjet.onClick.AddListener(delegate { _ChantierBTP.OuverturePanelInterractBatiment(_ChantierBTP.indicePanelInterractBatiment); });
        _ChantierBTP.panelInteractText.text = "Quel Batiment construire ?";
        _ChantierBTP.CreationBouton();
        _ChantierBTP.imageOccupe.GetComponent<RectTransform>().localScale = new Vector3(1f / _ChantierBTP.imageOccupe.transform.parent.parent.transform.localScale.x, 1f / _ChantierBTP.imageOccupe.transform.parent.parent.transform.localScale.y, 1);

        for (int ii = 0; ii < _ChantierBTP._InterractionsDisponibles.Length; ii++)
        {
            _ChantierBTP._InterractionsDisponibles[ii].GetComponent<Button>().onClick.AddListener(delegate { _ChantierBTP.SelectionCreationConstruire(); });
        }
        _ChantierBTP._EmplacementsCreations = new EmptyBatimentsConstruits[batimentsBTP.Length];
        for (int ii = 0; ii < batimentsBTP.Length; ii++)
        {
            _ChantierBTP._EmplacementsCreations[ii] = batimentsBTP[ii];
            batimentsBTP[ii].batimentGestion = _ChantierBTP;
        }
        #endregion

        #region La Ville
        //Instanciation de la porte de la ville
        _PorteVille = new PorteVille(repertoireSprites.porteVilleSprite, "PorteVille", true);
        _PorteVille.scaleObjet = new Vector2((float)fileJson.scaleXPorte/precisionFloat, (float)fileJson.scaleYPorte/precisionFloat);
        _PorteVille.transformObjet.localScale = _PorteVille.scaleObjet;
        _PorteVille.transformObjet.name = _PorteVille.nomObjet;
        _PorteVille.transformObjet.position = new Vector2((float)fileJson.positionXPorte / precisionFloat, (float)fileJson.positionYPorte / precisionFloat);
        Destroy(_PorteVille.imageOccupe.gameObject);
        


        //Instanciation du Chateau
        _Chateau = new Chateau(repertoireSprites.chateauSprite, "Chateau", true,3);
        _Chateau.scaleObjet = new Vector2((float)fileJson.scaleXChateau / precisionFloat, (float)fileJson.scaleYChateau / precisionFloat);
        _Chateau.transformObjet.localScale = _Chateau.scaleObjet;
        _Chateau.transformObjet.name = _Chateau.nomObjet;
        _Chateau.transformObjet.position = new Vector2((float)fileJson.positionXChateau / precisionFloat, (float)fileJson.positionYChateau / precisionFloat);
        _Chateau.buttonObjet.onClick.AddListener(delegate { _Chateau.OuverturePanelInterractBatiment(_Chateau.indicePanelInterractBatiment); });
        _Chateau.panelInteractText.text = "Que demander au roi ?";
        _Chateau._InterractionsDisponibles[0].GetComponent<Button>().onClick.AddListener(delegate { _Chateau.Chateau_PrendreRound(); });
        _Chateau._InterractionsDisponibles[1].GetComponent<Button>().onClick.AddListener(delegate { _Chateau.Chateau_PrendreBle(); });
        _Chateau._InterractionsDisponibles[2].GetComponent<Button>().onClick.AddListener(delegate { _Chateau.Chateau_ReparerPorte(); });
        _Chateau.imageOccupe.GetComponent<RectTransform>().localScale = new Vector3(1f / _Chateau.imageOccupe.transform.parent.parent.transform.localScale.x, 1f / _Chateau.imageOccupe.transform.parent.parent.transform.localScale.y, 1);
        #endregion


        #region Panel du Marché
        //Attribution des fonctions des boutons du marché
        for (int ii = 0;ii< panelParentBatimentInterract.GetChild(4).GetChild(1).childCount; ii++)
        {
            panelParentBatimentInterract.GetChild(4).GetChild(1).GetChild(ii).GetComponent<Button>().onClick.AddListener(delegate {_ChantierBTP.VendreRessourcesMarket(); });
        }

        #endregion


        #endregion


        //PopUps
        textPopUps = GameObject.Find("TextPopUps").GetComponent<Text>();
        textPopUps.text = "";

        #region Guizmo Rotation des Tours
        _GuizmoRotationTour = GameObject.Find("ImageRotationTour");
        _CloseGuizmoRotTourButton = _GuizmoRotationTour.transform.Find("ButtonEndChoice").GetComponent<Button>();
        _CloseGuizmoRotTourButton.onClick.AddListener(delegate { tours[0].ValidChoixRotationTour();});
        #endregion


#if !UNITY_EDITOR
        isModeDebug = false;
#endif


    }



    #region Instantiation Réserve des ennemis

    //Instanciation des Prefabs des Unites et affectation dans une réserve
     public void InstantiateUnite(int _Index)
    {
        GameObject monUniteJoueur = Instantiate(uniteBasePrefab);
        monUniteJoueur.transform.parent = reservePrefabUnitesJoueur.GetChild(_Index);
        UniteJoueur scriptUniteJoueur = monUniteJoueur.AddComponent<UniteJoueur>();
        scriptUniteJoueur.InitialisationUniteParent("UniteJoueur");
        scriptUniteJoueur.UniteJoueurInitialisation(repertoireSprites.unitesJoueurData[_Index]);
        scriptUniteJoueur.AttributionCaracteristiques();
        monUniteJoueur.SetActive(false);
    }

    public void InstantiateSuperUnite(int _Index)
    {
        GameObject maSuperUniteJoueur = Instantiate(uniteBasePrefab);
        maSuperUniteJoueur.transform.parent = reservePrefabSuperUnitesJoueur.GetChild(_Index);
        SuperUniteJoueur scriptUniteJoueur = maSuperUniteJoueur.AddComponent<SuperUniteJoueur>();
        scriptUniteJoueur.InitialisationUniteParent("SuperUniteJoueur");
        scriptUniteJoueur.UniteJoueurInitialisation(repertoireSprites.superUnitesJoueurData[_Index]);
        scriptUniteJoueur.AttributionCaracteristiques();
        maSuperUniteJoueur.SetActive(false);
    }

    public void InstantiateEnnemi(int _Index)
    {
        GameObject monEnnemi = Instantiate(uniteBasePrefab);
        monEnnemi.transform.parent = reservePrefabEnnemis.GetChild(_Index);
        Ennemi scriptEnnemi = monEnnemi.AddComponent<Ennemi>();
        scriptEnnemi.InitialisationUniteParent("Ennemi");
        scriptEnnemi.EnnemiInitialisation(repertoireSprites.ennemiData[_Index]);
        scriptEnnemi.AttributionCaracteristiques();
        scriptEnnemi.AttributionCaracteristiquesEnnemi();
        monEnnemi.SetActive(false);
    }
    #endregion


    //Gère le joueur actif
    public void SetJoueurActif()
    {
        StartCoroutine(AnimChangementJoueur());
    }

    #region Transition de joueur
    IEnumerator MoveElementUI(Transform elmtToMove, Vector3 dirToMove, Vector3 target, float speedMove, float speedScale)
    {
        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);

        float midDistY = (elmtToMove.position.y - target.y) / 2f;
        float posY0 = elmtToMove.position.y;
        int sens = 1;
        if (elmtToMove.position.y - target.y < 0)
        {
            sens = -1;
        }
        while (Vector3.Magnitude(elmtToMove.position - target) > 0.25f)
        {
            elmtToMove.Translate(sens * dirToMove.normalized * speedMove * FonctionsVariablesUtiles.deltaTime);

            if (sens * elmtToMove.position.y > sens * (posY0 - midDistY))
            {
                elmtToMove.localScale += Vector3.one * speedScale * FonctionsVariablesUtiles.deltaTime;
            }
            else
            {
                elmtToMove.localScale -= Vector3.one * speedScale * FonctionsVariablesUtiles.deltaTime;
            }
            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
            if (sens * elmtToMove.position.y < sens * target.y)
            {
                break;
            }
        }
        elmtToMove.localScale = Vector3.one;
        elmtToMove.gameObject.SetActive(false);
    }


    IEnumerator FusionUnites(Transform[] elmtToMove, Vector3 target,GameObject objectToActive)
    {
        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
        #region Initialisation
        Vector3[] target1 = new Vector3[elmtToMove.Length];
        float[] speed = new float[elmtToMove.Length];
        float[] speedScale = new float[elmtToMove.Length];
        float timeAnim = 0.5f;
        float scaleToUp = 4.5f;
        #endregion

        #region Fusion To Center
        for (int ii = 0; ii < target1.Length; ii++)
        {
            target1[ii] = Vector3.zero - elmtToMove[ii].position;
            speed[ii] = Vector3.Magnitude(target1[ii])/timeAnim;
            speedScale[ii] = scaleToUp / (Vector3.Magnitude(target1[ii])/speed[ii]);
        }

        while(Vector3.Magnitude(elmtToMove[0].position - Vector3.zero) > 0.1f)
        {
            for(int ii = 0; ii < target1.Length; ii++)
            {
                elmtToMove[ii].Translate(target1[ii].normalized * speed[ii] * Time.deltaTime);
                elmtToMove[ii].localScale += Vector3.one * speedScale[ii] * Time.deltaTime;
            }
            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
        }
        for (int ii = 1; ii < target1.Length; ii++)
        {
            elmtToMove[ii].gameObject.SetActive(false);
            //elmtToMove[ii].GetComponent<CallBacksUnitesJoueur>().isRecruted = false;
        }
        #endregion

        #region Scaling Centre
        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
        //Changer Speed
        float newScaleToUp = 8f;
        float timeToUnScale = 0.25f;
        speedScale[0] = (newScaleToUp-scaleToUp) / timeToUnScale;       //dist à parcourir/temps
        while (elmtToMove[0].localScale.x < newScaleToUp)
        {
            elmtToMove[0].localScale += Vector3.one * speedScale[0] * Time.deltaTime;
            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
        }
        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
        while (elmtToMove[0].localScale.x > 2.5f)
        {
            elmtToMove[0].localScale -= Vector3.one * speedScale[0] * Time.deltaTime;
            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
        }
        #endregion

        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);

        #region GoTo SuperSoldat
        target1[0] = target - elmtToMove[0].position;
        float timeToGo = 0.5f;
        speed[0] = target1[0].magnitude/timeToGo;
        while(Vector3.Magnitude(target - elmtToMove[0].position) > 0.15f)
        {
            elmtToMove[0].Translate(target1[0].normalized * speed[0] * FonctionsVariablesUtiles.deltaTime);
            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
        }
        elmtToMove[0].gameObject.SetActive(false);
        //elmtToMove[0].GetComponent<CallBacksUnitesJoueur>().isRecruted = false;
        #endregion

        objectToActive.SetActive(true);
    }


    public IEnumerator AnimChangementJoueur()
    {
        topChrono = false;

        if (myEventSystem.currentSelectedGameObject != null)
        {
            #region Détermine quelle action le joueur a réalisé
            if (myEventSystem.currentSelectedGameObject.transform.parent.parent != null &&
                myEventSystem.currentSelectedGameObject.transform.parent.parent.CompareTag("Ressources"))
            {
                typeActionJoueur = "Ressources";
            }
            else if (myEventSystem.currentSelectedGameObject.transform.parent.parent != null &&
                myEventSystem.currentSelectedGameObject.transform.parent.parent.CompareTag("EmplacementBTP"))
            {
                typeActionJoueur = "ConstructionBatiment";
            }
            else if (myEventSystem.currentSelectedGameObject != null &&
                myEventSystem.currentSelectedGameObject.name == "ButtonEndChoice")
            {
                typeActionJoueur = "ConstructionTour";
            }
            else if (myEventSystem.currentSelectedGameObject != null &&
                myEventSystem.currentSelectedGameObject.name == "ButtonRecruter")
            {
                typeActionJoueur = "RecruterUnites";
            }
            else if(myEventSystem.currentSelectedGameObject != null && myEventSystem.currentSelectedGameObject.transform.parent.name == "ButtonAmeliorer")
            {
                typeActionJoueur = "AmeliorerUnite";
            } else if(myEventSystem.currentSelectedGameObject != null &&
                myEventSystem.currentSelectedGameObject.transform.name == "ButtonPrendreBle")
            {
                typeActionJoueur = "ChateauPrendreBle";
            } else if(myEventSystem.currentSelectedGameObject != null &&
                myEventSystem.currentSelectedGameObject.transform.name == "ButtonReparerPorte")
            {
                typeActionJoueur = "ChateauReparerPorte";
            } else if(myEventSystem.currentSelectedGameObject != null &&
                myEventSystem.currentSelectedGameObject.transform.name == "ButtonPrendreRound")
            {
                typeActionJoueur = "ChateauPrendreRound";
            }
            //... Autres actions
            #endregion


            #region Initialisation Anim
            //Bloquage des interactions du joueur
            if (typeActionJoueur != "")
            {
                _ImageChangementJoueurActif.SetActive(true);
            }
            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
            Transform cibleElementGraph = GameObject.Find("JoueurPanelPlace").transform.GetChild(_JoueurActif - 1);
            if (nbRessourcesRecup > reservePionsUI.childCount)
            {
                for (int ii = reservePionsUI.childCount; ii < nbRessourcesRecup; ii++)
                {
                    GameObject pionsToInstance = Instantiate(prefabPionsUI, reservePionsUI);
                    pionsToInstance.SetActive(false);
                }
            }
            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);

            Vector3 dirToGo = new Vector3();
            Transform targetPions = null;
            GameObject batimentUtilise = null;

            if (typeActionJoueur != "RecruterUnites" && typeActionJoueur != "AmeliorerUnite" && typeActionJoueur != "ChateauPrendreBle" && typeActionJoueur != "ChateauReparerPorte" && typeActionJoueur != "ChateauPrendreRound")
            {
                targetPions = myEventSystem.currentSelectedGameObject.transform;
                if (typeActionJoueur != "ConstructionTour" && typeActionJoueur != "ConstructionBatiment")
                {
                    batimentUtilise = myEventSystem.currentSelectedGameObject.transform.parent.parent.gameObject;
                } else if(typeActionJoueur == "ConstructionTour")
                {
                    batimentUtilise = GameObject.Find("ChantierTours");
                } else if(typeActionJoueur == "ConstructionBatiment")
                {
                    batimentUtilise = GameObject.Find("ChantierBTP");
                }
            }
            else if (typeActionJoueur == "RecruterUnites" || typeActionJoueur == "AmeliorerUnite")
            {
                targetPions = GameObject.Find("CampMilitaire").transform;
                batimentUtilise = targetPions.gameObject;
            }
            else if (typeActionJoueur == "ChateauPrendreBle" || typeActionJoueur == "ChateauPrendreRound")
            {
                targetPions = GameObject.Find("Chateau").transform;
                batimentUtilise = targetPions.gameObject;
            }
            else if (typeActionJoueur == "ChateauReparerPorte")
            {
                targetPions = GameObject.Find("PorteVille").transform;
                batimentUtilise = GameObject.Find("Chateau");
            }

            dirToGo = cibleElementGraph.position - targetPions.position;
            float waitTime = 0.2f/nbRessourcesRecup;
            float speed = dirToGo.magnitude / (1 - waitTime);         //1 : tps que l'on veut pour l'anim complete      //0.2 temps que l'on veut attendre entre chaque ressource
            float speedScale = 4 / (dirToGo.magnitude / speed);     //4 : scale à gagner;
            #endregion

            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);


            #region Action de récupération de ressources
            if (typeActionJoueur == "Ressources")
            {
                for (int ii = 0; ii < nbRessourcesRecup; ii++)
                {
                    VFX_MoveRessources(reservePionsUI.GetChild(ii), targetPions, dirToGo, cibleElementGraph.position, speed, speedScale, ressourcesSprites[indexSpriteRessources[ii]]);
                    yield return new WaitForSeconds(waitTime);
                }
                yield return new WaitForSeconds(0.5f+ dirToGo.magnitude/speed);
            }
            #endregion
            #region Action de Construction Batiment
            else if (typeActionJoueur == "ConstructionBatiment")
            {
                for(int ii = 0; ii < nbRessourcesRecup; ii++)
                {
                    VFX_MoveRessources(reservePionsUI.GetChild(ii), cibleElementGraph, dirToGo, targetPions.position, speed, speedScale, ressourcesSprites[indexSpriteRessources[ii]]);
                    yield return new WaitForSeconds(waitTime);
                }
                yield return new WaitForSeconds(0.5f + dirToGo.magnitude / speed);
                //Désactivation du canvas de l'emplacement chantier et remise à l'échelle
                objetConstruit[0].transform.parent.GetChild(0).gameObject.SetActive(false);
                objetConstruit[0].transform.localScale = Vector3.one;
                objetConstruit[0].transform.parent.localScale = scaleConstruction;
                objetConstruit[0].SetActive(true);
            }
            #endregion
            #region Action de Construction Tour
            else if (typeActionJoueur == "ConstructionTour")
            {
                for (int ii = 0; ii < nbRessourcesRecup; ii++)
                {
                    VFX_MoveRessources(reservePionsUI.GetChild(ii), cibleElementGraph, dirToGo, targetPions.position, speed, speedScale, ressourcesSprites[indexSpriteRessources[ii]]);
                    yield return new WaitForSeconds(waitTime);
                }
                yield return new WaitForSeconds(0.5f + dirToGo.magnitude / speed);
                //Désactivation du canvas de l'emplacement chantier et remise à l'échelle
                objetConstruit[0].transform.parent.GetChild(0).gameObject.SetActive(false);
                objetConstruit[0].transform.localScale = Vector3.one;
                objetConstruit[0].transform.parent.localScale = scaleConstruction;
                objetConstruit[0].SetActive(false);
                objetConstruit[0].GetComponent<SpriteRenderer>().color = Color.white;
                objetConstruit[0].GetComponent<Animation>().enabled = true;
                yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                objetConstruit[0].SetActive(true);
            }
            #endregion
            #region Action de Recrutement Unités
            else if (typeActionJoueur == "RecruterUnites")
            {
                for (int ii = 0; ii < nbRessourcesRecup; ii++)
                {
                    VFX_MoveRessources(reservePionsUI.GetChild(ii), cibleElementGraph, dirToGo, targetPions.position, speed, speedScale, ressourcesSprites[indexSpriteRessources[ii]]);
                    yield return new WaitForSeconds(waitTime);
                }
                yield return new WaitForSeconds(0.5f + dirToGo.magnitude / speed);
                yield return new WaitForSeconds(0.5f);

                for (int ii = 0; ii < listUnitesAnimationActionJoueur.Count; ii++)
                {
                    targetPions = listUnitesAnimationActionJoueur[ii].transform;
                    dirToGo = listUnitesAnimationActionPositions[ii] - targetPions.position;
                    speed = dirToGo.magnitude;
                    speedScale = 4 / (dirToGo.magnitude / speed);
                    VFX_MoveRessources(listUnitesAnimationActionJoueur[ii].transform, targetPions, dirToGo, listUnitesAnimationActionPositions[ii], speed, speedScale, listUnitesAnimationActionJoueur[ii].GetComponent<SpriteRenderer>().sprite);
                }
                yield return new WaitForSeconds(1.5f* dirToGo.magnitude / speed);
                for (int ii = 0; ii < listUnitesAnimationActionJoueur.Count; ii++)
                {
                    objetConstruit[ii].SetActive(true);
                    NewRoundAlimentationReserve(reservePrefabUnitesJoueur);
                    Destroy(listUnitesAnimationActionJoueur[ii].gameObject,2);
                }
            }
            #endregion
            #region Action d'Améliorer Unités
            else if (typeActionJoueur == "AmeliorerUnite")
            {
                for (int ii = 0; ii < nbRessourcesRecup; ii++)
                {
                    VFX_MoveRessources(reservePionsUI.GetChild(ii), cibleElementGraph, dirToGo, targetPions.position, speed, speedScale, ressourcesSprites[indexSpriteRessources[ii]]);
                    yield return new WaitForSeconds(waitTime);
                }
                yield return new WaitForSeconds(0.5f + dirToGo.magnitude / speed);
                yield return new WaitForSeconds(0.5f);

                Transform[] unitesFusion = new Transform[objetConstruit.Count - 1];
                for(int ii = 0; ii < objetConstruit.Count - 1; ii++)
                {
                    unitesFusion[ii] = objetConstruit[ii].transform;
                }

                IEnumerator fusionCorout = FusionUnites(unitesFusion, objetConstruit[objetConstruit.Count - 1].transform.position, objetConstruit[objetConstruit.Count - 1]);
                Coroutine fusion = StartCoroutine(fusionCorout);
                while (fusionCorout.MoveNext())
                {
                    yield return null;
                }
                yield return fusion;

                objetConstruit[objetConstruit.Count-1].SetActive(true);     //SuperUnité à activer (dernier indice de la liste, les autres sont les unités à faire disparaître)
            }
            #endregion
            #region Actions du Chateau
            #region Action Chateau Prendre Blé
            else if(typeActionJoueur == "ChateauPrendreBle")
            {
                for(int ii = 0; ii < nbRessourcesRecup; ii++)
                {
                    VFX_MoveRessources(reservePionsUI.GetChild(ii), targetPions, dirToGo, cibleElementGraph.position, speed, speedScale, ressourcesSprites[indexSpriteRessources[ii]]);
                    yield return new WaitForSeconds(waitTime);
                }
                yield return new WaitForSeconds(0.5f + dirToGo.magnitude / speed);
            }
            #endregion
            #region Action Chateau Reparer Porte
            else if(typeActionJoueur == "ChateauReparerPorte")
            {
                int ressourcesReelles = nbRessourcesRecup;
                if(tableauJoueurs[_JoueurActif - 1].isPalissade)
                {
                    ressourcesReelles--;
                }

                for (int ii = 0; ii < ressourcesReelles; ii++)
                {
                    VFX_MoveRessources(reservePionsUI.GetChild(ii), cibleElementGraph, dirToGo, targetPions.position, speed, speedScale, ressourcesSprites[indexSpriteRessources[ii]]);
                    yield return new WaitForSeconds(waitTime);
                }
                yield return new WaitForSeconds(0.5f + dirToGo.magnitude / speed);
                if(tableauJoueurs[_JoueurActif - 1].isPalissade)
                {
                    targetPions = GameObject.Find("Chateau").transform;
                    dirToGo = cibleElementGraph.position - targetPions.position;
                    waitTime = 0.2f / nbRessourcesRecup;
                    speed = dirToGo.magnitude / (1 - waitTime);         //1 : tps que l'on veut pour l'anim complete      //0.2 temps que l'on veut attendre entre chaque ressource
                    speedScale = 4 / (dirToGo.magnitude / speed);     //4 : scale à gagner;


                    VFX_MoveRessources(reservePionsUI.GetChild(ressourcesReelles), targetPions, dirToGo, cibleElementGraph.position, speed, speedScale, ressourcesSprites[indexSpriteRessources[ressourcesReelles]]);
                    yield return new WaitForSeconds(waitTime);
                }
                yield return new WaitForSeconds(0.5f + dirToGo.magnitude / speed);
            }
            #endregion
            #region Action Chateau Prendre Round
            else if(typeActionJoueur == "ChateauPrendreRound")
            {
                for (int ii = 0; ii < nbRessourcesRecup; ii++)
                {
                    VFX_MoveRessources(reservePionsUI.GetChild(ii), targetPions , dirToGo, cibleElementGraph.position, speed, speedScale, ressourcesSprites[indexSpriteRessources[ii]]);
                    yield return new WaitForSeconds(waitTime);
                }
                yield return new WaitForSeconds(0.5f + dirToGo.magnitude / speed);
                if (!tableauJoueurs[_JoueurActif - 1].imagePrendreRound.activeSelf)
                {
                    tableauJoueurs[_JoueurActif - 1].imagePrendreRound.SetActive(true);
                }
                yield return new WaitForSeconds(tableauJoueurs[_JoueurActif - 1].imagePrendreRound.GetComponent<Animation>().clip.length*1.1f);
            }
            #endregion
            #endregion

            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);

            #region Bloque Interraction avec le batiment
            dirToGo = cibleElementGraph.position - batimentUtilise.transform.Find("CanvasInfos").Find("ImageOccupe").position;
            pionsActionJoueursGO[nbActionRealised] = batimentUtilise.transform.Find("CanvasInfos").Find("ImageOccupe").gameObject;
            waitTime = 0.2f;
            speed = dirToGo.magnitude / (waitTime*2);         //1 : tps que l'on veut pour l'anim complete      //0.2 temps que l'on veut attendre entre chaque ressource
            speedScale = 0;     //1 : scale à gagner;
            reservePionsUI.GetChild(0).localScale = Vector3.one * 0.5f;
            VFX_MoveRessources(reservePionsUI.GetChild(0), cibleElementGraph, dirToGo, batimentUtilise.transform.position, speed, speedScale, pionOccupesSprites[_JoueurActif - 1]);
            yield return new WaitForSeconds(0.5f + dirToGo.magnitude / speed);

            batimentUtilise.transform.Find("CanvasInfos").Find("ImageOccupe").gameObject.GetComponent<Image>().sprite = pionOccupesSprites[_JoueurActif - 1];
            batimentUtilise.transform.Find("CanvasInfos").Find("ImageOccupe").gameObject.SetActive(true);
            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
            #endregion


            #region Mise à jour statistiques achievements
            StartCoroutine(MoveImageAchievement(joueur1._NbRessourcesRecoltees,joueur2._NbRessourcesRecoltees,imageAchRessources));
            StartCoroutine(MoveImageAchievement(joueur1._NbUnitesRecrutees, joueur2._NbUnitesRecrutees, imageAchNbUnites));
            StartCoroutine(MoveImageAchievement(joueur1._NbSuperUnitesRecrutees, joueur2._NbSuperUnitesRecrutees, imageAchNbSuperUnites));
            StartCoroutine(MoveImageAchievement(joueur1.nbToursPossedees, joueur2.nbToursPossedees, imageAchTours));
            StartCoroutine(MoveImageAchievement(joueur1._NbBatimentsConstruits, joueur2._NbBatimentsConstruits, imageAchBatiments));
            StartCoroutine(MoveImageAchievement((int)joueur1._NbPvReparePorte, (int)joueur2._NbPvReparePorte, imageAchReparerPorte));
            StartCoroutine(MoveImageAchievement((int)(joueur1._TimeToMakeAction * 1000), (int)(joueur2._TimeToMakeAction * 1000),imageAchRapidite));
            #endregion

            #region Change le joueur actif
            if (_JoueurActif == 1)
            {
                _JoueurActif = 2;
            }
            else if (_JoueurActif == 2)
            {
                _JoueurActif = 1;
            }


            #region Gestion du round
            nbActionRealised++;

            //Changement de Round
            if (nbActionRealised >= 4)
            {
                nbActionRealised = 0;       //Remise à 0 du nombre d'actions
                textRounds.transform.parent.gameObject.SetActive(false);
                numeroRound++;              //Incrémentation du numéro du round
                textRounds.transform.parent.gameObject.SetActive(true);

                #region Victoire
                if (numeroRound > nbMaxRounds)
                {
                    isEndGame = true;
                    panelVictoire.SetActive(true);

                    #region Calcul des achievments 
                    CalculAchievements();
                    #endregion


                }
                #endregion

                #region Le jeu continue
                if (!isEndGame)
                {
                    #region Production des Ressources
                    //Production par round des ressources
                    _ChampBle.ProductionRessourceRound();
                    StartCoroutine(_ChampBle.DesactivationVFXProduction());
                    _RecolteBois.ProductionRessourceRound();
                    StartCoroutine(_RecolteBois.DesactivationVFXProduction());
                    _MineFer.ProductionRessourceRound();
                    StartCoroutine(_MineFer.DesactivationVFXProduction());
                    _CarrierePierre.ProductionRessourceRound();
                    StartCoroutine(_CarrierePierre.DesactivationVFXProduction());
                    #endregion

                    #region Réactivation batiments
                    _ChampBle.CheckBoutonInteract(true);
                    _MineFer.CheckBoutonInteract(true);
                    _CarrierePierre.CheckBoutonInteract(true);
                    _RecolteBois.CheckBoutonInteract(true);
                    _CampMilitaire.CheckBoutonInteract(true);
                    _ChantierTours.CheckBoutonInteract(true);
                    _ChantierBTP.CheckBoutonInteract(true);
                    _Chateau.CheckBoutonInteract(true);

                    if (pionsActionJoueursGO.Length > reservePionsUI.childCount)
                    {
                        for (int ii = reservePionsUI.childCount; ii <= pionsActionJoueursGO.Length; ii++)
                        {
                            GameObject pionsToInstance = Instantiate(prefabPionsUI, reservePionsUI);
                            pionsToInstance.SetActive(false);
                        }
                    }

                    int joueurPion = 0;
                    for (int ii = 0; ii < pionsActionJoueursGO.Length; ii++)
                    {
                        //Cas Joueur 1
                        if (pionsActionJoueursGO[ii].GetComponent<Image>().sprite.name == "éléments ui_52")  
                        {
                            Debug.Log("Mettre à jour nom de la sprite si besoin");
                            joueurPion = 0;
                        } 
                        //Cas Joueur 2
                        else if(pionsActionJoueursGO[ii].GetComponent<Image>().sprite.name == "éléments ui_51")
                        {
                            Debug.Log("Mettre à jour nom de la sprite si besoin");
                            joueurPion = 1;
                        }

                        cibleElementGraph = GameObject.Find("JoueurPanelPlace").transform.GetChild(joueurPion);
                        dirToGo = cibleElementGraph.position - pionsActionJoueursGO[ii].transform.position;
                        speed = dirToGo.magnitude / (waitTime * 2);         //1 : tps que l'on veut pour l'anim complete      //0.2 temps que l'on veut attendre entre chaque ressource
                        speedScale = 0;     //1 : scale à gagner;
                        reservePionsUI.GetChild(ii).localScale = Vector3.one * 0.5f;
                        VFX_MoveRessources(reservePionsUI.GetChild(ii), pionsActionJoueursGO[ii].transform, dirToGo, cibleElementGraph.position, speed, speedScale, pionOccupesSprites[joueurPion]);
                    }


                    //Retour pion Prendre Round
                    if (joueur1._ReserveRound || joueur2._ReserveRound)
                    {
                        if (joueur1._ReserveRound)
                        {
                            joueurPion = 0;
                        }
                        else if (joueur2._ReserveRound)
                        {
                            joueurPion = 1;
                        }
                        cibleElementGraph = GameObject.Find("Chateau").transform;
                        dirToGo = GameObject.Find("JoueurPanelPlace").transform.GetChild(joueurPion).position- cibleElementGraph.position;
                        speed = dirToGo.magnitude / (waitTime * 2);         //1 : tps que l'on veut pour l'anim complete      //0.2 temps que l'on veut attendre entre chaque ressource
                        speedScale = 0;     //1 : scale à gagner;
                        reservePionsUI.GetChild(reservePionsUI.childCount - 1).localScale = Vector3.one * 0.5f;
                        VFX_MoveRessources(reservePionsUI.GetChild(reservePionsUI.childCount - 1), GameObject.Find("JoueurPanelPlace").transform.GetChild(joueurPion), dirToGo, cibleElementGraph.position, speed, speedScale, ressourcesSprites[4]);
                    }


                    if (GameObject.FindGameObjectsWithTag("EmplacementBTP").Length > 0)
                    {
                        GameObject[] emplacementBatiment = GameObject.FindGameObjectsWithTag("EmplacementBTP");
                        for (int ii = 0; ii < GameObject.FindGameObjectsWithTag("EmplacementBTP").Length; ii++)
                        {
                            emplacementBatiment[ii].transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Button>().interactable = true;
                        }
                    }
                    joueur1.ActiveImagesActionRound();
                    joueur2.ActiveImagesActionRound();
                    #endregion

                    #region Remise des pions d'actions
                    for (int ii = 0; ii < pionsActionJoueursGO.Length; ii++)
                    {
                        pionsActionJoueursGO[ii].SetActive(false);
                    }
                    #endregion

                    #region Recrutement à la caserne
                    //Augmentation du nombre d'unité dans la caserne
                    _CampMilitaire.nbUniteRecrutable++;
                    if (isMaisonConstruite)
                    {
                        int chanceActivationMaison = Random.RandomRange(0, 2);
                        if (chanceActivationMaison == 0)
                        {
                            _CampMilitaire.nbUniteRecrutable++;
                        }
                    }

                    //Recrutement nouvelle unité réserve
                    _CampMilitaire.CaserneCreationSoldat();
                    #endregion

                    #region Instance d'unités à la Caserne
                    //Compter le nombre d'actifs dans les réserves Ennemis et Unités


                    #region New Instances Unités/Super Unités
                    NewRoundAlimentationReserve(reservePrefabUnitesJoueur);
                    NewRoundAlimentationReserve(reservePrefabSuperUnitesJoueur);
                    #endregion


                    #region Ennemis Déploiement
                    //Lancement Coroutine du déploiement des ennemis. Lancement unique au round approprié
                    if (numeroRound != 0 && numeroRound == roundActivationDeploiementEnnemi)
                    {
                        //Debug.Log("Réactiver le déploiement");
                        StartCoroutine(DeploiementEnnemis());
                    }

                    #endregion

                    #endregion


                    #region Autels et attribution de points de victoire
                    for (int ii = 0; ii < tableauJoueurs.Length; ii++)
                    {
                        for (int jj = 0; jj < tableauJoueurs[ii]._NombreAutels; jj++)
                        {
                            tableauJoueurs[ii]._VictoryPoints += NbVictoryPointsPerAutel;
                            tableauJoueurs[ii].AffichePointVictoireSolidarite(tableauJoueurs[ii].victoirePointText, tableauJoueurs[ii]._VictoryPoints);
                        }
                    }

                    #endregion


                    #region Tente de Soins
                    for (int ii = 0; ii < tableauJoueurs.Length; ii++)
                    {
                        if (tableauJoueurs[ii]._HasTenteSoins)
                        {
                            _ChantierBTP.BatimentTenteSoinsFonction(ii);
                        }
                    }
                    #endregion


                    #region Changement Joueur Actif
                    if (_Chateau._ReserveRoundActif)
                    {
                        
                        if (joueur1._ReserveRound && !joueur2._ReserveRound)
                        {
                            _JoueurActif = 1;
                        }
                        else if (!joueur1._ReserveRound && joueur2._ReserveRound)
                        {
                            _JoueurActif = 2;
                        }
                        //Remise à 0 de Réserve Round depuis le chateau
                        joueur1._ReserveRound = false;
                        joueur2._ReserveRound = false;
                        joueur1.imagePrendreRound.SetActive(false);
                        joueur2.imagePrendreRound.SetActive(false);
                        _Chateau._ReserveRoundActif = false;
                    }
                    #endregion


                    textRounds.text = "Round : " + numeroRound.ToString() + "/" + nbMaxRounds.ToString();       //Affichage Round
                    yield return new WaitForSeconds(1);
                }
                #endregion
            }
            #endregion


            

            #region Animation Image Changement joueur Actif
            IEnumerator animImageCorout = AnimationChangementJoueurActif(20f);
            Coroutine animationCorout = StartCoroutine(animImageCorout);
            while (animImageCorout.MoveNext())
            {
                yield return null;
            }
            yield return animationCorout;
            #endregion
            #endregion

        }


        #region Pion Joueur Actif
        if (_JoueurActif == 1)
        {
            _PionJoueurActifImage.sprite = joueur1._ImagePionActif;
        }
        else if (_JoueurActif == 2)
        {
            _PionJoueurActifImage.sprite = joueur2._ImagePionActif;
        }
        #endregion

        //Débloquage des interactions du joueur et remise à zero des variables
        _ImageChangementJoueurActif.SetActive(false);
        typeActionJoueur = "";
        scaleConstruction = Vector3.one;
        objetConstruit.Clear();
        listUnitesAnimationActionJoueur.Clear();
        listUnitesAnimationActionPositions.Clear();
        nbRessourcesRecup = 0;

        StartCoroutine(CompteTemps());
    }


    public void NewRoundAlimentationReserve(Transform _Reserve)
    {
        bool needNewInstances = false;
        int nbInactif = 0;
        int newInstances = 0;
        for (int ii = 0; ii < _Reserve.childCount; ii++)
        {
            nbInactif = CompteNbInactivesReserves(_Reserve.GetChild(ii));
            if (nbInactif < nbMiniInstancesInactiveReserves)
            {
                needNewInstances = true;
                newInstances = Mathf.Max(newInstances, nbMiniInstancesInactiveReserves - nbInactif);
            }
            if (needNewInstances)
            {
                if (_Reserve.name == "ReservePrefabUnitesJoueur")
                {
                    InstantiateUnite(ii);
                } else if(_Reserve.name == "ReservePrefabSuperUnitesJoueur")
                {
                    InstantiateSuperUnite(ii);
                } else if(_Reserve.name == "ReservePrefabEnnemis")
                {
                    InstantiateEnnemi(ii);
                }

                needNewInstances = false;
                newInstances = 0;
            }
        }
    }

    private IEnumerator CompteTemps()
    {
        topChrono = true;
        float temps = 0;
        while (topChrono)
        {
            temps += FonctionsVariablesUtiles.deltaTime;
            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
        }

        if(temps < tableauJoueurs[_JoueurActif - 1]._TimeToMakeAction)
        {
            tableauJoueurs[_JoueurActif - 1]._TimeToMakeAction = temps;
        }
    }



    void VFX_MoveRessources(Transform pionToMove, Transform startPion,Vector3 dirToGo,Vector3 target,float speed, float speedScale,Sprite spriteToGet)
    {
        pionToMove.GetComponent<SpriteRenderer>().sprite = spriteToGet;
        pionToMove.gameObject.SetActive(true);
        //pionToMove.transform.localScale = Vector3.one;
        pionToMove.transform.position = startPion.position;
        StartCoroutine(MoveElementUI(pionToMove.transform, dirToGo, target, speed, speedScale));
    }


    IEnumerator AnimationChangementJoueurActif(float speedAnim)
    {
        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
        float posX1 = -9.4f;
        float posX2 = 9.4f;
        imageChangementJoueur.SetActive(true);

        if (_JoueurActif == 2)       // -9.4 => 9.4
        {
            imageChangementJoueur.transform.position = new Vector3(posX1, imageChangementJoueur.transform.position.y, imageChangementJoueur.transform.position.z);
            while (imageChangementJoueur.transform.position.x < posX2)
            {
                posX1 += FonctionsVariablesUtiles.deltaTime * speedAnim;
                imageChangementJoueur.transform.position = new Vector3(posX1, imageChangementJoueur.transform.position.y, imageChangementJoueur.transform.position.z);
                yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                if(imageChangementJoueur.transform.position.x > posX2)
                {
                    posX1 = posX2;
                    imageChangementJoueur.transform.position = new Vector3(posX1, imageChangementJoueur.transform.position.y, imageChangementJoueur.transform.position.z);
                }
            }
        } else if(_JoueurActif == 1)
        {
            imageChangementJoueur.transform.position = new Vector3(posX2, imageChangementJoueur.transform.position.y, imageChangementJoueur.transform.position.z);
            while (imageChangementJoueur.transform.position.x > posX1)
            {
                posX2 -= FonctionsVariablesUtiles.deltaTime * speedAnim;
                imageChangementJoueur.transform.position = new Vector3(posX2, imageChangementJoueur.transform.position.y, imageChangementJoueur.transform.position.z);
                yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                if (imageChangementJoueur.transform.position.x < posX1)
                {
                    posX2 = posX1;
                    imageChangementJoueur.transform.position = new Vector3(posX2, imageChangementJoueur.transform.position.y, imageChangementJoueur.transform.position.z);
                }
            }
        }
        imageChangementJoueur.SetActive(false);
    }
    #endregion




    #region Instance Ennemis
    //Coroutine de déploiement des ennemis
    IEnumerator DeploiementEnnemis()
    {
        float chrono = 0f;
        int indTableauDeploiement = 0;

        //Initialisation, lancée de la première vague
        /*Debug.Log("Lancement vague");
        indTableauDeploiement++;*/

        while (!isEndGame)
        {
            float oldChrono = chrono;
            chrono += FonctionsVariablesUtiles.deltaTime;
            if ((int)chrono - (int)oldChrono == 1)
            {
                if(isCabaneEclaireur && indTableauDeploiement < periodeDeploiement.Length && (int)chrono == periodeDeploiement[indTableauDeploiement] - _SecondesBeforeMonstres)
                {
                    PopUpsFonction("Des ennemis arrivent dans "+_SecondesBeforeMonstres+" secondes !!", textPopUps);
                }


                //Debug.Log((int)chrono);
                if(indTableauDeploiement<periodeDeploiement.Length && (int)chrono == periodeDeploiement[indTableauDeploiement])
                {
                    //Debug.Log("Lancement vague");
                    LancementVagueMonstres(indTableauDeploiement);
                    indTableauDeploiement++;
                }
            }

            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
            if (isEndGame)
            {
                break;
            }
        }
    }

    public void LancementVagueMonstres(int _II)
    {
        string[] monstreChemin = new string[3];
        monstreChemin[0] = ParametresCarteOpenning.ficOptionParam.objet_vagues.o_vagues.o_structure_vagues[_II].s_chemin_1;
        monstreChemin[1] = ParametresCarteOpenning.ficOptionParam.objet_vagues.o_vagues.o_structure_vagues[_II].s_chemin_2;
        monstreChemin[2] = ParametresCarteOpenning.ficOptionParam.objet_vagues.o_vagues.o_structure_vagues[_II].s_chemin_3;

        for(int ii = 0; ii < monstreChemin.Length; ii++)
        {
            if(monstreChemin[ii] != " ")
            {
                for(int jj = 0; jj < reservePrefabEnnemis.childCount; jj++)
                {
                    if(reservePrefabEnnemis.GetChild(jj).GetChild(0).name == monstreChemin[ii])
                    {
                        //Activation pooling de l'ennemi
                        for(int kk=0;kk< reservePrefabEnnemis.GetChild(jj).childCount; kk++)
                        {
                            if (!reservePrefabEnnemis.GetChild(jj).GetChild(kk).gameObject.activeInHierarchy)
                            {
                                reservePrefabEnnemis.GetChild(jj).GetChild(kk).GetComponent<Ennemi>().way = ii;
                                reservePrefabEnnemis.GetChild(jj).GetChild(kk).GetComponent<Ennemi>().EnnemiPositionnementOnChemin();
                                StartCoroutine(reservePrefabEnnemis.GetChild(jj).GetChild(kk).GetComponent<Ennemi>().DeplacementEnnemi());
                                reservePrefabEnnemis.GetChild(jj).GetChild(kk).gameObject.SetActive(true);
                                if(kk >= reservePrefabEnnemis.GetChild(jj).childCount - 2)
                                {
                                    NewRoundAlimentationReserve(reservePrefabEnnemis);
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
    #endregion




    //Instantiation des reserves d'Ennemis et d'Unités Joueurs
    void InstantiateReservesEmpty(int _TailleReserve,string _BaseName,Transform _ParentReserve,UnitesRepertoire[] _UniteRepertoire)
    {
        for (int ii = 0; ii < _TailleReserve; ii++)
        {
            string nameGO = _BaseName + (ii + 1).ToString();
            GameObject newGO_Reserve;
            newGO_Reserve = new GameObject(nameGO);
            newGO_Reserve.transform.parent = _ParentReserve;
            newGO_Reserve.transform.localScale = _UniteRepertoire[ii].uniteScale;
        }
    }




    //Compte le nombre de GO actif dans la réserve
    public int CompteNbInactivesReserves(Transform reserveToCount)
    {
        int nbInactif = 0;
        for(int ii = 0; ii < reserveToCount.childCount; ii++)
        {
            if (!reserveToCount.GetChild(ii).gameObject.activeInHierarchy)
            {
                nbInactif++;
            }
        }

        return nbInactif;
    }



    //Donne les Fonctions aux boutons
    public IEnumerator FonctionsBoutonsPanelBatiment(Button monBouton, EmptyBatimentsConstruits monBat)
    {
        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
        monBouton.onClick.AddListener(delegate { monBat.OnConstructionBatiment(); });
        monBat.buttonObjet.enabled = false;
    }

    public IEnumerator FonctionsBoutonsAmeliorationUnite()
    {
        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
        
        for(int jj = 0;jj< reservePrefabUnitesJoueur.childCount; jj++)
        {
            _CampMilitaire.boutonsUnitesToUpgrade[jj].GetComponent<Button>().onClick.AddListener(delegate { _CampMilitaire.AmeliorerUnite(); });
            _CampMilitaire.boutonsUnitesToUpgrade[jj].SetActive(false);
        }
    }


    //Object Pooling activation GameObject
    public GameObject ActivationObjectListe(int _IndexObjet,Transform _Reserve)             //Cas Unités
    {
        Transform[] maListeUnites = new Transform[_Reserve.GetChild(_IndexObjet).childCount];
        for(int ii = 0; ii < maListeUnites.Length; ii++)
        {
            maListeUnites[ii] = _Reserve.GetChild(_IndexObjet).GetChild(ii);
        }

        int choix = 0;
        for (int ii = 0; ii < maListeUnites.Length; ii++)
        {
            if (!maListeUnites[ii].gameObject.GetComponent<UniteJoueur>().isRecruted)
            {
                maListeUnites[ii].gameObject.GetComponent<UniteJoueur>().isRecruted = true;
                choix = ii;
                break;
            }
        }
        return maListeUnites[choix].gameObject;
    }

    public GameObject ActivationObjectListe(Transform _MaListeObject)
    {
        Transform[] listeObjets = new Transform[_MaListeObject.childCount];
        int choix = 0;
        Debug.Log("Active");
        for (int ii = 0; ii < listeObjets.Length; ii++)
        {
            listeObjets[ii] = _MaListeObject.GetChild(ii);
        }

        for (int ii = 0; ii < listeObjets.Length; ii++)
        {
            if (!listeObjets[ii].gameObject.activeInHierarchy)
            {
                listeObjets[ii].gameObject.SetActive(true);
                choix = ii;
                break;
            }
        }

        return listeObjets[choix].gameObject;
    }



#region Popups
    public void PopUpsFonction(string _TextToAffich, Text _textToModif)
    {
        StartCoroutine(PopUpsCorout(_TextToAffich, _textToModif));
    }
    public IEnumerator PopUpsCorout(string _TextToAffich, Text _textToModif)
    {
        _textToModif.text = _TextToAffich;
        yield return new WaitForSeconds(2);
        _textToModif.text = "";
    }
#endregion



#region Interaction Batiment
    public void AttributeFonctionBatiment(Button monButton)
    {
        monButton.onClick.AddListener(delegate {_ChantierBTP.FonctionBatimentsSpeciauxGenerale(); });
    }
    #endregion



    #region Achievements
    public IEnumerator MoveImageAchievement(int ressourceJ1,int ressourceJ2,GameObject imageRessource)
    {
        if (!imageRessource.activeSelf && ressourceJ1 != ressourceJ2)
        {
            imageRessource.SetActive(true);
        }

        float posFinX = 720f;
        float currentPosX = imageRessource.GetComponent<RectTransform>().anchoredPosition.x;

        if (ressourceJ1 > ressourceJ2)
        {
            while (currentPosX > -posFinX)
            {
                currentPosX -= 20f;
                imageRessource.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentPosX, imageRessource.GetComponent<RectTransform>().anchoredPosition.y);
                yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                if(currentPosX <= -posFinX)
                {
                    imageRessource.GetComponent<RectTransform>().anchoredPosition = new Vector2(-posFinX, imageRessource.GetComponent<RectTransform>().anchoredPosition.y);
                    break;
                }
            }
        } else if (ressourceJ1 < ressourceJ2)
        {
            while (currentPosX < posFinX)
            {
                currentPosX += 20f;
                imageRessource.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentPosX, imageRessource.GetComponent<RectTransform>().anchoredPosition.y);
                yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                if (currentPosX >=  posFinX)
                {
                    imageRessource.GetComponent<RectTransform>().anchoredPosition = new Vector2(posFinX, imageRessource.GetComponent<RectTransform>().anchoredPosition.y);
                    break;
                }
            }
        }



        
        

    }

    public void CalculAchievements()
    {
        #region Récolte Ressources
        if (joueur1._NbRessourcesRecoltees > joueur2._NbRessourcesRecoltees)
        {
            joueur1._MoreRecolteRessources = true;
            Debug.Log("Joueur 1 a récolté le plus de ressources");
        }
        else if (joueur2._NbRessourcesRecoltees > joueur1._NbRessourcesRecoltees)
        {
            joueur2._MoreRecolteRessources = true;
            Debug.Log("Joueur 2 a récolté le plus de ressources");
        }
        #endregion
        #region Solidarity Points
        if (joueur1._SolidarityPoints > joueur2._SolidarityPoints)
        {
            joueur1._MoreSolidarityPoints = true;
        }
        else if (joueur1._SolidarityPoints < joueur2._SolidarityPoints)
        {
            joueur2._MoreSolidarityPoints = true;
        }
        #endregion
        #region Victory Points
        if (joueur1._VictoryPoints > joueur2._VictoryPoints)
        {
            joueur1._MoreVictoryPoints = true;
        }
        else if (joueur1._VictoryPoints < joueur2._VictoryPoints)
        {
            joueur2._MoreVictoryPoints = true;
        }
        #endregion
        #region Nombre d'unités recrutées
        if (joueur1._NbUnitesRecrutees > joueur2._NbUnitesRecrutees)
        {
            joueur1._MoreUnitesRecrutees = true;
        }
        else if (joueur1._NbUnitesRecrutees < joueur2._NbUnitesRecrutees)
        {
            joueur2._MoreUnitesRecrutees = true;
        }
        #endregion
        #region Nombre de super unités recrutées
        if (joueur1._NbSuperUnitesRecrutees > joueur2._NbSuperUnitesRecrutees)
        {
            joueur1._MoreSuperUnitesRecrutees = true;
        }
        else if (joueur1._NbSuperUnitesRecrutees < joueur2._NbSuperUnitesRecrutees)
        {
            joueur2._MoreSuperUnitesRecrutees = true;
        }
        #endregion
        #region Nombre d'ennemis tués
        if (joueur1._NbEnnemisKilled > joueur2._NbEnnemisKilled)
        {
            joueur1._MoreEnnemisKilled = true;
        }
        else if (joueur1._NbEnnemisKilled < joueur2._NbEnnemisKilled)
        {
            joueur2._MoreEnnemisKilled = true;
        }
        #endregion
        #region Nombre de tours construites
        if (joueur1.nbToursPossedees > joueur2.nbToursPossedees)
        {
            joueur1._MoreTowersConstruites = true;
        }
        else if (joueur1.nbToursPossedees < joueur2.nbToursPossedees)
        {
            joueur2._MoreTowersConstruites = true;
        }
        #endregion
        #region Nombre de batiments construits
        if (joueur1._NbBatimentsConstruits > joueur2._NbBatimentsConstruits)
        {
            joueur1._MoreBatimentsConstruits = true;
        }
        else if (joueur1._NbBatimentsConstruits < joueur2._NbBatimentsConstruits)
        {
            joueur2._MoreBatimentsConstruits = true;
        }
        #endregion
        #region Réparer porte de la ville
        if (joueur1._NbPvReparePorte > joueur2._NbPvReparePorte)
        {
            joueur1._MoreReparePorte = true;
        }
        else if (joueur1._NbPvReparePorte < joueur2._NbPvReparePorte)
        {
            joueur2._MoreReparePorte = true;
        }
        #endregion
        #region Temps pour jouer
        if(joueur1._TimeToMakeAction < joueur2._TimeToMakeAction)
        {
            joueur1._FastestPlayer = true;
        } else if(joueur1._TimeToMakeAction > joueur2._TimeToMakeAction)
        {
            joueur2._FastestPlayer = true;
        }
        #endregion
    }
    #endregion
}


