using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AvatarGestion : MonoBehaviour
{
    public int nbAvatarDispo;
    private int nbCurrentAvatar;
    public GameObject prefabPanelAvatar;
    public Transform parentListeAvatar;
    public List<AvatarPersonnage> avatarsDispo = new List<AvatarPersonnage>();

    //Variables temporaires pour créer avatar
    public InputField nomAvatarInputField;
    private int portraitAvatarImageIndice;
    private string nomTemp;
    private Sprite portraitTemp;


    // Start is called before the first frame update
    void Start()
    {
        CreateAvatarPanels();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateAvatarPanels()
    {
        for (int ii = nbCurrentAvatar; ii < nbAvatarDispo; ii++)
        {
            GameObject monAvatar = Instantiate(prefabPanelAvatar, parentListeAvatar);
        }
        if (nbCurrentAvatar < nbAvatarDispo)
        {
            nbCurrentAvatar = nbAvatarDispo;
        }
    }


    public void CreateAvatar()
    {
        nbAvatarDispo++;
        CreateAvatarPanels();
        Debug.Log("Avatar créé !");
    }

    public void SelectImageAvatar()
    {
        Debug.Log("Image sélectionnée");
        EventSystem myEventSystem;
        myEventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        if(myEventSystem.currentInputModule)
    }

    public void SelectNomAvatar()
    {
        if(nomAvatarInputField.text != "")
        {
            nomTemp = nomAvatarInputField.text;
        }
    }
}

[System.Serializable]
public class AvatarPersonnage
{
    public string nomAvatar;
    public Sprite portraitAvatar;
}
