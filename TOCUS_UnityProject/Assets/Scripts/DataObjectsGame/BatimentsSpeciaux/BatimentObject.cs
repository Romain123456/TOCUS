using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BatimentObject", menuName = "BatimentObject")]

public class BatimentObject : ScriptableObject
{
    public string batimentNom;          //Nom du batiment
    public Sprite batimentSprite;       //Sprite du batiment
    public int batimentPrixBle;         //Prix en blé du batiment
    public int batimentPrixBois;        //Prix en bois du batiment
    public int batimentPrixFer;         //Prix en fer du batiment
    public int batimentPrixPierre;      //Prix en pierre du batiment

    public Vector3 scaleBatiment;       //Scale du batiment

    public int nombreBatimentMaxParJoueur;          //Combien de bâtiments maximum le joueur peut construire ?

    public bool isBatimentPublic;                   //Est-ce que le bâtiment est public ou privé ?
}
