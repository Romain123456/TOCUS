using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitesParent : MonoBehaviour
{
    //Composants de l'unité
    [HideInInspector] public SpriteRenderer monSpriteRenderer;              //SpriteRenderer de l'unité
    [HideInInspector] public BoxCollider2D maBoxCollider;       //BoxCollider de l'unité
    [HideInInspector] public Animator monAnimator;                    //Animator de l'unité
    [HideInInspector] public Transform monTransform;                  //Transform de l'unité
    [HideInInspector] public Rigidbody2D monRigidBody;                //RigidBody de l'unité
    [HideInInspector] public Image barreVieImage;                     //Image de la barre de vie de l'unité
    [HideInInspector] public GameObject barreVieGO;                   //GameObject de toute la barre de vie
    //public GameObject weaponHitbox;                 //GameObject de la boite de collision de l'arme de l'unité
    [HideInInspector] public AnimatorStateInfo stateInfo;             //Informtations sur l'animation en cours de l'unité
    public RuntimeAnimatorController monAnimatorController;             //Controller de l'animator



    //Manager du niveau
    [HideInInspector] public LevelManager levelManager;                 //Script gérant le déroulé de la partie



    //Caractéristiques
    public float pv;                                //PV en cours de l'unité
    [HideInInspector] public float pvMax;                             //PV max de l'unité
    [HideInInspector] public Unite_Objects.typeVitalite uniteVitalite;                    //Vitalité de l'unité
    [HideInInspector] public Unite_Objects.typeInitiative uniteInitiative;                  //Initiative de l'unité
    [HideInInspector] public int uniteVitesseInitiative;              //Int pour traduire l'initiative de l'unité
    [HideInInspector] public Unite_Objects.typeDegats uniteDegats;                      //Type de valeur de dégats de l'unité
    [HideInInspector] public float uniteDegatsValue;                  //Float de valeur de dégats de l'unité
    [HideInInspector] public Unite_Objects.typeArmure uniteArmure;    //Type de valeur d'armure
    [HideInInspector] public float uniteArmureValue;                  //Float de la valeur d'armure
    [HideInInspector] public string nomUnite;                         //Nom de l'unité
    [HideInInspector] public bool isDead;                             //Est-ce que l'unité est morte ?
    [HideInInspector] public Sprite uniteSprites;                    //Attribué dans les classes enfants
    [HideInInspector] public int cheminChoisi;                        //Chemin de l'ennemi choisi parmi les 3 proposés
    [HideInInspector] public int positionOnChemin;                    //Position de l'unité sur le chemin
    [HideInInspector] public Unite_Objects.TypePortee unitePortee;    //Type de portée de l'unité
    [HideInInspector] public int porteeValue;                         //Valeur de la portée de l'unité


    [HideInInspector] public bool isFighting;                         //Est-ce que l'unité est en train de se battre ?
    [HideInInspector] public bool _Attacking;        //Attaque
    public bool isRecruted;

    //Coordonnées unités
    [HideInInspector] public int way = -999;
    [HideInInspector] public int placeChemin = -999;
    [HideInInspector] public int sens = 0;

    //Fonction Constructeur de la classe Unite
    public void InitialisationUniteParent(string _MonTag)
    {
        //Attribution des valeurs aux différentes variables des composants de l'unité
        monTransform = this.transform;
        monSpriteRenderer = this.GetComponent<SpriteRenderer>();
        maBoxCollider = this.GetComponent<BoxCollider2D>();
        monAnimator = this.GetComponent<Animator>();
        monRigidBody = this.GetComponent<Rigidbody2D>();
        barreVieGO = monTransform.GetChild(0).GetChild(0).gameObject;
        barreVieImage = monTransform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();     //Image de la barre de vie destinée à évoluer
        levelManager = GameObject.Find("Main Camera").GetComponent<LevelManager>();
        monTransform.tag = _MonTag;
    }







    //Fonction qui permet de mettre à jour la barre de vie en fonction des PV. à utiliser dans un callback pour ne pas surcharger l'update
    public void HealthBar_MaJ()
    {
        barreVieImage.fillAmount = pv / pvMax;
        //Affiche barre de vie que si pv < pvMax
        if (pv < pvMax)
        {
            barreVieGO.SetActive(true);
        }
        else if (pv >= pvMax)
        {
            barreVieGO.SetActive(false);
        }
    }


    public void ChangeAnimation(string _AnimationToActivate)
    {
        string[] _AnimationsName = new string[8];
        _AnimationsName[0] = "MarcheGaucheBool";
        _AnimationsName[1] = "MarcheFrontBool";
        _AnimationsName[2] = "MarcheBackBool";
        _AnimationsName[3] = "MarcheDroitBool";
        _AnimationsName[4] = "AttackGaucheBool";
        _AnimationsName[5] = "AttackDroitBool";
        _AnimationsName[6] = "MortDroiteBool";
        _AnimationsName[7] = "MortGaucheBool";

        if (_AnimationToActivate == "Idle")
        {
            for(int ii = 0; ii < _AnimationsName.Length; ii++)
            {
                monAnimator.SetBool(_AnimationsName[ii], false);
            }
        }
        else
        {
            for(int ii = 0; ii < _AnimationsName.Length; ii++)
            {
                if(_AnimationsName[ii] == _AnimationToActivate)
                {
                    monAnimator.SetBool(_AnimationsName[ii], true);
                } else
                {
                    monAnimator.SetBool(_AnimationsName[ii], false);
                }
            }
        }

    }


    #region Statistiques et caractéristiques
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
        if (uniteArmure.ToString() == "Aucune")
        {
            uniteArmureValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_armure.o_unites_monstres_valeurs_textuelles_armure.aucune;
        }
        else if (uniteArmure.ToString() == "Tres_Faible")
        {
            uniteArmureValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_armure.o_unites_monstres_valeurs_textuelles_armure.tres_faible;
        }
        else if (uniteArmure.ToString() == "Faible")
        {
            uniteArmureValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_armure.o_unites_monstres_valeurs_textuelles_armure.faible;
        }
        else if (uniteArmure.ToString() == "Normale")
        {
            uniteArmureValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_armure.o_unites_monstres_valeurs_textuelles_armure.normale;
        }
        else if (uniteArmure.ToString() == "Forte")
        {
            uniteArmureValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_armure.o_unites_monstres_valeurs_textuelles_armure.forte;
        }
        else if (uniteArmure.ToString() == "Tres_Forte")
        {
            uniteArmureValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_armure.o_unites_monstres_valeurs_textuelles_armure.tres_forte;
        }
        else if (uniteArmure.ToString() == "Enorme")
        {
            uniteArmureValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_armure.o_unites_monstres_valeurs_textuelles_armure.enorme;
        }
    }

    public void AttributionPortee()
    {
        if (unitePortee.ToString() == "Courte")
        {
            porteeValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_portee.o_unites_monstres_valeurs_textuelles_portee.courte;
        }
        else if (unitePortee.ToString() == "Longue")
        {
            porteeValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_portee.o_unites_monstres_valeurs_textuelles_portee.longue;
        }
        else if (unitePortee.ToString() == "LongueTraversante")
        {
            porteeValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_portee.o_unites_monstres_valeurs_textuelles_portee.longue_traversante;
        }
        else if (unitePortee.ToString() == "Tres_Longue")
        {
            porteeValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_portee.o_unites_monstres_valeurs_textuelles_portee.tres_longue;
        }
        else if (unitePortee.ToString() == "Lointaine")
        {
            porteeValue = JsonParametresGlobaux.ficParamGlobaux.objet_unites_monstres_valeurs_textuelles_portee.o_unites_monstres_valeurs_textuelles_portee.lointaine;
        }
        else if (unitePortee.ToString() == "Tres_Lointaine")
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
    #endregion


    #region Position de l'unité
    public void LiberationPlaceChemin(int _Way,int _PlaceChemin)
    {
        levelManager.cheminOccupantTransform[_Way][_PlaceChemin] = null;
    }
    #endregion

    #region Sens de la marche
    public int SensCalcul(float _maPosX,float _OtherPosX)
    {
        int _Sens = 0;
        if(_OtherPosX > _maPosX)
        {
            _Sens = 1;
        }
        else if (_OtherPosX < _maPosX)
        {
            _Sens = -1;
        }
        return _Sens;
    }
    #endregion


    #region Mort de l'Unité
    public void MortUnite()
    {
        StartCoroutine(CoroutineMort());
    }

    public IEnumerator CoroutineMort()
    {
        if(sens == 0)
        {
            sens = SensCalcul(levelManager.positionsChemin[way][placeChemin].x, levelManager.positionsChemin[way][placeChemin + 1].x);
        }
        if(sens == 1)
        {
            ChangeAnimation("MortDroiteBool");
        } else if(sens == -1)
        {
            ChangeAnimation("MortGaucheBool");
        }
        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);

        while (stateInfo.normalizedTime <= 1.01f)
        {
            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
            if(stateInfo.normalizedTime > 1.01f)
            {
                ChangeAnimation("Idle");
                break;
            }
        }

        LiberationPlaceChemin(way, placeChemin);
        isRecruted = false;
        isFighting = false;
        monTransform.gameObject.SetActive(false);
    }

    #endregion


    #region Attaque
    public IEnumerator AttaqueFonction(UnitesParent _Unite1, UnitesParent _Unite2)
    {
        _Attacking = true;
        _Unite1.sens = _Unite1.SensCalcul(_Unite1.monTransform.position.x, _Unite2.monTransform.position.x);
        if (_Unite1.sens == 1)
        {
            _Unite1.ChangeAnimation("AttackDroitBool");
        }
        else if (_Unite1.sens == -1)
        {
            _Unite1.ChangeAnimation("AttackGaucheBool");
        }

        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
        if (_Unite1.stateInfo.IsTag("attack"))
        {
            while (_Unite1.stateInfo.normalizedTime <= 1.01f)
            {
                yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
            }
        }
        _Unite1.ChangeAnimation("Idle");
        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
        _Unite2.pv -= _Unite1.uniteDegatsValue;
        _Unite2.HealthBar_MaJ();

        if(!_Unite2.isFighting && _Unite2.pv <= 0 && _Unite2.gameObject.activeSelf)
        {
            _Unite2.MortUnite();
        }

        _Attacking = false;
    }
    #endregion
}
