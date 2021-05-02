using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncludeSoundButtonWwise : MonoBehaviour
{
    private Button monBouton;
    private GameObject soundObject;

    // Start is called before the first frame update
    void Start()
    {
        monBouton = this.GetComponent<Button>();
        soundObject = GameObject.Find("Menu_Sounds");

        monBouton.onClick.AddListener(delegate { soundObject.GetComponent<SoundButtons>().menuHERO_RIP(); });
    }
}
