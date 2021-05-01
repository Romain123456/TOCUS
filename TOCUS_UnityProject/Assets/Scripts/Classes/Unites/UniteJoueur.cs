using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniteJoueur : UnitesParent
{
    [HideInInspector] public UnitesJoueurRepertoire infosUnite;
    public int joueurOwner;
    public bool isRecruted;

    // Start is called before the first frame update
    void Start()
    {
        /*PositionnementOnChemin();
        ActivationUnite();*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {

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

}
