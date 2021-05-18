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
        HealthBar_MaJ();
        placeChemin = 0;
        monTransform.position = levelManager.positionsChemin[way][placeChemin];
        levelManager.cheminOccupantTransform[way][placeChemin] = monTransform;
    }


    public IEnumerator DeplacementEnnemi()
    {
        while(placeChemin < levelManager.positionsChemin[way].Length && pv > 0)
        {
            //Si les pv de l'ennemi sont < 0, on casse tout !
            if(pv <= 0 || placeChemin >= levelManager.positionsChemin[way].Length)
            {
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
            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
            placeChemin++;
            if (placeChemin < levelManager.positionsChemin[way].Length)
            {
                LiberationPlaceChemin(way, placeChemin - 1);
            }
            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime * 25);
        }

        if(pv > 0)
        {
            StartCoroutine(AttaquePorte());
        }
    }


    public IEnumerator AttaquePorte()
    {
        sens = SensCalcul(monTransform.position.x, levelManager._PorteVille.transformObjet.position.x);
        if(sens == 1)
        {
            ChangeAnimation("AttackDroitBool");
        } else if(sens == -1)
        {
            ChangeAnimation("AttackGaucheBool");
        }
        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
        while (levelManager._PorteVille.pv > 0)
        {
            yield return new WaitForSeconds(0.8f);
            levelManager._PorteVille.pv -= uniteDegatsValue;
            levelManager._PorteVille.FillAmountHealthBarImage();

            if(levelManager._PorteVille.pv <= 0)
            {
                levelManager._PorteVille.transformObjet.gameObject.SetActive(false);
                levelManager.panelDefaite.SetActive(true);
                break;
            }
        }
    }
    #endregion



    #region Attaque 
    public IEnumerator CombatUnites()
    {
        UnitesParent adversaire = levelManager.cheminOccupantTransform[way][placeChemin + 1].GetComponent<UnitesParent>();
        UnitesParent ennemi = monTransform.GetComponent<UnitesParent>();

        adversaire.isFighting = true;
        ennemi.isFighting = true;

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
        ennemi.isFighting = false;
        adversaire.isFighting = false;
    }
   
    
    #endregion


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Munition"))
        {
            if(collision.transform.parent.GetComponent<Tours>() != null)
            {
                Debug.Log("Temporaire : prendre en compte les types d'impact des tours");
                pv -= collision.transform.parent.GetComponent<Tours>().puissanceTour;
                HealthBar_MaJ();
                if(pv <= 0)
                {
                    MortUnite();
                }
            }
        }
    }

}
