using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New UniteJoueurObject", menuName = "UniteJoueurObject")]
public class UniteJoueurObject : Unite_Objects
{
    public Sprite[] uniteSpriteBase;        //Sprite de base de l'unité (0 : joueur1, 1 : joueur2)
    public int unitePrixBle;                //Prix en blé de l'unité
    public int uniteListeOrdre;             //Ordre dans la liste. Doit être égal à l'ordre donné dans RepertoireDesSprites

    public RuntimeAnimatorController[] uniteAnimatorController;               //Animator de l'unité  (0 : joueur1, 1 : joueur2)

}
