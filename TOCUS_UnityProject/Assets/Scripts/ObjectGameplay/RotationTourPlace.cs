using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotationTourPlace : MonoBehaviour
{
    private Vector2 diffPosSourisImage;
    private float tanSouris;
    [HideInInspector] public float cosAngle;
    [HideInInspector] public float sinAngle;
    public GameObject boutonRotation;
    private float boutonRotationPosBase = 50.0f;
    PointerEventData monEventData;
    private bool alreadyClick;
    private LevelManager scriptManager;
    [HideInInspector] public Transform tourToRotate;

    // Start is called before the first frame update
    void Start()
    {
        scriptManager = GameObject.Find("Main Camera").GetComponent<LevelManager>();
        StartCoroutine(AssignEvent());
    }

    // Update is called once per frame
    void Update()
    {
        //PositionBouton();
        if (alreadyClick && monEventData.eligibleForClick && scriptManager.myEventSystem.currentSelectedGameObject == boutonRotation)
        {
            PositionBouton();
        }
    }



    IEnumerator AssignEvent()
    {
        yield return new WaitForSeconds(3*FonctionsVariablesUtiles.deltaTime);
        monEventData = EventSystem.current.gameObject.GetComponent<StandAloneInputModuleCustom>().GetLastPointerEventDataPublic(-1);
        alreadyClick = true;
        this.gameObject.SetActive(false);
    }

    public void PositionBouton()
    {
        diffPosSourisImage = new Vector2(Input.mousePosition.x - this.GetComponent<RectTransform>().anchoredPosition.x, Input.mousePosition.y - this.GetComponent<RectTransform>().anchoredPosition.y);
        tanSouris = diffPosSourisImage.y / diffPosSourisImage.x;
        cosAngle = Mathf.Cos(Mathf.Atan(tanSouris));
        sinAngle = Mathf.Sin(Mathf.Atan(tanSouris));

        if (Input.mousePosition.x < this.GetComponent<RectTransform>().anchoredPosition.x)
        {
            cosAngle = -cosAngle;
            sinAngle = -sinAngle;
        }

        boutonRotation.GetComponent<RectTransform>().anchoredPosition = new Vector2(boutonRotationPosBase * cosAngle, boutonRotationPosBase * sinAngle);
    }



}
