using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


//Classe BatimentConstructions héritant de ObjetsGeneralites
public class BatimentConstructions : ObjetsGeneralites
{
    [HideInInspector] public EmptyBatimentsConstruits[] _EmplacementsCreations;         //Tableau des emplacements où l'on peut placer des bariments
    //Répertoire des caractéristiques des types de création : Tour, bariment, unité
    [HideInInspector] public ToursRepertoire tourConstruite;
    [HideInInspector] public BatimentsRepertoire batimentBuild;
    [HideInInspector] public UnitesJoueurRepertoire uniteEngaged;

    [HideInInspector] public int boutonSelect;               //Indice du bouton sélectionné par le joueur dans les différents panels d'interaction
    private bool isConstructionPossible;        //Bool permettant de vérifier si la construction est possible
    private bool canChangeActiveJoueur;         //Bool permettant de verifier si l'on peut changer le joueur actif

    //Nombre de boutons à activer au démarrage du jeu
    [HideInInspector] public int nbBoutonInterractionsPossible;


    //Propre à la Caserne       //Sprite des unités en attente
    [HideInInspector] public int nbUniteRecrutable;
    [HideInInspector] public List<SpriteRenderer> spritesUnitesAttente = new List<SpriteRenderer>();
    [HideInInspector] public List<UnitesJoueurRepertoire> unitesJoueurReserve = new List<UnitesJoueurRepertoire>();
    [HideInInspector] public GameObject uniteAttentePrefab;


    //Indice de l'unité améliorable
    [HideInInspector] public GameObject[] boutonsUnitesToUpgrade;
    [HideInInspector] public int indUniteCanUpgrade;


    //Pour le mortier
    [HideInInspector] public List<GameObject> listeMunitions;
    private int coutFerMortier = 1;
    [HideInInspector] public GameObject mortierViseur;          //Viseur du mortier où est placé le script MortierViseur


    //Fonction constructeur de BatimentConstructions. Attribution des valeurs propres à la classe et destruction des composants du prefab inutiles.
    public BatimentConstructions(Sprite _spriteBaseObjet, string _NomObjet, bool _IsInteractable, int _IndicePanelInterractBatiment)
    {
        ConstructeursBatiment(_spriteBaseObjet, _NomObjet, _IsInteractable);
        Destroy(nbTextRessources_GO);
        Destroy(healthBarMax_GO);
        Destroy(batimentConstruit);
        Destroy(imageFlagJoueur);
        if (_NomObjet != "CampMilitaire")
        {
            Destroy(spritesUnitesFiles);
        } else
        {
            if (!levelManager.isModeDebug)
            {
                nbUniteRecrutable = 1;
            } else
            {
                nbUniteRecrutable = levelManager.uniteRecrutableCaserne;
            }
            //Mettre Sprite des Unités
            uniteAttentePrefab = spritesUnitesFiles.transform.GetChild(0).gameObject;
            uniteAttentePrefab.transform.parent = null;
            uniteAttentePrefab.SetActive(false);

            CaserneCreationSoldat();

        }

        indicePanelInterractBatiment = _IndicePanelInterractBatiment;           //Donne le code du batiment pour ouvrir le bon panel, 0 : Tours, 1: ChantierBTP, 2 : Caserne Unité
        panelInteractText = levelManager.panelParentBatimentInterract.GetChild(indicePanelInterractBatiment).GetChild(0).GetComponent<Text>();      //Texte du panel d'interaction avec le batiment
        if (_IndicePanelInterractBatiment == 0)      //Pour les Tours
        {
            if (levelManager.nbTours <= 4)
            {
                nbBoutonInterractionsPossible = levelManager.nbTours;
            } else
            {
                nbBoutonInterractionsPossible = 4;
            }
        } else if (_IndicePanelInterractBatiment == 1)      // Pour les BTP
        {
            if (levelManager.repertoireSprites.listeBatiments.Length < 4)
            {
                nbBoutonInterractionsPossible = levelManager.repertoireSprites.listeBatiments.Length;
            }
            else if (levelManager.repertoireSprites.listeBatiments.Length >= 4)
            {
                nbBoutonInterractionsPossible = 4;
            }
        } else if (_IndicePanelInterractBatiment == 2)
        {
            nbBoutonInterractionsPossible = 2;
        }

        _InterractionsDisponibles = new GameObject[nbBoutonInterractionsPossible];    //Nombre de boutons du panel


    }


    //Création des soldats et mise en place à la Caserne
    public void CaserneCreationSoldat()
    {
        int currentII = unitesJoueurReserve.Count;
        int nbUniteMaxLine = 5;
        int line = currentII / nbUniteMaxLine;
        for (int ii = currentII; ii < nbUniteRecrutable; ii++)
        {
            unitesJoueurReserve.Add(levelManager.repertoireSprites.unitesJoueurData[Random.RandomRange(0, levelManager.repertoireSprites.unitesJoueurData.Length)]);
            GameObject uniteReserviste = Instantiate(uniteAttentePrefab, spritesUnitesFiles.transform);
            uniteReserviste.SetActive(true);
            uniteReserviste.transform.localScale = 0.4f * Vector3.one;
            Vector3 positionAttente = new Vector3(-2 + (ii - line * nbUniteMaxLine), 1 - line, 0);  //-2 : position arbitraire, à régler
            if (positionAttente.x > 2)
            {
                positionAttente.x = -2;
                line++;
            }
            positionAttente.y = 1 - line;

            uniteReserviste.transform.localPosition = positionAttente;
            spritesUnitesAttente.Add(uniteReserviste.GetComponent<SpriteRenderer>());
            spritesUnitesAttente[ii].sprite = unitesJoueurReserve[ii].uniteSpriteBase[0];   //Sprite dans la file d'attente de la caserne
        }
    }




    //Creation des Boutons du panel pour chaque type de batiment en fonction des données du répertoire des Sprites  
    public void CreationBouton()
    {
        for (int ii = 0; ii < _InterractionsDisponibles.Length; ii++)
        {
            _InterractionsDisponibles[ii] = levelManager.panelParentBatimentInterract.GetChild(indicePanelInterractBatiment).GetChild(1).GetChild(ii).gameObject;
            if (transformObjet.name == "ChantierTours")
            {
                _InterractionsDisponibles[ii].GetComponent<Image>().sprite = repertoireSprite.toursData[ii].tourSprite;
                _InterractionsDisponibles[ii].transform.GetChild(0).GetComponent<Text>().text = repertoireSprite.toursData[ii].tourNom;
                _InterractionsDisponibles[ii].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = repertoireSprite.toursData[ii].tourPrixBle.ToString();
                _InterractionsDisponibles[ii].transform.GetChild(2).GetChild(0).GetComponent<Text>().text = repertoireSprite.toursData[ii].tourPrixBois.ToString();
                _InterractionsDisponibles[ii].transform.GetChild(3).GetChild(0).GetComponent<Text>().text = repertoireSprite.toursData[ii].tourPrixFer.ToString();
                _InterractionsDisponibles[ii].transform.GetChild(4).GetChild(0).GetComponent<Text>().text = repertoireSprite.toursData[ii].tourPrixPierre.ToString();
            } else if (transformObjet.name == "ChantierBTP")
            {
                _InterractionsDisponibles[ii].SetActive(true);
                _InterractionsDisponibles[ii].GetComponent<Image>().sprite = repertoireSprite.batimentsData[ii].batimentSprite;
                _InterractionsDisponibles[ii].transform.GetChild(0).GetComponent<Text>().text = repertoireSprite.batimentsData[ii].batimentNom;
                _InterractionsDisponibles[ii].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = repertoireSprite.batimentsData[ii].batimentPrixBle.ToString();
                _InterractionsDisponibles[ii].transform.GetChild(2).GetChild(0).GetComponent<Text>().text = repertoireSprite.batimentsData[ii].batimentPrixBois.ToString();
                _InterractionsDisponibles[ii].transform.GetChild(3).GetChild(0).GetComponent<Text>().text = repertoireSprite.batimentsData[ii].batimentPrixFer.ToString();
                _InterractionsDisponibles[ii].transform.GetChild(4).GetChild(0).GetComponent<Text>().text = repertoireSprite.batimentsData[ii].batimentPrixPierre.ToString();
            } else if (transformObjet.name == "CampMilitaire")
            {
                //Bouton 1 : Recruter une unité
                //_InterractionsDisponibles[ii].GetComponent<Image>().sprite = repertoireSprite.unitesData[ii].uniteSpriteBase[levelManager._JoueurActif-1];
                //_InterractionsDisponibles[ii].transform.GetChild(0).GetComponent<Text>().text = repertoireSprite.unitesJoueurData[ii].uniteNom;
                if (ii == 0)
                {
                    _InterractionsDisponibles[ii].transform.GetChild(0).GetComponent<Text>().text = "Recruter unité";
                    _InterractionsDisponibles[ii].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "1";
                } else if (ii == 1)
                {
                    _InterractionsDisponibles[ii].transform.GetChild(0).GetComponent<Text>().text = "Améliorer unité";
                    _InterractionsDisponibles[ii].GetComponent<Button>().interactable = false;
                    _InterractionsDisponibles[ii].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "2";

                    //Création des boutons pour choisir l'unité à améliorer si plusieurs sont améliorables
                    boutonsUnitesToUpgrade = new GameObject[levelManager.reserveTypePrefabUnitesJoueur.Length];
                    for (int jj = 0; jj < levelManager.reserveTypePrefabUnitesJoueur.Length; jj++)
                    {
                        GameObject boutonUnitesToUpload = new GameObject();
                        boutonUnitesToUpload.AddComponent<RectTransform>();
                        boutonUnitesToUpload.AddComponent<Button>();
                        boutonUnitesToUpload.AddComponent<Image>();
                        boutonUnitesToUpload.transform.parent = _InterractionsDisponibles[ii].transform;
                        boutonUnitesToUpload.GetComponent<RectTransform>().localScale = Vector3.one;
                        Vector3 newPos = Vector3.one * jj * 200;
                        newPos.y = 0;
                        newPos.z = 0;
                        boutonUnitesToUpload.GetComponent<RectTransform>().anchoredPosition3D = newPos;
                        boutonUnitesToUpload.GetComponent<Button>().targetGraphic = boutonUnitesToUpload.GetComponent<Image>();
                        //boutonUnitesToUpload.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,1);
                        boutonUnitesToUpload.name = "BoutonAmelioreUnite_Type" + (jj + 1).ToString();

                        //boutonUnitesToUpload.GetComponent<Button>().onClick.AddListener(delegate { AmeliorerUnite(); });

                        boutonsUnitesToUpgrade[jj] = boutonUnitesToUpload;
                    }
                }
                //_InterractionsDisponibles[ii].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = repertoireSprite.unitesJoueurData[ii].unitePrixBle.ToString();
            }

        }
    }



    //Selection batiment à construire en fonction du panel ouvert et du type de batiment sélectionné
    public void SelectionCreationConstruire()
    {

        if (levelManager.myEventSystem.currentSelectedGameObject != null)
        {
            boutonSelect = levelManager.myEventSystem.currentSelectedGameObject.transform.GetSiblingIndex();        //récupère le bouton sélectionné
        }

        if (transformObjet.name == "ChantierTours")
        {
            SelectionTourConstruction();
        } else if (transformObjet.name == "ChantierBTP")
        {
            SelectionBatimentConstruction();
        } else if (transformObjet.name == "CampMilitaire")
        {
            SelectionUniteConstruction();
        }


        //Si on constuit une tour ou un batiment, on change de panel et on doit sélectionner l'emplacement de construction
        if (isConstructionPossible)
        {
            panelInteractText.text = "Où le mettre ?";      //Changement du texte
            levelManager.panelParentBatimentInterract.GetChild(indicePanelInterractBatiment).GetChild(1).gameObject.SetActive(false);   //Desactivation des boutons
            //Les boutons des emplacements disponibles et libres pour la construcitons sont mis en avant
            for (int ii = 0; ii < _EmplacementsCreations.Length; ii++)              //Pour tous les emplacements de construction relatif au type de batiment, on active l'interactabilité du bouton et on passe l'ordre de rendu de la sprite devant le panel
            {
                if (!_EmplacementsCreations[ii].isConstruit)
                {
                    _EmplacementsCreations[ii].buttonObjet.enabled = true;
                    _EmplacementsCreations[ii].boutonGOObjet.transform.parent.GetComponent<Canvas>().sortingOrder = 4;
                }
            }
        }
    }


    //Sélectionne la tour à construire
    public void SelectionTourConstruction()
    {
        tourConstruite = repertoireSprite.toursData[boutonSelect];      //Vérifie quelle tour on veut construire
        isConstructionPossible = false;         //Initialisation de la vérification des ressources du joueurs
        //-1 pour l'indice tableau
        //Si le joueur a les ressources nécessaires, on peut construire, sinon non
        if (levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[0] >= tourConstruite.tourPrixBle &&
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[1] >= tourConstruite.tourPrixBois &&
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[2] >= tourConstruite.tourPrixFer &&
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[3] >= tourConstruite.tourPrixPierre)
        {
            isConstructionPossible = true;
        }
        else
        {
            isConstructionPossible = false;
            levelManager.PopUpsFonction("Pas assez de ressources !!", levelManager.textPopUps);
        }



    }


    //Sélectionne le batiment à construire
    public void SelectionBatimentConstruction()
    {
        batimentBuild = repertoireSprite.batimentsData[boutonSelect];             //Vérifie quel batiment on veut construire
        isConstructionPossible = false;   //Initialisation de la vérification des ressources du joueurs
        //-1 pour l'indice tableau
        //Si le joueur a les ressources nécessaires, on peut construire et donc choisir l'emplacement, sinon non
        if (levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[0] >= batimentBuild.batimentPrixBle &&
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[1] >= batimentBuild.batimentPrixBois &&
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[2] >= batimentBuild.batimentPrixFer &&
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[3] >= batimentBuild.batimentPrixPierre)
        {
            if (batimentBuild.batimentNom != "Autel")
            {
                isConstructionPossible = true;
            } else
            {
                if (levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._NombreAutels < batimentBuild.nombreBatimentMaxParJoueur)
                {
                    isConstructionPossible = true;
                } else
                {
                    isConstructionPossible = false;
                    levelManager.PopUpsFonction("Nombre d'autel maximum construits !!", levelManager.textPopUps);
                }
            }
        }
        else
        {
            isConstructionPossible = false;
            levelManager.PopUpsFonction("Pas assez de ressources !!", levelManager.textPopUps);
        }
    }



    //Recrutement des unités classiques
    public void RecrutementUnites(int _RecrutementMax)
    {
        for (int ii = 0; ii < _RecrutementMax; ii++)
        {
            uniteEngaged = unitesJoueurReserve[0];
            GameObject monUnite = levelManager.ActivationObjectListe(levelManager.prefabUnitesJoueur, uniteEngaged.uniteListeOrdre);        //On active dans la liste l'unité choisie
            monUnite.transform.GetComponent<SpriteRenderer>().sprite = uniteEngaged.uniteSpriteBase[levelManager._JoueurActif - 1];   //On donne la sprite relative au joueur actif
            monUnite.transform.GetComponent<Animator>().runtimeAnimatorController = uniteEngaged.uniteAnimatorController[levelManager._JoueurActif - 1];        //On donne le bon Animator Controller
            monUnite.GetComponent<CallBacksUnitesJoueur>().monUniteJoueur.uniteJoueurOwner = levelManager._JoueurActif - 1;


            //Si Fortin, amélioration des stats
            if (levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._HasFortin)
            {
                monUnite.GetComponent<CallBacksUnitesJoueur>().monUniteJoueur.UniteJoueurAmeliorationStatistiques(monUnite.GetComponent<CallBacksUnitesJoueur>());
                monUnite.GetComponent<CallBacksUnitesJoueur>().monUniteJoueur.pv = monUnite.GetComponent<CallBacksUnitesJoueur>().monUniteJoueur.pvMax;
                monUnite.GetComponent<CallBacksUnitesJoueur>().monUniteJoueur.HealthBar_MaJ();
            }


            unitesJoueurReserve.Remove(unitesJoueurReserve[0]);
            spritesUnitesAttente.Remove(spritesUnitesAttente[0]);
            Destroy(spritesUnitesFiles.transform.GetChild(ii).gameObject);
            //Incrémente le compte de soldat d'un même type
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._NbUnites[uniteEngaged.uniteListeOrdre]++;
        }

        //Paiement des ressources
        levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[0] -= _RecrutementMax;     //On paie le prix en ressources
        for (int ii = 0; ii < levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes.Length; ii++)
        {
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1].AfficheNbRessources(ii);     //Affichage nombre ressources
        }

        //Mise à jours nb unités recrutables
        nbUniteRecrutable -= _RecrutementMax;

        //Changer le tour de jeu
        ChangeJoueurActif();
        levelManager.panelParentBatimentInterract.gameObject.SetActive(false);      //Fermeture du panel
    }


    public void RecrutementUnitesSpeciales(Transform reserveUniteSpeciale)
    {
        int indiceUniteSpeciale = FindUniteSpecialeIndiceRepertoire(reserveUniteSpeciale.GetChild(0).name);
        uniteEngaged = levelManager.repertoireSprites.unitesSpecialesData[indiceUniteSpeciale];
        bool canRecruter = true;
        
        //Si on est dans la cave et que l'on recrute un assassin, tester si le joueur qui veut recruter n'a pas atteint le nombre d'assassins maximum requis (possiblement, faire la même chose avec un autre type d'unité
        if(uniteEngaged.uniteNom == "Assassin")
        {
            if(levelManager.tableauJoueurs[levelManager._JoueurActif-1]._NbAssassinPossede >= levelManager.NbAssassinMaxPerJoueur)
            {
                canRecruter = false;
            }
        }

        if (canRecruter)
        {
            GameObject monUnite = levelManager.ActivationObjectListe(reserveUniteSpeciale);
            monUnite.transform.GetComponent<Animator>().runtimeAnimatorController = uniteEngaged.uniteAnimatorController[levelManager._JoueurActif - 1];        //On donne le bon Animator Controller
            monUnite.GetComponent<CallBacksUnitesJoueur>().monUniteJoueur.uniteJoueurOwner = levelManager._JoueurActif - 1;

            //Paiement des ressources
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[0] -= uniteEngaged.unitePrixBle;     //On paie le prix en ressources
            for (int ii = 0; ii < levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes.Length; ii++)
            {
                levelManager.tableauJoueurs[levelManager._JoueurActif - 1].AfficheNbRessources(ii);     //Affichage nombre ressources
            }

            //Instanciation d'une nouvelle unité dans la réserve si aucune n'est désactivée
            //Teste si une unité est désactivée
            bool needNewInstance = true;
            for (int ii = 0; ii < reserveUniteSpeciale.childCount; ii++)
            {
                if (!reserveUniteSpeciale.GetChild(ii).gameObject.activeInHierarchy)
                {
                    needNewInstance = false;
                    break;
                }
            }
            if (needNewInstance)
            {
                int way = Random.RandomRange(0, 3);
                levelManager.InstanciationUnites_AffectationReserve(reserveUniteSpeciale, indiceUniteSpeciale, way, levelManager.repertoireSprites.unitesSpecialesData);
            }


            //Si on recrute un assassin, on augmente le nombre d'assassins possédés par le joueur
            if(uniteEngaged.uniteNom == "Assassin")
            {
                levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._NbAssassinPossede++;
            }

        } else
        {
            levelManager.PopUpsFonction("Impossible de recruter cette unité", levelManager.textPopUps);
        }

    }



    public void SelectionUniteConstruction()
    {
        //Si on a autant ou plus de blé que le nombre d'unités recrutable dans la caserne, on recrute toutes les unités
        if (nbUniteRecrutable > 0)
        {
            if (levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[0] >= nbUniteRecrutable)
            {
                //Recrutement du nombre d'unités possibles
                RecrutementUnites(nbUniteRecrutable);
            }
            //Si on a moins de blé, on en recrute autant qu'on peut
            else if (levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[0] < nbUniteRecrutable && levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[0] > 0)
            {
                //Recrutement du nombre d'unités possibles
                RecrutementUnites(levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[0]);
            }
            else if (levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[0] == 0)
            {
                levelManager.PopUpsFonction("Pas assez de ressources !!", levelManager.textPopUps);
            }
        }
        else if (nbUniteRecrutable == 0)
        {
            levelManager.PopUpsFonction("Pas de soldats disponibles !!", levelManager.textPopUps);
        }

    }




    public void AmeliorerUnite()
    {
        if (levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[2] >= levelManager.repertoireSprites.superUnitesJoueurData[indUniteCanUpgrade].unitePrixFer)
        {

            //Récupérer l'indice de l'unité améliorable indUniteCanUpgrade
            indUniteCanUpgrade = levelManager.myEventSystem.currentSelectedGameObject.transform.GetSiblingIndex() - 2;


            //Accéder à la réserve en question
            //Desactiver uniteMinToUpgrade GO dans la réserve d'indice indUniteCanUpgrade. Les unités doivent appartenir au joueur
            int uniteToDesactive = 0;

            for (int ii = 0; ii < levelManager.reserveTypePrefabUnitesJoueur[indUniteCanUpgrade].childCount; ii++)
            {
                if (levelManager.reserveTypePrefabUnitesJoueur[indUniteCanUpgrade].GetChild(ii).gameObject.activeInHierarchy && levelManager.reserveTypePrefabUnitesJoueur[indUniteCanUpgrade].GetChild(ii).GetComponent<CallBacksUnitesJoueur>().monUniteJoueur.uniteJoueurOwner == levelManager._JoueurActif - 1)
                {

                    levelManager.reserveTypePrefabUnitesJoueur[indUniteCanUpgrade].GetChild(ii).gameObject.SetActive(false);
                    uniteToDesactive++;
                    if (uniteToDesactive >= uniteMinToUpgrade)
                    {
                        levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._NbUnites[indUniteCanUpgrade] -= uniteMinToUpgrade;
                        break;
                    }
                }
            }


            GameObject maSuperUnite = levelManager.ActivationObjectListe(levelManager.prefabSuperUnitesJoueur, indUniteCanUpgrade);
            //Sprite de l'Unité
            maSuperUnite.transform.GetComponent<SpriteRenderer>().sprite = levelManager.repertoireSprites.superUnitesJoueurData[indUniteCanUpgrade].uniteSpriteBase[levelManager._JoueurActif - 1];   //On donne la sprite relative au joueur actif
            maSuperUnite.transform.GetComponent<Animator>().runtimeAnimatorController = levelManager.repertoireSprites.superUnitesJoueurData[indUniteCanUpgrade].uniteAnimatorController[levelManager._JoueurActif - 1];        //On donne le bon Animator Controller
            maSuperUnite.transform.GetComponent<CallBacksSuperUnitesJoueur>().maSuperUniteJoueur.uniteJoueurOwner = levelManager._JoueurActif - 1;

            //Si Fortin, amélioration des stats
            if (levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._HasFortin)
            {
                maSuperUnite.GetComponent<CallBacksSuperUnitesJoueur>().maSuperUniteJoueur.SuperUniteJoueurAmeliorationStatistiques(maSuperUnite.GetComponent<CallBacksSuperUnitesJoueur>());
                maSuperUnite.GetComponent<CallBacksSuperUnitesJoueur>().maSuperUniteJoueur.pv = maSuperUnite.GetComponent<CallBacksSuperUnitesJoueur>().maSuperUniteJoueur.pvMax;
                maSuperUnite.GetComponent<CallBacksSuperUnitesJoueur>().maSuperUniteJoueur.HealthBar_MaJ();
            }


            //Désactive le panel
            levelManager.panelParentBatimentInterract.gameObject.SetActive(false);      //Fermeture du panel

            //Paye les ressources
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[2] -= levelManager.repertoireSprites.superUnitesJoueurData[indUniteCanUpgrade].unitePrixFer;     //On paie le prix en ressources
            for (int ii = 0; ii < levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes.Length; ii++)
            {
                levelManager.tableauJoueurs[levelManager._JoueurActif - 1].AfficheNbRessources(ii);     //Affichage nombre ressources
            }

            for (int jj = 0; jj < levelManager.reserveTypePrefabUnitesJoueur.Length; jj++)
            {
                boutonsUnitesToUpgrade[jj].SetActive(false);
            }

            //Changer le tour de jeu
            ChangeJoueurActif();
        }
        else
        {
            levelManager.PopUpsFonction("Pas assez de ressources !!", levelManager.textPopUps);
        }

    }


    public void ActiveAmeliorationUnite()
    {
        for (int jj = 0; jj < levelManager.reserveTypePrefabUnitesJoueur.Length; jj++)
        {
            boutonsUnitesToUpgrade[jj].SetActive(true);
        }
    }



    //Fonctions spécifiques aux bâtiments spéciaux
    public void FonctionBatimentsSpeciauxGenerale()
    {
        //On peut changer le joueur actif
        canChangeActiveJoueur = true;

        //Cette fonction est affectée à tous les bâtiments construits !
        string nameBatimentConstruit = levelManager.myEventSystem.currentSelectedGameObject.transform.parent.parent.name;
        if (nameBatimentConstruit == "Hopital")
        {
            BatimentHopitalFonction();
        } else if (nameBatimentConstruit == "Hutte" || nameBatimentConstruit == "Cave")
        {
            BatimentUniteSpecialeFonction();
        } else if(nameBatimentConstruit == "Mortier")
        {
            BatimentMortierFonction();
        } else if(nameBatimentConstruit == "Market")
        {
            BatimentMarketFonction();
        }
        //Change le joueur actif;
        if (canChangeActiveJoueur)
        {
            ChangeJoueurActif();
        }
    }

    public void BatimentHopitalFonction()
    {
        Debug.Log("Soigne les unités du joueur qui l'invoque");  //On accède directement à chaque réserve d'unités

        for (int ii = 0; ii < levelManager.reservePrefabUnitesJoueur.childCount; ii++)
        {
            for (int jj = 0; jj < levelManager.reserveTypePrefabUnitesJoueur[ii].childCount; jj++)
            {
                if (levelManager.reserveTypePrefabUnitesJoueur[ii].GetChild(jj).gameObject.activeSelf && levelManager.reserveTypePrefabUnitesJoueur[ii].GetChild(jj).GetComponent<CallBacksUnitesJoueur>().monUniteJoueur.uniteJoueurOwner == levelManager._JoueurActif)
                {
                    levelManager.reserveTypePrefabUnitesJoueur[ii].GetChild(jj).GetComponent<CallBacksUnitesJoueur>().monUniteJoueur.pv = levelManager.reserveTypePrefabUnitesJoueur[ii].GetChild(jj).GetComponent<CallBacksUnitesJoueur>().monUniteJoueur.pvMax;
                    levelManager.reserveTypePrefabUnitesJoueur[ii].GetChild(jj).GetComponent<CallBacksUnitesJoueur>().monUniteJoueur.HealthBar_MaJ();
                }
            }
        }

        for(int ii= 0; ii < levelManager.reservePrefabSuperUnitesJoueur.childCount; ii++)
        {
            for(int jj = 0; jj < levelManager.reserveTypePrefabSuperUnitesJoueur[ii].childCount; jj++)
            {
                if(levelManager.reserveTypePrefabSuperUnitesJoueur[ii].GetChild(jj).gameObject.activeSelf && levelManager.reserveTypePrefabSuperUnitesJoueur[ii].GetChild(jj).GetComponent<CallBacksSuperUnitesJoueur>().maSuperUniteJoueur.uniteJoueurOwner == levelManager._JoueurActif)
                {
                    levelManager.reserveTypePrefabSuperUnitesJoueur[ii].GetChild(jj).GetComponent<CallBacksSuperUnitesJoueur>().maSuperUniteJoueur.pv = levelManager.reserveTypePrefabSuperUnitesJoueur[ii].GetChild(jj).GetComponent<CallBacksSuperUnitesJoueur>().maSuperUniteJoueur.pvMax;
                    levelManager.reserveTypePrefabSuperUnitesJoueur[ii].GetChild(jj).GetComponent<CallBacksSuperUnitesJoueur>().maSuperUniteJoueur.HealthBar_MaJ();
                }
            }
        }

        Debug.Log("Ajouter soin des unités spéciales");
    }

    public void BatimentUniteSpecialeFonction()
    {
        RecrutementUnitesSpeciales(levelManager.myEventSystem.currentSelectedGameObject.transform.parent.parent.GetChild(1));
    }




    public void BatimentUniteSpecialesRecrutementInitiatilisation(GameObject _GO_Batiment,string _NomReserve, string _NomUniteSpeciale)
    {
        GameObject reserveBatimentPrefab = new GameObject(_NomReserve);
        reserveBatimentPrefab.transform.parent = _GO_Batiment.transform;
        reserveBatimentPrefab.transform.localScale = Vector3.one;
        reserveBatimentPrefab.transform.localPosition = Vector3.zero;

        int way = Random.RandomRange(0, 3);
        int indiceUniteSpeciale = FindUniteSpecialeIndiceRepertoire(_NomUniteSpeciale);

        levelManager.InstanciationUnites_AffectationReserve(reserveBatimentPrefab.transform, indiceUniteSpeciale, way, levelManager.repertoireSprites.unitesSpecialesData);       //0 : correspond aux barbares
    }


    int FindUniteSpecialeIndiceRepertoire(string _NomUniteSpeciale)
    {
        int choix = 0;
        for (int ii = 0; ii < levelManager.repertoireSprites.listeUniteSpecialesObject.Length; ii++)
        {
            if (_NomUniteSpeciale == levelManager.repertoireSprites.listeUniteSpecialesObject[ii].name)
            {
                choix = ii;
            }
        }

        return choix;
    }

    

    public void BatimentMaisonFonction()
    {
        levelManager.isMaisonConstruite = true;
        levelManager.myEventSystem.currentSelectedGameObject.transform.parent.parent.GetChild(1).GetChild(0).GetChild(0).GetComponent<Button>().enabled = false;
        Debug.Log("Plus de militaires en attente au camp");
    }


    public void BatimentAutelFonction()
    {
        levelManager.myEventSystem.currentSelectedGameObject.transform.parent.parent.GetChild(1).GetChild(0).GetChild(0).GetComponent<Button>().enabled = false;
        levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._NombreAutels++;
        Debug.Log("Afficher le nombre d'autels dans l'interface");
    }


    public void BatimentFosseFonction()
    {
        levelManager.myEventSystem.currentSelectedGameObject.transform.parent.parent.GetChild(1).GetChild(0).GetChild(0).GetComponent<Button>().enabled = false;
        levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._HasFosse = true;
        Debug.Log("Afficher que l'on possède la Fosse dans l'interface");
    }


    public void BatimentMonumentAuxMortsFonction()
    {
        levelManager.myEventSystem.currentSelectedGameObject.transform.parent.parent.GetChild(1).GetChild(0).GetChild(0).GetComponent<Button>().enabled = false;
        levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._HasMonumentAuxMorts = true;
        Debug.Log("Afficher que l'on possède le Monument aux Morts dans l'interface");
    }


    public void BatimentCabaneEclaireurFonction()
    {
        levelManager.myEventSystem.currentSelectedGameObject.transform.parent.parent.GetChild(1).GetChild(0).GetChild(0).GetComponent<Button>().enabled = false;
        levelManager.isCabaneEclaireur = true;
        Debug.Log("Afficher que l'on possède la Cabane de l'éclaireur dans l'interface");
    }


    public void BatimentTenteSoinsInitialisation()
    {
        levelManager.myEventSystem.currentSelectedGameObject.transform.parent.parent.GetChild(1).GetChild(0).GetChild(0).GetComponent<Button>().enabled = false;
        levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._HasTenteSoins = true;
        Debug.Log("Afficher que l'on possède la Tente de Soins dans l'interface");
    }


    public void BatimentTenteSoinsFonction(int _JoueurInd)
    {
        GameObject[] unites = GameObject.FindGameObjectsWithTag("UniteJoueur");
        GameObject[] superUnites = GameObject.FindGameObjectsWithTag("SuperUniteJoueur");

        List<GameObject> listeUnites = new List<GameObject>();
        if(unites.Length != 0)
        {
            for(int ii = 0; ii < unites.Length; ii++)
            {
                if(unites[ii].GetComponent<CallBacksUnitesJoueur>().monUniteJoueur.uniteJoueurOwner == _JoueurInd)
                {
                    listeUnites.Add(unites[ii]);
                }
            }
        }

        if(superUnites.Length != 0)
        {
            for(int ii = 0; ii < superUnites.Length; ii++)
            {
                if(superUnites[ii].GetComponent<CallBacksSuperUnitesJoueur>().maSuperUniteJoueur.uniteJoueurOwner == _JoueurInd)
                {
                    listeUnites.Add(superUnites[ii]);
                }
            }
        }

        int indUniteSoigner = Random.RandomRange(0, listeUnites.Count);
        if (listeUnites[indUniteSoigner].CompareTag("UniteJoueur"))
        {
            listeUnites[indUniteSoigner].GetComponent<CallBacksUnitesJoueur>().monUniteJoueur.pv = listeUnites[indUniteSoigner].GetComponent<CallBacksUnitesJoueur>().monUniteJoueur.pvMax;
            listeUnites[indUniteSoigner].GetComponent<CallBacksUnitesJoueur>().monUniteJoueur.HealthBar_MaJ();
        } else if (listeUnites[indUniteSoigner].CompareTag("SuperUniteJoueur"))
        {
            listeUnites[indUniteSoigner].GetComponent<CallBacksSuperUnitesJoueur>().maSuperUniteJoueur.pv = listeUnites[indUniteSoigner].GetComponent<CallBacksSuperUnitesJoueur>().maSuperUniteJoueur.pvMax;
            listeUnites[indUniteSoigner].GetComponent<CallBacksSuperUnitesJoueur>().maSuperUniteJoueur.HealthBar_MaJ();
        }

        /*for(int ii = 0; ii < listeUnites.Count; ii++)
        {
            Debug.Log(listeUnites[ii].name);
        }*/

    }


    public void BatimentFortinCreationFonction()
    {
        #region Amélioration des Unités déjà en place
        for (int ii = 0; ii < levelManager.reserveTypePrefabUnitesJoueur.Length; ii++)
        {
            for (int jj = 0; jj < levelManager.reserveTypePrefabUnitesJoueur[ii].childCount; jj++)
            {
                if (levelManager.reserveTypePrefabUnitesJoueur[ii].GetChild(jj).gameObject.activeInHierarchy && levelManager.reserveTypePrefabUnitesJoueur[ii].GetChild(jj).GetComponent<CallBacksUnitesJoueur>().monUniteJoueur.uniteJoueurOwner == levelManager._JoueurActif - 1)
                {
                    levelManager.reserveTypePrefabUnitesJoueur[ii].GetChild(jj).GetComponent<CallBacksUnitesJoueur>().monUniteJoueur.UniteJoueurAmeliorationStatistiques(levelManager.reserveTypePrefabUnitesJoueur[ii].GetChild(jj).GetComponent<CallBacksUnitesJoueur>());
                }
            }
        }
        #endregion

        #region Amélioration SuperUnités déjà en place
        for (int ii = 0; ii < levelManager.reserveTypePrefabSuperUnitesJoueur.Length; ii++)
        {
            for (int jj = 0; jj < levelManager.reserveTypePrefabSuperUnitesJoueur[ii].childCount; jj++)
            {
                if (levelManager.reserveTypePrefabSuperUnitesJoueur[ii].GetChild(jj).gameObject.activeInHierarchy && levelManager.reserveTypePrefabSuperUnitesJoueur[ii].GetChild(jj).GetComponent<CallBacksSuperUnitesJoueur>().maSuperUniteJoueur.uniteJoueurOwner == levelManager._JoueurActif - 1)
                {
                    levelManager.reserveTypePrefabSuperUnitesJoueur[ii].GetChild(jj).GetComponent<CallBacksSuperUnitesJoueur>().maSuperUniteJoueur.SuperUniteJoueurAmeliorationStatistiques(levelManager.reserveTypePrefabSuperUnitesJoueur[ii].GetChild(jj).GetComponent<CallBacksSuperUnitesJoueur>());
                }
            }
        }
        #endregion

        levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._HasFortin = true;

        levelManager.myEventSystem.currentSelectedGameObject.transform.parent.parent.GetChild(1).GetChild(0).GetChild(0).GetComponent<Button>().enabled = false;
    }


    public void BatimentEcoleDeMagieFonction()
    {
        //Amélioration des tours déjà en place
        GameObject[] placementTours = GameObject.FindGameObjectsWithTag("EmplacementTour");
        int nbToursEnPlace = placementTours.Length;
        for(int ii = 0; ii < nbToursEnPlace; ii++)
        {
            if(placementTours[ii].transform.GetChild(1).gameObject.activeSelf && placementTours[ii].transform.GetChild(1).GetComponent<Tours>().joueurOwner == levelManager._JoueurActif - 1)
            {
                placementTours[ii].transform.GetChild(1).GetComponent<Tours>().AmeliorationToursStatistiques();
            }
            
        }

        //Le joueur acquiert l'école de magie
        levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._HasEcoleDeMagie = true;

        levelManager.myEventSystem.currentSelectedGameObject.transform.parent.parent.GetChild(1).GetChild(0).GetChild(0).GetComponent<Button>().enabled = false;
    }


    public void BatimentMortierInitialisation()
    {
        mortierViseur = new GameObject("MortierViseur");
        mortierViseur.SetActive(false);
        mortierViseur.transform.parent = levelManager.myEventSystem.currentSelectedGameObject.transform.parent.parent.GetChild(1);
        mortierViseur.AddComponent<MortierBatiment>();
        mortierViseur.GetComponent<MortierBatiment>().monBatiment = this;
        mortierViseur.transform.position = Vector3.zero;
        mortierViseur.transform.localScale = Vector3.one;

        listeMunitions = new List<GameObject>();
        for(int ii = 0; ii < 2; ii++)
        {
            GameObject munitionPrefab = (GameObject)Instantiate(Resources.Load("Prefab/MunitionPrefab"));
            munitionPrefab.transform.parent = levelManager.myEventSystem.currentSelectedGameObject.transform.parent.parent.GetChild(1).transform;
            munitionPrefab.transform.localPosition = Vector3.zero;
            munitionPrefab.transform.localScale = new Vector3(0.66f, 1, 1);
            Destroy(munitionPrefab.GetComponent<CircleCollider2D>());
            munitionPrefab.AddComponent<CapsuleCollider2D>();
            munitionPrefab.GetComponent<CapsuleCollider2D>().direction = CapsuleDirection2D.Horizontal;
            munitionPrefab.GetComponent<CapsuleCollider2D>().enabled = false;
            listeMunitions.Add(munitionPrefab);
            munitionPrefab.SetActive(false);
        }
    }

    public void BatimentMortierFonction()
    {
        canChangeActiveJoueur = false;

        //Tester si le joueur a les sous ! (1 de fer)        
        if (levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[2] >= coutFerMortier)
        {
            //Le faire payer !!
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[2] -= coutFerMortier;
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1].AfficheNbRessources(2);

            levelManager.buttonSelectedTemp = levelManager.myEventSystem.currentSelectedGameObject.transform.parent.parent.gameObject;


            //Choisi la position de tir
            mortierViseur.SetActive(true);
            //mortierViseur.GetComponent<CircleCollider2D>().enabled = false;

     
        }
        else
        {
            levelManager.PopUpsFonction("Pas assez de ressources !!", levelManager.textPopUps);
        }


    }

    public IEnumerator MortierChoixTir()
    {
        bool tirChoisi = false;
        float _Amplitude = 2f;
        while (!tirChoisi)
        {
            yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
            if (Input.GetMouseButtonDown(0))
            {
                tirChoisi = true;
                int indListeMunition = 0;

                //Activer la munition
                for (int ii = 0; ii < listeMunitions.Count; ii++)
                {
                    if (!listeMunitions[ii].activeInHierarchy)
                    {
                        listeMunitions[ii].SetActive(true);
                        indListeMunition = ii;
                        break;
                    }
                }

                //Lancer le tir
                Vector3 targetMortierPosition = levelManager.maMainCamera.ScreenToWorldPoint(Input.mousePosition);
                targetMortierPosition.z = 0;
                Vector3 directionMortier = targetMortierPosition - listeMunitions[indListeMunition].transform.position;
                Vector3 scale0 = listeMunitions[indListeMunition].transform.localScale;
                Vector3 position0 = listeMunitions[indListeMunition].transform.position;


                #region Tir du Mortier
                int nbStepTir = 10;
                float dx = (targetMortierPosition.x - listeMunitions[indListeMunition].transform.position.x) / ((float)nbStepTir + 1);
                float dy1 = _Amplitude / ((float)nbStepTir / 2);
                float dy2 = ((listeMunitions[indListeMunition].transform.position.y + (dy1 * ((float)nbStepTir / 2))) - targetMortierPosition.y) / ((float)nbStepTir / 2);

                int nbTir = 0;
                while (nbTir <= nbStepTir)
                {
                    float newPosY = listeMunitions[indListeMunition].transform.position.y;
                    if (nbTir < nbStepTir / 2)
                    {
                        //Phase montée
                        //Debug.Log("Phase 1, "+nbTir);
                        newPosY += dy1;
                    }
                    else if (nbTir == nbStepTir / 2)
                    {
                        //Phase plane
                        //Debug.Log("Phase 2, " + nbTir);
                        newPosY = newPosY;
                    }
                    else if (nbTir > nbStepTir / 2 && nbTir <= nbStepTir)
                    {
                        //Phase descente
                        //Debug.Log("Phase 3, " + nbTir);
                        newPosY -= dy2;
                    }
                    listeMunitions[indListeMunition].transform.position = new Vector3(listeMunitions[indListeMunition].transform.position.x + dx, newPosY, listeMunitions[indListeMunition].transform.position.z);
                    nbTir++;
                    yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime * 10);
                }
                yield return new WaitForSeconds(FonctionsVariablesUtiles.deltaTime);
                listeMunitions[indListeMunition].GetComponent<CapsuleCollider2D>().enabled = true;
                listeMunitions[indListeMunition].transform.localScale = new Vector3(1.5f, 1, 1);
                yield return new WaitForSeconds(1.5f);
                listeMunitions[indListeMunition].transform.localScale = scale0;
                listeMunitions[indListeMunition].transform.position = position0;
                listeMunitions[indListeMunition].SetActive(false);
                #endregion





                //listeMunitions[indListeMunition].transform.position = targetMortierPosition;

            }
            if (tirChoisi)
            {
                mortierViseur.SetActive(false);
                break;
            }
        }

        //Changer le joueur actif
        ChangeJoueurActif();
    }



    public void BatimentMarketInitialisation()
    {
        Debug.Log("Afficher que l'on possède le Market dans l'interface");
    }


    public void VendreRessourcesMarket()
    {
        levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._VictoryPoints += levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[levelManager.myEventSystem.currentSelectedGameObject.transform.GetSiblingIndex()];
        levelManager.tableauJoueurs[levelManager._JoueurActif - 1].AffichePointVictoireSolidarite(levelManager.tableauJoueurs[levelManager._JoueurActif - 1].victoirePointText, levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._VictoryPoints);

        levelManager.tableauJoueurs[levelManager._JoueurActif-1]._RessourcesPossedes[levelManager.myEventSystem.currentSelectedGameObject.transform.GetSiblingIndex()] = 0;
        levelManager.tableauJoueurs[levelManager._JoueurActif-1].AfficheNbRessources(levelManager.myEventSystem.currentSelectedGameObject.transform.GetSiblingIndex());
        levelManager.panelParentBatimentInterract.gameObject.SetActive(false);
        levelManager.panelParentBatimentInterract.GetChild(4).gameObject.SetActive(false);
        ChangeJoueurActif();
    }

    public void BatimentMarketFonction()
    {
        canChangeActiveJoueur = false;
        levelManager.panelParentBatimentInterract.gameObject.SetActive(true);
        for(int ii = 0; ii < levelManager.panelParentBatimentInterract.childCount - 1; ii++)
        {
            levelManager.panelParentBatimentInterract.GetChild(ii).gameObject.SetActive(false);
        }
        levelManager.panelParentBatimentInterract.GetChild(4).gameObject.SetActive(true);
    }

}
