using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniteJoueur : UnitesParent
{
    [HideInInspector] public UnitesJoueurRepertoire infosUnite;
    public int joueurOwner;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        stateInfo = monAnimator.GetCurrentAnimatorStateInfo(0);

        AttaqueDistance();
    }

    private void OnEnable()
    {
        isFighting = false;
    }

    private void OnDisable()
    {
        if (way != -999 && placeChemin != -999)
        {
            LiberationPlaceChemin(way, placeChemin);
        }
    }
    public void UniteJoueurInitialisation(UnitesJoueurRepertoire monUniteRepertoire)
    {
        infosUnite = monUniteRepertoire;
        maBoxCollider.size = infosUnite.uniteBoxCollSize;
        monTransform.GetChild(0).localScale = infosUnite.uniteScaleCanvas;
        monTransform.GetChild(0).localPosition = infosUnite.unitePositionCanvas;
        monTransform.name = infosUnite.uniteNom;
        uniteVitalite = infosUnite.uniteTypeVitalite;
        uniteInitiative = infosUnite.uniteTypeInitiative;
        uniteDegats = infosUnite.uniteTypeDegats;
        uniteArmure = infosUnite.uniteTypeArmure;
        unitePortee = infosUnite.uniteTypePortee;
    }


    public void ActivationUnite()
    {
        pv = pvMax;
        HealthBar_MaJ();
        joueurOwner = levelManager._JoueurActif - 1;
        uniteSprites = infosUnite.uniteSpriteBase[joueurOwner];
        monSpriteRenderer.sprite = uniteSprites;
        monAnimatorController = infosUnite.uniteAnimatorController[joueurOwner];
        monAnimator.runtimeAnimatorController = monAnimatorController;
    }


    public void PositionnementOnChemin()
    {
        bool isCheminOk = false;
        while (!isCheminOk)
        {
            way = Random.Range(0, levelManager.positionsChemin.Length);
            placeChemin = Random.Range(1, levelManager.positionsChemin[way].Length);
            monTransform.position = levelManager.positionsChemin[way][placeChemin];
            if (levelManager.cheminOccupantTransform[way][placeChemin] == null)
            {
                isCheminOk = true;
            }
        }
        levelManager.cheminOccupantTransform[way][placeChemin] = monTransform;
    }


    public void AttaqueDistance()
    {
        if(unitePortee.ToString() != "Courte" && !isFighting)
        {
            for (int ii = 0; ii < porteeValue; ii++)
            {
                if (placeChemin - (ii + 2) >= 0)
                {
                    if (levelManager.cheminOccupantTransform[way][placeChemin - (ii + 2)] != null && !_Attacking)
                    {
                        StartCoroutine(AttaqueFonction(monTransform.GetComponent<UnitesParent>(), levelManager.cheminOccupantTransform[way][placeChemin - (ii + 2)].GetComponent<UnitesParent>()));
                        
                       /* Debug.Log(placeChemin - (ii + 1) + "  " + way);
                        Debug.Log(levelManager.cheminOccupantTransform[way][placeChemin - (ii + 1)]);*/
                    }
                }
            }
        }
    }
}
