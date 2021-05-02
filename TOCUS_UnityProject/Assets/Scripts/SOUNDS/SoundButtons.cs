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

    public void menuHERO01()
    {
        AkSoundEngine.PostEvent("UI_menu_VX_HERO_01", gameObject);
    }

    public void menuHERO02()
    {
        AkSoundEngine.PostEvent("UI_menu_VX_HERO_02", gameObject);
    }

    public void menuHERO03()
    {
        AkSoundEngine.PostEvent("UI_menu_VX_HERO_03", gameObject);
    }

    public void menuHERO04()
    {
        AkSoundEngine.PostEvent("UI_menu_VX_HERO_04", gameObject);
    }


    public void menuHERO_RIP()
    {
        AkSoundEngine.PostEvent("UI_menu_hero_RIP", gameObject);
    }

    public void menuHERO_CREATE()
    {
        AkSoundEngine.PostEvent("UI_menu_hero_CREATE", gameObject);
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