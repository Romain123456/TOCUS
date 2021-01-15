using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallBacksSuperUnitesJoueur : CallBacksUnitesJoueur
{
    [HideInInspector] public SuperUnitesJoueur maSuperUniteJoueur;


    private void OnEnable()
    {
        maSuperUniteJoueur.UnitesJoueurPositionnementActivation();
        maSuperUniteJoueur.pv = maSuperUniteJoueur.pvMax;
        maSuperUniteJoueur.HealthBar_MaJ();
        maSuperUniteJoueur.uniteJoueurOwner = maSuperUniteJoueur.levelManager._JoueurActif;
        maSuperUniteJoueur.isFighting = false;
    }

    private void OnDisable()
    {
        maSuperUniteJoueur.LiberationPositionChemin();
        maSuperUniteJoueur.UniteJoueurRetourStatsBases();
        maSuperUniteJoueur.isDead = false;
        ennemiTransform = null;
        maSuperUniteJoueur.ChoixChemin();
    }

    void Update()
    {
        if (maSuperUniteJoueur.pv <= 0 && !maSuperUniteJoueur.isDead && this.transform.tag == "SuperUniteJoueur")
        {
            StartCoroutine(maSuperUniteJoueur.MortUnite(maSuperUniteJoueur.monAnimator, maSuperUniteJoueur.monTransform.position.x, ennemiTransform.position.x));
            maSuperUniteJoueur.isDead = true;
            maSuperUniteJoueur.FonctionMortUniteJoueur();
        }
    }
}
