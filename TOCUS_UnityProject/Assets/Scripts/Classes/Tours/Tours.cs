using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tours : MonoBehaviour
{
    //Variables de la tour
    [HideInInspector] public Transform tourTransform;
    private LevelManager scriptManager;

    public int joueurOwner;

    #region Portee
    public string _TypePortee;
    public float coeffPorteeVisee;
    #endregion


    #region Visée
    public string _FormeVisee;
    [HideInInspector] public Vector2 offsetColliderVisee;
    [HideInInspector] public Vector2[] sizesColliderVisee;
    [HideInInspector] public GameObject viseurTour;
    [HideInInspector] public Sprite viseeSprite;
    #endregion

    #region Tir
    private float countTir;
    public float cadenceTir;
    public Transform tourTarget;
    public TourObject.typeCadence _TypeCadence;

    public TourObject.typePuissance _TypePuissance;
    public float puissanceTour;

    [HideInInspector] public float speedTir = 10f;
    #endregion

    #region Rotation Tour

    #endregion


    #region Munition
    private List<GameObject> munitionList;
    private int nbInstanceMunitionPrefab = 5;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        tourTransform = this.transform;
        scriptManager = GameObject.Find("Main Camera").GetComponent<LevelManager>();

        //SpriteVisee();
        CreationEmptyVisee();
        TourMiseEnPlacePortee();
        TourMiseEnPlaceVisee();
        TourCadenceTirInit();
        InstantiationMunitions();

        //Initialisation du countTir pour tirer rapidement
        countTir = cadenceTir - 2*FonctionsVariablesUtiles.deltaTime;


        //StartCoroutine(PutRotationTour());
    }

    // Update is called once per frame
    void Update()
    {
        TourTirOnTrigger(cadenceTir);
        PutRotationTour();
    }



    //Fonctions de la mise en place de la tour
    #region Instantiation Munitions
    void InstantiationMunitions()
    {
        munitionList = new List<GameObject>();
        for (int ii = 0; ii < nbInstanceMunitionPrefab; ii++)
        {

            GameObject munitionPrefab = (GameObject)Instantiate(Resources.Load("Prefab/MunitionPrefab"));
            munitionPrefab.transform.parent = tourTransform;
            munitionPrefab.transform.localPosition = Vector3.zero;
            munitionList.Add(munitionPrefab);
            munitionPrefab.SetActive(false);
        }
    }

    #endregion



    /*void SpriteVisee()
    {
        if(_FormeVisee == "Ligne")
        {
            
        } else if(_FormeVisee == "CirculaireCentree")
        {
            viseeSprite = scriptManager.repertoireSprites.tourCirculaireVisee;
        } else if(_FormeVisee == "Cone")
        {
            viseeSprite = scriptManager.repertoireSprites.tourConeVisee;
        }
    }*/


    #region Creation Empty Visee
    void CreationEmptyVisee()
    {
        viseurTour = new GameObject();
        viseurTour.transform.name = "ViseeTour";
        viseurTour.transform.parent = this.transform;
        viseurTour.transform.localPosition = Vector3.zero;
        viseurTour.transform.localScale = Vector3.one;
        viseurTour.AddComponent<ViseurTours_Collision>();

        //Empty pour permettre la rotation uniquement du viseur 
        GameObject parentViseur = new GameObject();
        parentViseur.transform.name = "ParentViseeTour";
        parentViseur.transform.parent = this.transform;
        parentViseur.transform.localPosition = Vector3.zero;
        parentViseur.transform.localScale = Vector3.one;
        parentViseur.transform.rotation = Quaternion.Euler(Vector3.zero);
        viseurTour.transform.parent = parentViseur.transform;
    }

    #endregion


    #region Portee
    public void TourMiseEnPlacePortee()
    {
        if (_TypePortee == "Absolue")
        {
            coeffPorteeVisee = JsonParametresGlobaux.ficParamGlobaux.objet_tours_valeurs_textuelles_portee.o_tours_valeurs_textuelles_portee.absolue;
        } else if (_TypePortee == "Tres_Lointaine")
        {
            coeffPorteeVisee = JsonParametresGlobaux.ficParamGlobaux.objet_tours_valeurs_textuelles_portee.o_tours_valeurs_textuelles_portee.tres_lointaine;
        } else if (_TypePortee == "Lointaine")
        {
            coeffPorteeVisee = JsonParametresGlobaux.ficParamGlobaux.objet_tours_valeurs_textuelles_portee.o_tours_valeurs_textuelles_portee.lointaine;
        } else if(_TypePortee == "Normale")
        {
            coeffPorteeVisee = JsonParametresGlobaux.ficParamGlobaux.objet_tours_valeurs_textuelles_portee.o_tours_valeurs_textuelles_portee.normale;
        } else if(_TypePortee == "Courte")
        {
            coeffPorteeVisee = JsonParametresGlobaux.ficParamGlobaux.objet_tours_valeurs_textuelles_portee.o_tours_valeurs_textuelles_portee.lointaine;
        } else if(_TypePortee == "Tres_Courte")
        {
            coeffPorteeVisee = JsonParametresGlobaux.ficParamGlobaux.objet_tours_valeurs_textuelles_portee.o_tours_valeurs_textuelles_portee.tres_courte;
        }
        else if (_TypePortee == "Contact")
        {
            coeffPorteeVisee = JsonParametresGlobaux.ficParamGlobaux.objet_tours_valeurs_textuelles_portee.o_tours_valeurs_textuelles_portee.contact;
        }
    }

    #endregion



    #region Visee
    public void TourMiseEnPlaceVisee()
    {
        if(_FormeVisee == "Ligne")
        {
            viseurTour.gameObject.AddComponent<BoxCollider2D>();  //Ajout du BoxCollider
            BoxCollider2D maBoxCollider = viseurTour.GetComponent<BoxCollider2D>();       //Variable pour avoir la BoxCollider

            viseurTour.gameObject.AddComponent<LineRenderer>();       //Ajout de la LineRenderer
            offsetColliderVisee = new Vector2(10, 1);
            sizesColliderVisee = new Vector2[1];
            sizesColliderVisee[0] = new Vector2(20, 1);
            ViseeLigneBoxColliderParametrisation(maBoxCollider);              //Paramétrisation de la BoxCollider
            ViseeLigneLineRendererParametrisation(maBoxCollider);

            viseurTour.transform.localScale = new Vector3(coeffPorteeVisee, 1, 1);
            //Mettre les valeurs des paramètres pour la ligne
        }
        else if(_FormeVisee == "CirculaireCentree")
        {
            viseurTour.gameObject.AddComponent<CircleCollider2D>();     //Ajout du CircleCollider
            CircleCollider2D monCircleCollider = viseurTour.GetComponent<CircleCollider2D>();       //Variable pour avoir le CircleCollider
            viseurTour.gameObject.AddComponent<SpriteRenderer>();       //Ajout du SpriteRenderer
            SpriteRenderer monSpriteRenderer = viseurTour.GetComponent<SpriteRenderer>();   //Variable du SpriteRenderer            
            offsetColliderVisee = new Vector2(5, -2);
            sizesColliderVisee = new Vector2[1];
            sizesColliderVisee[0] = new Vector2(2, 2);
            ViseeCirculaireSpriteParametrisation(monSpriteRenderer);            //Param du SpriteRenderer
            ViseeCirculaireColliderParametrisation(monCircleCollider);      //Param du Collider

            viseurTour.transform.localPosition = new Vector3(6*coeffPorteeVisee, 0, 0);
        }
        else if(_FormeVisee == "Cone")
        {
            viseurTour.gameObject.AddComponent<PolygonCollider2D>();        //Ajout du PolygonCollider
            PolygonCollider2D monPolygonCollider = viseurTour.GetComponent<PolygonCollider2D>();        //Variable PolygonCollider
            ViseeConeColliderParametrisation(monPolygonCollider);
            viseurTour.gameObject.AddComponent<LineRenderer>();
            LineRenderer viseurRender = viseurTour.GetComponent<LineRenderer>();
            ViseeConeSpriteParametrisation(viseurRender);
            viseurTour.transform.localScale = new Vector3(coeffPorteeVisee, 1, 1);
        }

    }

    #region Visee Ligne
    void ViseeLigneBoxColliderParametrisation(BoxCollider2D _MaBoxCollider)
    {
        _MaBoxCollider.isTrigger = true;         //On en fait un Trigger
        _MaBoxCollider.offset = offsetColliderVisee;     //On place l'offset
        _MaBoxCollider.size = sizesColliderVisee[0];     //On place la taille
    }

    void ViseeLigneLineRendererParametrisation(BoxCollider2D _MaBoxCollider)
    {
        LineRenderer maLineRenderer = viseurTour.GetComponent<LineRenderer>();        //Attribution de la LineRenderer dans une variable temporaire
        maLineRenderer.useWorldSpace = false;
        maLineRenderer.positionCount = 2;
        maLineRenderer.alignment = LineAlignment.TransformZ;
        maLineRenderer.widthMultiplier = _MaBoxCollider.size.y;
        Vector3[] positionsLineRenderer = new Vector3[2];
        positionsLineRenderer[0] = new Vector3(_MaBoxCollider.offset.x-_MaBoxCollider.size.x/2.0f, _MaBoxCollider.offset.y, 0);
        positionsLineRenderer[1] = new Vector3(_MaBoxCollider.offset.x + _MaBoxCollider.size.x / 2.0f, _MaBoxCollider.offset.y, 0);
        maLineRenderer.SetPositions(positionsLineRenderer);

    }
    #endregion

    #region Visee Circulaire 
    void ViseeCirculaireColliderParametrisation(CircleCollider2D _MonCircleCollider)
    {
        _MonCircleCollider.isTrigger = true;        //On fait un Trigger
        viseurTour.transform.localPosition = offsetColliderVisee;    //On place le viseur
        viseurTour.transform.localScale = sizesColliderVisee[0];
    }

    void ViseeCirculaireSpriteParametrisation(SpriteRenderer _monSpriteRenderer)
    {
        viseeSprite = scriptManager.repertoireSprites.tourCirculaireVisee;
        _monSpriteRenderer.sprite = viseeSprite;
    }
    #endregion


    #region Visee Cone
    void ViseeConeColliderParametrisation(PolygonCollider2D _monCollider)
    {
        _monCollider.isTrigger = true;
        _monCollider.pathCount = 1;
        Vector2[] pathPolygon = new Vector2[3];
        pathPolygon[0] = new Vector2(0, 0);
        pathPolygon[1] = new Vector2(10, 2);
        pathPolygon[2] = new Vector2(10, -2);
        _monCollider.SetPath(0, pathPolygon);
    }

    void ViseeConeSpriteParametrisation(LineRenderer _monSpriteRenderer)
    {
        _monSpriteRenderer.useWorldSpace = false;
        _monSpriteRenderer.widthCurve = AnimationCurve.Linear(0, 0, 1, 1);
        Vector3[] lineViseur = new Vector3[2];
        lineViseur[0] = new Vector3(0, 0, 0);
        lineViseur[1] = new Vector3(10, 0, 0);
        _monSpriteRenderer.SetPositions(lineViseur);
    }

    #endregion

    #endregion


    #region Tir
    public void TourCadenceTirInit()
    {
        if(_TypeCadence.ToString() == "Permanente")
        {
            cadenceTir = JsonParametresGlobaux.ficParamGlobaux.objet_tours_valeurs_textuelles_cadence.o_tours_valeurs_textuelles_cadence.permanente;
        } else if(_TypeCadence.ToString() == "Tres_Rapide")
        {
            cadenceTir = JsonParametresGlobaux.ficParamGlobaux.objet_tours_valeurs_textuelles_cadence.o_tours_valeurs_textuelles_cadence.tres_rapide;
        } else if(_TypeCadence.ToString() == "Rapide")
        {
            cadenceTir = JsonParametresGlobaux.ficParamGlobaux.objet_tours_valeurs_textuelles_cadence.o_tours_valeurs_textuelles_cadence.rapide;
        } else if(_TypeCadence.ToString() == "Normale")
        {
            cadenceTir = JsonParametresGlobaux.ficParamGlobaux.objet_tours_valeurs_textuelles_cadence.o_tours_valeurs_textuelles_cadence.normale;
        } else if(_TypeCadence.ToString() == "Lente")
        {
            cadenceTir = JsonParametresGlobaux.ficParamGlobaux.objet_tours_valeurs_textuelles_cadence.o_tours_valeurs_textuelles_cadence.lente;
        } else if(_TypeCadence.ToString() == "Tres_Lente")
        {
            cadenceTir = JsonParametresGlobaux.ficParamGlobaux.objet_tours_valeurs_textuelles_cadence.o_tours_valeurs_textuelles_cadence.tres_lente;
        }

        if(_TypePuissance.ToString() == "Nulle")
        {
            puissanceTour = JsonParametresGlobaux.ficParamGlobaux.objet_tours_valeurs_textuelles_puissance.o_tours_valeurs_textuelles_puissance.nulle;
        }
        else if (_TypePuissance.ToString() == "Tres_Faible")
        {
            puissanceTour = JsonParametresGlobaux.ficParamGlobaux.objet_tours_valeurs_textuelles_puissance.o_tours_valeurs_textuelles_puissance.tres_faible;
        }
        else if(_TypePuissance.ToString() == "Faible")
        {
            puissanceTour = JsonParametresGlobaux.ficParamGlobaux.objet_tours_valeurs_textuelles_puissance.o_tours_valeurs_textuelles_puissance.faible;
        }
        else if (_TypePuissance.ToString() == "Normale")
        {
            puissanceTour = JsonParametresGlobaux.ficParamGlobaux.objet_tours_valeurs_textuelles_puissance.o_tours_valeurs_textuelles_puissance.normale;
        }
        else if(_TypePuissance.ToString() == "Forte")
        {
            puissanceTour = JsonParametresGlobaux.ficParamGlobaux.objet_tours_valeurs_textuelles_puissance.o_tours_valeurs_textuelles_puissance.forte;
        }
        else if (_TypePuissance.ToString() == "Tres_Forte")
        {
            puissanceTour = JsonParametresGlobaux.ficParamGlobaux.objet_tours_valeurs_textuelles_puissance.o_tours_valeurs_textuelles_puissance.tres_forte;
        }
        else if (_TypePuissance.ToString() == "Enorme")
        {
            puissanceTour = JsonParametresGlobaux.ficParamGlobaux.objet_tours_valeurs_textuelles_puissance.o_tours_valeurs_textuelles_puissance.enorme;
        }
        else if (_TypePuissance.ToString() == "Exponentielle")
        {
            puissanceTour = JsonParametresGlobaux.ficParamGlobaux.objet_tours_valeurs_textuelles_puissance.o_tours_valeurs_textuelles_puissance.exponentielle;
        }
    }

    void TourTirOnTrigger(float _CadenceTir)
    {
        if(tourTarget != null)
        {
            countTir += FonctionsVariablesUtiles.deltaTime;
            if(countTir >= cadenceTir)
            {
                countTir = 0;
                TirTourPooling();
            }
        }
    }
    
    void TirTourPooling()
    {
        for(int ii = 0; ii < munitionList.Count; ii++)
        {
            if (!munitionList[ii].activeInHierarchy)
            {
                munitionList[ii].SetActive(true);
                //StartCoroutine(DesactivationMunition(munitionList[ii], cadenceTir * 2));
                StartCoroutine(TirMunition(munitionList[ii], tourTarget,2.0f));
                break;
            }
        }
    }

    IEnumerator DesactivationMunition(GameObject _MaMunition,float _TpsVieMunition)
    {
        yield return new WaitForSeconds(_TpsVieMunition);
        _MaMunition.SetActive(false);
        _MaMunition.transform.localPosition = Vector3.zero;
    }
    


    public IEnumerator TirMunition(GameObject _MaMunition,Transform _TargetTir,float _Amplitude)
    {
        Vector3 directionTir = _TargetTir.position - _MaMunition.transform.position;
        int nbStepTir = 10;
        float dx = (_TargetTir.position.x - _MaMunition.transform.position.x) / ((float)nbStepTir+1);
        float dy1 = _Amplitude/((float)nbStepTir/2);
        float dy2 = ((_MaMunition.transform.position.y + (dy1 * ((float)nbStepTir / 2))) - _TargetTir.position.y) / ((float)nbStepTir / 2);

        int nbTir = 0;
        while(nbTir <= nbStepTir)
        {
            float newPosY = _MaMunition.transform.position.y;
            if (nbTir < nbStepTir / 2)
            {
                //Phase montée
                //Debug.Log("Phase 1, "+nbTir);
                newPosY += dy1;
            } else if(nbTir == nbStepTir / 2)
            {
                //Phase plane
                //Debug.Log("Phase 2, " + nbTir);
                newPosY = newPosY;
            } else if(nbTir > nbStepTir/2 && nbTir <= nbStepTir)
            {
                //Phase descente
                //Debug.Log("Phase 3, " + nbTir);
                newPosY -= dy2;
            }
            _MaMunition.transform.position = new Vector3(_MaMunition.transform.position.x + dx, newPosY, _MaMunition.transform.position.z);
            nbTir++;
            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime * speedTir);
        }
        yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
        _MaMunition.SetActive(false);
        _MaMunition.transform.localPosition = Vector3.zero;
        yield return null;
    }
    #endregion



    void PutRotationTour()
    {
        //yield return null;
        if (scriptManager._GuizmoRotationTour.GetComponent<RotationTourPlace>().tourToRotate == this.transform)
        {
            float angle = 0;
            angle = Mathf.Atan(scriptManager._GuizmoRotationTour.GetComponent<RotationTourPlace>().sinAngle / scriptManager._GuizmoRotationTour.GetComponent<RotationTourPlace>().cosAngle) * Mathf.Rad2Deg;
            if (scriptManager._GuizmoRotationTour.GetComponent<RotationTourPlace>().cosAngle < 0)
            {
                angle += 180;
            }

            if (scriptManager._GuizmoRotationTour.GetComponent<RotationTourPlace>().sinAngle != 0)
            {
                transform.Find("ParentViseeTour").rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }



    public void AmeliorationToursStatistiques()
    {
        if (_TypeCadence > 0)
        {
            _TypeCadence--;
        }
        if (_TypePuissance > 0)
        {
            _TypePuissance--;
        }
        TourCadenceTirInit();
    }
}
