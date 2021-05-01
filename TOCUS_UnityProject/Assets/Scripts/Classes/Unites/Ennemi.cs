using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemi : UnitesParent
{
    //Vitesse de déplacement
    [HideInInspector] public string typeSpeedDeplacement;
    [HideInInspector] public float speedMove;                       //Vitesse de déplacement de l'ennemi


    // Start is called before the first frame update
    void Start()
    {
        EnnemiPositionnementOnChemin();
        StartCoroutine(DeplacementEnnemi());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnnemiInitialisation(EnnemiRepertoire _EnnemiRepertoire)
    {
        Destroy(monTransform.GetComponent<Animation>());
        monTransform.localScale = Vector3.one;
        uniteSprites = _EnnemiRepertoire.ennemiSprite;
        monSpriteRenderer.sprite = uniteSprites;
        maBoxCollider.size = _EnnemiRepertoire.uniteBoxCollSize;
        monTransform.GetChild(0).localScale = _EnnemiRepertoire.uniteScaleCanvas;
        monTransform.GetChild(0).localPosition = _EnnemiRepertoire.unitePositionCanvas;
        monTransform.name = _EnnemiRepertoire.uniteNom;
        uniteVitalite = _EnnemiRepertoire.uniteTypeVitalite;
        uniteInitiative = _EnnemiRepertoire.uniteTypeInitiative;
        uniteDegats = _EnnemiRepertoire.uniteTypeDegats;
        uniteArmure = _EnnemiRepertoire.uniteTypeArmure;
        unitePortee = _EnnemiRepertoire.uniteTypePortee;
        typeSpeedDeplacement = _EnnemiRepertoire.typeSpeedMoveEnnemi;
        monAnimatorController = _EnnemiRepertoire.ennemiAnimatorController;
        monAnimator.runtimeAnimatorController = monAnimatorController;
    }


    public void AttributionCaracteristiquesEnnemi()
    {
        if (typeSpeedDeplacement == "Tres_Lente")
        {
            speedMove = 10 / JsonParametresGlobaux.ficParamGlobaux.objet_monstres_valeurs_textuelles_deplacement.o_monstres_valeurs_textuelles_deplacement.tres_lente;
        }
        else if (typeSpeedDeplacement == "Lente")
        {
            speedMove = 10 / JsonParametresGlobaux.ficParamGlobaux.objet_monstres_valeurs_textuelles_deplacement.o_monstres_valeurs_textuelles_deplacement.lente;
        }
        else if (typeSpeedDeplacement == "Normale")
        {
            speedMove = 10 / JsonParametresGlobaux.ficParamGlobaux.objet_monstres_valeurs_textuelles_deplacement.o_monstres_valeurs_textuelles_deplacement.normale;
        }
        else if (typeSpeedDeplacement == "Rapide")
        {
            speedMove = 10 / JsonParametresGlobaux.ficParamGlobaux.objet_monstres_valeurs_textuelles_deplacement.o_monstres_valeurs_textuelles_deplacement.rapide;
        }
        else if (typeSpeedDeplacement == "Tres_Rapide")
        {
            speedMove = 10 / JsonParametresGlobaux.ficParamGlobaux.objet_monstres_valeurs_textuelles_deplacement.o_monstres_valeurs_textuelles_deplacement.tres_rapide;
        }
    }




    #region Deplacement Ennemi
    public void EnnemiPositionnementOnChemin()
    {
        way = 0;
        placeChemin = 0;
        monTransform.position = levelManager.positionsChemin[way][placeChemin];
        levelManager.cheminOccupantTransform[way][placeChemin] = monTransform;
    }


    public IEnumerator DeplacementEnnemi()
    {
        while(placeChemin < levelManager.positionsChemin[way].Length)
        {
            monTransform.position = levelManager.positionsChemin[way][placeChemin];
            levelManager.cheminOccupantTransform[way][placeChemin] = monTransform;
            if (placeChemin + 1 < levelManager.positionsChemin[way].Length)
            {
                if(levelManager.positionsChemin[way][placeChemin+1].x > levelManager.positionsChemin[way][placeChemin].x)
                {
                    monAnimator.SetBool("MarcheGaucheBool", false);
                    monAnimator.SetBool("MarcheDroitBool", true);
                } else if (levelManager.positionsChemin[way][placeChemin + 1].x < levelManager.positionsChemin[way][placeChemin].x)
                {
                    monAnimator.SetBool("MarcheGaucheBool", true);
                    monAnimator.SetBool("MarcheDroitBool", false);
                }


                while (levelManager.cheminOccupantTransform[way][placeChemin + 1] != null)
                {
                    monAnimator.SetBool("MarcheGaucheBool", false);
                    monAnimator.SetBool("MarcheDroitBool", false);
                    yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                }
            }
            placeChemin++;
            LiberationPlaceChemin(way, placeChemin - 1);
            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime * speedMove);
        }
    }
    #endregion



}
