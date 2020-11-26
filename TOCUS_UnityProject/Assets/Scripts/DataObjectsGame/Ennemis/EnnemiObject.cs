using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnnemiObject", menuName = "EnnemiObject")]
public class EnnemiObject : Unite_Objects
{
    public float ennemiSpeedMove;               //Vitesse de déplacement de l'ennemi
    public Sprite ennemiSprite;         //Sprite principale

    public RuntimeAnimatorController ennemiAnimatorController;               //Animator Controller de l'ennemi
    public int nbPointsVictoire;            //Combien de points de victoire donne l'ennemi ?
}

