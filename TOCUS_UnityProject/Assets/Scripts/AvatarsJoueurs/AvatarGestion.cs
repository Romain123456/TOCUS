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
    private bool portraitChoisi;

    //FonctionVariables Utiles
    public FonctionsVariablesUtiles scriptFonctions;
    static public int joueurCurrentChoix = -999;

    //Choisir l'avatar
    static public AvatarPersonnage[] avatarChoisis = new AvatarPersonnage[2];
    public Transform panelMatch;

    //Go Map
    public GameObject panelChoixMap;
    public GameObject panelPersonnages;

    // Start is called before the first frame update
    void Start()
    {
        CreateAvatarPanels();

        avatarChoisis[0] = new AvatarPersonnage();
        avatarChoisis[1] = new AvatarPersonnage();

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
            monAvatar.transform.Find("TextNomAvatar").GetComponent<Text>().text = nomTemp;
            monAvatar.transform.Find("ImagePortraitAvatar").GetComponent<Image>().sprite = portraitTemp;
            monAvatar.transform.Find("ButtonSupprimer").GetComponent<Button>().onClick.AddListener(delegate { SupprimerAvatar(); });
            monAvatar.transform.Find("ImagePortraitAvatar").GetComponent<Button>().onClick.AddListener(delegate { ChoisirAvatar(); });
        }
        if (nbCurrentAvatar < nbAvatarDispo)
        {
            nbCurrentAvatar = nbAvatarDispo;
        }
    }


    public void CreateAvatar()
    {
        EventSystem myEventSystem;
        myEventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        if (nomTemp != "" && portraitChoisi)
        {
            nbAvatarDispo++;
            CreateAvatarPanels();
            scriptFonctions.ButtonClosePanel(myEventSystem.currentSelectedGameObject.transform.parent.gameObject);
            Debug.Log("Avatar créé ! Ajouter la save");
            nomTemp = "";
            nomAvatarInputField.text = "";
            portraitTemp = null;
            portraitChoisi = false;
        } else
        {
            Debug.Log("Manque champs pour créer l'avatar");
        }
    }

    public void SelectImageAvatar()
    {
        EventSystem myEventSystem;
        myEventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        if(myEventSystem.currentSelectedGameObject != null)
        {
            portraitAvatarImageIndice = myEventSystem.currentSelectedGameObject.transform.GetSiblingIndex();
            portraitTemp = myEventSystem.currentSelectedGameObject.GetComponent<Image>().sprite;
            portraitChoisi = true;
        }
    }

    public void SelectNomAvatar()
    {
        if(nomAvatarInputField.text != "")
        {
            nomTemp = nomAvatarInputField.text;
        }
    }

    public void SupprimerAvatar()
    {
        EventSystem myEventSystem;
        myEventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        nbAvatarDispo--;
        if (nbCurrentAvatar > nbAvatarDispo)
        {
            nbCurrentAvatar = nbAvatarDispo;
        }
        if (joueurCurrentChoix != -999)
        {
            avatarChoisis[joueurCurrentChoix].indiceAvatarSelected = -999;
        }
        Destroy(myEventSystem.currentSelectedGameObject.transform.parent.gameObject);
        Debug.Log("Supprime Avatar + ajouter dans la save");
    }

    public void ChoisirAvatar()
    {
        EventSystem myEventSystem;
        myEventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        
        avatarChoisis[joueurCurrentChoix].nomAvatar = myEventSystem.currentSelectedGameObject.transform.parent.Find("TextNomAvatar").GetComponent<Text>().text;
        avatarChoisis[joueurCurrentChoix].portraitAvatar = myEventSystem.currentSelectedGameObject.transform.parent.Find("ImagePortraitAvatar").GetComponent<Image>().sprite;

        panelMatch.gameObject.SetActive(true);

        if (panelMatch.GetChild(joueurCurrentChoix).Find("ButtonSupprimer") != null)
        {
            Destroy(panelMatch.GetChild(joueurCurrentChoix).Find("ButtonSupprimer").gameObject);
        }
        if (panelMatch.GetChild(joueurCurrentChoix).Find("ImagePortraitAvatar").GetComponent<Button>() != null)
        {
            Destroy(panelMatch.GetChild(joueurCurrentChoix).Find("ImagePortraitAvatar").GetComponent<Button>());
        }
        panelMatch.GetChild(joueurCurrentChoix).gameObject.SetActive(true);
        panelMatch.GetChild(joueurCurrentChoix).Find("TextNomAvatar").GetComponent<Text>().text = avatarChoisis[joueurCurrentChoix].nomAvatar;
        panelMatch.GetChild(joueurCurrentChoix).Find("ImagePortraitAvatar").GetComponent<Image>().sprite = avatarChoisis[joueurCurrentChoix].portraitAvatar;
        if(panelMatch.GetChild(0).gameObject.activeSelf && panelMatch.GetChild(1).gameObject.activeSelf)
        {
            panelMatch.GetChild(2).gameObject.SetActive(true);
        }

        if (avatarChoisis[joueurCurrentChoix].indiceAvatarSelected != -999)
        {
            myEventSystem.currentSelectedGameObject.transform.parent.parent.GetChild(avatarChoisis[joueurCurrentChoix].indiceAvatarSelected).Find("ImagePortraitAvatar").GetComponent<Button>().interactable = true;
        }

        avatarChoisis[joueurCurrentChoix].indiceAvatarSelected = myEventSystem.currentSelectedGameObject.transform.parent.GetSiblingIndex();
        scriptFonctions.ButtonClosePanel(myEventSystem.currentSelectedGameObject.transform.parent.parent.parent.gameObject);
        myEventSystem.currentSelectedGameObject.transform.parent.parent.GetChild(avatarChoisis[joueurCurrentChoix].indiceAvatarSelected).Find("ImagePortraitAvatar").GetComponent<Button>().interactable = false;
    }


    public void LancerChoixMap()
    {
        panelPersonnages.SetActive(false);
        panelChoixMap.SetActive(true);
    }

}

[System.Serializable]
public class AvatarPersonnage
{
    public string nomAvatar;
    public Sprite portraitAvatar;

    [HideInInspector] public int indiceAvatarSelected = -999;
}
