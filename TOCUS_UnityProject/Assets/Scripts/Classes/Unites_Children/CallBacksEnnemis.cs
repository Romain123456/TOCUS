using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Gère toutes les fonctions CallBack (Collision, Update...) des ennemis
public class CallBacksEnnemis : MonoBehaviour
{
    [HideInInspector] public Ennemis monEnnemi;

    /*private void Start()
    {
        monEnnemi.LoadSpriteSheet();
    }*/

    private void OnEnable()
    {
        //Remise à pvMax des PV
        monEnnemi.pv = monEnnemi.pvMax;
        monEnnemi.HealthBar_MaJ();
        StartCoroutine(monEnnemi.DeplacementEnnemi());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //monEnnemi.isFighting = true;
        if(collision.transform.tag == "Munition")       
        {
            if (collision.transform.parent.GetComponent<Tours>() != null)       //Pour les tours
            {
                //L'ennemi perd des pv
                monEnnemi.pv -= collision.transform.parent.GetComponent<Tours>().puissanceTour;
                monEnnemi.HealthBar_MaJ();
                //Si ses pv sont <= 0, l'ennemi est désactivé
                if (monEnnemi.pv <= 0)
                {
                    monEnnemi.joueurHiting = collision.transform.parent.GetComponent<Tours>().joueurOwner;
                }
            } else if(collision.transform.parent.name == "Mortier")
            {
                //L'ennemi perd des pv
                monEnnemi.pv -= collision.transform.parent.Find("MortierViseur").GetComponent<MortierBatiment>().puissanceMortier;
                monEnnemi.HealthBar_MaJ();
                if (monEnnemi.pv <= 0)
                {
                    monEnnemi.joueurHiting = monEnnemi.levelManager._JoueurActif - 1;
                }
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }


    private void Update()
    {
        monEnnemi.ActualisationStateInfo();
        //monEnnemi.ActivationHitboxArme();

        monEnnemi.CheckPlaceSuivanteLibre();


        if (monEnnemi.pv <= 0 && !monEnnemi.isDead)
        {
            FonctionMortEnnemi();
            /*StartCoroutine(monEnnemi.MortUnite(monEnnemi.monAnimator));
            monEnnemi.levelManager.tableauJoueurs[monEnnemi.joueurHiting]._VictoryPoints += monEnnemi._VictoryPointGain;
            monEnnemi.levelManager.tableauJoueurs[monEnnemi.joueurHiting].AffichePointVictoireSolidarite(monEnnemi.levelManager.tableauJoueurs[monEnnemi.joueurHiting].victoirePointText, monEnnemi.levelManager.tableauJoueurs[monEnnemi.joueurHiting]._VictoryPoints);
            monEnnemi.isDead = true;*/
        }


        if (monEnnemi.isFighting && monEnnemi.isCombatWait && !monEnnemi.isFightingPorteWait && !monEnnemi.isDead)        //Combat Unité
        {
            StartCoroutine(monEnnemi.FonctionCombat(monEnnemi.combatAdversaire));
            monEnnemi.isCombatWait = false;
        }

        if(monEnnemi.isFighting && monEnnemi.isFightingPorteWait && !monEnnemi.isCombatWait)         //Combat Porte
        {
            StartCoroutine(monEnnemi.AttaquePorte());
            monEnnemi.isFightingPorteWait = false;
        }


        
    }


    void FonctionMortEnnemi()
    {
        StartCoroutine(monEnnemi.MortUnite(monEnnemi.monAnimator));
        monEnnemi.levelManager.tableauJoueurs[monEnnemi.joueurHiting]._VictoryPoints += monEnnemi._VictoryPointGain;
        if (monEnnemi.levelManager.tableauJoueurs[monEnnemi.joueurHiting]._HasFosse)
        {
            monEnnemi.levelManager.tableauJoueurs[monEnnemi.joueurHiting]._VictoryPoints += monEnnemi.levelManager._NbVictoryPointsFosse;
            Debug.Log("Paramètre à régler et à placer dans une option");
        }

        monEnnemi.levelManager.tableauJoueurs[monEnnemi.joueurHiting].AffichePointVictoireSolidarite(monEnnemi.levelManager.tableauJoueurs[monEnnemi.joueurHiting].victoirePointText, monEnnemi.levelManager.tableauJoueurs[monEnnemi.joueurHiting]._VictoryPoints);
        monEnnemi.isDead = true;
    }


   /* private void LateUpdate()
    {
        monEnnemi.ChangeSpriteAnimation();
    }*/

    private void OnDisable()
    {
        monEnnemi.ChoixChemin();
        monEnnemi.CheminDefinitionEnnemi(monEnnemi.levelManager.positionsEnnemisChemin);
        monEnnemi.LiberationPositionChemin();
        monEnnemi.isFighting = false;
        monEnnemi.isFightingPorteWait = false;
        monEnnemi.isCombatWait = false;
        monEnnemi.isDead = false;
    }
}
