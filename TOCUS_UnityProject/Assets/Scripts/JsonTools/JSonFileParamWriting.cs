using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//Script pour écrire dans le fichier JSon.
/// <summary>
/// Pour les variables nbTours et nbEmplacementBTP:
/// - Placer des emptys aux positions choisies (les renommer en conséquence)
/// - Leur attribuer un tag (déjà fait pour nbTours)
/// - Chercher les GameObject avec le tag
/// - attribuer à nb... la taille du tableau de GameObject
/// 
/// Pour les Vector2 de positions des batiments ou tours :
/// - Donner la taille du tableau égale à nb...
/// - Attribuer à la coordonnée x (y) la position x (y) du GameObject, pour chaque index
/// 
/// Pour les chemins des positions des ennemis
/// - Créer un empty ennemi pour chaque chemin
/// - Créer un empty parent pour chaque chemin
/// - Créer des positions clés (en mettre autant que l'on veut de précision)
/// - Dans le lancement la coroutine MouvementCheminEnnemi, donner l'empty ennemi, l'empty parent chemin, l'index du chemin et le pas de déplacement
/// - Réitérer pour le nombre de chemin total + décommenter les variables et les parties de code gérant les 3 chemins
/// - Lancer la coroutine pour chaque chemin
/// 
/// 
/// Adapter la fonction EcritureJson avec les autres variables à écrire. 
/// Dans le fichier JSon de base, donner des variables arbitraires avant de lancer l'écriture, erreur sinon !
/// Pour savoir combien en donner, ne pas hésiter à faire un Debug.Log() (=print !) de la dimension des tableaux !
/// 
/// 
/// 
/// ATTENTION, à la lecture des fichiers Json, il faudra arrondir à la nième décimale choisie !
/// </summary>


public class JSonFileParamWriting : MonoBehaviour
{
    //Variables pour écrire le fichier JSON
    public string nomJsonFileCurrent;       //Nom du fichier Json à modifier

    //Path et fichier JSon
    private string path;                    //Chemin du fichier Json
    private string jSonString;              //Données du fichier Json en format texte
    private JSonParameters fileJSon;        //Classe regroupant les paramètres du fichier Json
    public float precisionFloat;

    //Paramètres pour écrire dans le JSon
    private int nbTours;                        //Nombre d'emplacements de tours
    private Vector2[] positionTours;            //Position d'emplacements des tours
    private Vector2[] scaleTours;               //Echelle d'emplacements des tours
    public int[] modifAccessTours;              //Modificateurs d'accès des emplacements des tours
    private int nbEmplacementBTP;               //Nombre d'emplacements de batiments
    private Vector2[] positionBTP;              //Position des emplacements des batiments
    private Vector2[] scaleBTP;                 //Echelle des emplacements des batiments
    public int[] modifAccesBTP;                 //Modificateurs d'accès des emplacements des batiments
    private Transform positionPorte;            //Transform de la porte
    private Vector2[] positionRecoltes;         //Position des récoltes
    private Vector2[] scaleRecoltes;            //Echelle des récoltes
    public int[] productionRecoltes;            //Nombre de récoltes produite à chaque round
    private Transform positionCampMilitaire;    //Transform du camp militaire
    private Transform transformConstructTours;  //Transform du batiment de construction des tours
    private Transform transformConstructBTP;    //Transform du batiment de construction des batiments
    private Transform transformChateau;         //Transform du chateau


    //Mur de la ville
    private Transform murVilleTransform;              //Mur de la ville

    //Arbres
    private GameObject[] arbres_Type1;       //GO des différents types d'arbres
    private GameObject[] arbres_Type2;
    private GameObject[] arbres_Type3;
    private GameObject[] arbres_Type4;
    private GameObject[] arbres_Type5;

    //Pierres
    private GameObject[] pierre_Type1;

    // Start is called before the first frame update
    void Start()
    {
        fileJSon = new JSonParameters();        //Instanciation de la classe JSonParameters

        //Ouverture FileJSon
        fileJSon = fileJSon.OuvertureJson(nomJsonFileCurrent);
        path = Application.streamingAssetsPath + "/" + nomJsonFileCurrent + ".json";       

        //Taille du tableau des positions des tours
        GameObject[] toursEmpty = GameObject.FindGameObjectsWithTag("Tours");           //Tableau temporaire des emplacements de tours
        nbTours = toursEmpty.Length;                    //Affectation de la variable du nombre de tours
        positionTours = new Vector2[nbTours];           //Insanciation du tableau des positions des tours
        scaleTours = new Vector2[nbTours];              //Insanciation du tableau des scales des tours
        //Ecriture du Vector2 de position et scale des tours
        for (int ii = 0; ii < nbTours; ii++)
        {
            positionTours[ii].x = toursEmpty[ii].transform.position.x;
            positionTours[ii].y = toursEmpty[ii].transform.position.y;
            scaleTours[ii].x = toursEmpty[ii].transform.localScale.x;
            scaleTours[ii].y = toursEmpty[ii].transform.localScale.y;
        }

        //Taille du tableau des positions des emplacementsBTP
        GameObject[] _BTP_Empty = GameObject.FindGameObjectsWithTag("BTP");           //Tableau temporaire des emplacements de batiments
        nbEmplacementBTP = _BTP_Empty.Length;                    //Affectation de la variable du nombre de batiments
        positionBTP = new Vector2[nbEmplacementBTP];           //Insanciation du tableau des positions des batiments
        scaleBTP = new Vector2[nbEmplacementBTP];              //Insanciation du tableau des scales des batiments
        //Ecriture du Vector2 de position et scale des batiments constructibles
        for (int ii = 0; ii < nbEmplacementBTP; ii++)
        {
            positionBTP[ii].x = _BTP_Empty[ii].transform.position.x;
            positionBTP[ii].y = _BTP_Empty[ii].transform.position.y;
            scaleBTP[ii].x = _BTP_Empty[ii].transform.localScale.x;
            scaleBTP[ii].y = _BTP_Empty[ii].transform.localScale.y;
        }


        //Transform de la Porte
        positionPorte = GameObject.Find("PorteVille").transform;

        //Taille du tableau des position et scale des récoltes
        positionRecoltes = new Vector2[4];
        scaleRecoltes = new Vector2[4];
        //Attribution pour toutes les ressources de la position et de l'échelle de l'objet
        positionRecoltes[0] = new Vector2(GameObject.Find("ChampBle").transform.position.x, GameObject.Find("ChampBle").transform.position.y);
        positionRecoltes[1] = new Vector2(GameObject.Find("RecolteBois").transform.position.x, GameObject.Find("RecolteBois").transform.position.y);
        positionRecoltes[2] = new Vector2(GameObject.Find("MineFer").transform.position.x, GameObject.Find("MineFer").transform.position.y);
        positionRecoltes[3] = new Vector2(GameObject.Find("CarrierePierre").transform.position.x, GameObject.Find("CarrierePierre").transform.position.y);
        scaleRecoltes[0] = new Vector2(GameObject.Find("ChampBle").transform.localScale.x, GameObject.Find("ChampBle").transform.localScale.y);
        scaleRecoltes[1] = new Vector2(GameObject.Find("RecolteBois").transform.localScale.x, GameObject.Find("RecolteBois").transform.localScale.y);
        scaleRecoltes[2] = new Vector2(GameObject.Find("MineFer").transform.localScale.x, GameObject.Find("MineFer").transform.localScale.y);
        scaleRecoltes[3] = new Vector2(GameObject.Find("CarrierePierre").transform.localScale.x, GameObject.Find("CarrierePierre").transform.localScale.y);
        

       
        //Transform des batiments camp militaire, construction de tours, construction de batiments, chateau
        positionCampMilitaire = GameObject.Find("CampMilitaire").transform;
        transformConstructTours = GameObject.Find("ConstructTour").transform;
        transformConstructBTP = GameObject.Find("ConstructBTP").transform;
        transformChateau = GameObject.Find("Chateau").transform;



        //Mur de la ville
        murVilleTransform = GameObject.Find("MurVille").transform;

        //Arbres
        arbres_Type1 = GameObject.FindGameObjectsWithTag("Arbre_Type1");
        arbres_Type2 = GameObject.FindGameObjectsWithTag("Arbre_Type2");
        arbres_Type3 = GameObject.FindGameObjectsWithTag("Arbre_Type3");
        arbres_Type4 = GameObject.FindGameObjectsWithTag("Arbre_Type4");
        arbres_Type5 = GameObject.FindGameObjectsWithTag("Arbre_Type5");


        //Pierres
        pierre_Type1 = GameObject.FindGameObjectsWithTag("Pierre_Type1");


        // Fin de la partie qui s'écrit dans le fichier JsonFile Base

        EcritureJson(fileJSon);
    }

    //Exemple de Méthode d'écriture dans un fichier JSON
    void EcritureJson(JSonParameters jsonfile)
    {
        //Précision des float
        jsonfile.precisionFloat = (int)precisionFloat;

        //Nombre de tours
        jsonfile.nbTours = nbTours;
        jsonfile.modifAccessTours = new int[jsonfile.nbTours];
        jsonfile.positionXTours = new int[jsonfile.nbTours];
        jsonfile.positionYTours = new int[jsonfile.nbTours];
        jsonfile.scaleXTours = new int[jsonfile.nbTours];
        jsonfile.scaleYTours = new int[jsonfile.nbTours];


        //Positions des tours
        for (int ii = 0; ii < nbTours; ii++)
        {
            jsonfile.modifAccessTours[ii] = modifAccessTours[ii];
            jsonfile.positionXTours[ii] = (int)(positionTours[ii].x * precisionFloat);
            jsonfile.positionYTours[ii] = (int)(positionTours[ii].y * precisionFloat);
            jsonfile.scaleXTours[ii] = (int)(scaleTours[ii].x * precisionFloat);
            jsonfile.scaleYTours[ii] = (int)(scaleTours[ii].y * precisionFloat);
        }


        //Nombre de BTP
        jsonfile.nbEmplacementBTP = nbEmplacementBTP;
        jsonfile.positionXBTP = new int[jsonfile.nbEmplacementBTP];
        jsonfile.positionYBTP = new int[jsonfile.nbEmplacementBTP];
        jsonfile.scaleXBTP = new int[jsonfile.nbEmplacementBTP];
        jsonfile.scaleYBTP = new int[jsonfile.nbEmplacementBTP];
        jsonfile.modifAccesBTP = new int[jsonfile.nbEmplacementBTP];

        //Positions des BTP
        for (int ii = 0; ii < nbEmplacementBTP; ii++)
        {
            jsonfile.positionXBTP[ii] = (int)(positionBTP[ii].x * precisionFloat);
            jsonfile.positionYBTP[ii] = (int)(positionBTP[ii].y * precisionFloat);
            jsonfile.scaleXBTP[ii] = (int)(scaleBTP[ii].x * precisionFloat);
            jsonfile.scaleYBTP[ii] = (int)(scaleBTP[ii].y * precisionFloat);
            jsonfile.modifAccesBTP[ii] = modifAccesBTP[ii];
        }


        //Position /Scale Porte
        jsonfile.positionXPorte = (int)(positionPorte.transform.position.x * precisionFloat);
        jsonfile.positionYPorte = (int)(positionPorte.transform.position.y * precisionFloat);
        jsonfile.scaleXPorte = (int)(positionPorte.transform.localScale.x * precisionFloat);
        jsonfile.scaleYPorte = (int)(positionPorte.transform.localScale.y * precisionFloat);


        jsonfile.positionXRecoltes = new int[positionRecoltes.Length];
        jsonfile.positionYRecoltes = new int[positionRecoltes.Length];
        jsonfile.scaleXRecoltes = new int[positionRecoltes.Length];
        jsonfile.scaleYRecoltes = new int[positionRecoltes.Length];
        jsonfile.productionRecoltes = new int[positionRecoltes.Length];

        //Positions / Scales Récoltes
        for (int ii = 0; ii < positionRecoltes.Length; ii++)
        {
            jsonfile.positionXRecoltes[ii] = (int)(positionRecoltes[ii].x * precisionFloat);
            jsonfile.positionYRecoltes[ii] = (int)(positionRecoltes[ii].y * precisionFloat);
            jsonfile.scaleXRecoltes[ii] = (int)(scaleRecoltes[ii].x * precisionFloat);
            jsonfile.scaleYRecoltes[ii] = (int)(scaleRecoltes[ii].y * precisionFloat);
            jsonfile.productionRecoltes[ii] = productionRecoltes[ii];
        }

        //Position /Scale Camp Militaire
        jsonfile.positionXCampMilitaire = (int)(positionCampMilitaire.transform.position.x * precisionFloat);
        jsonfile.positionYCampMilitaire = (int)(positionCampMilitaire.transform.position.y * precisionFloat);
        jsonfile.scaleXCampMilitaire = (int)(positionCampMilitaire.transform.localScale.x * precisionFloat);
        jsonfile.scaleYCampMilitaire = (int)(positionCampMilitaire.transform.localScale.y * precisionFloat);


        //Position /Scale ConstructTour
        jsonfile.positionXConstructTours = (int)(transformConstructTours.transform.position.x * precisionFloat);
        jsonfile.positionYConstructTours = (int)(transformConstructTours.transform.position.y * precisionFloat);
        jsonfile.scaleXConstructTours = (int)(transformConstructTours.transform.localScale.x * precisionFloat);
        jsonfile.scaleYConstructTours = (int)(transformConstructTours.transform.localScale.y * precisionFloat);


        //Position /Scale ConstructBTP
        jsonfile.positionXConstructBTP = (int)(transformConstructBTP.transform.position.x * precisionFloat);
        jsonfile.positionYConstructBTP = (int)(transformConstructBTP.transform.position.y * precisionFloat);
        jsonfile.scaleXConstructBTP = (int)(transformConstructBTP.transform.localScale.x * precisionFloat);
        jsonfile.scaleYConstructBTP = (int)(transformConstructBTP.transform.localScale.y * precisionFloat);


        //Position /Scale Chateau
        jsonfile.positionXChateau = (int)(transformChateau.transform.position.x * precisionFloat);
        jsonfile.positionYChateau = (int)(transformChateau.transform.position.y * precisionFloat);
        jsonfile.scaleXChateau = (int)(transformChateau.transform.localScale.x * precisionFloat);
        jsonfile.scaleYChateau = (int)(transformChateau.transform.localScale.y * precisionFloat);


        //Mur Ville
        jsonfile.posMurVilleX = (int)(murVilleTransform.transform.position.x * precisionFloat);
        jsonfile.posMurVilleY = (int)(murVilleTransform.transform.position.y * precisionFloat);
        jsonfile.posMurVilleZ = (int)(murVilleTransform.transform.position.z * precisionFloat);
        jsonfile.scaleXMurVille = (int)(murVilleTransform.transform.localScale.x * precisionFloat);
        jsonfile.scaleYMurVille = (int)(murVilleTransform.transform.localScale.y * precisionFloat);
        jsonfile.scaleZMurVille = (int)(murVilleTransform.transform.localScale.z * precisionFloat);


        #region Arbres Map
        //Arbres
        jsonfile.posX_Arbre_Type1 = new int[arbres_Type1.Length];
        jsonfile.posY_Arbre_Type1 = new int[arbres_Type1.Length];
        jsonfile.posZ_Arbre_Type1 = new int[arbres_Type1.Length];
        jsonfile.posX_Arbre_Type2 = new int[arbres_Type2.Length];
        jsonfile.posY_Arbre_Type2 = new int[arbres_Type2.Length];
        jsonfile.posZ_Arbre_Type2 = new int[arbres_Type2.Length];
        jsonfile.posX_Arbre_Type3 = new int[arbres_Type3.Length];
        jsonfile.posY_Arbre_Type3 = new int[arbres_Type3.Length];
        jsonfile.posZ_Arbre_Type3 = new int[arbres_Type3.Length];
        jsonfile.posX_Arbre_Type4 = new int[arbres_Type4.Length];
        jsonfile.posY_Arbre_Type4 = new int[arbres_Type4.Length];
        jsonfile.posZ_Arbre_Type4 = new int[arbres_Type4.Length];
        jsonfile.posX_Arbre_Type5 = new int[arbres_Type5.Length];
        jsonfile.posY_Arbre_Type5 = new int[arbres_Type5.Length];
        jsonfile.posZ_Arbre_Type5 = new int[arbres_Type5.Length];

        jsonfile.scaleX_Arbre_Type1 = new int[arbres_Type1.Length];
        jsonfile.scaleY_Arbre_Type1 = new int[arbres_Type1.Length];
        jsonfile.scaleZ_Arbre_Type1 = new int[arbres_Type1.Length];
        jsonfile.scaleX_Arbre_Type2 = new int[arbres_Type2.Length];
        jsonfile.scaleY_Arbre_Type2 = new int[arbres_Type2.Length];
        jsonfile.scaleZ_Arbre_Type2 = new int[arbres_Type2.Length];
        jsonfile.scaleX_Arbre_Type3 = new int[arbres_Type3.Length];
        jsonfile.scaleY_Arbre_Type3 = new int[arbres_Type3.Length];
        jsonfile.scaleZ_Arbre_Type3 = new int[arbres_Type3.Length];
        jsonfile.scaleX_Arbre_Type4 = new int[arbres_Type4.Length];
        jsonfile.scaleY_Arbre_Type4 = new int[arbres_Type4.Length];
        jsonfile.scaleZ_Arbre_Type4 = new int[arbres_Type4.Length];
        jsonfile.scaleX_Arbre_Type5 = new int[arbres_Type5.Length];
        jsonfile.scaleY_Arbre_Type5 = new int[arbres_Type5.Length];
        jsonfile.scaleZ_Arbre_Type5 = new int[arbres_Type5.Length];

        for(int ii = 0; ii < arbres_Type1.Length; ii++)
        {
            jsonfile.posX_Arbre_Type1[ii] = (int)(arbres_Type1[ii].transform.position.x * precisionFloat);
            jsonfile.posY_Arbre_Type1[ii] = (int)(arbres_Type1[ii].transform.position.y * precisionFloat);
            jsonfile.posZ_Arbre_Type1[ii] = (int)(arbres_Type1[ii].transform.position.z * precisionFloat);
            jsonfile.scaleX_Arbre_Type1[ii] = (int)(arbres_Type1[ii].transform.localScale.x * precisionFloat);
            jsonfile.scaleY_Arbre_Type1[ii] = (int)(arbres_Type1[ii].transform.localScale.y * precisionFloat);
            jsonfile.scaleZ_Arbre_Type1[ii] = (int)(arbres_Type1[ii].transform.localScale.z * precisionFloat);
        }

        for (int ii = 0; ii < arbres_Type2.Length; ii++)
        {
            jsonfile.posX_Arbre_Type2[ii] = (int)(arbres_Type2[ii].transform.position.x * precisionFloat);
            jsonfile.posY_Arbre_Type2[ii] = (int)(arbres_Type2[ii].transform.position.y * precisionFloat);
            jsonfile.posZ_Arbre_Type2[ii] = (int)(arbres_Type2[ii].transform.position.z * precisionFloat);
            jsonfile.scaleX_Arbre_Type2[ii] = (int)(arbres_Type2[ii].transform.localScale.x * precisionFloat);
            jsonfile.scaleY_Arbre_Type2[ii] = (int)(arbres_Type2[ii].transform.localScale.y * precisionFloat);
            jsonfile.scaleZ_Arbre_Type2[ii] = (int)(arbres_Type2[ii].transform.localScale.z * precisionFloat);
        }

        for (int ii = 0; ii < arbres_Type3.Length; ii++)
        {
            jsonfile.posX_Arbre_Type3[ii] = (int)(arbres_Type3[ii].transform.position.x * precisionFloat);
            jsonfile.posY_Arbre_Type3[ii] = (int)(arbres_Type3[ii].transform.position.y * precisionFloat);
            jsonfile.posZ_Arbre_Type3[ii] = (int)(arbres_Type3[ii].transform.position.z * precisionFloat);
            jsonfile.scaleX_Arbre_Type3[ii] = (int)(arbres_Type3[ii].transform.localScale.x * precisionFloat);
            jsonfile.scaleY_Arbre_Type3[ii] = (int)(arbres_Type3[ii].transform.localScale.y * precisionFloat);
            jsonfile.scaleZ_Arbre_Type3[ii] = (int)(arbres_Type3[ii].transform.localScale.z * precisionFloat);
        }

        for (int ii = 0; ii < arbres_Type4.Length; ii++)
        {
            jsonfile.posX_Arbre_Type4[ii] = (int)(arbres_Type4[ii].transform.position.x * precisionFloat);
            jsonfile.posY_Arbre_Type4[ii] = (int)(arbres_Type4[ii].transform.position.y * precisionFloat);
            jsonfile.posZ_Arbre_Type4[ii] = (int)(arbres_Type4[ii].transform.position.z * precisionFloat);
            jsonfile.scaleX_Arbre_Type4[ii] = (int)(arbres_Type4[ii].transform.localScale.x * precisionFloat);
            jsonfile.scaleY_Arbre_Type4[ii] = (int)(arbres_Type4[ii].transform.localScale.y * precisionFloat);
            jsonfile.scaleZ_Arbre_Type4[ii] = (int)(arbres_Type4[ii].transform.localScale.z * precisionFloat);
        }

        for (int ii = 0; ii < arbres_Type5.Length; ii++)
        {
            jsonfile.posX_Arbre_Type5[ii] = (int)(arbres_Type5[ii].transform.position.x * precisionFloat);
            jsonfile.posY_Arbre_Type5[ii] = (int)(arbres_Type5[ii].transform.position.y * precisionFloat);
            jsonfile.posZ_Arbre_Type5[ii] = (int)(arbres_Type5[ii].transform.position.z * precisionFloat);
            jsonfile.scaleX_Arbre_Type5[ii] = (int)(arbres_Type5[ii].transform.localScale.x * precisionFloat);
            jsonfile.scaleY_Arbre_Type5[ii] = (int)(arbres_Type5[ii].transform.localScale.y * precisionFloat);
            jsonfile.scaleZ_Arbre_Type5[ii] = (int)(arbres_Type5[ii].transform.localScale.z * precisionFloat);
        }
        #endregion


        #region Pierres map
        jsonfile.posX_Pierre_Type1 = new int[pierre_Type1.Length];
        jsonfile.posY_Pierre_Type1 = new int[pierre_Type1.Length];
        jsonfile.posZ_Pierre_Type1 = new int[pierre_Type1.Length];
        jsonfile.scaleX_Pierre_Type1 = new int[pierre_Type1.Length];
        jsonfile.scaleY_Pierre_Type1 = new int[pierre_Type1.Length];
        jsonfile.scaleZ_Pierre_Type1 = new int[pierre_Type1.Length];

        for (int ii = 0; ii < pierre_Type1.Length; ii++)
        {
            jsonfile.posX_Pierre_Type1[ii] = (int)(pierre_Type1[ii].transform.position.x * precisionFloat);
            jsonfile.posY_Pierre_Type1[ii] = (int)(pierre_Type1[ii].transform.position.y * precisionFloat);
            jsonfile.posZ_Pierre_Type1[ii] = (int)(pierre_Type1[ii].transform.position.z * precisionFloat);
            jsonfile.scaleX_Pierre_Type1[ii] = (int)(pierre_Type1[ii].transform.localScale.x * precisionFloat);
            jsonfile.scaleY_Pierre_Type1[ii] = (int)(pierre_Type1[ii].transform.localScale.y * precisionFloat);
            jsonfile.scaleZ_Pierre_Type1[ii] = (int)(pierre_Type1[ii].transform.localScale.z * precisionFloat);
        }
        #endregion



        //Fin de l'attribution des valeurs pour le fichier JSon
        //Ecriture dans le fichier
        jSonString = JsonUtility.ToJson(jsonfile,true);     //Lever le true lorsque l'on écrira les fichiers finaux (optimisation taille)
        File.WriteAllText(path, jSonString);
        
        Debug.Log("Ecriture Terminée !");
    }

}


public class JSonParameters
{
    //Précision des Floats
    public int precisionFloat;

    //Nombre d'emplacement des tours
    public int nbTours;
    //Positions des emplacements des emplacements des tours
    public int[] positionXTours;
    public int[] positionYTours;
    public int[] scaleXTours;
    public int[] scaleYTours;
    public int[] modifAccessTours;

    //Nombre des emplacements des BTP
    public int nbEmplacementBTP;
    //Positions des emplacements des BTP
    public int[] positionXBTP;
    public int[] positionYBTP;
    public int[] scaleXBTP;
    public int[] scaleYBTP;
    //Modificateurs d'accès des BTP (int, =0 dispo à tous, =1 dispo J1, =2 dispo J2)
    public int[] modifAccesBTP;

    //Position/Scale Porte 
    public int positionXPorte;
    public int positionYPorte;
    public int scaleXPorte;
    public int scaleYPorte;

    //Position/Scale Recoltes (ii=0 blé, ii=1 bois, ii=2 fer, ii=3 pierre)
    public int[] positionXRecoltes;
    public int[] positionYRecoltes;
    public int[] scaleXRecoltes;
    public int[] scaleYRecoltes;
    public int[] productionRecoltes;


    //Position/Scale du camp militaire (recrutement des unités)
    public int positionXCampMilitaire;
    public int positionYCampMilitaire;
    public int scaleXCampMilitaire;
    public int scaleYCampMilitaire;


    //Position/Scale du constructeur de Tours
    public int positionXConstructTours;
    public int positionYConstructTours;
    public int scaleXConstructTours;
    public int scaleYConstructTours;


    //Position/Scale du constructeur de Batiments
    public int positionXConstructBTP;
    public int positionYConstructBTP;
    public int scaleXConstructBTP;
    public int scaleYConstructBTP;


    //Position/Scale du Chateau
    public int positionXChateau;
    public int positionYChateau;
    public int scaleXChateau;
    public int scaleYChateau;


    //Position/Scale du mur de la ville
    public int posMurVilleX;
    public int posMurVilleY;
    public int posMurVilleZ;
    public int scaleXMurVille;
    public int scaleYMurVille;
    public int scaleZMurVille;


    //Position/Scale des arbres
    public int[] posX_Arbre_Type1;
    public int[] posY_Arbre_Type1;
    public int[] posZ_Arbre_Type1;
    public int[] posX_Arbre_Type2;
    public int[] posY_Arbre_Type2;
    public int[] posZ_Arbre_Type2;
    public int[] posX_Arbre_Type3;
    public int[] posY_Arbre_Type3;
    public int[] posZ_Arbre_Type3;
    public int[] posX_Arbre_Type4;
    public int[] posY_Arbre_Type4;
    public int[] posZ_Arbre_Type4;
    public int[] posX_Arbre_Type5;
    public int[] posY_Arbre_Type5;
    public int[] posZ_Arbre_Type5;

    public int[] scaleX_Arbre_Type1;
    public int[] scaleY_Arbre_Type1;
    public int[] scaleZ_Arbre_Type1;
    public int[] scaleX_Arbre_Type2;
    public int[] scaleY_Arbre_Type2;
    public int[] scaleZ_Arbre_Type2;
    public int[] scaleX_Arbre_Type3;
    public int[] scaleY_Arbre_Type3;
    public int[] scaleZ_Arbre_Type3;
    public int[] scaleX_Arbre_Type4;
    public int[] scaleY_Arbre_Type4;
    public int[] scaleZ_Arbre_Type4;
    public int[] scaleX_Arbre_Type5;
    public int[] scaleY_Arbre_Type5;
    public int[] scaleZ_Arbre_Type5;

    //Position/Scale des Pierres
    public int[] posX_Pierre_Type1;
    public int[] posY_Pierre_Type1;
    public int[] posZ_Pierre_Type1;

    public int[] scaleX_Pierre_Type1;
    public int[] scaleY_Pierre_Type1;
    public int[] scaleZ_Pierre_Type1;



    //Mettre les Scales de tous les types d'Unités (tableau de float pour X et Y)


    // Ouverture d'un fichier JSon
    public JSonParameters OuvertureJson(string nomFichier)
    {
        string path = Application.streamingAssetsPath + "/" + nomFichier + ".json";
        JSonParameters fileJSon;
        string jSonString = File.ReadAllText(path);
        fileJSon = JsonUtility.FromJson<JSonParameters>(jSonString);
        return fileJSon;
    }




}


