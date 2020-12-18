using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class LevelManager : MonoBehaviour
{
    //MODE DEBUG
    public bool isModeDebug;
    public int uniteRecrutableCaserne;

    //Caracteristiques du Niveau
    public string nomNiveau;
    private JSonParameters fileJson;
    private float precisionFloat;

    [HideInInspector] public RepertoireDesSprites repertoireSprites;
    public EventSystem myEventSystem;
    [HideInInspector] public Camera maMainCamera;


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
    private int nbEmplacementBTP;
    private EmptyBatimentsConstruits[] batimentsBTP;
    [HideInInspector] public int nbTours;
    private EmptyBatimentsConstruits[] tours;
    [HideInInspector] public GameObject _GuizmoRotationTour;
    [HideInInspector] public Button _CloseGuizmoRotTourButton;


    //Unites
    //Ennemis
    [HideInInspector] public Vector2[,] positionsEnnemisChemin;
    [HideInInspector] public Transform[,] positionsCheminLibre;       //=0 : Libre, =1 : Unité joueur, =2 : Ennemi

    //Emptys de réserve
    [HideInInspector] public int nbInstanceReserves;
    [HideInInspector] public int nbMiniInstancesInactiveReserves;                   //Nombre minimum d'instances inactives
    //Unites
    [HideInInspector] public Transform reservePrefabUnitesJoueur;
    [HideInInspector] public Transform[] reserveTypePrefabUnitesJoueur;
    [HideInInspector] public Transform[,] prefabUnitesJoueur;
    //Ennemis
    [HideInInspector] public Transform reservePrefabEnnemis;
    [HideInInspector] public Transform[] reserveTypePrefabEnnemis;
    [HideInInspector] public Transform[,] prefabEnnemis;
    //Super Unités
    [HideInInspector] public Transform reservePrefabSuperUnitesJoueur;
    [HideInInspector] public Transform[] reserveTypePrefabSuperUnitesJoueur;
    [HideInInspector] public Transform[,] prefabSuperUnitesJoueur;



    //Les Joueurs
    [HideInInspector] public Joueur joueur1;
    [HideInInspector] public Joueur joueur2;
    [HideInInspector] public Joueur[] tableauJoueurs = new Joueur[2];
    


    //Le "Pion" du joueur actif
    [HideInInspector] public GameObject _PionJoueurActif;
    private RectTransform _PionJoueurActifTransform;
    private Image _PionJoueurActifImage;
    [HideInInspector] public int _JoueurActif;
    [HideInInspector] public int nbActionRealised;              //Compte le nombre d'action réalisée dans le round


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
        numeroRound = 1;
        nbMaxRounds = fileJson.nbMaxRounds;
        textRounds = GameObject.Find("TextRound").GetComponent<Text>();
        textRounds.text = "Round : \n" + numeroRound.ToString() + "/" + nbMaxRounds.ToString();
        roundActivationDeploiementEnnemi = ParametresCarteOpenning.ficOptionParam.objet_vagues.o_vagues.i_nb_rounds_avant_premiere_vague;
        periodeDeploiement = new int[ParametresCarteOpenning.ficOptionParam.objet_vagues.o_vagues.o_structure_vagues.Length];
        for(int ii = 0; ii < periodeDeploiement.Length; ii++)
        {
            periodeDeploiement[ii] = ParametresCarteOpenning.ficOptionParam.objet_vagues.o_vagues.o_structure_vagues[ii].i_tps;
        }

        //Ennemis par round
        repertoireSprites.EnnemiDataCreate();
        /*ennemisPerRound = new int[repertoireSprites.ennemiData.Length, nbMaxRounds];
        for(int ii = 0; ii < nbMaxRounds; ii++)
        {
            ennemisPerRound[0, ii] = fileJson.nbEnnemisRound_type1[ii];
        }
        for (int ii = 0; ii < nbMaxRounds; ii++)
        {
            ennemisPerRound[1, ii] = fileJson.nbEnnemisRound_type2[ii];
        }*/


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



        #region Attribution des réserves et instanciation des réserves
        nbInstanceReserves = 5;
        nbMiniInstancesInactiveReserves = 4;

        #region PositionChemins
        positionsCheminLibre = new Transform[3, fileJson.positionXEnnemis1.Length];
        for(int ii = 0; ii < positionsCheminLibre.GetLength(0); ii++)
        {
            for (int jj = 0; jj < positionsCheminLibre.GetLength(1); jj++)
            {
                positionsCheminLibre[ii, jj] = null;
            }
        }

        #endregion


        #region Les Ennemis
        //Tableau des Chemins des ennemis
        //On réunit dans une seule variable tous les chemins précalculés pour les ennemis
        positionsEnnemisChemin = new Vector2[3, fileJson.positionXEnnemis1.Length];
        for (int ii = 0; ii < positionsEnnemisChemin.GetLength(1); ii++)
        {
            positionsEnnemisChemin[0, ii].x = (float)(fileJson.positionXEnnemis1[ii]) / precisionFloat;
            positionsEnnemisChemin[0, ii].y = (float)(fileJson.positionYEnnemis1[ii]) / precisionFloat;
            positionsEnnemisChemin[1, ii].x = (float)(fileJson.positionXEnnemis2[ii]) / precisionFloat;
            positionsEnnemisChemin[1, ii].y = (float)(fileJson.positionYEnnemis2[ii]) / precisionFloat;
            positionsEnnemisChemin[2, ii].x = (float)(fileJson.positionXEnnemis3[ii]) / precisionFloat;
            positionsEnnemisChemin[2, ii].y = (float)(fileJson.positionYEnnemis3[ii]) / precisionFloat;
        }

        reservePrefabEnnemis = GameObject.Find("ReservePrefabEnnemis").transform;
        InstantiateReservesEmpty(repertoireSprites.ennemiData.Length, "ReservePrefabEnnemis", reservePrefabEnnemis);


        reserveTypePrefabEnnemis = new Transform[reservePrefabEnnemis.childCount];
        prefabEnnemis = new Transform[reserveTypePrefabEnnemis.Length, nbInstanceReserves];
        for (int ii = 0; ii < reserveTypePrefabEnnemis.Length; ii++)
        {
            reserveTypePrefabEnnemis[ii] = reservePrefabEnnemis.GetChild(ii);
            int jj = 0;
            while (jj < nbInstanceReserves)
            {
                //int way = Random.Range(0, 3);
                InstanciationEnnemi_AffectationReserve(reserveTypePrefabEnnemis[ii], ii);  
                jj++;
            }
        }
        #endregion



        #region Unites Joueur
        reservePrefabUnitesJoueur = GameObject.Find("ReservePrefabUnitesJoueur").transform;
        repertoireSprites.UniteJoueurCreate();          //Crée les objets unités dans le répertoire
        InstantiateReservesEmpty(repertoireSprites.unitesJoueurData.Length, "ReservePrefabUnitesJoueur_Type", reservePrefabUnitesJoueur);         //On instantie les réserves d'unités

        reserveTypePrefabUnitesJoueur = new Transform[reservePrefabUnitesJoueur.childCount];
        prefabUnitesJoueur = new Transform[reserveTypePrefabUnitesJoueur.Length, nbInstanceReserves];
        for (int ii = 0; ii < reserveTypePrefabUnitesJoueur.Length; ii++)
        {
            reserveTypePrefabUnitesJoueur[ii] = reservePrefabUnitesJoueur.GetChild(ii);
            int jj = 0;
            while (jj < nbInstanceReserves)
            {
                int way = Random.Range(0, 3);
                InstanciationUnites_AffectationReserve(reserveTypePrefabUnitesJoueur[ii], ii, way,repertoireSprites.unitesJoueurData);
                prefabUnitesJoueur[ii, jj] = reserveTypePrefabUnitesJoueur[ii].GetChild(jj);
                jj++;
            }
        }
        #endregion


        #region Super Unites Joueur
        reservePrefabSuperUnitesJoueur = GameObject.Find("ReservePrefabSuperUnitesJoueur").transform;
        repertoireSprites.SuperUnitesJoueurCreate();
        InstantiateReservesEmpty(repertoireSprites.superUnitesJoueurData.Length, "ReservePrefabSuperUnitesJoueur_Type", reservePrefabSuperUnitesJoueur);    //On instantie les réserves de superunités

        reserveTypePrefabSuperUnitesJoueur = new Transform[reservePrefabSuperUnitesJoueur.childCount];
        prefabSuperUnitesJoueur = new Transform[reserveTypePrefabSuperUnitesJoueur.Length, nbInstanceReserves];
        for(int ii = 0; ii < reserveTypePrefabSuperUnitesJoueur.Length; ii++)
        {
            reserveTypePrefabSuperUnitesJoueur[ii] = reservePrefabSuperUnitesJoueur.GetChild(ii);
            int jj = 0;
            while (jj < nbInstanceReserves)
            {
                int way = Random.Range(0, 3);
                InstanciationSuperUnites_AffectationReserve(reserveTypePrefabSuperUnitesJoueur[ii], ii, way);
                prefabSuperUnitesJoueur[ii, jj] = reserveTypePrefabSuperUnitesJoueur[ii].GetChild(jj);
                jj++;
            }
        }
        #endregion




        #endregion


        #region Joueur
        //Joueur
        joueur1 = new Joueur(repertoireSprites.herosData[0].herosNom, repertoireSprites.herosData[0].herosSprite, false, 1, isModeDebug, repertoireSprites.herosData[0].pionActif);
        joueur2 = new Joueur(repertoireSprites.herosData[1].herosNom, repertoireSprites.herosData[1].herosSprite, false, 2, isModeDebug, repertoireSprites.herosData[1].pionActif);
        tableauJoueurs[0] = joueur1;
        tableauJoueurs[1] = joueur2;

        _JoueurActif = 1;
        _PionJoueurActif = GameObject.Find("ImagePionJoueurActif");
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

        //Bouton Recruter Unité assignation
        _CampMilitaire._InterractionsDisponibles[0].GetComponent<Button>().onClick.AddListener(delegate { _CampMilitaire.SelectionCreationConstruire(); });

        //Bouton Améliorer Unité assignation (Mettre Fonction activer boutons d'amélioration si on a plus d'1 type d'unités
        if (reserveTypePrefabUnitesJoueur.Length <= 1)
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
        }

        //Chargement des types de tours
        repertoireSprites.nbDataTours = nbTours;
        repertoireSprites.ToursCreate();


        //Instanciation du batiment construction Tours
        _ChantierTours = new BatimentConstructions(repertoireSprites.chantierTourSprite, "ChantierTours", true,0);
        _ChantierTours.scaleObjet = new Vector2((float)fileJson.scaleXConstructTours / precisionFloat, (float)fileJson.scaleYConstructTours / precisionFloat);
        _ChantierTours.transformObjet.GetChild(0).localScale = _ChantierTours.scaleObjet;
        _ChantierTours.transformObjet.name = _ChantierTours.nomObjet;
        _ChantierTours.transformObjet.position = new Vector2((float)fileJson.positionXConstructTours / precisionFloat, (float)fileJson.positionYConstructTours / precisionFloat);
        _ChantierTours.buttonObjet.onClick.AddListener(delegate { _ChantierTours.OuverturePanelInterractBatiment(_ChantierTours.indicePanelInterractBatiment); });
        _ChantierTours.panelInteractText.text = "Tour à construire ?";
        _ChantierTours.CreationBouton();
        //Activation/Désactivation des boutons/surplus de boutons
        for(int ii=0;ii< panelParentBatimentInterract.GetChild(0).GetChild(1).childCount; ii++)
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
        #endregion


        #region Panel du Marché
        //Attribution des fonctions des boutons du marché
        for(int ii = 0;ii< panelParentBatimentInterract.GetChild(4).GetChild(1).childCount; ii++)
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
    }


   





    //Permet l'instanciation d'un ennemi en choisissant son chemin et son index dans le répertoire pour lui donner ses propriétés
    void InstanciationEnnemi_AffectationReserve(Transform reserve ,int _IndexRepertoire)
    {
        Ennemis monEnnemi = new Ennemis(repertoireSprites.ennemiData[_IndexRepertoire].ennemiSprite, repertoireSprites.ennemiData[_IndexRepertoire].uniteBoxCollSize, repertoireSprites.ennemiData[_IndexRepertoire].uniteScaleCanvas, repertoireSprites.ennemiData[_IndexRepertoire].unitePositionCanvas);
        monEnnemi.nomUnite = repertoireSprites.ennemiData[_IndexRepertoire].uniteNom;
        monEnnemi.monTransform.name = monEnnemi.nomUnite;
        monEnnemi.monTransform.localScale = repertoireSprites.ennemiData[_IndexRepertoire].uniteScale;
        //monEnnemi.weaponHitbox.GetComponent<CallBacksWeapons>().att = repertoireSprites.ennemiData[_IndexRepertoire].ennemiAttaque;
        monEnnemi.speedMove = 10/repertoireSprites.ennemiData[_IndexRepertoire].ennemiSpeedMove;
        //monEnnemi.cheminChoisi = _Chemin;
        //monEnnemi.CheminDefinitionEnnemi(positionsEnnemisChemin);
        //monEnnemi.SpriteSheetName = "Sprites/"+monEnnemi.nomUnite+"_SpriteSheet";
        monEnnemi.monTransform.parent = reserve;
        monEnnemi.uniteVitalite = repertoireSprites.ennemiData[_IndexRepertoire].uniteTypeVitalite;
        monEnnemi.uniteInitiative = repertoireSprites.ennemiData[_IndexRepertoire].uniteTypeInitiative;
        monEnnemi.uniteDegats = repertoireSprites.ennemiData[_IndexRepertoire].uniteTypeDegats;
        monEnnemi.monAnimator.runtimeAnimatorController = repertoireSprites.ennemiData[_IndexRepertoire].ennemiAnimatorController;
        monEnnemi._VictoryPointGain = repertoireSprites.ennemiData[_IndexRepertoire].pointVictoireGain;
        monEnnemi.AttributionCaracteristiques();
    }


    //Instanciation des Prefabs des Unites et affectation dans une réserve
    public void InstanciationUnites_AffectationReserve(Transform reserve,int _IndexRepertoire,int _Chemin, UnitesJoueurRepertoire[] _MesDataUnitesJoueurRepertoire)
    {
        //Constructeur renseigne les éléments principaux
        UnitesJoueur monUniteJoueur = new UnitesJoueur(_MesDataUnitesJoueurRepertoire[_IndexRepertoire].uniteSpriteBase[0], _MesDataUnitesJoueurRepertoire[_IndexRepertoire].uniteBoxCollSize, _MesDataUnitesJoueurRepertoire[_IndexRepertoire].uniteScaleCanvas, _MesDataUnitesJoueurRepertoire[_IndexRepertoire].unitePositionCanvas, "UniteJoueur");

        //Met l'unité dans la bonne réserve
        monUniteJoueur.monTransform.parent = reserve;

        //Taille et nom de l'unité
        monUniteJoueur.nomUnite = _MesDataUnitesJoueurRepertoire[_IndexRepertoire].uniteNom;
        monUniteJoueur.monTransform.name = monUniteJoueur.nomUnite;
        monUniteJoueur.monTransform.localScale = _MesDataUnitesJoueurRepertoire[_IndexRepertoire].uniteScale;

        //Place l'unité
        monUniteJoueur.cheminChoisi = _Chemin;
        monUniteJoueur.CheminDefinitionUnitesJoueur(positionsEnnemisChemin);

        //Attribue les caractéristiques
        monUniteJoueur.uniteVitalite = _MesDataUnitesJoueurRepertoire[_IndexRepertoire].uniteTypeVitalite;
        monUniteJoueur.uniteVitaliteBase = _MesDataUnitesJoueurRepertoire[_IndexRepertoire].uniteTypeVitalite;
        monUniteJoueur.uniteInitiative = _MesDataUnitesJoueurRepertoire[_IndexRepertoire].uniteTypeInitiative;
        monUniteJoueur.uniteInitiativeBase = _MesDataUnitesJoueurRepertoire[_IndexRepertoire].uniteTypeInitiative;
        monUniteJoueur.uniteDegats = _MesDataUnitesJoueurRepertoire[_IndexRepertoire].uniteTypeDegats;
        monUniteJoueur.uniteDegatsBase = _MesDataUnitesJoueurRepertoire[_IndexRepertoire].uniteTypeDegats;
        monUniteJoueur.uniteOrdreListe = _MesDataUnitesJoueurRepertoire[_IndexRepertoire].uniteListeOrdre;
        monUniteJoueur.AttributionCaracteristiques();
    }
    

    void InstanciationSuperUnites_AffectationReserve(Transform reserve, int _IndexRepertoire, int _Chemin)
    {
        SuperUnitesJoueur maSuperUniteJoueur = new SuperUnitesJoueur(repertoireSprites.superUnitesJoueurData[_IndexRepertoire].uniteSpriteBase[0], repertoireSprites.superUnitesJoueurData[_IndexRepertoire].uniteBoxCollSize, repertoireSprites.superUnitesJoueurData[_IndexRepertoire].uniteScaleCanvas, repertoireSprites.superUnitesJoueurData[_IndexRepertoire].unitePositionCanvas, "SuperUniteJoueur");
        maSuperUniteJoueur.monTransform.parent = reserve;
        maSuperUniteJoueur.nomUnite = repertoireSprites.superUnitesJoueurData[_IndexRepertoire].uniteNom;
        maSuperUniteJoueur.monTransform.name = maSuperUniteJoueur.nomUnite;
        maSuperUniteJoueur.monTransform.localScale = repertoireSprites.superUnitesJoueurData[_IndexRepertoire].uniteScale;
        maSuperUniteJoueur.cheminChoisi = _Chemin;
        maSuperUniteJoueur.CheminDefinitionUnitesJoueur(positionsEnnemisChemin);
        maSuperUniteJoueur.uniteVitalite = repertoireSprites.superUnitesJoueurData[_IndexRepertoire].uniteTypeVitalite;
        maSuperUniteJoueur.uniteVitaliteBase = repertoireSprites.superUnitesJoueurData[_IndexRepertoire].uniteTypeVitalite;
        maSuperUniteJoueur.uniteInitiative = repertoireSprites.superUnitesJoueurData[_IndexRepertoire].uniteTypeInitiative;
        maSuperUniteJoueur.uniteInitiativeBase = repertoireSprites.superUnitesJoueurData[_IndexRepertoire].uniteTypeInitiative;
        maSuperUniteJoueur.uniteDegats = repertoireSprites.superUnitesJoueurData[_IndexRepertoire].uniteTypeDegats;
        maSuperUniteJoueur.uniteDegatsBase = repertoireSprites.superUnitesJoueurData[_IndexRepertoire].uniteTypeDegats;
        maSuperUniteJoueur.uniteOrdreListe = repertoireSprites.superUnitesJoueurData[_IndexRepertoire].uniteListeOrdre;
        maSuperUniteJoueur.AttributionCaracteristiques();
    }



    //Gère le joueur actif
    public void SetJoueurActif()
    {
        if(_JoueurActif == 1)
        {
            //_PionJoueurActifTransform.anchoredPosition = new Vector2(-535,-305);
            _PionJoueurActifImage.sprite = joueur1._ImagePionActif;
            /*joueur1.ImageInterfaceActif.SetActive(true);
            joueur2.ImageInterfaceActif.SetActive(false);*/
        } else if(_JoueurActif == 2)
        {
            _PionJoueurActifImage.sprite = joueur2._ImagePionActif;
            /*joueur1.ImageInterfaceActif.SetActive(false);
            joueur2.ImageInterfaceActif.SetActive(true);*/
            //_PionJoueurActifTransform.anchoredPosition = new Vector2(535, -305);
        }
    }

    //Gère le nombre d'actions et les rounds de jeu
    public void GestionActionRound()
    {
        nbActionRealised++;

        if (nbActionRealised >= 4)
        {
            ChangementRound();
        }
    }

    //Gère le changement de round
    private void ChangementRound()
    {
        nbActionRealised = 0;       //Remise à 0 du nombre d'actions
        numeroRound++;              //Incrémentation du numéro du round

        #region Victoire
        if (numeroRound > nbMaxRounds)
        {
            isEndGame = true;
            panelVictoire.SetActive(true);
        }
        #endregion

        #region Le jeu continue
        if (!isEndGame)
        {
            textRounds.text = "Round : " + numeroRound.ToString() + "/" + nbMaxRounds.ToString();       //Affichage Round


            #region Production des Ressources
            //Production par round des ressources
            _ChampBle.ProductionRessourceRound();
            _RecolteBois.ProductionRessourceRound();
            _MineFer.ProductionRessourceRound();
            _CarrierePierre.ProductionRessourceRound();
            #endregion


            #region Recrutement à la caserne
            //Augmentation du nombre d'unité dans la caserne
            _CampMilitaire.nbUniteRecrutable++;
            if (isMaisonConstruite)
            {
                int chanceActivationMaison = Random.RandomRange(0, 2);
                if(chanceActivationMaison == 0)
                {
                    _CampMilitaire.nbUniteRecrutable++;
                }
            }

            //Recrutement nouvelle unité réserve
            _CampMilitaire.CaserneCreationSoldat();
            #endregion

            #region Instance d'unités à la Caserne
            //Compter le nombre d'actifs dans les réserves Ennemis et Unités
            bool needNewInstances = false;
            int nbInactif = 0;
            int newInstances = 0;

            #region New Instances Unités
            //Unités
            for (int ii = 0; ii < reserveTypePrefabUnitesJoueur.Length; ii++)
            {
                nbInactif = CompteNbInactivesReserves(reserveTypePrefabUnitesJoueur[ii]);
                if(nbInactif < nbMiniInstancesInactiveReserves)
                {
                    needNewInstances = true;
                    newInstances = Mathf.Max(newInstances, nbMiniInstancesInactiveReserves - nbInactif);
                }
            }
            if (needNewInstances)
            {
                int oldDimension = prefabUnitesJoueur.GetLength(1);
                prefabUnitesJoueur = new Transform[reserveTypePrefabUnitesJoueur.Length, oldDimension + newInstances];
                for (int ii = 0; ii < reserveTypePrefabUnitesJoueur.Length; ii++)
                {
                    for (int jj = 0; jj < newInstances; jj++)
                    {
                        int way = Random.Range(0, 3);
                        InstanciationUnites_AffectationReserve(reserveTypePrefabUnitesJoueur[ii], ii,way,repertoireSprites.unitesJoueurData);
                    }
                    for(int jj = 0; jj < prefabUnitesJoueur.GetLength(1); jj++)
                    {
                        prefabUnitesJoueur[ii, jj] = reserveTypePrefabUnitesJoueur[ii].GetChild(jj);
                    }
                }
                newInstances = 0;
                needNewInstances = false;
            }
            #endregion


            #region New Instances SuperUnites
            for(int ii = 0; ii < reserveTypePrefabSuperUnitesJoueur.Length; ii++)
            {
                nbInactif = CompteNbInactivesReserves(reserveTypePrefabSuperUnitesJoueur[ii]);
                if(nbInactif < nbMiniInstancesInactiveReserves)
                {
                    needNewInstances = true;
                    newInstances = Mathf.Max(newInstances, nbMiniInstancesInactiveReserves - nbInactif);
                }
            }
            if (needNewInstances)
            {
                int oldDimension = prefabSuperUnitesJoueur.GetLength(1);
                prefabSuperUnitesJoueur = new Transform[reserveTypePrefabSuperUnitesJoueur.Length, oldDimension + newInstances];

                for (int ii = 0; ii < reserveTypePrefabSuperUnitesJoueur.Length; ii++)
                {
                    for (int jj = 0; jj < newInstances; jj++)
                    {
                        int way = Random.Range(0, 3);
                        InstanciationSuperUnites_AffectationReserve(reserveTypePrefabSuperUnitesJoueur[ii], ii, way);
                    }
                    for (int jj = 0; jj < prefabSuperUnitesJoueur.GetLength(1); jj++)
                    {
                        prefabSuperUnitesJoueur[ii, jj] = reserveTypePrefabSuperUnitesJoueur[ii].GetChild(jj);
                    }
                }

                newInstances = 0;
                needNewInstances = false;
            }

            #endregion

            #region Ennemis Déploiement
            //Lancement Coroutine du déploiement des ennemis. Lancement unique au round approprié
            if(numeroRound !=0 && numeroRound == roundActivationDeploiementEnnemi)
            {
                //Debug.Log("Réactiver le déploiement");
                StartCoroutine(DeploiementEnnemis());
            }

            #endregion

            #endregion


            #region Autels et attribution de points de victoire
            for (int ii = 0; ii < tableauJoueurs.Length; ii++)
            {
                for(int jj = 0; jj < tableauJoueurs[ii]._NombreAutels; jj++)
                {
                    tableauJoueurs[ii]._VictoryPoints += NbVictoryPointsPerAutel;
                    tableauJoueurs[ii].AffichePointVictoireSolidarite(tableauJoueurs[ii].victoirePointText, tableauJoueurs[ii]._VictoryPoints);
                }
            }

            #endregion


            #region Tente de Soins
            for(int ii = 0; ii < tableauJoueurs.Length; ii++)
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
                if(joueur1._ReserveRound && !joueur2._ReserveRound)
                {
                    _JoueurActif = 1;
                } else if (!joueur1._ReserveRound && joueur2._ReserveRound)
                {
                    _JoueurActif = 2;
                }
                SetJoueurActif();
                //Remise à 0 de Réserve Round depuis le chateau
                joueur1._ReserveRound = false;
                joueur2._ReserveRound = false;
                _Chateau._ReserveRoundActif = false;
            }
            #endregion
        }
        #endregion

    }


    //New Instance d'ennemis 
    public void NewInstancesEnnemis()
    {
        bool needNewInstances = false;
        int nbInactif = 0;
        int newInstances = 0;
        for (int ii = 0; ii < reserveTypePrefabEnnemis.Length; ii++)
        {
            nbInactif = CompteNbInactivesReserves(reserveTypePrefabEnnemis[ii]);
            if (nbInactif < nbMiniInstancesInactiveReserves)
            {
                needNewInstances = true;
                newInstances = Mathf.Max(newInstances, nbMiniInstancesInactiveReserves - nbInactif);
            }
        }
        if (needNewInstances)
        {
            int oldDimension = prefabEnnemis.GetLength(1);
            prefabEnnemis = new Transform[reserveTypePrefabEnnemis.Length, oldDimension + newInstances];
            for (int ii = 0; ii < reserveTypePrefabEnnemis.Length; ii++)
            {
                for (int jj = 0; jj < newInstances; jj++)
                {
                    //int way = Random.Range(0, 3);
                    InstanciationEnnemi_AffectationReserve(reserveTypePrefabEnnemis[ii], ii);
                }
                for (int jj = 0; jj < prefabEnnemis.GetLength(1); jj++)
                {
                    prefabEnnemis[ii, jj] = reserveTypePrefabEnnemis[ii].GetChild(jj);
                }
            }
            newInstances = 0;
            needNewInstances = false;
        }
    }



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


                Debug.Log((int)chrono);
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
                                reservePrefabEnnemis.GetChild(jj).GetChild(kk).GetComponent<CallBacksEnnemis>().monEnnemi.cheminChoisi = ii;
                                reservePrefabEnnemis.GetChild(jj).GetChild(kk).GetComponent<CallBacksEnnemis>().monEnnemi.CheminDefinitionEnnemi(positionsEnnemisChemin);
                                reservePrefabEnnemis.GetChild(jj).GetChild(kk).gameObject.SetActive(true);
                                if(kk >= reservePrefabEnnemis.GetChild(jj).childCount - 2)
                                {
                                    NewInstancesEnnemis();
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }
    }





    //Instantiation des reserves d'Ennemis et d'Unités Joueurs
    void InstantiateReservesEmpty(int _TailleReserve,string _BaseName,Transform _ParentReserve)
    {
        for (int ii = 0; ii < _TailleReserve; ii++)
        {
            string nameGO = _BaseName + (ii + 1).ToString();
            GameObject newGO_Reserve;
            newGO_Reserve = new GameObject(nameGO);
            newGO_Reserve.transform.parent = _ParentReserve;
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
        for(int jj = 0;jj< reserveTypePrefabUnitesJoueur.Length; jj++)
        {
            _CampMilitaire.boutonsUnitesToUpgrade[jj].GetComponent<Button>().onClick.AddListener(delegate { _CampMilitaire.AmeliorerUnite(); });
            _CampMilitaire.boutonsUnitesToUpgrade[jj].SetActive(false);
        }
    }


    //Object Pooling activation GameObject
    public GameObject ActivationObjectListe(Transform[,] monRepertoire,int _IndexObjet)
    {
        Transform[] listeObjets = new Transform[monRepertoire.GetLongLength(1)];
        int choix = 0;
        for(int ii = 0; ii < listeObjets.Length; ii++)
        {
            listeObjets[ii] = monRepertoire[_IndexObjet, ii];
        }

        for(int ii = 0; ii < listeObjets.Length; ii++)
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

    public GameObject ActivationObjectListe(Transform _MaListeObject)
    {
        Transform[] listeObjets = new Transform[_MaListeObject.childCount];
        int choix = 0;

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
}


