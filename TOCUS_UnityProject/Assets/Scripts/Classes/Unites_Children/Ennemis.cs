using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemis : Unites
{
    //Combat
    public Transform combatAdversaire;              //Adversaire de combat
    [HideInInspector] public bool isCombatWait;                    //Est-ce que le combat est lancé ?
    [HideInInspector] public bool isFightingPorteWait;                //Est-ce que l'ennemi combat la porte ?
    public CallBacksEnnemis _CallBackScripts;

    //Récompenses
    [HideInInspector] public int _VictoryPointGain;                //Combien de point de victoire l'ennemi donne-t-il ?
    [HideInInspector] public int joueurHiting;                      //Quel joueur tape l'ennemi à ce moment-là ?


    //Vitesse de déplacement
    [HideInInspector] public string typeSpeedDeplacement;
    [HideInInspector] public float speedMove;                       //Vitesse de déplacement de l'ennemi

    //Fonction constructeur de l'ennemi. Appelle la fonction définie dans Unité et attribue les variables spécifiques à la classe Ennemi
    public Ennemis(Sprite _maSpriteBase, Vector2 _SizeBoxCollider, Vector2 _ScaleCanvasHealthBar, Vector2 _PositionCanvasHealthBar)
    {
        ConstructeurUnites(_maSpriteBase,_SizeBoxCollider, _ScaleCanvasHealthBar, _PositionCanvasHealthBar);
        monTransform.tag = "Ennemi";
        monGO.layer = 10;
        //weaponHitbox.layer = 10;
        monGO.AddComponent<CallBacksEnnemis>();
        monGO.GetComponent<CallBacksEnnemis>().monEnnemi = this;
        _CallBackScripts = monGO.GetComponent<CallBacksEnnemis>();
        Destroy(monGO.GetComponent<Animation>());
        monGO.SetActive(false);
    }

    public void AttributionCaracteristiquesEnnemi()
    {
        if(typeSpeedDeplacement == "Tres_Lente")
        {
            speedMove = 10/JsonParametresGlobaux.ficParamGlobaux.objet_monstres_valeurs_textuelles_deplacement.o_monstres_valeurs_textuelles_deplacement.tres_lente;
        } else if (typeSpeedDeplacement == "Lente")
        {
            speedMove = 10 / JsonParametresGlobaux.ficParamGlobaux.objet_monstres_valeurs_textuelles_deplacement.o_monstres_valeurs_textuelles_deplacement.lente;
        } else if(typeSpeedDeplacement == "Normale")
        {
            speedMove = 10 / JsonParametresGlobaux.ficParamGlobaux.objet_monstres_valeurs_textuelles_deplacement.o_monstres_valeurs_textuelles_deplacement.normale;
        } else if(typeSpeedDeplacement == "Rapide")
        {
            speedMove = 10 / JsonParametresGlobaux.ficParamGlobaux.objet_monstres_valeurs_textuelles_deplacement.o_monstres_valeurs_textuelles_deplacement.rapide;
        } else if(typeSpeedDeplacement == "Tres_Rapide")
        {
            speedMove = 10 / JsonParametresGlobaux.ficParamGlobaux.objet_monstres_valeurs_textuelles_deplacement.o_monstres_valeurs_textuelles_deplacement.tres_rapide;
        }
    }


    //Attribue les valeurs de positions de l'ennemi qu'il emprunte lors de ses déplacements et initialise la position de l'ennemi à la position 0
    public void CheminDefinitionEnnemi(Vector2[][] _Positions)
    {
        positionsTableau = new Vector2[_Positions[cheminChoisi].Length];
        for (int ii = 0; ii < positionsTableau.Length; ii++)
        {
            positionsTableau[ii] = _Positions[cheminChoisi][ii];
        }
        monTransform.position = positionsTableau[0];
    }

   
    //Coroutine de déplacement des ennemis. Parcourt le tableau de positions de l'ennemis et place l'ennemi à la position de l'indice en cours. Attend un laps de temps défini par la vitesse de l'ennemi avant d'aller à l'indice suivant. Gère également le sens de la sprite en fonction de l'orientation de la marche. Si l'ennemi est en combat, le déplacement est mis en pause.
    public IEnumerator DeplacementEnnemi()
    {
        positionOnChemin = 0;
        while(positionOnChemin < positionsTableau.Length)
        {
            if(levelManager.isLose || levelManager.isEndGame || isDead)
            {
                break;
            }
            monTransform.position = positionsTableau[positionOnChemin];
           
            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime*speedMove);
            if (positionOnChemin < positionsTableau.Length - 1)     //Si on ne se bat pas et que la position de l'ennemi est inférieure à la dimension du nombre de positions
            {
                //Si le point de chemin suivant est libre, on met le point actuel libre et le point suivant non libre
                while (levelManager.cheminOccupantTransform[cheminChoisi][positionOnChemin + 1] != null)     //On attend tant que le point de chemin suivant n'est pas libre
                {
                    if (!isFighting)
                    {
                        monAnimator.SetBool("MarcheGaucheBool", false);
                        monAnimator.SetBool("MarcheDroitBool", false);
                    }
                    yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                    if(levelManager.cheminOccupantTransform[cheminChoisi][positionOnChemin + 1] == null)     
                    {
                        break;
                    }
                }

                if (positionsTableau[positionOnChemin].x < positionsTableau[positionOnChemin + 1].x)    //A gauche
                {
                    monAnimator.SetBool("MarcheGaucheBool", false);
                    monAnimator.SetBool("MarcheDroitBool", true);
                }
                else
                {                           //A droite
                    monAnimator.SetBool("MarcheGaucheBool", true);
                    monAnimator.SetBool("MarcheDroitBool", false);
                }

                levelManager.cheminOccupantTransform[cheminChoisi][positionOnChemin] = null;
                levelManager.cheminOccupantTransform[cheminChoisi][positionOnChemin + 1] = monTransform;
                positionOnChemin++;
            }
            else
            {
                yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                if (levelManager.isLose || levelManager.isEndGame)
                {
                    break;
                }
            }
        }
    }



    //Combat Fonction : On lance le combat depuis l'Ennemi (choix arbitraire)
    public IEnumerator FonctionCombat(Transform _Adversaire)
    {
        monAnimator.SetBool("MarcheGaucheBool", false);
        monAnimator.SetBool("MarcheDroitBool", false);
        //yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
        
        //Si l'unité joueur est en train d'attaquer à distance, on attend la fin de l'attaque avant de lancer le combat

        if (_Adversaire.tag == "UniteJoueur")
        {
            CallBacksUnitesJoueur adversaireScript;
            adversaireScript = _Adversaire.GetComponent<CallBacksUnitesJoueur>();
            adversaireScript.ennemiTransform = monTransform;
            adversaireScript.monUniteJoueur.isFighting = true;

            int initiativeEnnemi = uniteVitesseInitiative;
            int initiativeUniteJoueur = adversaireScript.monUniteJoueur.uniteVitesseInitiative;

            float degatsEnnemi = uniteDegatsValue;
            float degatsUnitesJoueur = adversaireScript.monUniteJoueur.uniteDegatsValue;
            float armureEnnemi = uniteArmureValue;
            float armureUniteJoueur = adversaireScript.monUniteJoueur.uniteArmureValue;

            bool isWinner = false;


            //Cas 1 : initiativeEnnemi > initiativeUniteJoueur, l'Ennemi commence
            if (initiativeEnnemi > initiativeUniteJoueur)
            {
                //Tant que les pv de l'ennemi ou de l'unité joueur sont > 0
                while (pv > 0 && adversaireScript.monUniteJoueur.pv > 0)
                {
                    string animName = "";

                    #region Attaque de l'Ennemi
                    //L'ennemi attaque
                    yield return new WaitForSeconds(0.5f);
                    animName = DeterminationAnimationSensAttaque(monTransform.position.x, adversaireScript.transform.position.x, monAnimator);

                    AnimatorStateInfo ennemiStateInfo;
                    yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                    ennemiStateInfo = monAnimator.GetCurrentAnimatorStateInfo(0);

                    while (!ennemiStateInfo.IsTag("attack"))
                    {
                        ennemiStateInfo = monAnimator.GetCurrentAnimatorStateInfo(0);
                        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                        if (ennemiStateInfo.IsTag("attack"))
                        {
                            break;
                        }
                    }

                    while (ennemiStateInfo.normalizedTime < 1)
                    {
                        ennemiStateInfo = monAnimator.GetCurrentAnimatorStateInfo(0);
                        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                    }

                    ennemiStateInfo = FinTourAttaque(monAnimator, animName);
                    AttaquePV_MaJ(degatsEnnemi,armureUniteJoueur ,adversaireScript);

                    if (adversaireScript.monUniteJoueur.pv <= 0f)
                    {
                        isWinner = true;
                        break;
                    }
                    #endregion


                    #region Attaque de l'Unité Joueur
                    //L'unité joueur attaque
                    else
                    {
                        yield return new WaitForSeconds(0.5f);

                        animName = DeterminationAnimationSensAttaque(adversaireScript.transform.position.x, monTransform.position.x, adversaireScript.monUniteJoueur.monAnimator);

                        AnimatorStateInfo adversaireAnimClipInfo;
                        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                        adversaireAnimClipInfo = adversaireScript.monUniteJoueur.monAnimator.GetCurrentAnimatorStateInfo(0);

                        while (!adversaireAnimClipInfo.IsTag("attack"))
                        {
                            adversaireAnimClipInfo = adversaireScript.monUniteJoueur.monAnimator.GetCurrentAnimatorStateInfo(0);
                            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                            if (adversaireAnimClipInfo.IsTag("attack"))
                            {
                                break;
                            }
                        }

                        while (adversaireAnimClipInfo.normalizedTime < 1)
                        {
                            adversaireAnimClipInfo = adversaireScript.monUniteJoueur.monAnimator.GetCurrentAnimatorStateInfo(0);
                            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                        }

                        joueurHiting = adversaireScript.monUniteJoueur.uniteJoueurOwner;
                        adversaireAnimClipInfo = FinTourAttaque(adversaireScript.monUniteJoueur.monAnimator, animName);

                        AttaquePV_MaJ(degatsUnitesJoueur,armureEnnemi);

                        if (pv <= 0)
                        {
                            isWinner = false;
                            break;
                        }
                    }
                    #endregion
                }
            }
            //Cas 2 : initiativeEnnemi <= initiativeUniteJoueur, l'UniteJoueur commence
            else if (initiativeEnnemi <= initiativeUniteJoueur)
            {
                //Tant que les pv de l'unité du joueur et de l'ennemi sont supérieurs à 0
                while (pv > 0 && adversaireScript.monUniteJoueur.pv > 0)
                {
                    string animName = "";

                    #region Attaque de l'Unité Joueur
                    //L'unité joueur attaque

                    yield return new WaitForSeconds(0.5f);

                    animName = DeterminationAnimationSensAttaque(adversaireScript.transform.position.x, monTransform.position.x, adversaireScript.monUniteJoueur.monAnimator);
                    
                    AnimatorStateInfo adversaireAnimClipInfo;
                    yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                    adversaireAnimClipInfo = adversaireScript.monUniteJoueur.monAnimator.GetCurrentAnimatorStateInfo(0);

                    while (!adversaireAnimClipInfo.IsTag("attack"))
                    {
                        adversaireAnimClipInfo = adversaireScript.monUniteJoueur.monAnimator.GetCurrentAnimatorStateInfo(0);
                        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                        if (adversaireAnimClipInfo.IsTag("attack"))
                        {
                            break;
                        }
                    }

                    while (adversaireAnimClipInfo.normalizedTime < 1)
                    {
                        adversaireAnimClipInfo = adversaireScript.monUniteJoueur.monAnimator.GetCurrentAnimatorStateInfo(0);
                        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                    }

                    joueurHiting = adversaireScript.monUniteJoueur.uniteJoueurOwner;
                    adversaireAnimClipInfo = FinTourAttaque(adversaireScript.monUniteJoueur.monAnimator, animName);

                    AttaquePV_MaJ(degatsUnitesJoueur,armureEnnemi);

                    if (pv <= 0)
                    {
                        isWinner = false;
                        break;
                    }
                    #endregion

                    #region Attaque de l'Ennemi
                    //L'ennemi attaque
                    else
                    {
                        yield return new WaitForSeconds(0.5f);
                        animName = DeterminationAnimationSensAttaque(monTransform.position.x, adversaireScript.transform.position.x, monAnimator);

                        AnimatorStateInfo ennemiStateInfo;
                        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                        ennemiStateInfo = monAnimator.GetCurrentAnimatorStateInfo(0);

                        while (!ennemiStateInfo.IsTag("attack"))
                        {
                            ennemiStateInfo = monAnimator.GetCurrentAnimatorStateInfo(0);
                            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                            if (ennemiStateInfo.IsTag("attack"))
                            {
                                break;
                            }
                        }

                        while (ennemiStateInfo.normalizedTime < 1)
                        {
                            ennemiStateInfo = monAnimator.GetCurrentAnimatorStateInfo(0);
                            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                        }

                        ennemiStateInfo = FinTourAttaque(monAnimator, animName);
                        AttaquePV_MaJ(degatsEnnemi,armureUniteJoueur, adversaireScript);

                        if (adversaireScript.monUniteJoueur.pv <= 0)
                        {
                            isWinner = true;
                            break;
                        }
                    }
                    #endregion

                }

            }

            isCombatWait = false;     //On est plus en combat
            isFighting = false;
            adversaireScript.monUniteJoueur.isFighting = false;
        }

        else if(_Adversaire.tag == "SuperUniteJoueur")
        {
            CallBacksSuperUnitesJoueur adversaireScript;
            adversaireScript = _Adversaire.GetComponent<CallBacksSuperUnitesJoueur>();
            adversaireScript.ennemiTransform = monTransform;
            adversaireScript.maSuperUniteJoueur.isFighting = true;
            int initiativeEnnemi = uniteVitesseInitiative;
            int initiativeUniteJoueur = adversaireScript.maSuperUniteJoueur.uniteVitesseInitiative;

            float degatsEnnemi = uniteDegatsValue;
            float degatsUnitesJoueur = adversaireScript.maSuperUniteJoueur.uniteDegatsValue;

            float armureEnnemi = uniteArmureValue;
            float armureUnitesJoueur = adversaireScript.maSuperUniteJoueur.uniteArmureValue;

            bool isWinner = false;


            //Cas 1 : initiativeEnnemi > initiativeUniteJoueur, l'Ennemi commence
            if (initiativeEnnemi > initiativeUniteJoueur)
            {
                //Tant que les pv de l'ennemi ou de l'unité joueur sont > 0
                while (pv > 0 || adversaireScript.maSuperUniteJoueur.pv > 0)
                {
                    string animName = "";

                    #region Attaque de l'Ennemi
                    //L'ennemi attaque
                    yield return new WaitForSeconds(0.5f);
                    animName = DeterminationAnimationSensAttaque(monTransform.position.x, adversaireScript.transform.position.x, monAnimator);

                    AnimatorStateInfo ennemiStateInfo;
                    yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                    ennemiStateInfo = monAnimator.GetCurrentAnimatorStateInfo(0);

                    while (!ennemiStateInfo.IsTag("attack"))
                    {
                        ennemiStateInfo = monAnimator.GetCurrentAnimatorStateInfo(0);
                        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                        if (ennemiStateInfo.IsTag("attack"))
                        {
                            break;
                        }
                    }

                    while (ennemiStateInfo.normalizedTime < 1)
                    {
                        ennemiStateInfo = monAnimator.GetCurrentAnimatorStateInfo(0);
                        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                    }

                    ennemiStateInfo = FinTourAttaque(monAnimator, animName);
                    AttaquePV_MaJ(degatsEnnemi,armureUnitesJoueur, adversaireScript);


                    if (adversaireScript.maSuperUniteJoueur.pv <= 0)
                    {
                        isWinner = true;
                        break;
                    }
                    #endregion


                    #region Attaque de l'Unité Joueur
                    //L'unité joueur attaque
                    else
                    {
                        yield return new WaitForSeconds(0.5f);

                        animName = DeterminationAnimationSensAttaque(adversaireScript.transform.position.x, monTransform.position.x, adversaireScript.maSuperUniteJoueur.monAnimator);

                        AnimatorStateInfo adversaireAnimClipInfo;
                        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                        adversaireAnimClipInfo = adversaireScript.maSuperUniteJoueur.monAnimator.GetCurrentAnimatorStateInfo(0);

                        while (!adversaireAnimClipInfo.IsTag("attack"))
                        {
                            adversaireAnimClipInfo = adversaireScript.maSuperUniteJoueur.monAnimator.GetCurrentAnimatorStateInfo(0);
                            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                            if (adversaireAnimClipInfo.IsTag("attack"))
                            {
                                break;
                            }
                        }

                        while (adversaireAnimClipInfo.normalizedTime < 1)
                        {
                            adversaireAnimClipInfo = adversaireScript.maSuperUniteJoueur.monAnimator.GetCurrentAnimatorStateInfo(0);
                            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                        }
                        joueurHiting = adversaireScript.maSuperUniteJoueur.uniteJoueurOwner;
                        adversaireAnimClipInfo = FinTourAttaque(adversaireScript.maSuperUniteJoueur.monAnimator, animName);

                        AttaquePV_MaJ(degatsUnitesJoueur,armureEnnemi);

                        if (pv <= 0)
                        {
                            isWinner = false;
                            break;
                        }
                    }
                    #endregion
                }
            }
            //Cas 2 : initiativeEnnemi <= initiativeUniteJoueur, l'UniteJoueur commence
            else if (initiativeEnnemi <= initiativeUniteJoueur)
            {
                //Tant que les pv de l'unité du joueur et de l'ennemi sont supérieurs à 0
                while (pv > 0 || adversaireScript.maSuperUniteJoueur.pv > 0)
                {
                    string animName = "";

                    #region Attaque de l'Unité Joueur
                    //L'unité joueur attaque

                    yield return new WaitForSeconds(0.5f);

                    animName = DeterminationAnimationSensAttaque(adversaireScript.transform.position.x, monTransform.position.x, adversaireScript.maSuperUniteJoueur.monAnimator);

                    AnimatorStateInfo adversaireAnimClipInfo;
                    yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                    adversaireAnimClipInfo = adversaireScript.maSuperUniteJoueur.monAnimator.GetCurrentAnimatorStateInfo(0);

                    while (!adversaireAnimClipInfo.IsTag("attack"))
                    {
                        adversaireAnimClipInfo = adversaireScript.maSuperUniteJoueur.monAnimator.GetCurrentAnimatorStateInfo(0);
                        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                        if (adversaireAnimClipInfo.IsTag("attack"))
                        {
                            break;
                        }
                    }

                    while (adversaireAnimClipInfo.normalizedTime < 1)
                    {
                        adversaireAnimClipInfo = adversaireScript.maSuperUniteJoueur.monAnimator.GetCurrentAnimatorStateInfo(0);
                        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                    }
                    joueurHiting = adversaireScript.maSuperUniteJoueur.uniteJoueurOwner;
                    adversaireAnimClipInfo = FinTourAttaque(adversaireScript.maSuperUniteJoueur.monAnimator, animName);

                    AttaquePV_MaJ(degatsUnitesJoueur,armureEnnemi);

                    if (pv <= 0)
                    {
                        isWinner = false;
                        break;
                    }
                    #endregion

                    #region Attaque de l'Ennemi
                    //L'ennemi attaque
                    else
                    {
                        yield return new WaitForSeconds(0.5f);
                        animName = DeterminationAnimationSensAttaque(monTransform.position.x, adversaireScript.transform.position.x, monAnimator);

                        AnimatorStateInfo ennemiStateInfo;
                        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                        ennemiStateInfo = monAnimator.GetCurrentAnimatorStateInfo(0);

                        while (!ennemiStateInfo.IsTag("attack"))
                        {
                            ennemiStateInfo = monAnimator.GetCurrentAnimatorStateInfo(0);
                            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                            if (ennemiStateInfo.IsTag("attack"))
                            {
                                break;
                            }
                        }

                        while (ennemiStateInfo.normalizedTime < 1)
                        {
                            ennemiStateInfo = monAnimator.GetCurrentAnimatorStateInfo(0);
                            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                        }

                        ennemiStateInfo = FinTourAttaque(monAnimator, animName);
                        AttaquePV_MaJ(degatsEnnemi,armureUnitesJoueur, adversaireScript);


                        if (adversaireScript.monUniteJoueur.pv <= 0)
                        {
                            isWinner = true;
                            break;
                        }
                    }
                    #endregion

                }

            }



            isCombatWait = false;     //On est plus en combat
            isFighting = false;
            adversaireScript.maSuperUniteJoueur.isFighting = false;
        }

    }


 




    //Fonction de collision entre la hitbox de l'arme d'un ennemi et la porte de la ville
    public IEnumerator AttaquePorte()
    {
        monAnimator.SetBool("MarcheGaucheBool", false);
        monAnimator.SetBool("MarcheDroitBool", false);

        string animName = "";
        if (levelManager._PorteVille.monGO != null && levelManager._PorteVille.monGO.transform.position.x - monTransform.position.x > 0)
        {
            animName = "AttackDroitBool";
        } else
        {
            animName = "AttackGaucheBool";
        }
        
        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
        AnimatorStateInfo ennemiStateInfo;
        ennemiStateInfo = monAnimator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);

        while (levelManager._PorteVille.pv >= 0)
        {
            monAnimator.SetBool(animName, true);
            if (levelManager.isEndGame)
            {
                break;
            }

            float normalTimeAnim = 0;

            while (normalTimeAnim < 0.95f)
            {
                yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                ennemiStateInfo = monAnimator.GetCurrentAnimatorStateInfo(0);
                normalTimeAnim = ennemiStateInfo.normalizedTime - (int)ennemiStateInfo.normalizedTime;
                if (normalTimeAnim >= 0.95f)
                {
                    monAnimator.SetBool(animName, false);
                    levelManager._PorteVille.pv -= uniteDegatsValue;                   //Diminution des pv de la porte
                    levelManager._PorteVille.FillAmountHealthBarImage();  //Affichage du nombre de pv de la porte sur sa barre de vie
                    break;
                }
            }

           
            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
            //yield return new WaitForSeconds(0.5f);


            //Si les pv de la porte sont négatifs ou nuls, la porte est détruite. 
            if (levelManager._PorteVille.pv <= 0)
            {
                Destroy(levelManager._PorteVille.monGO);
                levelManager.isLose = true;
                levelManager.panelDefaite.SetActive(true);
                break;
            }
        }
        isFightingPorteWait = false;
        isFighting = false;
    }



    public void CheckPlaceSuivanteLibre()
    {
        if (monTransform.tag == "Ennemi")
        {
            if (positionOnChemin + 1 < positionsTableau.Length)
            {
                //Si devant, il y a une unité joueur, combat.
                if (levelManager.cheminOccupantTransform[cheminChoisi][positionOnChemin + 1] != null && (levelManager.cheminOccupantTransform[cheminChoisi][positionOnChemin + 1].tag == "UniteJoueur" || levelManager.cheminOccupantTransform[cheminChoisi][positionOnChemin + 1].tag == "SuperUniteJoueur"))
                {
                    if (!isFighting)
                    {
                        isFighting = true;
                        isCombatWait = true;
                        //On assigne l'adversaire du combat
                        combatAdversaire = levelManager.cheminOccupantTransform[cheminChoisi][positionOnChemin + 1];
                    }
                }
                else
                {
                    isFighting = false;
                }
            }
            else
            {
                if (!isFighting)
                {
                    //On est à la porte !! Attaque de la porte
                    isFighting = true;
                    isFightingPorteWait = true;
                    /*monAnimator.SetTrigger("isFighting");*/
                }
            }
        }
    }

}
