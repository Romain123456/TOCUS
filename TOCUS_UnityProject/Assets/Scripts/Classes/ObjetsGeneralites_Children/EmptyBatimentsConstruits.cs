using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Classe EmptyBatimentsConstruits héritant de ObjetsGeneralites
public class EmptyBatimentsConstruits : ObjetsGeneralites
{
    //Modificateur accès de l'emplacement. Est-ce que les deux joueurs y ont accès ou seulement un des 2 et lequel ?
    public int modifAccesBat;
    //private Color[] accesColor = new Color[3];
    public BatimentConstructions batimentGestion;                       //Type de batiment qui construit sur cet emplacement

    public bool isConstruit;         //Le placement est construit!

    //Fonction constructeur de la classe EmptyBatimentsConstuits, attribuant les valeurs spécifiques de la classe et détruit les composants inutiles du préfab
    public EmptyBatimentsConstruits(Sprite _spriteBaseObjet, string _NomObjet,bool _IsInteractable,string _monTag)
    {
        ConstructeursBatiment(_spriteBaseObjet, _NomObjet,_IsInteractable);
        Destroy(nbTextRessources_GO);
        Destroy(healthBarMax_GO);
        Destroy(spritesUnitesFiles);
        transformObjet.tag = _monTag;               //Tag de l'objet
        batimentConstruit.SetActive(false);         //Désactivation du bâtiment Construit
        if (transformObjet.CompareTag("EmplacementBTP"))
        {
            Destroy(batimentConstruit.GetComponent<SpriteRenderer>());
            batimentConstruit.transform.GetChild(0).GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            batimentConstruit.transform.GetChild(0).GetComponent<Canvas>().sortingOrder = 1;
            batimentConstruit.transform.GetChild(0).localScale = Vector3.one;
        } else if (transformObjet.CompareTag("EmplacementTour"))
        {
            Destroy(batimentConstruit.transform.GetChild(0).gameObject);
        }
    }


    //Affiche un drapeau indiquant à quel joueur appartient l'emplacement. Si les deux joueurs y ont accès, le composant est détruit
    public void FlagBatimentAcces()
    {
        if(modifAccesBat == 0)
        {
            Destroy(imageFlagJoueur);
        } else if(modifAccesBat == 1)
        {
            imageFlagJoueur.GetComponent<Image>().sprite = repertoireSprite.flagJ1;
        } else if(modifAccesBat == 2)
        {
            imageFlagJoueur.GetComponent<Image>().sprite = repertoireSprite.flagJ2;
        }
        //imageObjet.color = accesColor[modifAccesBat];
    }



    //Construction du batiment lorsque le joueur a choisi le batiment qu'il veut construire et s'il a les ressources nécessaires
    public void OnConstructionBatiment()
    {
        bool canConstruct = false;      //Déclaration d'une variable temporaire pour savoir si on peut construire

        //Vérification du modificateur d'accès pour savoir si le joueur a accès à cet emplacement. Si le joueur y a accès, on peut construire à cet emplacement
        if(modifAccesBat == 0 || modifAccesBat == levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._NumeroJoueur)
        {
            canConstruct = true;
        } else
        {
            canConstruct = false;
            levelManager.PopUpsFonction("Pas accès à cet emplacement",levelManager.textPopUps);
        }

        //Si on peut construire, construction
        if (canConstruct)
        {
            //batimentConstruit.SetActive(true);              //Activation du GameObject du batiment construit, enfant de l'emplacement
            Debug.Log("Ligne à supprimer puisque gérée ensuite dans l'animation");
            if (batimentConstruit.GetComponent<SpriteRenderer>() != null)
            {
                batimentConstruit.GetComponent<SpriteRenderer>().sortingOrder = 1;              //L'ordre de rendu de la sprite du batiment est passé au dessus de celle de l'emplacement
            }

            for (int ii = 0; ii < batimentGestion._EmplacementsCreations.Length; ii++)      //Pour tous les emplacements de construction relatif au type de batiment, on désactive l'interactabilité du bouton et on passe l'ordre de rendu de la sprite sous le panel
            {
                batimentGestion._EmplacementsCreations[ii].buttonObjet.enabled = false;
                batimentGestion._EmplacementsCreations[ii].boutonGOObjet.transform.parent.GetComponent<Canvas>().sortingOrder = 0;
            }
            batimentGestion.levelManager.panelParentBatimentInterract.gameObject.SetActive(false);          //On désactive le panel d'interaction
            isConstruit = true;         //On dit que cet emplacement a été construit

            //Remettre le panel à 0
            batimentGestion.levelManager.panelParentBatimentInterract.GetChild(batimentGestion.indicePanelInterractBatiment).GetChild(1).gameObject.SetActive(true);

            //En fonction du type de chantier de construction, on construit une tour ou un batiment
            if (batimentGestion.nomObjet == "ChantierTours")
            {
                ConstructionDeTour();
            } else if(batimentGestion.nomObjet == "ChantierBTP")
            {
                ConstructionDeBatiment();
            }

            //Changer le tour de jeu
            if (batimentGestion.nomObjet != "ChantierTours")
            {
                //Tout ce qui est requis pour l'animation
                AnimationAchatConstructionUtilitaire(batimentConstruit, batimentGestion.batimentBuild);
                ChangeJoueurActif();
            }
        }
    }


    public void AnimationAchatConstructionUtilitaire(GameObject maConstruction,BatimentsRepertoire monBatiment)         //Cas Batiments
    {
        maConstruction.SetActive(false);
        maConstruction.GetComponent<Animation>().enabled = true;
        levelManager.objetConstruit.Add(maConstruction);
        levelManager.nbRessourcesRecup = monBatiment.batimentPrixBle + monBatiment.batimentPrixBois + monBatiment.batimentPrixFer + monBatiment.batimentPrixPierre;
        levelManager.scaleConstruction = monBatiment.scaleBatiment;

        //Index des sprites
        levelManager.indexSpriteRessources = new int[levelManager.nbRessourcesRecup];
        for (int ii = 0; ii < monBatiment.batimentPrixBle; ii++)
        {
            levelManager.indexSpriteRessources[ii] = 0;
        }
        int blePbois = monBatiment.batimentPrixBle + monBatiment.batimentPrixBois;
        for (int ii = monBatiment.batimentPrixBle; ii < blePbois; ii++)
        {
            levelManager.indexSpriteRessources[ii] = 1;
        }
        int blePboisPfer = monBatiment.batimentPrixBle + monBatiment.batimentPrixBois + monBatiment.batimentPrixFer;
        for (int ii = blePbois; ii < blePboisPfer; ii++)
        {
            levelManager.indexSpriteRessources[ii] = 2;
        }
        for (int ii = blePboisPfer; ii < levelManager.nbRessourcesRecup; ii++)
        {
            levelManager.indexSpriteRessources[ii] = 3;
        }
    }


    public void AnimationAchatConstructionUtilitaire(GameObject maConstruction, ToursRepertoire monBatiment)            //Cas Tours
    {
        int ind0prixTourTableau = batimentGestion.RecupIndiceNbTourParJoueur();
        int prixBle = levelManager.prixToursParToursJoueur[ind0prixTourTableau, 0];
        int prixBois = levelManager.prixToursParToursJoueur[ind0prixTourTableau, 1];
        int prixFer = levelManager.prixToursParToursJoueur[ind0prixTourTableau, 2];
        int prixPierre = levelManager.prixToursParToursJoueur[ind0prixTourTableau, 3];

        levelManager.objetConstruit.Add(maConstruction);
        levelManager.nbRessourcesRecup = prixBle + prixBois + prixFer + prixPierre;
        levelManager.scaleConstruction = monBatiment.scaleTour;

        //Index des sprites
        levelManager.indexSpriteRessources = new int[levelManager.nbRessourcesRecup];
        for (int ii = 0; ii < prixBle; ii++)
        {
            levelManager.indexSpriteRessources[ii] = 0;
        }
        int blePbois = prixBle +prixBois;
        for (int ii = prixBle; ii < blePbois; ii++)
        {
            levelManager.indexSpriteRessources[ii] = 1;
        }
        int blePboisPfer = prixBle + prixBois + prixFer;
        for (int ii = blePbois; ii < blePboisPfer; ii++)
        {
            levelManager.indexSpriteRessources[ii] = 2;
        }
        for (int ii = blePboisPfer; ii < levelManager.nbRessourcesRecup; ii++)
        {
            levelManager.indexSpriteRessources[ii] = 3;
        }
    }





    //Construction de Tour
    public void ConstructionDeTour()
    {
        batimentConstruit.GetComponent<SpriteRenderer>().sprite = batimentGestion.tourConstruite.tourSprite;        //La sprite de la tour est donnée depuis la tour choisie par le joueur
        batimentConstruit.GetComponent<SpriteRenderer>().color = new Color(0.1f, 0.1f, 0.1f);       //La sprite est girsée
        batimentConstruit.SetActive(true);

        batimentConstruit.transform.name = batimentGestion.tourConstruite.tourNom;
        batimentConstruit.transform.localPosition = batimentGestion.tourConstruite.positionTour;
        batimentConstruit.transform.localScale = batimentGestion.tourConstruite.scaleTour;
        batimentConstruit.AddComponent<Tours>();            //On ajoute un script Tours à la tour;
        Tours nouvelleTourConstruite = batimentConstruit.GetComponent<Tours>();
        nouvelleTourConstruite._FormeVisee = batimentGestion.tourConstruite.tourTypeVisee.ToString();

        /*nouvelleTourConstruite.offsetColliderVisee = batimentGestion.tourConstruite.offsetColliderVisee;        //Offset affecté à la tour
        nouvelleTourConstruite.sizesColliderVisee = batimentGestion.tourConstruite.sizesColliderVisee;      //Sizes collider affectée à la tour
        */
        //nouvelleTourConstruite.viseeSprite = batimentGestion.tourConstruite.viseeSprite;        //Visée affectée à la tour
        nouvelleTourConstruite._TypeCadence = batimentGestion.tourConstruite.tourTypeCadence;    //Cadence affectée à la tour
        nouvelleTourConstruite._TypePuissance = batimentGestion.tourConstruite.tourTypePuissance;    //Puissance affectée à la tour
        nouvelleTourConstruite._TypePortee = batimentGestion.tourConstruite.tourTypePortee.ToString();
        nouvelleTourConstruite.joueurOwner = levelManager._JoueurActif - 1;
        nouvelleTourConstruite._DureeDegats = batimentGestion.tourConstruite.tourTypeDureeDegats;
        nouvelleTourConstruite._LargeurViseeTir = batimentGestion.tourConstruite.tourTypeLargeurVisee;
        nouvelleTourConstruite._TypeImpact = batimentGestion.tourConstruite.tourTypeImpact;

        if (levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._HasEcoleDeMagie)
        {
            nouvelleTourConstruite.AmeliorationToursStatistiques();
        }


        //Paiement des ressources
        //Etablissement du prix de la tour en fonction du nombre possédé par le joueur
        int ind0prixTourTableau = batimentGestion.RecupIndiceNbTourParJoueur();

        for (int ii = 0; ii < levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes.Length; ii++)
        {
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[ii] -= levelManager.prixToursParToursJoueur[ind0prixTourTableau, ii];
        }

        if(levelManager.tableauJoueurs[levelManager._JoueurActif - 1].isGrue)
        {
            int ressourceGift = Random.Range(0, 4);
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[ressourceGift]++;
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1].AfficheNbRessources(ressourceGift);
        }


        //Affichage du nouveau nombre de ressources par le joueur
        for (int ii = 0; ii < levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes.Length; ii++)
        {
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1].AfficheNbRessources(ii);
        }
        levelManager.tableauJoueurs[levelManager._JoueurActif - 1].nbToursPossedees++;      //Incrémentation du nombre de tours possédées
        batimentGestion.panelInteractText.text = "Tour à construire ?";     //Remise du texte du panel à son ancienne valeur

        //Retire de la liste la tour construite
        //Désactive le bouton si il reste moins de 4 tours à construire
        //Remplace la tour construite par la prochaine sur la liste (CreationBouton)
        #region Reorganisation Boutons et listes
        if (repertoireSprite.toursData.Count > 4)
        {
            repertoireSprite.toursData.Insert(batimentGestion.boutonSelect, repertoireSprite.toursData[4]);
            repertoireSprite.toursData.Remove(repertoireSprite.toursData[5]);
        } 
        repertoireSprite.toursData.Remove(repertoireSprite.toursData[batimentGestion.boutonSelect]);

        for (int ii = 0; ii < batimentGestion._InterractionsDisponibles.Length; ii++)
        {
            batimentGestion._InterractionsDisponibles[ii].GetComponent<Button>().onClick.AddListener(delegate { batimentGestion.SelectionCreationConstruire(); });
        }

        //Desactivation du dernier bouton si il reste moins de 4 tours
        if (repertoireSprite.toursData.Count <= 3)
        {
            batimentGestion.nbBoutonInterractionsPossible--;
            batimentGestion._InterractionsDisponibles[repertoireSprite.toursData.Count].SetActive(false);
        }
        batimentGestion._InterractionsDisponibles = new GameObject[batimentGestion.nbBoutonInterractionsPossible];
        batimentGestion.CreationBouton();
        #endregion

        AnimationAchatConstructionUtilitaire(batimentConstruit, batimentGestion.tourConstruite);

        //Apparition du curseur pour la rotation de la tour
        levelManager._GuizmoRotationTour.SetActive(true);
        levelManager._GuizmoRotationTour.transform.position = transformObjet.position;
        levelManager._GuizmoRotationTour.GetComponent<RotationTourPlace>().tourToRotate = nouvelleTourConstruite.transform;
    }


    public void ConstructionDeBatiment()
    {
        //Si on a construit un bâtiment de compétence passive, on y applique la fonction
        if(batimentGestion.batimentBuild.batimentNom == "Maison")
        {
            batimentGestion.BatimentMaisonFonction();
        } else if(batimentGestion.batimentBuild.batimentNom == "Autel")
        {
            batimentGestion.BatimentAutelFonction();
        } else if(batimentGestion.batimentBuild.batimentNom == "Fortin")
        {
            batimentGestion.BatimentFortinCreationFonction();
        } else if(batimentGestion.batimentBuild.batimentNom == "Ecole de Magie")
        {
            batimentGestion.BatimentEcoleDeMagieFonction();
        } else if(batimentGestion.batimentBuild.batimentNom == "Hutte")
        {
            batimentGestion.BatimentUniteSpecialesRecrutementInitiatilisation(batimentConstruit.gameObject,"ReserveBarbare","Barbare");
        } else if ( batimentGestion.batimentBuild.batimentNom == "Cave")
        {
            batimentGestion.BatimentUniteSpecialesRecrutementInitiatilisation(batimentConstruit.gameObject, "ReserveAssassin", "Assassin");
        }else if(batimentGestion.batimentBuild.batimentNom == "Auberge De Bagarreur")
        {
            batimentGestion.BatimentUniteSpecialesRecrutementInitiatilisation(batimentConstruit.gameObject, "ReserveBagarreur", "Bagarreur");
        }
        else if(batimentGestion.batimentBuild.batimentNom == "Mortier")
        {
            batimentGestion.BatimentMortierInitialisation();
        } else if(batimentGestion.batimentBuild.batimentNom == "Fosse")
        {
            batimentGestion.BatimentFosseFonction();
        } else if(batimentGestion.batimentBuild.batimentNom == "Monuments Aux Morts")
        {
            batimentGestion.BatimentMonumentAuxMortsFonction();
        } else if(batimentGestion.batimentBuild.batimentNom == "Tente de Soins")
        {
            batimentGestion.BatimentTenteSoinsInitialisation();
        } else if (batimentGestion.batimentBuild.batimentNom == "Cabane Eclaireur")
        {
            batimentGestion.BatimentCabaneEclaireurFonction();
        } else if(batimentGestion.batimentBuild.batimentNom == "Market")
        {
            batimentGestion.BatimentMarketInitialisation();
        } else if(batimentGestion.batimentBuild.batimentNom == "Portail Demoniaque")
        {
            batimentGestion.BatimentPortailDemoniaqueFonction();
        } else if (batimentGestion.batimentBuild.batimentNom == "Hache De Guerre")
        {
            batimentGestion.BatimentHacheDeGuerreFonction();
        } else if(batimentGestion.batimentBuild.batimentNom == "Camp Entrainement")
        {
            batimentGestion.BatimentCampEntrainementFonction();
        } else if(batimentGestion.batimentBuild.batimentNom == "Moulin" || batimentGestion.batimentBuild.batimentNom == "Stock De Pierre" || batimentGestion.batimentBuild.batimentNom == "Stock De Bois" || batimentGestion.batimentBuild.batimentNom == "Fourneau")
        {
            batimentGestion.BatimentUpgradeRecolteFonction();
        } else if(batimentGestion.batimentBuild.batimentNom == "Feu De Camp")
        {
            batimentGestion.BatimentFeuDeCampFonction();
        } else if(batimentGestion.batimentBuild.batimentNom == "Palissade")
        {
            batimentGestion.BatimentPalissadeFonction();
        } else if(batimentGestion.batimentBuild.batimentNom == "Entrepot")
        {
            batimentGestion.BatimentEntrepotFonction();
        } else if(batimentGestion.batimentBuild.batimentNom == "Grue")
        {
            batimentGestion.BatimentGrueFonction();
        }

        if (levelManager.tableauJoueurs[levelManager._JoueurActif - 1].isEntrepot && batimentGestion.batimentBuild.batimentNom != "Entrepot")
        {
            int ressourceGift = Random.Range(0, 4);
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[ressourceGift]++;
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1].AfficheNbRessources(ressourceGift);
        }

        Debug.Log("Batiment Construit");

        batimentConstruit.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = batimentGestion.batimentBuild.batimentSprite;          //La sprite du batiment est donnée depuis la tour choisie par le joueur
        batimentConstruit.transform.name = batimentGestion.batimentBuild.batimentNom;

        //Fonction du batiment construit
        batimentGestion.levelManager.AttributeFonctionBatiment(batimentConstruit.transform.GetChild(0).GetChild(0).GetComponent<Button>());


        //Si le batiment est public, on ajoute des points de solidarité
        if (batimentGestion.batimentBuild.isBatimentPublic)
        {
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._SolidarityPoints += JsonParametresGlobaux.ficParamGlobaux.objet_divers.o_divers.i_gain_solidarite_par_batiment_public;
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1].AffichePointVictoireSolidarite(levelManager.tableauJoueurs[levelManager._JoueurActif - 1].solidarityPointText, levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._SolidarityPoints);
        }

        //Paiment des ressources
        levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[0] -= batimentGestion.batimentBuild.batimentPrixBle;
        levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[1] -= batimentGestion.batimentBuild.batimentPrixBois;
        levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[2] -= batimentGestion.batimentBuild.batimentPrixFer;
        levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[3] -= batimentGestion.batimentBuild.batimentPrixPierre;
        //Affichage du nouveau nombre de ressources par le joueur
        for (int ii = 0; ii < levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes.Length; ii++)
        {
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1].AfficheNbRessources(ii);
        }
        batimentGestion.panelInteractText.text = "Batiment à construire ?";    //Remise du texte du panel à son ancienne valeur

        //Retire de la liste le bâtiment construit
        //Désactive le bouton si il reste moins de 4 tours à construire
        //Remplace le batiment construit par la prochaine sur la liste (CreationBouton)
        #region Reorganisation Boutons et listes
        repertoireSprite.batimentsData.Remove(repertoireSprite.batimentsData[batimentGestion.boutonSelect]);

        for (int ii = 0; ii < batimentGestion._InterractionsDisponibles.Length; ii++)
        {
            batimentGestion._InterractionsDisponibles[ii].GetComponent<Button>().onClick.AddListener(delegate { batimentGestion.SelectionCreationConstruire(); });
        }

        if (repertoireSprite.batimentsData.Count <= 3)
        {
            batimentGestion.nbBoutonInterractionsPossible--;
            batimentGestion._InterractionsDisponibles[repertoireSprite.batimentsData.Count].SetActive(false);
        }
        batimentGestion._InterractionsDisponibles = new GameObject[batimentGestion.nbBoutonInterractionsPossible];
        batimentGestion.CreationBouton();
        #endregion
    }



    //Valide le choix de la rotation de la Tour. Static pour y accéder depuis LevelManager sans instance + attribuer au bouton adequat
    public void ValidChoixRotationTour()
    {
        levelManager._GuizmoRotationTour.SetActive(false);
        ChangeJoueurActif();
        Debug.Log("Faire animation Changement de joueur");
    }

}
