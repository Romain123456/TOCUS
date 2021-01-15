using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallBacksUnitesJoueur : MonoBehaviour
{
    [HideInInspector] public UnitesJoueur monUniteJoueur;
    [HideInInspector] public Transform ennemiTransform;

    private void OnEnable()
    {
        monUniteJoueur.UnitesJoueurPositionnementActivation();
        monUniteJoueur.pv = monUniteJoueur.pvMax;
        monUniteJoueur.HealthBar_MaJ();
        monUniteJoueur.uniteJoueurOwner = monUniteJoueur.levelManager._JoueurActif;
        monUniteJoueur.isFighting = false;
    }

    private void OnDisable()
    {
        
        monUniteJoueur.LiberationPositionChemin();
        monUniteJoueur.isDead = false;
        ennemiTransform = null;
        monUniteJoueur.UniteJoueurRetourStatsBases();

        if (monUniteJoueur.pv <= 0)
        {
            //On retire 1 au nombre d'unité du type au joueur quand l'unité meurt
            monUniteJoueur.levelManager.tableauJoueurs[monUniteJoueur.levelManager._JoueurActif - 1]._NbUnites[monUniteJoueur.uniteOrdreListe]--;
        }
        monUniteJoueur.ChoixChemin();
    }

    private void Update()
    {
        
        if(monUniteJoueur.pv <= 0 && !monUniteJoueur.isDead && this.transform.tag == "UniteJoueur")
        {
            StartCoroutine(monUniteJoueur.MortUnite(monUniteJoueur.monAnimator,monUniteJoueur.monTransform.position.x,ennemiTransform.position.x));
            monUniteJoueur.isDead = true;
            Debug.Log("mort");
            monUniteJoueur.FonctionMortUniteJoueur();
        }

        if(monUniteJoueur.pv > 0 && !monUniteJoueur.isDead && !monUniteJoueur.isFighting)
        {
            monUniteJoueur.VerificationCheminOccupePortee();
            if (monUniteJoueur.isCombatDistance && !monUniteJoueur.combatDistanceLaunch)
            {
                StartCoroutine(monUniteJoueur.CombatDistance());
            }
        }
        //monUniteJoueur.CheckPlaceSuivanteLibre();
    }

}
