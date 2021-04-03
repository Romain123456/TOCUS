using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


//Classe Parent de toutes les unités du jeu (Ennemis et Joueur). Regroupe tout ce qui est commun entre ces deux types d'unités
public class Unites : MonoBehaviour
{
    //Variables de la classe
    //Composants
    public GameObject monGO;                        //GameObject de l'unité
    public SpriteRenderer monSpriteRenderer;        //SpriteRenderer de l'unité
    public BoxCollider2D maBoxCollider;             //BoxCollider de l'unité
    public Animator monAnimator;                    //Animator de l'unité
    public Transform monTransform;                  //Transform de l'unité
    public Rigidbody2D monRigidBody;                //RigidBody de l'unité
    public Image barreVieImage;                     //Image de la barre de vie de l'unité
    public GameObject barreVieGO;                   //GameObject de toute la barre de vie
    //public GameObject weaponHitbox;                 //GameObject de la boite de collision de l'arme de l'unité
    public AnimatorStateInfo stateInfo;             //Informtations sur l'animation en cours de l'unité


    //Manager du niveau
    [HideInInspector] public LevelManager levelManager;                 //Script gérant le déroulé de la partie


    //Animations
    public RuntimeAnimatorController unitAnimatorController;            //Animator Controller de l'unité
    public int hashAttaque;                         //Identifiant de l'animation Attaque de l'unité

    //Propriétés
    public float pv;                                //PV en cours de l'unité
    public float pvMax;                             //PV max de l'unité
    public Unite_Objects.typeVitalite uniteVitalite;                    //Vitalité de l'unité
    public Unite_Objects.typeInitiative uniteInitiative;                  //Initiative de l'unité
    public int uniteVitesseInitiative;              //Int pour traduire l'initiative de l'unité
    public Unite_Objects.typeDegats uniteDegats;                      //Type de valeur de dégats de l'unité
    public float uniteDegatsValue;                  //Float de valeur de dégats de l'unité
    public Unite_Objects.typeArmure uniteArmure;    //Type de valeur d'armure
    public float uniteArmureValue;                  //Float de la valeur d'armure
    public Vector2 scaleUnite;                      //Echelle de l'unité
    public string nomUnite;                         //Nom de l'unité
    public bool isDead;                             //Est-ce que l'unité est morte ?
    public Sprite uniteSprites;                    //Attribué dans les classes enfants
    public int cheminChoisi;                        //Chemin de l'ennemi choisi parmi les 3 proposés
    public int positionOnChemin;                    //Position de l'unité sur le chemin
    public Unite_Objects.TypePortee unitePortee;    //Type de portée de l'unité
    public int porteeValue;                         //Valeur de la portée de l'unité

    public bool isFighting;                         //Est-ce que l'unité est en train de se battre ?


    //Vector de positions de l'unité
    public Vector2[] positionsTableau;               //Positions de l'unité récupérées via le fichier json

    //Combat à distance
    [HideInInspector] public bool isCombatDistance;
    [HideInInspector] public bool combatDistanceLaunch;
    [HideInInspector] public Transform targetDistance;


    /* #region Changement Sprite Anim
     // The name of the sprite sheet to use
     public string SpriteSheetName;
     // The name of the currently loaded sprite sheet
     private string LoadedSpriteSheetName;
     // The dictionary containing all the sliced up sprites in the sprite sheet
     private Dictionary<string, Sprite> spriteSheet;
     #endregion*/



    //Fonction Constructeur de la classe Unite
    public Unites()
    {
        //Attribution des valeurs aux différentes variables des composants de l'unité
        monGO = (GameObject)Instantiate(Resources.Load("Prefab/UnitePrefab"));
        monTransform = monGO.transform;
        monSpriteRenderer = monGO.GetComponent<SpriteRenderer>();
        maBoxCollider = monGO.GetComponent<BoxCollider2D>();
        monAnimator = monGO.GetComponent<Animator>();
        monRigidBody = monGO.GetComponent<Rigidbody2D>();
        barreVieGO = monTransform.GetChild(0).GetChild(0).gameObject;
        barreVieImage = monTransform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();     //Image de la barre de vie destinée à évoluer
        levelManager = GameObject.Find("Main Camera").GetComponent<LevelManager>();
    }


    //Fonction appelée dans le constructeur de l'unité afin de charger les valeurs propres aux différentes unités. Attribue les valeurs disponibles dans le répertoire des Sprites
    public void ConstructeurUnites(Sprite _maSpriteBase,Vector2 _SizeBoxCollider,Vector2 _ScaleCanvasHealthBar,Vector2 _PositionCanvasHealthBar)
    {
        uniteSprites = _maSpriteBase;
        monSpriteRenderer.sprite = uniteSprites;
        maBoxCollider.size = _SizeBoxCollider;
        monGO.transform.GetChild(0).localScale = _ScaleCanvasHealthBar;
        monGO.transform.GetChild(0).localPosition = _PositionCanvasHealthBar;
    }


    //Fonction permettant de tirer au hasard le chemin choisi par l'unité
    public void ChoixChemin()
    {
        int way = Random.Range(0, 3);
        cheminChoisi = way;
    }



    //Fonction qui permet de mettre à jour la barre de vie en fonction des PV. à utiliser dans un callback pour ne pas surcharger l'update
    public void HealthBar_MaJ()
    {
        barreVieImage.fillAmount = pv / pvMax;
        //Affiche barre de vie que si pv < pvMax
        if(pv < pvMax)
        {
            barreVieGO.SetActive(true);
        } else if(pv >= pvMax)
        {
            barreVieGO.SetActive(false);
        }
    }


    //Attirbue les Caractéristiques de l'unité
    public void AttributionPV()
    {
        if (uniteVitalite.ToString() == "_1PV")
        {
            pvMax = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_vitalite.o_unites_monstres_valeurs_textuelles_vitalite._1pv;
        }
        else if (uniteVitalite.ToString() == "Tres_Faible")
        {
            pvMax = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_vitalite.o_unites_monstres_valeurs_textuelles_vitalite.tres_faible;
        }
        else if (uniteVitalite.ToString() == "Faible")
        {
            pvMax = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_vitalite.o_unites_monstres_valeurs_textuelles_vitalite.faible;
        }
        else if (uniteVitalite.ToString() == "Normale")
        {
            pvMax = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_vitalite.o_unites_monstres_valeurs_textuelles_vitalite.normale;
        }
        else if (uniteVitalite.ToString() == "Forte")
        {
            pvMax = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_vitalite.o_unites_monstres_valeurs_textuelles_vitalite.forte;
        }
        else if (uniteVitalite.ToString() == "Tres_Forte")
        {
            pvMax = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_vitalite.o_unites_monstres_valeurs_textuelles_vitalite.tres_forte;
        }
        else if (uniteVitalite.ToString() == "Enorme")
        {
            pvMax = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_vitalite.o_unites_monstres_valeurs_textuelles_vitalite.enorme;
        }
    }

    public void AttributionInitiative()
    {
        if (uniteInitiative.ToString() == "Premier")
        {
            uniteVitesseInitiative = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_initiative.o_unites_monstres_valeurs_textuelles_initiative.premier;
        }
        else if (uniteInitiative.ToString() == "Tres_Rapide")
        {
            uniteVitesseInitiative = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_initiative.o_unites_monstres_valeurs_textuelles_initiative.tres_rapide;
        }
        else if (uniteInitiative.ToString() == "Rapide")
        {
            uniteVitesseInitiative = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_initiative.o_unites_monstres_valeurs_textuelles_initiative.rapide;
        }
        else if (uniteInitiative.ToString() == "Normale")
        {
            uniteVitesseInitiative = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_initiative.o_unites_monstres_valeurs_textuelles_initiative.normale;
        }
        else if (uniteInitiative.ToString() == "Lente")
        {
            uniteVitesseInitiative = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_initiative.o_unites_monstres_valeurs_textuelles_initiative.lente;
        }
        else if (uniteInitiative.ToString() == "Tres_Lente")
        {
            uniteVitesseInitiative = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_initiative.o_unites_monstres_valeurs_textuelles_initiative.tres_lente;
        }
        else if (uniteInitiative.ToString() == "Dernier")
        {
            uniteVitesseInitiative = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_initiative.o_unites_monstres_valeurs_textuelles_initiative.dernier;
        }
    }

    public void AttributionDegats()
    {
        if (uniteDegats.ToString() == "OneShot")
        {
            uniteDegatsValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_degats.o_unites_monstres_valeurs_textuelles_degats.one_shot;
        }
        else if (uniteDegats.ToString() == "Enormes")
        {
            uniteDegatsValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_degats.o_unites_monstres_valeurs_textuelles_degats.enormes;
        }
        else if (uniteDegats.ToString() == "Tres_Forts")
        {
            uniteDegatsValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_degats.o_unites_monstres_valeurs_textuelles_degats.tres_forts;
        }
        else if (uniteDegats.ToString() == "Forts")
        {
            uniteDegatsValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_degats.o_unites_monstres_valeurs_textuelles_degats.forts;
        }
        else if (uniteDegats.ToString() == "Normaux")
        {
            uniteDegatsValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_degats.o_unites_monstres_valeurs_textuelles_degats.normaux;
        }
        else if (uniteDegats.ToString() == "Faibles")
        {
            uniteDegatsValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_degats.o_unites_monstres_valeurs_textuelles_degats.faibles;
        }
        else if (uniteDegats.ToString() == "Tres_Faibles")
        {
            uniteDegatsValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_degats.o_unites_monstres_valeurs_textuelles_degats.tres_faibles;
        }
        else if (uniteDegats.ToString() == "Aucun")
        {
            uniteDegatsValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_degats.o_unites_monstres_valeurs_textuelles_degats.aucun;
        }
    }

    //Attribution type armure
    public void AttributionArmure()
    {
        if(uniteArmure.ToString() == "Aucune")
        {
            uniteArmureValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_armure.o_unites_monstres_valeurs_textuelles_armure.aucune;
        } else if(uniteArmure.ToString() == "Tres_Faible")
        {
            uniteArmureValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_armure.o_unites_monstres_valeurs_textuelles_armure.tres_faible;
        } else if(uniteArmure.ToString() == "Faible")
        {
            uniteArmureValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_armure.o_unites_monstres_valeurs_textuelles_armure.faible;
        } else if(uniteArmure.ToString() == "Normale")
        {
            uniteArmureValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_armure.o_unites_monstres_valeurs_textuelles_armure.normale;
        } else if(uniteArmure.ToString() == "Forte")
        {
            uniteArmureValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_armure.o_unites_monstres_valeurs_textuelles_armure.forte;
        } else if(uniteArmure.ToString() == "Tres_Forte")
        {
            uniteArmureValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_armure.o_unites_monstres_valeurs_textuelles_armure.tres_forte;
        } else if(uniteArmure.ToString() == "Enorme")
        {
            uniteArmureValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_armure.o_unites_monstres_valeurs_textuelles_armure.enorme;
        }
    }

    public void AttributionPortee()
    {
        if(unitePortee.ToString() == "Courte")
        {
            porteeValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_portee.o_unites_monstres_valeurs_textuelles_portee.courte;
        } else if(unitePortee.ToString() == "Longue")
        {
            porteeValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_portee.o_unites_monstres_valeurs_textuelles_portee.longue;
        } else if(unitePortee.ToString() == "LongueTraversante")
        {
            porteeValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_portee.o_unites_monstres_valeurs_textuelles_portee.longue_traversante;
        } else if(unitePortee.ToString() == "Tres_Longue")
        {
            porteeValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_portee.o_unites_monstres_valeurs_textuelles_portee.tres_longue;
        } else if(unitePortee.ToString() == "Lointaine")
        {
            porteeValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_portee.o_unites_monstres_valeurs_textuelles_portee.lointaine;
        } else if(unitePortee.ToString() == "Tres_Lointaine")
        {
            porteeValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_portee.o_unites_monstres_valeurs_textuelles_portee.tres_lointaine;
        }
    }

    public void AttributionCaracteristiques()
    {
        //PV
        AttributionPV();

        //Initiative
        AttributionInitiative();

        //Dégats
        AttributionDegats();

        //Armure
        AttributionArmure();

        //Portee
        AttributionPortee();
    }



    //Liberation de l'emplacement du chemin
    public void LiberationPositionChemin()
    {
        levelManager.cheminOccupantTransform[cheminChoisi][positionOnChemin] = null;
    }



    public void VerificationCheminOccupePortee()
    {
        int sensVerif = 1;
        if(monTransform.tag != "Ennemi" && monTransform.tag != "Boss")
        {
            sensVerif = -1;
        }
        for (int ii = 2; ii <= porteeValue; ii++)
        {
            Debug.Log("ICI");
            if (positionOnChemin + sensVerif * ii < levelManager.cheminOccupantTransform[cheminChoisi].Length &&
                positionOnChemin + sensVerif * ii >= 0)
            {

                if (levelManager.cheminOccupantTransform[cheminChoisi][positionOnChemin + sensVerif * ii] != null)
                {
                    Transform target = levelManager.cheminOccupantTransform[cheminChoisi][positionOnChemin + sensVerif * ii];
                    if ((monTransform.tag != "Ennemi" && monTransform.tag != "Boss") && (target.tag == "Ennemi" || target.tag == "Boss") ||
                        ((monTransform.tag == "Ennemi" || monTransform.tag == "Boss") && (target.tag != "Ennemi" && target.tag != "Boss")))
                    {
                        if (!isCombatDistance && !combatDistanceLaunch)
                        {
                            isCombatDistance = true;
                            targetDistance = target;
                        }
                        break;
                    }
                }
            }
        }
    }


    public IEnumerator CombatDistance()
    {
        combatDistanceLaunch = true;
        string anim_name = DeterminationAnimationSensAttaque(monTransform.position.x, targetDistance.position.x, monAnimator);

        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
        stateInfo = monAnimator.GetCurrentAnimatorStateInfo(0);

        while (!stateInfo.IsTag("attack"))
        {
            stateInfo = monAnimator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
            if (stateInfo.IsTag("attack"))
            {
                break;
            }
        }
        while (stateInfo.normalizedTime < 1)
        {
            stateInfo = monAnimator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
        }

        //Debug.Log("Shoot");

        targetDistance.GetComponent<CallBacksEnnemis>().monEnnemi.AttaquePV_MaJ(uniteDegatsValue, targetDistance.GetComponent<CallBacksEnnemis>().monEnnemi.uniteArmureValue);
        stateInfo = FinTourAttaque(monAnimator, anim_name);

        targetDistance = null;
        isCombatDistance = false;
        combatDistanceLaunch = false;
    }


    //Attaque : Détermination sens de l'animation d'attaque + Lancement animation Attaque
    public string DeterminationAnimationSensAttaque(float _MaPosition, float _PositionAdversaire,Animator _MonAnimator)
    {
        string animName = "";
        if(_MaPosition - _PositionAdversaire < 0)
        {
            animName = "AttackGaucheBool";
        } else
        {
            animName = "AttackDroitBool";
        }

        _MonAnimator.SetBool(animName, true);
        return animName;
    }
   
    //Attaque : Fin de l'animation d'Attaque, retour Idle
    public AnimatorStateInfo FinTourAttaque(Animator _MonAnimator,string _AnimName)
    {
        _MonAnimator.SetBool(_AnimName, false);
        AnimatorStateInfo monAnimatorStateInfo = _MonAnimator.GetCurrentAnimatorStateInfo(0);

        return monAnimatorStateInfo;
    }

    //Attaque : Mise à jour PV et affichage
    public void AttaquePV_MaJ(float _Degats,float _Armure)        //Cas (Super)UnitéJoueur vs Ennemi
    {
        if (_Degats > _Armure)
        {
            pv = pv - (_Degats - _Armure);
        }
        HealthBar_MaJ();
    }

    public void AttaquePV_MaJ(float _Degats,float _Armure , CallBacksUnitesJoueur monJoueur)      //Cas Ennemi vs Unité Joueur
    {
        if (_Degats > _Armure)
        {
            monJoueur.monUniteJoueur.pv = monJoueur.monUniteJoueur.pv - (_Degats - _Armure);
        }
        monJoueur.monUniteJoueur.HealthBar_MaJ();
    }

    public void AttaquePV_MaJ(float _Degats, float _Armure, CallBacksSuperUnitesJoueur monJoueur)      //Cas Ennemi vs SuperUnité Joueur
    {
        if (_Degats > _Armure)
        {
            monJoueur.maSuperUniteJoueur.pv = monJoueur.maSuperUniteJoueur.pv - (_Degats - _Armure);
        }
        monJoueur.maSuperUniteJoueur.HealthBar_MaJ();
    }



    //Attaque : Mort de l'Unité
    public IEnumerator MortUnite(Animator _MonAnimator)              //Cas Ennemi
    {
        _MonAnimator.SetBool("MarcheGaucheBool", false);
        _MonAnimator.SetBool("MarcheDroitBool", false);
        _MonAnimator.SetBool("MarcheFrontBool", false);
        _MonAnimator.SetBool("MarcheBackBool", false);
        _MonAnimator.SetBool("MortBool", true);

        levelManager.tableauJoueurs[monTransform.GetComponent<CallBacksEnnemis>().monEnnemi.joueurHiting]._VictoryPoints += monTransform.GetComponent<CallBacksEnnemis>().monEnnemi._VictoryPointGain;

        AnimatorStateInfo monAnimatorStateInfo = _MonAnimator.GetCurrentAnimatorStateInfo(0);
        while (!monAnimatorStateInfo.IsName("Mort"))
        {
            monAnimatorStateInfo = _MonAnimator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
            if (monAnimatorStateInfo.IsName("Mort"))
            {
                break;
            }
        }

        while(monAnimatorStateInfo.normalizedTime < 0.95f)
        {
            monAnimatorStateInfo = _MonAnimator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
        }
        _MonAnimator.SetBool("MortBool",false);
        monGO.SetActive(false);
    }


    public IEnumerator MortUnite(Animator _MonAnimator, float _MaPositionX, float _PositionAdversaireX)     //Cas UnitéJoueur ou SuperUnité
    {
        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
        string animName = "";
        if (_MaPositionX - _PositionAdversaireX < 0)
        {
            animName = "MortDroiteBool";
        } else
        {
            animName = "MortGaucheBool";
        }
        _MonAnimator.SetBool(animName, true);
        AnimatorStateInfo monAnimatorStateInfo = _MonAnimator.GetCurrentAnimatorStateInfo(0);
        while (!monAnimatorStateInfo.IsTag("mort"))
        {
            monAnimatorStateInfo = _MonAnimator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
            if (monAnimatorStateInfo.IsTag("mort"))
            {
                break;
            }
        }

        while (monAnimatorStateInfo.normalizedTime < 0.95f)
        {
            monAnimatorStateInfo = _MonAnimator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
        }
        _MonAnimator.SetBool(animName, false);
        monGO.SetActive(false);

    }

    

    //Actualise l'état d'animation de l'unité
    public void ActualisationStateInfo()
    {
        stateInfo = monAnimator.GetCurrentAnimatorStateInfo(0);
    }


}
