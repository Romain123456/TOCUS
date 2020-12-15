using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TourObject", menuName = "TourObject")]
public class TourObject : ScriptableObject
{
    public string tourNom;                  //Nom de la tour
    public Sprite tourSprite;               //Sprite de la tour

    public Vector3 positionTour;            //Position de la tour par rapport à l'emplacement
    public Vector3 scaleTour;               //Scale de la tour

    public enum typeVisee { Ligne, Circulaire, Cone,DoubleCone };       //Liste des types de Visée
    public typeVisee tourTypeVisee;             //Type de visée de la tour

    //public Vector2 offsetColliderVisee;              //Offset du Collider de la visée de la tour;
    //public Vector2[] sizesColliderVisee;            //Tableau des positions déterminant la taille du collider de la visée de la tour. 
                                                    //Pour Ligne : Size = 1 
                                                    //Pour Cercle : Size = 1
    //public Sprite viseeSprite;          //Sprite de la visée de la tour (Valable pour visée circulaire et Cone)

    public enum typeCadence {Permanente,Tres_Rapide, Rapide,Normale, Lente,Tres_Lente };        //Liste des types de cadences de tir
    public typeCadence tourTypeCadence;             //Type de cadence de tir

    public enum typePuissance {Exponentielle,Enorme,Tres_Forte, Forte,Normale, Faible,Tres_Faible,Nulle};        //Liste des types de puissance
    public typePuissance tourTypePuissance;         //Type de puissance


    public enum typePortee {Absolue,Tres_Lointaine, Lointaine, Normale, Courte,Tres_Courte,Contact};           //Liste des types de portées
    public typePortee tourTypePortee;           //Type de Portée

    public enum typeDureeDegats { Ponctuelle,Courte,Longue,Tres_Longue,Permanente};     //Liste des types de durées de dégâts
    public typeDureeDegats tourTypeDureeDegats;     //Type de durée de dégats

    public enum typeLargeurVisee { Grande,Normale,Petite};      //Liste des types de largeur de visée
    public typeLargeurVisee tourTypeLargeurVisee;           //Type de largeur de visée

    public enum typeImpact { Individuel, Double, Triple, Rebondissant, Traversant, Zone};     //Liste des types d'impact
    public typeImpact tourTypeImpact;       //Type d'impact
}