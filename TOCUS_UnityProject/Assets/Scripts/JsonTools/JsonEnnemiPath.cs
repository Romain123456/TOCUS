using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using PathCreation;

public class JsonEnnemiPath : MonoBehaviour
{
    private JSonFileParamWriting scriptMainLevel;
    private string jsonFileName;

    public Transform[] ennemisTransform;
    public PathCreator[] ennemisPaths;
    private float[] distanceTravelled;

    private List<Vector2> chemin1 = new List<Vector2>();
    private List<Vector2> chemin2 = new List<Vector2>();
    private List<Vector2> chemin3 = new List<Vector2>();

    private bool isWritten;

    public int precisionPath;

    JsonEnnemiPositions ficJson;

    // Start is called before the first frame update
    void Start()
    {
        scriptMainLevel = this.GetComponent<JSonFileParamWriting>();
        jsonFileName = scriptMainLevel.nomJsonFileCurrent + "_EnnemisPaths.json";
        distanceTravelled = new float[ennemisTransform.Length];

        ficJson = new JsonEnnemiPositions();
    }



    private void FixedUpdate()
    {
        for (int ii = 0; ii < ennemisPaths.Length; ii++)
        {
            if (distanceTravelled[ii] < ennemisPaths[ii].path.length)
            {
                ennemisTransform[ii].position = ennemisPaths[ii].path.GetPointAtDistance(distanceTravelled[ii]);
                distanceTravelled[ii] += Time.fixedDeltaTime*precisionPath;
                if (ii == 0)
                {
                    chemin1.Add(ennemisTransform[ii].position);
                } else if (ii == 1)
                {
                    chemin2.Add(ennemisTransform[ii].position);
                } else if (ii == 2)
                {
                    chemin3.Add(ennemisTransform[ii].position);
                }
            } 
        }

        if(distanceTravelled[0] >= ennemisPaths[0].path.length &&
            distanceTravelled[1] >= ennemisPaths[1].path.length &&
            distanceTravelled[2] >= ennemisPaths[2].path.length &&
            !isWritten)
        {
            isWritten = true;
            EcritureInJson(ficJson);
            Debug.Log("Ecriture Chemins Terminée !!");
        }
        
    }

    void EcritureInJson(JsonEnnemiPositions _ficJson)
    {
        string jSonString;
        string path = Application.streamingAssetsPath + "/" + jsonFileName;

        _ficJson.ennemi1PosX = new int[chemin1.Count];
        _ficJson.ennemi1PosY = new int[chemin1.Count];
        for (int ii = 0; ii < chemin1.Count; ii++)
        {
            _ficJson.ennemi1PosX[ii] = (int)(chemin1[ii].x * 100);
            _ficJson.ennemi1PosY[ii] = (int)(chemin1[ii].y * 100);
        }

        _ficJson.ennemi2PosX = new int[chemin2.Count];
        _ficJson.ennemi2PosY = new int[chemin2.Count];
        for (int ii = 0; ii < chemin2.Count; ii++)
        {
            _ficJson.ennemi2PosX[ii] = (int)(chemin2[ii].x * 100);
            _ficJson.ennemi2PosY[ii] = (int)(chemin2[ii].y * 100);
        }

        _ficJson.ennemi3PosX = new int[chemin3.Count];
        _ficJson.ennemi3PosY = new int[chemin3.Count];
        for (int ii = 0; ii < chemin3.Count; ii++)
        {
            _ficJson.ennemi3PosX[ii] = (int)(chemin3[ii].x * 100);
            _ficJson.ennemi3PosY[ii] = (int)(chemin3[ii].y * 100);
        }

        jSonString = JsonUtility.ToJson(_ficJson, true);     //Lever le true lorsque l'on écrira les fichiers finaux (optimisation taille)
        File.WriteAllText(path, jSonString);


    }


}

public class JsonEnnemiPositions
{
    public int[] ennemi1PosX;
    public int[] ennemi1PosY;
    public int[] ennemi2PosX;
    public int[] ennemi2PosY;
    public int[] ennemi3PosX;
    public int[] ennemi3PosY;

    // Ouverture d'un fichier JSon
    public JsonEnnemiPositions OuvertureJson(string nomFichier)
    {
        string path = Application.streamingAssetsPath + "/" + nomFichier + ".json";
        JsonEnnemiPositions fileJSon;
        string jSonString = File.ReadAllText(path);
        fileJSon = JsonUtility.FromJson<JsonEnnemiPositions>(jSonString);
        return fileJSon;
    }
}
