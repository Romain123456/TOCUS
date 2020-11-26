using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccueilManager : MonoBehaviour
{
    void Awake()
    {
        ActivationDesactivationPanelsMenu();
    }


    // Permet d'activer le panel de menu et désactiver les autres au départ
    void ActivationDesactivationPanelsMenu()
    {
        GameObject canvasPanels = GameObject.Find("Canvas");
        for (int ii = 0; ii < canvasPanels.transform.childCount; ii++)
        {
            if (ii == 0)
            {
                canvasPanels.transform.GetChild(ii).gameObject.SetActive(true);
            }
            else
            {
                canvasPanels.transform.GetChild(ii).gameObject.SetActive(false);
            }
        }
    }








    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
