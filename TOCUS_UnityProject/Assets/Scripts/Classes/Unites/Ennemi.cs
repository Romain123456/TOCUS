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
        /*EnnemiPositionnementOnChemin();
        StartCoroutine(DeplacementEnnemi());*/
    }

    // Update is called once per frame
    void Update()
    {
        stateInfo = monAnimator.GetCurrentAnimatorStateInfo(0);
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
        pv = pvMax;
        placeChemin = 0;
        monTransform.position = levelManager.positionsChemin[way][placeChemin];
        levelManager.cheminOccupantTransform[way][placeChemin] = monTransform;
    }


    public IEnumerator DeplacementEnnemi()
    {
        while(placeChemin < levelManager.positionsChemin[way].Length)
        {
            //Si les pv de l'ennemi sont < 0, on casse tout !
            if(pv < 0)
            {
                MortUnite();
                break;
            }

            monTransform.position = levelManager.positionsChemin[way][placeChemin];
            levelManager.cheminOccupantTransform[way][placeChemin] = monTransform;
            if (placeChemin + 1 < levelManager.positionsChemin[way].Length)
            {
                sens = SensCalcul(levelManager.positionsChemin[way][placeChemin].x, levelManager.positionsChemin[way][placeChemin + 1].x);
                if(sens == 1)
                {
                    ChangeAnimation("MarcheDroitBool");
                }
                else if (sens == -1)
                {
                    ChangeAnimation("MarcheGaucheBool");
                }

                while (levelManager.cheminOccupantTransform[way][placeChemin + 1] != null)
                {
                    //Test Attaque
                    if ((levelManager.cheminOccupantTransform[way][placeChemin + 1].CompareTag("UniteJoueur") ||
                        levelManager.cheminOccupantTransform[way][placeChemin + 1].CompareTag("SuperUniteJoueur")) && pv >= 0)
                    {
                        isFighting = true;
                        StartCoroutine(CombatUnites());
                        while (isFighting)
                        {
                            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                            if (!isFighting)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        ChangeAnimation("Idle");
                    }
                    yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                }
            }
            placeChemin++;
            if (placeChemin < levelManager.positionsChemin[way].Length)
            {
                LiberationPlaceChemin(way, placeChemin - 1);
            }
            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime * 25);
        }
    }
    #endregion



    #region Attaque 
    private bool _Attacking;
    public IEnumerator CombatUnites()
    {
        UnitesParent adversaire = levelManager.cheminOccupantTransform[way][placeChemin + 1].GetComponent<UnitesParent>();
        UnitesParent ennemi = monTransform.GetComponent<UnitesParent>();

        if(adversaire.uniteVitesseInitiative >= uniteVitesseInitiative)
        {

            while (adversaire.pv > 0 && ennemi.pv > 0)
            {
                //Unité joueur attaque en premier
                StartCoroutine(AttaqueFonction(adversaire, ennemi));
                while (_Attacking)
                {
                    yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                    if (!_Attacking)
                    {
                        break;
                    }
                }

                if (ennemi.pv > 0)
                {
                    yield return new WaitForSeconds(0.5f);
                    StartCoroutine(AttaqueFonction(ennemi, adversaire));
                    while (_Attacking)
                    {
                        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                        if (!_Attacking)
                        {
                            break;
                        }
                    }
                    if (adversaire.pv <= 0)
                    {
                        adversaire.MortUnite();
                        Debug.Log("Adversaire Mort !");
                        break;
                    }
                }
                else if (ennemi.pv <= 0)
                {
                    ennemi.MortUnite();
                    Debug.Log("Ennemi Mort !");
                    break;
                }
            }
        }
        else if(adversaire.uniteVitesseInitiative < uniteVitesseInitiative)
        {

            while (ennemi.pv > 0 && adversaire.pv > 0)
            {
                //Ennemi attaque en premier
                StartCoroutine(AttaqueFonction(ennemi, adversaire));
                while (_Attacking)
                {
                    yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                    if (!_Attacking)
                    {
                        break;
                    }
                }

                if (adversaire.pv > 0)
                {
                    yield return new WaitForSeconds(0.5f);
                    StartCoroutine(AttaqueFonction(adversaire, ennemi));
                    while (_Attacking)
                    {
                        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                        if (!_Attacking)
                        {
                            break;
                        }
                    }
                    if (ennemi.pv <= 0)
                    {
                        ennemi.MortUnite();
                        Debug.Log("Ennemi Mort !");
                        break;
                    }
                }
                else if (adversaire.pv <= 0)
                {
                    adversaire.MortUnite();
                    Debug.Log("Adversaire Mort !");
                    break;
                }
            }
        }
        isFighting = false;
    }
    #endregion


    public IEnumerator AttaqueFonction(UnitesParent _Unite1,UnitesParent _Unite2)
    {
        _Attacking = true;
        _Unite1.sens = _Unite1.SensCalcul(_Unite1.monTransform.position.x,_Unite2.monTransform.position.x);
        if (_Unite1.sens == 1)
        {
            _Unite1.ChangeAnimation("AttackDroitBool");
        } else if(_Unite1.sens == -1)
        {
            _Unite1.ChangeAnimation("AttackGaucheBool");
        }

        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
        if (_Unite1.stateInfo.IsTag("attack"))
        {
            while(_Unite1.stateInfo.normalizedTime <= 1.01f)
            {
                yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                if(_Unite1.stateInfo.normalizedTime > 1.01f)
                {
                    _Unite1.ChangeAnimation("Idle");
                }
            }
        }
        _Unite2.pv -= _Unite1.uniteDegatsValue;
        _Unite2.HealthBar_MaJ();

        _Attacking = false;
    }

}
