using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TourObject", menuName = "TourObject")]
public class TourObject : ScriptableObject
{
    public string tourNom;                  //Nom de la tour
    public Sprite tourSprite;               //Sprite de la tour
    public int tourPrixBle;                 //Prix en blé de la tour
    public int tourPrixBois;                //Prix en bois de la tour
    public int tourPrixFer;                 //Prix en fer de la tour
    public int tourPrixPierre;              //Prix en pierre de la tour

    public Vector3 positionTour;            //Position de la tour par rapport à l'emplacement
    public Vector3 scaleTour;               //Scale de la tour

    public enum typeVisee { Ligne, CirculaireCentree, Cone };       //Liste des types de Visée
    public typeVisee tourTypeVisee;             //Type de visée de la tour

    //public Vector2 offsetColliderVisee;              //Offset du Collider de la visée de la tour;
    //public Vector2[] sizesColliderVisee;            //Tableau des positions déterminant la taille du collider de la visée de la tour. 
                                                    //Pour Ligne : Size = 1 
                                                    //Pour Cercle : Size = 1
    //public Sprite viseeSprite;          //Sprite de la visée de la tour (Valable pour visée circulaire et Cone)

    public enum typeCadence { Rapide, Lente };        //Liste des types de cadences de tir
    public typeCadence tourTypeCadence;             //Type de cadence de tir

    public enum typePuissance { Importante, Forte, Faible};        //Liste des types de puissance
    public typePuissance tourTypePuissance;         //Type de puissance


    public enum typePortee { Lointaine, Normale, Courte};           //Liste des types de portées
    public typePortee tourTypePortee;           //Type de Portée
}
