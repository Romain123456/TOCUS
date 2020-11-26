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
        /*accesColor[0] = new Color(1, 1, 1);
        accesColor[1] = new Color(0, 1, 0);
        accesColor[2] = new Color(0, 0, 1);*/
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
            batimentConstruit.SetActive(true);              //Activation du GameObject du batiment construit, enfant de l'emplacement
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
                ChangeJoueurActif();
            }
        }
    }



    //Construction de Tour
    public void ConstructionDeTour()
    {
        batimentConstruit.GetComponent<SpriteRenderer>().sprite = batimentGestion.tourConstruite.tourSprite;        //La sprite de la tour est donnée depuis la tour choisie par le joueur
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

        if (levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._HasEcoleDeMagie)
        {
            nouvelleTourConstruite.AmeliorationToursStatistiques();
        }


        //Paiement des ressources
        levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[0] -= batimentGestion.tourConstruite.tourPrixBle;
        levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[1] -= batimentGestion.tourConstruite.tourPrixBois;
        levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[2] -= batimentGestion.tourConstruite.tourPrixFer;
        levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes[3] -= batimentGestion.tourConstruite.tourPrixPierre;
        //Affichage du nouveau nombre de ressources par le joueur
        for (int ii = 0; ii < levelManager.tableauJoueurs[levelManager._JoueurActif - 1]._RessourcesPossedes.Length; ii++)
        {
            levelManager.tableauJoueurs[levelManager._JoueurActif - 1].AfficheNbRessources(ii);
        }
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
        } else if(batimentGestion.batimentBuild.batimentNom == "Mortier")
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
        }




        batimentConstruit.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = batimentGestion.batimentBuild.batimentSprite;          //La sprite du batiment est donnée depuis la tour choisie par le joueur
        batimentConstruit.transform.name = batimentGestion.batimentBuild.batimentNom;


        //Echelle du batiment
        //batimentConstruit.transform.localScale = batimentGestion.batimentBuild.scaleBatiment;
        batimentConstruit.transform.localScale = Vector3.one;


        //Fonction du batiment construit
        batimentGestion.levelManager.AttributeFonctionBatiment(batimentConstruit.transform.GetChild(0).GetChild(0).GetComponent<Button>());


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
    }

}
