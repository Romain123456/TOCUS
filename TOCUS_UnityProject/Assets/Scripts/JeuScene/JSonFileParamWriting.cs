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

    //Chemin des ennemis
    public GameObject emptyEnnemi1;             //GO de l'ennemi 1 pour le calcul des chemins
    public GameObject emptyEnnemi2;             //GO de l'ennemi 2 pour le calcul des chemins
    public GameObject emptyEnnemi3;             //GO de l'ennemi 3 pour le calcul des chemins
    public GameObject chemin1Parent;             //GO du parent des positions clés du chemin1
    public GameObject chemin2Parent;             //GO du parent des positions clés du chemin2
    public GameObject chemin3Parent;             //GO du parent des positions clés du chemin3
    private GameObject[] keyPosEnnemi1;          //GO des positions clés du chemin1
    private GameObject[] keyPosEnnemi2;          //GO des positions clés du chemin2
    private GameObject[] keyPosEnnemi3;          //GO des positions clés du chemin3
    private List<Vector2> positionsEnnemi1 = new List<Vector2>();          //Positions clés du chemin1
    private List<Vector2> positionsEnnemi2 = new List<Vector2>();          //Positions clés du chemin2
    private List<Vector2> positionsEnnemi3 = new List<Vector2>();          //Positions clés du chemin3
    private bool chemin1Fini;                   //Le chemin 1 est fini de calculer
    private bool chemin2Fini;                   //Le chemin 2 est fini de calculer
    private bool chemin3Fini;                   //Le chemin 3 est fini de calculer

    //Nombre d'ennemis instanciés par round (ajouter quand on aura tous les types d'ennemis). La dimension du tableau est égale au nombre de rounds
    public int[] ennemi_type1;
    public int[] ennemi_type2;


    //Nombre de rounds
    public int nbMaxRounds; 

    // Start is called before the first frame update
    void Start()
    {
        fileJSon = new JSonParameters();        //Instanciation de la classe JSonParameters

        //Ouverture FileJSon
        fileJSon = fileJSon.OuvertureJson(nomJsonFileCurrent);
        path = Application.streamingAssetsPath + "/" + nomJsonFileCurrent + ".json";

        // Cette Partie s'écrit dans le fichier JsonFile Base
        //Position des ennemis sur leur chemin
        //Chemin 1
        if (chemin1Parent != null)
        {
            keyPosEnnemi1 = new GameObject[chemin1Parent.transform.childCount];         //Instanciation du tableau de nombre de positions de l'ennemi1
            for (int ii = 0; ii < keyPosEnnemi1.Length; ii++)                           //Attribution des GO de toutes les positions clés de l'ennemi
            {
                keyPosEnnemi1[ii] = chemin1Parent.transform.GetChild(ii).gameObject;
            }
            StartCoroutine(MouvementCheminEnnemi(emptyEnnemi1, chemin1Parent, positionsEnnemi1, 1, 10));            //Ecriture des positions de l'ennemis
        }
        //Chemin 2 (Idem Chemin 1)
        if (chemin2Parent != null)
        {
            keyPosEnnemi2 = new GameObject[chemin2Parent.transform.childCount];
            for (int ii = 0; ii < keyPosEnnemi2.Length; ii++)
            {
                keyPosEnnemi2[ii] = chemin2Parent.transform.GetChild(ii).gameObject;
            }
            StartCoroutine(MouvementCheminEnnemi(emptyEnnemi2, chemin2Parent, positionsEnnemi2, 2, 10));
        }
        //Chemin 3 (Idem Chemin 1)
        if (chemin3Parent != null)
        {
            keyPosEnnemi3 = new GameObject[chemin3Parent.transform.childCount];
            for (int ii = 0; ii < keyPosEnnemi3.Length; ii++)
            {
                keyPosEnnemi3[ii] = chemin3Parent.transform.GetChild(ii).gameObject;
            }
            StartCoroutine(MouvementCheminEnnemi(emptyEnnemi3, chemin3Parent, positionsEnnemi3, 3, 10));
        }

        


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

        // Fin de la partie qui s'écrit dans le fichier JsonFile Base

    }



    private bool canWrite;      //Est-ce que l'on peut écrire dans le fichier Json
    private bool isWritten;     //Est-ce que le fichier JSon est fini d'écrire
    void Update()
    {
        //Dès que les ennemis on parcouru leur chemin, on peut écrire dans le fichier JSon
        if (chemin1Fini && chemin2Fini && chemin3Fini)
        {
            canWrite = true;
        }

        //Quand on peut écrire, on lance une fois l'écriture
        if (canWrite && !isWritten)
        {
            isWritten = true;
            EcritureJson(fileJSon);
            
        }
    }





    IEnumerator MouvementCheminEnnemi(GameObject emptyEnnemi, GameObject cheminChoisi, List<Vector2> positionsEnnemi, int indexChemin, int nbDeplacementMax)
    {
        //Précision du déplacement (modifie la taille des variables de position et la précision du déplacement
        positionsEnnemi.Clear();

        //Initialisation de l'empty à la position 0 du chemin
        emptyEnnemi.transform.position = cheminChoisi.transform.GetChild(0).transform.position;

        for (int ii = 1; ii < cheminChoisi.transform.childCount; ii++)
        {
            Vector3 cheminIter = (cheminChoisi.transform.GetChild(ii).transform.position - emptyEnnemi.transform.position)/nbDeplacementMax;
            int nbDeplacement = 0;
            // Attention, nbDeplacementMax est le nombre de déplacement entre chaque position clés
            while (nbDeplacement < nbDeplacementMax)
            {
                emptyEnnemi.transform.position += cheminIter;
                positionsEnnemi.Add(new Vector2(emptyEnnemi.transform.position.x, emptyEnnemi.transform.position.y));
                nbDeplacement++;
                yield return new WaitForSeconds(Time.smoothDeltaTime);
            }
        }

        if (indexChemin == 1)
        {
            chemin1Fini = true;
        } else if (indexChemin == 2)
        {
            chemin2Fini = true;
        } else if (indexChemin == 3)
        {
            chemin3Fini = true;
        } 
        yield return null;
    }


    //Exemple de Méthode d'écriture dans un fichier JSON
    void EcritureJson(JSonParameters jsonfile)
    {
        //Précision des float
        jsonfile.precisionFloat = (int)precisionFloat;

        //Ecriture des paramètres
        //Position des chemins des ennemis
        //Chemin 1
        for (int ii = 0; ii < positionsEnnemi1.Count; ii++)
        {
            jsonfile.positionXEnnemis1[ii] = (int)(positionsEnnemi1[ii].x * precisionFloat);
            jsonfile.positionYEnnemis1[ii] = (int)(positionsEnnemi1[ii].y * precisionFloat);
        }
        //Chemin 2
        for (int ii = 0; ii < positionsEnnemi2.Count; ii++)
        {
            
            jsonfile.positionXEnnemis2[ii] = (int)(positionsEnnemi2[ii].x * precisionFloat);
            jsonfile.positionYEnnemis2[ii] = (int)(positionsEnnemi2[ii].y * precisionFloat);
        }
        //Chemin 3
        for (int ii = 0; ii < positionsEnnemi3.Count; ii++)
        {

            jsonfile.positionXEnnemis3[ii] = (int)(positionsEnnemi3[ii].x * precisionFloat);
            jsonfile.positionYEnnemis3[ii] = (int)(positionsEnnemi3[ii].y * precisionFloat);
        }

        //Nombre d'ennemis par round
        for(int ii = 0; ii < ennemi_type1.Length; ii++)
        {
            jsonfile.nbEnnemisRound_type1[ii] = ennemi_type1[ii];
        }
        for (int ii = 0; ii < ennemi_type2.Length; ii++)
        {
            jsonfile.nbEnnemisRound_type2[ii] = ennemi_type2[ii];
        }


        //Nombre de tours
        jsonfile.nbTours = nbTours;
        //Positions des tours
        for(int ii = 0; ii < nbTours; ii++)
        {
            jsonfile.modifAccessTours[ii] = modifAccessTours[ii];
            jsonfile.positionXTours[ii] = (int)(positionTours[ii].x * precisionFloat);
            jsonfile.positionYTours[ii] = (int)(positionTours[ii].y * precisionFloat);
            jsonfile.scaleXTours[ii] = (int)(scaleTours[ii].x * precisionFloat);
            jsonfile.scaleYTours[ii] = (int)(scaleTours[ii].y * precisionFloat);
        }


        //Nombre de BTP
        jsonfile.nbEmplacementBTP = nbEmplacementBTP;
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


        //Nb Max Round
        jsonfile.nbMaxRounds = nbMaxRounds;


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

    //Position pour les chemins des ennemis
    public int[] positionXEnnemis1;
    public int[] positionYEnnemis1;
    public int[] positionXEnnemis2;
    public int[] positionYEnnemis2;
    public int[] positionXEnnemis3;
    public int[] positionYEnnemis3;

    //Nombre d'ennemis instancié par round (faire plus tard un tableau à plusieurs dimensions pour chaque type d'ennemis)
    public int[] nbEnnemisRound_type1;
    public int[] nbEnnemisRound_type2;


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


    //Nombre de rounds
    public int nbMaxRounds;

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