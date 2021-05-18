using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Gère la classe PorteVille, héritant de ObjetsGeneralites
public class PorteVille : ObjetsGeneralites
{
    //Variables portes
    public float pvMax;          //PvMax de la porte
    public float pv;                    //Pv en cours de la porte

    [HideInInspector] public Image healthBarImage;              //Image de la barre de vie évolutive de la porte
    [HideInInspector] public BoxCollider2D porteBoxCollider;    //BoxCollider de la porte


    //Fonction constructeur de PorteVille. Attribue les valeurs aux variables spécifiques de la porte. Détruits les composants inutiles du préfab. Fixe la position et l'échelle de la barre de vie et de la boite de collision.
   public PorteVille(Sprite _spriteBaseObjet, string _NomObjet,bool _IsInteractable)
    {
        ConstructeursBatiment(_spriteBaseObjet, _NomObjet,_IsInteractable);
        Destroy(nbTextRessources_GO);
        Destroy(imageFlagJoueur);
        Destroy(batimentConstruit);
        Destroy(spritesUnitesFiles);

        pvMax = (float)JsonParametresGlobaux.ficParamGlobaux.objet_divers.o_divers.i_vitalite_porte;
        pv = pvMax;
        healthBarMax_GO.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0.6f);
        healthBarMax_GO.GetComponent<RectTransform>().localScale = new Vector3(0.2f, 0.2f, 1);
        healthBarImage = healthBarMax_GO.transform.GetChild(0).transform.GetComponent<Image>();

        porteBoxCollider = monGO.AddComponent<BoxCollider2D>();
        porteBoxCollider.isTrigger = true;
        porteBoxCollider.offset = new Vector2(0.1f, 0.05f);

        transformObjet.GetChild(0).GetComponent<Canvas>().sortingOrder = 1;

        FillAmountHealthBarImage();
    }

    //Permet de mettre à jour la barre de vie en fonction des pv
    public void FillAmountHealthBarImage()
    {
        healthBarImage.fillAmount = pv / pvMax;
        if(pv < pvMax)
        {
            healthBarMax_GO.SetActive(true);
        } else if(pv >= pvMax)
        {
            pv = pvMax;
            healthBarMax_GO.SetActive(false);
        } 
    }
}
