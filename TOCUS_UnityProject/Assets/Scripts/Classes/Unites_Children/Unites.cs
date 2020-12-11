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
    public float speedMove;                         //Vitesse de déplacement de l'unité
    public bool isDead;                             //Est-ce que l'unité est morte ?
    public Sprite uniteSprites;                    //Attribué dans les classes enfants
    public int cheminChoisi;                        //Chemin de l'ennemi choisi parmi les 3 proposés
    public int positionOnChemin;                    //Position de l'unité sur le chemin


    //Vector de positions de l'unité
    public Vector2[] positionsTableau;               //Positions de l'unité récupérées via le fichier json


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
        /*weaponHitbox = monTransform.GetChild(1).gameObject;
        weaponHitbox.SetActive(false);*/
        levelManager = GameObject.Find("Main Camera").GetComponent<LevelManager>();

        //hashAttaque = Animator.StringToHash("Base Layer.Attaque");          //ID de l'animation attaque
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
        int way = Random.RandomRange(0, 3);
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
    }



    //Liberation de l'emplacement du chemin
    public void LiberationPositionChemin()
    {
        levelManager.positionsCheminLibre[cheminChoisi, positionOnChemin] = null;
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

    /*#region Changement Sprite Anim
    // Runs after the animation has done its work
    public void ChangeSpriteAnimation()
    {
        // Check if the sprite sheet name has changed (possibly manually in the inspector)
        if (this.LoadedSpriteSheetName != this.SpriteSheetName)
        {
            // Load the new sprite sheet
            this.LoadSpriteSheet();
        }

        // Swap out the sprite to be rendered by its name
        // Important: The name of the sprite must be the same!
        monSpriteRenderer.sprite = spriteSheet[monSpriteRenderer.sprite.name];
    }

    // Loads the sprites from a sprite sheet
    public void LoadSpriteSheet()
    {
        // Load the sprites from a sprite sheet file (png). 
        // Note: The file specified must exist in a folder named Resources
        var sprites = Resources.LoadAll<Sprite>(SpriteSheetName);
        spriteSheet = sprites.ToDictionary(x => x.name, x => x);

        // Remember the name of the sprite sheet in case it is changed later
        LoadedSpriteSheetName = SpriteSheetName;
    }
    #endregion*/


    //Actualise l'état d'animation de l'unité
    public void ActualisationStateInfo()
    {
        stateInfo = monAnimator.GetCurrentAnimatorStateInfo(0);
    }

    //Activation Hitbox arme ==> a faire plus propre avec des valeurs limites en variables
    //Permet d'activer la boite de collision à des moments clés de l'animation attaque et la désactive ensuite
   /* public void ActivationHitboxArme()
    {
        if(stateInfo.nameHash == hashAttaque)
        {
            float animTime = stateInfo.normalizedTime - (int)stateInfo.normalizedTime;
            
            if(animTime>0.5f && animTime < 0.8f)
            {
                if (!weaponHitbox.activeInHierarchy)
                {
                    weaponHitbox.SetActive(true);
                }
            } else
            {
                weaponHitbox.SetActive(false);
            }
        } else
        {
            if (weaponHitbox.activeSelf)
            {
                weaponHitbox.SetActive(false);
            }
        }
    }
    */

}
