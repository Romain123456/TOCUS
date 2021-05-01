using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButtons : MonoBehaviour
{

    public void menuIN()
    {
        AkSoundEngine.PostEvent("UI_menu_IN", gameObject);
    }
    public void menuOUT()
    {
        AkSoundEngine.PostEvent("UI_menu_OUT", gameObject);
    }

    public void menuMOVE()
    {
        AkSoundEngine.PostEvent("UI_menu_MOVE", gameObject);
    }

    public void menuVALIDATION()
    {
        AkSoundEngine.PostEvent("UI_menu_VALIDATION", gameObject);
    }

    public void menuPLAYGAME()
    {
        AkSoundEngine.PostEvent("UI_menu_PLAYGAME", gameObject);
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