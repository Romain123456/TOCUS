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
                PerteEnnemiPV(collision.transform.parent.GetComponent<Tours>());
                if(collision.transform.parent.GetComponent<Tours>()._DureeDegats != "Ponctuelle")
                {
                    StartCoroutine(DegatsDuree(collision.gameObject));
                }
            } else if(collision.transform.parent.name == "Mortier")
            {
                PerteEnnemiPV(collision.transform.parent.Find("MortierViseur").GetComponent<MortierBatiment>());
            }

        }
    }

    //Coroutine de dégats sur la durée
    public IEnumerator DegatsDuree(GameObject _monCollider)
    {
        float tempsEntreHitDegats = JsonParametresGlobaux.ficParamGlobaux.objet_divers.o_divers.f_temps_entre_hit_degats_tours;
        float tempsDegats = _monCollider.transform.parent.GetComponent<Tours>().tempsDureeDegats;

        yield return new WaitForSeconds(tempsEntreHitDegats);
        while (tempsDegats > 0)
        {
            PerteEnnemiPV(_monCollider.transform.parent.GetComponent<Tours>());
            tempsDegats -= tempsEntreHitDegats;
            yield return new WaitForSeconds(tempsEntreHitDegats);
            if (tempsDegats <= 0)
            {
                break;
            }
        }
    }


    public void PerteEnnemiPV(Tours maTour)
    {
        //L'ennemi perd des pv
        if (maTour.puissanceTour > monEnnemi.uniteArmureValue)
        {
            monEnnemi.pv = monEnnemi.pv - (maTour.puissanceTour - monEnnemi.uniteArmureValue);
        }
        monEnnemi.HealthBar_MaJ();
        //Si ses pv sont <= 0, l'ennemi est désactivé
        if (monEnnemi.pv <= 0)
        {
            monEnnemi.joueurHiting = maTour.joueurOwner;
        }
    }

    public void PerteEnnemiPV(MortierBatiment monMortier)
    {
        //L'ennemi perd des pv
        if (monMortier.puissanceMortier > monEnnemi.uniteArmureValue)
        {
            monEnnemi.pv = monEnnemi.pv - (monMortier.puissanceMortier - monEnnemi.uniteArmureValue);
        }
        monEnnemi.HealthBar_MaJ();
        if (monEnnemi.pv <= 0)
        {
            monEnnemi.joueurHiting = monEnnemi.levelManager._JoueurActif - 1;
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
