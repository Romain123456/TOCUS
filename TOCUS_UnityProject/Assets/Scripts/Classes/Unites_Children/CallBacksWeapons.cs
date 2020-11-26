using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Script attaché à la hitbox du prefab des Unités
public class CallBacksWeapons : MonoBehaviour
{
    [HideInInspector] public LevelManager levelManagerScript;           //script levelManager gérant la partie
    [HideInInspector] public float att;                                 //variable d'attaque de cette hitbox

    void Start()
    {
        levelManagerScript = GameObject.Find("Main Camera").GetComponent<LevelManager>();       //attribution du script level manager
    }

    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(this.gameObject.layer == 10 && collision.name == "PorteVille" && this.gameObject.name == "WeaponHitbox")
        {
            CollisionPorteVille_ArmeEnnemi();
        }
    }


    //Fonction de collision entre la hitbox de l'arme d'un ennemi et la porte de la ville
    public void CollisionPorteVille_ArmeEnnemi()
    {
        levelManagerScript._PorteVille.pv -= att;                   //Diminution des pv de la porte
        levelManagerScript._PorteVille.FillAmountHealthBarImage();  //Affichage du nombre de pv de la porte sur sa barre de vie

        //Si les pv de la porte sont négatifs ou nuls, la porte est détruite. 
        // TODO : Faire perdre la partie !
        if (levelManagerScript._PorteVille.pv <= 0)
        {
            Destroy(levelManagerScript._PorteVille.monGO);
            levelManagerScript.isLose = true;
            levelManagerScript.panelDefaite.SetActive(true);
        }
    }
}
