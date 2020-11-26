using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


// Gère les fonctions et les variables utiles à tout le jeu
public class FonctionsVariablesUtiles : MonoBehaviour
{
    //Variables
    static public float deltaTime;
    static public string nom_du_niveau;


    //Start
    void Start()
    {
        
    }

    //Update
    void Update()
    {
        deltaTime = Time.smoothDeltaTime;
    }



    //Méthode Quitter : permet de quitter le jeu
    public void Quitter()
    {
        Application.Quit();
    }


    //Méthode Switch Panel : permet d'activer le panel choisi et de désactiver le panel en cours
    public void SwitchPanel (GameObject panelChoisi)
    {
        EventSystem myEventSystem;
        myEventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        GameObject panelDesactiver;

        if(myEventSystem.currentSelectedGameObject != null)
        {
            panelDesactiver = myEventSystem.currentSelectedGameObject.transform.parent.gameObject;
            panelChoisi.SetActive(true);
            panelDesactiver.SetActive(false);
        } 
    }


    //Méthode Charger Scène : permet de charger la scène 
    public void ChargerSceneProto(string sceneChoisie)
    {
        SceneManager.LoadScene(sceneChoisie);
        Time.timeScale = 1;
    }
    public void ChargeScene(string sceneChoisie)
    {
        EventSystem myEventSystem;
        myEventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        nom_du_niveau = myEventSystem.currentSelectedGameObject.GetComponent<BoutonMapScript>().nomDuNiveau;
        Time.timeScale = 1;
        StartCoroutine(LoadSceneAsync(sceneChoisie));  
    }

    IEnumerator LoadSceneAsync(string sceneChoisie)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneChoisie);
        yield return null;
        // Wait until the asynchronous scene fully loads
        /*while (!asyncLoad.isDone)
        {
            yield return null;
        }*/
    }


    public void ButtonClosePanel(GameObject monPanel)
    {
        monPanel.SetActive(false);
    }


    public void ResumeFonction(GameObject monPanel)
    {
        monPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void PauseFonction(GameObject monPanel)
    {
        monPanel.SetActive(true);
        Time.timeScale = 0;
    }
}
