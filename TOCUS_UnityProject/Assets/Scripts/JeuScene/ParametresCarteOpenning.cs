using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ParametresCarteOpenning : MonoBehaviour
{
    public string level;
    [HideInInspector] public string fichierJsonParam;
    [HideInInspector] public string pathJsonParam;

    static public JsonOptionsParam ficOptionParam;

    // Start is called before the first frame update
    void Awake()
    {
        fichierJsonParam = "parametres_carte_" + level + ".json";
        pathJsonParam = Application.streamingAssetsPath + "/" + fichierJsonParam;

        ficOptionParam = new JsonOptionsParam();
        ficOptionParam = ficOptionParam.OuvertureJsonFile(pathJsonParam);
        //Debug.Log(ficOptionParam.objet_vagues.o_vagues.o_structure_vagues[1].s_chemin_1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public class JsonOptionsParam
{
    public Objet_Reglagles objet_reglages;
    public Objet_Vagues objet_vagues;

    public JsonOptionsParam OuvertureJsonFile (string _Path)
    {
        JsonOptionsParam fileJson;
        string jSonString = File.ReadAllText(_Path);
        fileJson = JsonUtility.FromJson<JsonOptionsParam>(jSonString);
        fileJson.objet_reglages = JsonUtility.FromJson<Objet_Reglagles>(jSonString);
        fileJson.objet_vagues = JsonUtility.FromJson<Objet_Vagues>(jSonString);
        return fileJson;
    }
}



#region o_reglages
[Serializable]
public class Objet_Reglagles
{
    public O_Reglagles o_reglages;
}
[Serializable]
public class O_Reglagles
{
    public string[] arr_s_batiments_disponibles;
    public string[] arr_s_tours_disponibles;
    public string[] arr_s_unites_disponibles;
    public string[] arr_s_superunites_disponibles;
    public string[] arr_s_unites_speciales_disponibles;
    public string[] arr_s_ennemis_disponibles;
}
#endregion



#region o_vagues
[Serializable]
public class Objet_Vagues
{
    public O_Vagues o_vagues;
}

[Serializable]
public class O_Vagues
{
    public int i_nb_rounds_avant_premiere_vague;
    public O_StructureVagues[] o_structure_vagues;
}


[Serializable]
public class O_StructureVagues
{
    public int i_tps;
    public string s_chemin_1;
    public string s_chemin_2;
    public string s_chemin_3;
}
#endregion