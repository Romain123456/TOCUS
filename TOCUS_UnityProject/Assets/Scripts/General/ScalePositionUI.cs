using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScalePositionUI : MonoBehaviour
{
    // A adapter si on change de référence
    private float xmax = 1236;
    private float ymax = 1024;
    public Vector2 positionBase;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(ScaleUI(FonctionsVariablesUtiles.deltaTime));
    }

    // Update is called once per frame
    void Update()
    {
        //A lever dans le jeu
        ReSizeScaleUI();
    }

    float heightScreen;
    float widthScreen;
    public void ReSizeScaleUI()
    {
        heightScreen = Screen.height;
        widthScreen = Screen.width;

        float rapporPosX = positionBase.x / xmax;
        Debug.Log(rapporPosX);
        float newPosX = rapporPosX * widthScreen;
        float rapporPosY = positionBase.y / ymax;
        float newPosY = rapporPosY * heightScreen;

        this.GetComponent<RectTransform>().anchoredPosition = new Vector2(newPosX, newPosY);
        this.GetComponent<RectTransform>().localScale = new Vector3(widthScreen/xmax,heightScreen/ymax, 1);
    }

    public IEnumerator ScaleUI(float timeWait)
    {
        yield return new WaitForSeconds(timeWait);
        ReSizeScaleUI();
    }

}
