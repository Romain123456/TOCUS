using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe UnitesJoueur hérite de Unites
public class UnitesJoueur : Unites
{
    [HideInInspector] public int uniteJoueurOwner;            //Propriétaire de l'unité
    [HideInInspector] public int uniteOrdreListe;           //Ordre dans la liste

    [HideInInspector] public Unite_Objects.typeVitalite uniteVitaliteBase;                //Vitalité de base de l'unité
    [HideInInspector] public Unite_Objects.typeInitiative uniteInitiativeBase;              //Initiative de base de l'unité
    [HideInInspector] public Unite_Objects.typeDegats uniteDegatsBase;                      //Degats de base de l'unité

    [HideInInspector] public int victoryPointPerDeath;          //Nombre de points de victoire si l'unité meurt et que le joueur possède le Monument aux morts

    //Fonciton Constructeur de la classe unité. Affecteur les valeurs spécifiques aux variables, tag, script de CallBacks, puis désactive l'unité à son instanciation
    public UnitesJoueur(Sprite _maSpriteBase, Vector2 _SizeBoxCollider, Vector2 _ScaleCanvasHealthBar, Vector2 _PositionCanvasHealthBar, string _MonTag)
    {
        ConstructeurUnitesJoueur(_maSpriteBase, _SizeBoxCollider,_ScaleCanvasHealthBar,_PositionCanvasHealthBar, _MonTag);
    }

    public void ConstructeurUnitesJoueur(Sprite _maSpriteBase, Vector2 _SizeBoxCollider, Vector2 _ScaleCanvasHealthBar, Vector2 _PositionCanvasHealthBar,string _MonTag)
    {
        ConstructeurUnites(_maSpriteBase, _SizeBoxCollider, _ScaleCanvasHealthBar, _PositionCanvasHealthBar);
        monTransform.tag = _MonTag;

        if (_MonTag == "UniteJoueur")
        {
            monGO.AddComponent<CallBacksUnitesJoueur>();
            monGO.GetComponent<CallBacksUnitesJoueur>().monUniteJoueur = this;
        }
        monGO.SetActive(false);

        victoryPointPerDeath = JsonParametresGlobaux.ficParamGlobaux.objet_divers.o_divers.i_gain_victoire_monument_aux_morts;
    }


    //ATTENTION, LES UNITES JOUEURS DOIVENT PARCOURIR LE CHEMIN EN INDICES SENS INVERSES !!!
    public void CheminDefinitionUnitesJoueur(Vector2[][] _Positions)
    {
        positionsTableau = new Vector2[_Positions[cheminChoisi].Length];
        for(int ii = 0;ii <= positionsTableau.Length - 1; ii++)
        {
            positionsTableau[ii] = _Positions[cheminChoisi][ii];
        }
    }

    public void UnitesJoueurPositionnementActivation()
    {
        //Check si la place est libre ! positionOnChemin doit être choisit au hasard !!
        positionOnChemin = Random.Range(1, positionsTableau.Length);
        while (levelManager.cheminOccupantTransform[cheminChoisi][positionOnChemin] != null)
        {
            positionOnChemin--;
            if (levelManager.cheminOccupantTransform[cheminChoisi][positionOnChemin] == null)
            {
                break;
            }
        }
        monTransform.position = positionsTableau[positionOnChemin];
    }

    public void UnitesJoueurPrendPlace()
    {
        levelManager.cheminOccupantTransform[cheminChoisi][positionOnChemin] = monTransform;
    }



    public void UniteJoueurRetourStatsBases()
    {
        uniteVitalite = uniteVitaliteBase;
        uniteInitiative = uniteInitiativeBase;
        uniteDegats = uniteDegatsBase;
        //uniteDegats
        AttributionCaracteristiques();
    }


    public void UniteJoueurAmeliorationStatistiques(CallBacksUnitesJoueur _MonUniteJoueur)
    {
        if(_MonUniteJoueur.monUniteJoueur.uniteVitalite > 0)
        {
            _MonUniteJoueur.monUniteJoueur.uniteVitalite--;
        }
        if (_MonUniteJoueur.monUniteJoueur.uniteInitiative > 0)
        {
            _MonUniteJoueur.monUniteJoueur.uniteInitiative--;
        }
        if (_MonUniteJoueur.monUniteJoueur.uniteDegats > 0)
        {
            _MonUniteJoueur.monUniteJoueur.uniteDegats--;
        }
        _MonUniteJoueur.monUniteJoueur.AttributionCaracteristiques();
        _MonUniteJoueur.monUniteJoueur.HealthBar_MaJ();
    }


    public void FonctionMortUniteJoueur()
    {
        if (levelManager.tableauJoueurs[uniteJoueurOwner]._HasMonumentAuxMorts)
        {
            levelManager.tableauJoueurs[uniteJoueurOwner]._VictoryPoints += victoryPointPerDeath;
            Debug.Log("Paramètre à régler et à placer dans les options");
            levelManager.tableauJoueurs[uniteJoueurOwner].AffichePointVictoireSolidarite(levelManager.tableauJoueurs[uniteJoueurOwner].victoirePointText, levelManager.tableauJoueurs[uniteJoueurOwner]._VictoryPoints);
        }
    }

}
