﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Classe Chateau héritant de ObjetsGeneralites
public class Chateau : ObjetsGeneralites
{
    //Variables du Chateau
    [HideInInspector] public bool _ReserveRoundActif;           //Est-ce qu'un joueur a réservé la première place au prochain round ?
    [HideInInspector] public int _GainSolidarityPoints;        //Gain de points de solidarité en réparant la porte
    [HideInInspector] public float _PvPorteReparer;             //Nb de PV réparé à la porte

    //Fonction constructeur du chateau. Attribue les valeurs aux variables propres au chateau. Détruit les composants du prefab inutiles au chateau. 
    public Chateau(Sprite _spriteBaseObjet, string _NomObjet, bool _IsInteractable, int _IndicePanelInterractBatiment)
    {
        ConstructeursBatiment(_spriteBaseObjet, _NomObjet,_IsInteractable);
        Destroy(nbTextRessources_GO);
        Destroy(healthBarMax_GO);
        Destroy(imageFlagJoueur);
        Destroy(batimentConstruit);
        Destroy(spritesUnitesFiles);

        indicePanelInterractBatiment = _IndicePanelInterractBatiment;       //Attribution de l'indice du chateau dans le panel des écrans d'interaction
        panelInteractText = levelManager.panelParentBatimentInterract.GetChild(indicePanelInterractBatiment).GetChild(0).GetComponent<Text>();
        _InterractionsDisponibles = new GameObject[levelManager.panelParentBatimentInterract.GetChild(indicePanelInterractBatiment).GetChild(1).childCount];    //Attribution des Boutons du panel d'interaction avec le joueur propre au chateau
        for(int ii = 0; ii < _InterractionsDisponibles.Length; ii++)
        {
            _InterractionsDisponibles[ii] = levelManager.panelParentBatimentInterract.GetChild(indicePanelInterractBatiment).GetChild(1).GetChild(ii).gameObject;
        }


        _GainSolidarityPoints = JsonParametresGlobaux.ficParamGlobaux.objet_divers.o_divers.i_gain_solidarite_reparation_porte;
        _PvPorteReparer = JsonParametresGlobaux.ficParamGlobaux.objet_divers.o_divers.i_gain_pv_reparation_porte;
    }


    //Gameplay du chateau : prendre 2 de blé
    public void Chateau_PrendreBle()
    {
        levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[0] += 2;     //Ajoute 2 de blé au joueur actif
        levelManager.tableauJoueurs[levelManager._JoueurActif - 1].AfficheNbRessources(0);          //Affiche les ressources du joueur actif
        levelManager.panelParentBatimentInterract.gameObject.SetActive(false);                      //Ferme le panel
        
        levelManager.nbRessourcesRecup = 2;
        //Eventualité de présence de la palissade
        PalissadeFonction();

        //Index des sprites
        levelManager.indexSpriteRessources = new int[levelManager.nbRessourcesRecup];
        for (int ii = 0; ii < 2; ii++)
        {
            levelManager.indexSpriteRessources[ii] = 0;
        }
        if(levelManager.tableauJoueurs[levelManager._JoueurActif - 1].isPalissade)
        {
            levelManager.indexSpriteRessources[2] = 1;
        }

        //Changer le tour de jeu
        ChangeJoueurActif();        
    }

    //Gameplay du chateau : prendre la main au round suivant
    public void Chateau_PrendreRound()
    {
        _ReserveRoundActif = true;          //Quelqu'un a pris la main
        levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._ReserveRound = true;        //Le joueur actif a pris la main
        levelManager.panelParentBatimentInterract.gameObject.SetActive(false);          //Fermeture du panel

        //Animation Prendre round Chateau
        levelManager.nbRessourcesRecup = 1;

        //Eventualité de présence de la palissade
        PalissadeFonction();

        levelManager.indexSpriteRessources = new int[levelManager.nbRessourcesRecup];
        levelManager.indexSpriteRessources[0] = 4;
        if (levelManager.tableauJoueurs[levelManager._JoueurActif - 1].isPalissade)
        {
            levelManager.indexSpriteRessources[levelManager.nbRessourcesRecup - 1] = 1;
        }

        //Changer le tour de jeu
        ChangeJoueurActif();
    }


    //Gameplay du chateau : réparer un peu la porte et gagner des points de solidarité
    public void Chateau_ReparerPorte()
    {
        //Teste pour savoir si on a les ressources
        if (levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[0] >= JsonParametresGlobaux.ficParamGlobaux.objet_divers.o_divers.arr_cout_reparation_porte[0] &&
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[1] >= JsonParametresGlobaux.ficParamGlobaux.objet_divers.o_divers.arr_cout_reparation_porte[1] &&
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[2] >= JsonParametresGlobaux.ficParamGlobaux.objet_divers.o_divers.arr_cout_reparation_porte[2] &&
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[3] >= JsonParametresGlobaux.ficParamGlobaux.objet_divers.o_divers.arr_cout_reparation_porte[3])
        {

            int prixBle = JsonParametresGlobaux.ficParamGlobaux.objet_divers.o_divers.arr_cout_reparation_porte[0];
            int prixBois = JsonParametresGlobaux.ficParamGlobaux.objet_divers.o_divers.arr_cout_reparation_porte[1];
            int prixFer = JsonParametresGlobaux.ficParamGlobaux.objet_divers.o_divers.arr_cout_reparation_porte[2];
            int prixPierre = JsonParametresGlobaux.ficParamGlobaux.objet_divers.o_divers.arr_cout_reparation_porte[3];


            //Calcul du nombre de ressources nécessaire
            levelManager.nbRessourcesRecup = prixBle + prixBois + prixFer + prixPierre;

            //Répare un peu la porte
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._NbPvReparePorte += _PvPorteReparer;
            levelManager._PorteVille.pv += _PvPorteReparer;
            if (levelManager._PorteVille.pv >= levelManager._PorteVille.pvMax)
            {
                levelManager._PorteVille.pv = levelManager._PorteVille.pvMax;
            }
            levelManager._PorteVille.FillAmountHealthBarImage();

            //Eventualité de présence de la palissade
            PalissadeFonction();

            //Index des sprites
            levelManager.indexSpriteRessources = new int[levelManager.nbRessourcesRecup];
            for(int ii = 0; ii < prixBle; ii++)
            {
                levelManager.indexSpriteRessources[ii] = 0;
            }
            for(int ii = prixBle; ii < prixBle + prixBois; ii++)
            {
                levelManager.indexSpriteRessources[ii] = 1;
            }
            for(int ii= prixBle + prixBois;ii< prixBle + prixBois + prixFer; ii++)
            {
                levelManager.indexSpriteRessources[ii] = 2;
            }
            for(int ii= prixBle + prixBois + prixFer;ii < prixBle + prixBois + prixFer + prixPierre; ii++)
            {
                levelManager.indexSpriteRessources[ii] = 3;
            }
            if(levelManager.tableauJoueurs[levelManager._JoueurActif - 1].isPalissade)
            {
                levelManager.indexSpriteRessources[levelManager.nbRessourcesRecup-1] = 1;
            }

            //Gagner points de solidarité
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._SolidarityPoints += _GainSolidarityPoints;
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1].AffichePointVictoireSolidarite(levelManager.tableauJoueurs[levelManager._JoueurActif - 1].solidarityPointText, levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._SolidarityPoints);

            levelManager.panelParentBatimentInterract.gameObject.SetActive(false);                      //Ferme le panel
                                                                                                        
            //Changer le tour de jeu
            ChangeJoueurActif();
        } else
        {
            levelManager.PopUpsFonction("Pas assez de ressources !!", levelManager.textPopUps);
        }
    }


    public void PalissadeFonction()
    {
        if (levelManager.tableauJoueurs[levelManager._JoueurActif - 1].isPalissade)
        {
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[1]++;
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1].AfficheNbRessources(1);
            levelManager.nbRessourcesRecup++;
        }
    }
}
