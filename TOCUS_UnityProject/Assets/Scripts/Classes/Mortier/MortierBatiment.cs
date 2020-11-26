using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortierBatiment : MonoBehaviour
{
    [HideInInspector] public BatimentConstructions monBatiment;
    public float puissanceMortier = 10;

    private void OnEnable()
    {
        StartCoroutine(monBatiment.MortierChoixTir());  
    }



}
