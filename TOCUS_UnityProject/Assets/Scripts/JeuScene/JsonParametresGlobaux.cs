using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class JsonParametresGlobaux : MonoBehaviour
{
    [HideInInspector] public string ficJsonParamGlobaux;
    [HideInInspector] public string pathJsonParamGlobaux;

    static public JsonParamGlobaux ficParamGlobaux;

    // Start is called before the first frame update
    void Awake()
    {
        ficJsonParamGlobaux = "parametres_globaux_1.json";
        pathJsonParamGlobaux = Application.streamingAssetsPath + "/" + ficJsonParamGlobaux;
        ficParamGlobaux = new JsonParamGlobaux();
        ficParamGlobaux = ficParamGlobaux.OuvertureJsonFileParamGlobaux(pathJsonParamGlobaux);

        ficParamGlobaux.objet_tours_sniper.o_tours_sniper.arr_ameliorations2D = Make2DTableau(ficParamGlobaux.objet_tours_sniper.o_tours_sniper.arr_ameliorations, ficParamGlobaux.objet_tours_sniper.o_tours_sniper.arr_ameliorations2D, 2, 3);
        ficParamGlobaux.objet_tours_mitraillette.o_tours_mitraillette.arr_ameliorations2D =  Make2DTableau(ficParamGlobaux.objet_tours_mitraillette.o_tours_mitraillette.arr_ameliorations, ficParamGlobaux.objet_tours_mitraillette.o_tours_mitraillette.arr_ameliorations2D, 2, 3);
        ficParamGlobaux.objet_tours_shotgun.o_tours_shotgun.arr_ameliorations2D = Make2DTableau(ficParamGlobaux.objet_tours_shotgun.o_tours_shotgun.arr_ameliorations, ficParamGlobaux.objet_tours_shotgun.o_tours_shotgun.arr_ameliorations2D, 2, 3);
        ficParamGlobaux.objet_tours_canon.o_tours_canon.arr_ameliorations2D = Make2DTableau(ficParamGlobaux.objet_tours_canon.o_tours_canon.arr_ameliorations, ficParamGlobaux.objet_tours_canon.o_tours_canon.arr_ameliorations2D, 2, 3);
        ficParamGlobaux.objet_tours_poison.o_tours_poison.arr_ameliorations2D = Make2DTableau(ficParamGlobaux.objet_tours_poison.o_tours_poison.arr_ameliorations, ficParamGlobaux.objet_tours_poison.o_tours_poison.arr_ameliorations2D, 2, 3);
        ficParamGlobaux.objet_tours_feu.o_tours_feu.arr_ameliorations2D = Make2DTableau(ficParamGlobaux.objet_tours_feu.o_tours_feu.arr_ameliorations, ficParamGlobaux.objet_tours_feu.o_tours_feu.arr_ameliorations2D, 2, 3);
        ficParamGlobaux.objet_tours_electrique.o_tours_electrique.arr_ameliorations2D = Make2DTableau(ficParamGlobaux.objet_tours_electrique.o_tours_electrique.arr_ameliorations, ficParamGlobaux.objet_tours_electrique.o_tours_electrique.arr_ameliorations2D, 2, 3);
        ficParamGlobaux.objet_tours_laser.o_tours_laser.arr_ameliorations2D = Make2DTableau(ficParamGlobaux.objet_tours_laser.o_tours_laser.arr_ameliorations, ficParamGlobaux.objet_tours_laser.o_tours_laser.arr_ameliorations2D, 2, 3);
        ficParamGlobaux.objet_tours_tromblon.o_tours_tromblon.arr_ameliorations2D = Make2DTableau(ficParamGlobaux.objet_tours_tromblon.o_tours_tromblon.arr_ameliorations, ficParamGlobaux.objet_tours_tromblon.o_tours_tromblon.arr_ameliorations2D, 2, 3);
        ficParamGlobaux.objet_cout_achat_tours_selon_nombre.o_cout_achat_tours_selon_nombre.arr_cout_achat_tour_joueur_possede_2D = Make2DTableau(ficParamGlobaux.objet_cout_achat_tours_selon_nombre.o_cout_achat_tours_selon_nombre.arr_cout_achat_tour_joueur_possede, ficParamGlobaux.objet_cout_achat_tours_selon_nombre.o_cout_achat_tours_selon_nombre.arr_cout_achat_tour_joueur_possede_2D, 8, 4);
    }

    public string[,] Make2DTableau(string[] monTableau1D,string[,] monTableau2D,int nbLines,int nbCols)
    {
        monTableau2D = new string[nbLines, nbCols];
        int kk = 0;
        for(int ii = 0; ii < nbLines; ii++)
        {
            for(int jj = 0; jj < nbCols; jj++)
            {
                monTableau2D[ii, jj] = monTableau1D[kk];
                kk++;
            }
        }
        return monTableau2D;
    }

    public int[,] Make2DTableau(int[] monTableau1D, int[,] monTableau2D, int nbLines, int nbCols)
    {
        monTableau2D = new int[nbLines, nbCols];
        int kk = 0;
        for (int ii = 0; ii < nbLines; ii++)
        {
            for (int jj = 0; jj < nbCols; jj++)
            {
                monTableau2D[ii, jj] = monTableau1D[kk];
                kk++;
            }
        }
        return monTableau2D;
    }

}


public class JsonParamGlobaux
{
    public Objet_Divers objet_divers;
    public Objet_cout_achat_tours_selon_nombre objet_cout_achat_tours_selon_nombre;
    public Objet_tir_mortier objet_tir_mortier;
    public Objet_batiments objet_batiments;
    public Objet_tours_valeurs_textuelles_portee objet_tours_valeurs_textuelles_portee;
    public Objet_monstres_valeurs_textuelles_deplacement objet_monstres_valeurs_textuelles_deplacement;
    public Objet_tours_valeurs_textuelles_largeur_visee objet_tours_valeurs_textuelles_largeur_visee;
    public Objet_tours_valeurs_textuelles_cadence objet_tours_valeurs_textuelles_cadence;
    public Objet_tours_valeurs_textuelles_puissance objet_tours_valeurs_textuelles_puissance;
    public Objet_tours_valeurs_textuelles_duree_degats objet_tours_valeurs_textuelles_duree_degats;
    public Objet_tours_sniper objet_tours_sniper;
    public Objet_tours_mitraillette objet_tours_mitraillette;
    public Objet_tours_shotgun objet_tours_shotgun;
    public Objet_tours_canon objet_tours_canon;
    public Objet_tours_poison objet_tours_poison;
    public Objet_tours_feu objet_tours_feu;
    public Objet_tours_electrique objet_tours_electrique;
    public Objet_tours_laser objet_tours_laser;
    public Objet_tours_tromblon objet_tours_tromblon;
    public Objet_unites_monstres_valeurs_textuelles_vitalite objet_unites_monstres_valeurs_textuelles_vitalite;
    public Objet_unites_monstres_valeurs_textuelles_armure objet_unites_monstres_valeurs_textuelles_armure;
    public Objet_unites_monstres_valeurs_textuelles_initiative objet_unites_monstres_valeurs_textuelles_initiative;
    public Objet_unites_monstres_valeurs_textuelles_degats objet_unites_monstres_valeurs_textuelles_degats;
    public Objet_unites_monstres_valeurs_textuelles_portee objet_unites_monstres_valeurs_textuelles_portee;
    public Objet_unites_soldat objet_unites_soldat;
    public Objet_unites_chevalier objet_unites_chevalier;
    public Objet_unites_assassin objet_unites_assassin;
    public Objet_unites_bagarreur objet_unites_bagarreur;
    public Objet_unites_barbare objet_unites_barbare;
    public Objet_unites_lancier objet_unites_lancier;
    public Objet_unites_archer objet_unites_archer;
    public Objet_unites_mage objet_unites_mage;
    public Objet_monstres_basique objet_monstres_basique;
    public Objet_monstres_costaud objet_monstres_costaud;
    public Objet_monstres_boxeur objet_monstres_boxeur;

    public JsonParamGlobaux OuvertureJsonFileParamGlobaux (string _Path)
    {
        JsonParamGlobaux fileJson;
        string jSonString = File.ReadAllText(_Path);
        fileJson = JsonUtility.FromJson<JsonParamGlobaux>(jSonString);
        fileJson.objet_divers = JsonUtility.FromJson<Objet_Divers>(jSonString);
        fileJson.objet_cout_achat_tours_selon_nombre = JsonUtility.FromJson<Objet_cout_achat_tours_selon_nombre>(jSonString);
        fileJson.objet_tir_mortier = JsonUtility.FromJson<Objet_tir_mortier>(jSonString);
        fileJson.objet_batiments = JsonUtility.FromJson<Objet_batiments>(jSonString);
        fileJson.objet_tours_valeurs_textuelles_portee = JsonUtility.FromJson<Objet_tours_valeurs_textuelles_portee>(jSonString);
        fileJson.objet_monstres_valeurs_textuelles_deplacement = JsonUtility.FromJson<Objet_monstres_valeurs_textuelles_deplacement>(jSonString);
        fileJson.objet_tours_valeurs_textuelles_largeur_visee = JsonUtility.FromJson<Objet_tours_valeurs_textuelles_largeur_visee>(jSonString);
        fileJson.objet_tours_valeurs_textuelles_cadence = JsonUtility.FromJson<Objet_tours_valeurs_textuelles_cadence>(jSonString);
        fileJson.objet_tours_valeurs_textuelles_puissance = JsonUtility.FromJson<Objet_tours_valeurs_textuelles_puissance>(jSonString);
        fileJson.objet_tours_valeurs_textuelles_duree_degats = JsonUtility.FromJson<Objet_tours_valeurs_textuelles_duree_degats>(jSonString);
        fileJson.objet_tours_sniper = JsonUtility.FromJson<Objet_tours_sniper>(jSonString);
        fileJson.objet_tours_mitraillette = JsonUtility.FromJson<Objet_tours_mitraillette>(jSonString);
        fileJson.objet_tours_shotgun = JsonUtility.FromJson<Objet_tours_shotgun>(jSonString);
        fileJson.objet_tours_canon = JsonUtility.FromJson<Objet_tours_canon>(jSonString);
        fileJson.objet_tours_poison = JsonUtility.FromJson<Objet_tours_poison>(jSonString);
        fileJson.objet_tours_feu = JsonUtility.FromJson<Objet_tours_feu>(jSonString);
        fileJson.objet_tours_electrique = JsonUtility.FromJson<Objet_tours_electrique>(jSonString);
        fileJson.objet_tours_laser = JsonUtility.FromJson<Objet_tours_laser>(jSonString);
        fileJson.objet_tours_tromblon = JsonUtility.FromJson<Objet_tours_tromblon>(jSonString);
        fileJson.objet_unites_monstres_valeurs_textuelles_vitalite = JsonUtility.FromJson<Objet_unites_monstres_valeurs_textuelles_vitalite>(jSonString);
        fileJson.objet_unites_monstres_valeurs_textuelles_armure = JsonUtility.FromJson<Objet_unites_monstres_valeurs_textuelles_armure>(jSonString);
        fileJson.objet_unites_monstres_valeurs_textuelles_initiative = JsonUtility.FromJson<Objet_unites_monstres_valeurs_textuelles_initiative>(jSonString);
        fileJson.objet_unites_monstres_valeurs_textuelles_degats = JsonUtility.FromJson<Objet_unites_monstres_valeurs_textuelles_degats>(jSonString);
        fileJson.objet_unites_monstres_valeurs_textuelles_portee = JsonUtility.FromJson<Objet_unites_monstres_valeurs_textuelles_portee>(jSonString);
        fileJson.objet_unites_soldat = JsonUtility.FromJson<Objet_unites_soldat>(jSonString);
        fileJson.objet_unites_chevalier = JsonUtility.FromJson<Objet_unites_chevalier>(jSonString);
        fileJson.objet_unites_assassin = JsonUtility.FromJson<Objet_unites_assassin>(jSonString);
        fileJson.objet_unites_bagarreur = JsonUtility.FromJson<Objet_unites_bagarreur>(jSonString);
        fileJson.objet_unites_barbare = JsonUtility.FromJson<Objet_unites_barbare>(jSonString);
        fileJson.objet_unites_lancier = JsonUtility.FromJson<Objet_unites_lancier>(jSonString);
        fileJson.objet_unites_archer = JsonUtility.FromJson<Objet_unites_archer>(jSonString);
        fileJson.objet_unites_mage = JsonUtility.FromJson<Objet_unites_mage>(jSonString);
        fileJson.objet_monstres_basique = JsonUtility.FromJson<Objet_monstres_basique>(jSonString);
        fileJson.objet_monstres_costaud = JsonUtility.FromJson<Objet_monstres_costaud>(jSonString);
        fileJson.objet_monstres_boxeur = JsonUtility.FromJson<Objet_monstres_boxeur>(jSonString);

        return fileJson;
    }
}

#region o_divers
[Serializable]
public class Objet_Divers
{
    public O_Divers o_divers;
}

[Serializable]
public class O_Divers
{
    public int i_vitalite_porte;
    public int[] arr_cout_reparation_porte;
    public int i_gain_solidarite_reparation_porte;
    public int i_gain_pv_reparation_porte;

    public int i_gain_solidarite_par_batiment_public;

    public int i_gain_victoire_par_ennemi_mort;
    public int i_gain_victoire_autel;
    public int i_gain_victoire_monument_aux_morts;
    public int i_gain_victoire_de_la_fosse;


    public int[] arr_cout_unite_a_la_caserne;
    public int[] arr_cout_creation_super_soldat;
    public int i_nb_assassin_max_par_joueur;

    public float f_temps_entre_hit_degats_tours;
}
#endregion


#region o_cout_achat_tours_selon_nombre
[Serializable]
public class Objet_cout_achat_tours_selon_nombre
{
    public O_cout_achat_tours_selon_nombre o_cout_achat_tours_selon_nombre;
}


[Serializable]
public class O_cout_achat_tours_selon_nombre
{
    public int[] arr_cout_achat_tour_joueur_possede;
    public int[,] arr_cout_achat_tour_joueur_possede_2D;
}
#endregion


#region o_tir_mortier
[Serializable]
public class Objet_tir_mortier
{
    public O_tir_mortier o_tir_mortier;
}

[Serializable]
public class O_tir_mortier
{
    public int[] arr_cout_tirer_mortier;
    public string s_forme;
    public string s_puissance;
}

#endregion

#region O_batiments
[Serializable]
public class Objet_batiments
{
    public O_batiments o_batiments;
}

[Serializable]
public class O_batiments
{
    public int[] arr_cout_fortin;
    public int[] arr_cout_ecole_de_magie;
    public int[] arr_cout_autel;
    public int[] arr_cout_tente_de_soin;
    public int[] arr_cout_monument_aux_morts;
    public int[] arr_cout_fosse;
    public int[] arr_cout_portail_demoniaque;

    public int[] arr_cout_guilde_assassin;
    public int[] arr_cout_hutte_barbare;
    public int[] arr_cout_auberge_bagarreur;

    public int[] arr_cout_maison;
    public int[] arr_cout_hopital;
    public int[] arr_cout_mortier;
    public int[] arr_cout_marche;
    public int[] arr_cout_cabane_de_eclaireur;
    public int[] arr_cout_hache_de_guerre;
    public int[] arr_cout_camp_entrainement;

    public int[] arr_cout_moulin;
    public int[] arr_cout_stock_de_bois;
    public int[] arr_cout_stock_de_pierre;
    public int[] arr_cout_fourneau;
    public int[] arr_cout_feu_de_camp;
    public int[] arr_cout_palissade;
    public int[] arr_cout_entrepot;
    public int[] arr_cout_grue;
}
#endregion

#region o_tours_valeurs_textuelles_portee
[Serializable]
public class Objet_tours_valeurs_textuelles_portee
{
    public O_tours_valeurs_textuelles_portee o_tours_valeurs_textuelles_portee;
}

[Serializable]
public class O_tours_valeurs_textuelles_portee
{
    public int contact;
    public int tres_courte;
    public int courte;
    public int normale;
    public int lointaine;
    public int tres_lointaine;
    public int absolue; 
}
#endregion

#region o_monstres_valeurs_textuelles_deplacement
[Serializable]
public class Objet_monstres_valeurs_textuelles_deplacement
{
    public O_monstres_valeurs_textuelles_deplacement o_monstres_valeurs_textuelles_deplacement;
}

[Serializable]
public class O_monstres_valeurs_textuelles_deplacement
{
    public float tres_lente;
    public float lente;
    public float normale;
    public float rapide;
    public float tres_rapide;
}

#endregion


#region o_tours_valeurs_textuelles_largeur_visee
[Serializable]
public class Objet_tours_valeurs_textuelles_largeur_visee
{
    public O_tours_valeurs_textuelles_largeur_visee o_tours_valeurs_textuelles_largeur_visee;
}

[Serializable]
public class O_tours_valeurs_textuelles_largeur_visee
{
    public int petite;
    public int normale;
    public int grande;
}
#endregion

#region o_tours_valeurs_textuelles_cadence
[Serializable]
public class Objet_tours_valeurs_textuelles_cadence
{
    public O_tours_valeurs_textuelles_cadence o_tours_valeurs_textuelles_cadence;
}

[Serializable]
public class O_tours_valeurs_textuelles_cadence
{
    public int tres_lente;
    public int lente;
    public int normale;
    public int rapide;
    public int tres_rapide;
    public int permanente;
}
#endregion


#region o_tours_valeurs_textuelles_puissance
[Serializable]
public class Objet_tours_valeurs_textuelles_puissance
{
    public O_tours_valeurs_textuelles_puissance o_tours_valeurs_textuelles_puissance;
}

[Serializable]
public class O_tours_valeurs_textuelles_puissance
{
    public int nulle;
    public int tres_faible;
    public int faible;
    public int normale;
    public int forte;
    public int tres_forte;
    public int enorme;
    public int exponentielle;
}
#endregion

#region o_tours_valeurs_textuelles_duree_degats
[Serializable]
public class Objet_tours_valeurs_textuelles_duree_degats
{
    public O_tours_valeurs_textuelles_duree_degats o_tours_valeurs_textuelles_duree_degats;
}

[Serializable]
public class O_tours_valeurs_textuelles_duree_degats
{
    public int ponctuelle;
    public int courte;
    public int longue;
    public int tres_longue;
    public int permanente;
}
#endregion

#region o_tours_sniper
[Serializable]
public class Objet_tours_sniper
{
    public O_tours_sniper o_tours_sniper;
}

[Serializable]
public class O_tours_sniper
{
    public string s_forme;
    public string s_portee;
    public string s_cadence;
    public string s_puissance;
    public string s_impact;
    public string s_duree_degats;
    public string[] arr_ameliorations;
    public string[,] arr_ameliorations2D;
}
#endregion


#region o_tours_mitraillette
[Serializable]
public class Objet_tours_mitraillette
{
    public O_tours_mitraillette o_tours_mitraillette;
}

[Serializable]
public class O_tours_mitraillette
{
    public string s_forme;
    public string s_portee;
    public string s_cadence;
    public string s_puissance;
    public string s_impact;
    public string s_duree_degats;
    public string[] arr_ameliorations;
    public string[,] arr_ameliorations2D;
}
#endregion

#region o_tours_shotgun
[Serializable]
public class Objet_tours_shotgun
{
    public O_tours_shotgun o_tours_shotgun;
}

[Serializable]
public class O_tours_shotgun
{
    public string s_forme;
    public string s_portee;
    public string s_cadence;
    public string s_puissance;
    public string s_impact;
    public string s_duree_degats;
    public string[] arr_ameliorations;
    public string[,] arr_ameliorations2D;
}
#endregion

#region o_tours_canon
[Serializable]
public class Objet_tours_canon
{
    public O_tours_canon o_tours_canon;
}

[Serializable]
public class O_tours_canon
{
    public string s_forme;
    public string s_portee;
    public string s_cadence;
    public string s_puissance;
    public string s_impact;
    public string s_duree_degats;
    public string[] arr_ameliorations;
    public string[,] arr_ameliorations2D;
}
#endregion

#region o_tours_poison
[Serializable]
public class Objet_tours_poison
{
    public O_tours_poison o_tours_poison;
}

[Serializable]
public class O_tours_poison
{
    public string s_forme;
    public string s_portee;
    public string s_cadence;
    public string s_puissance;
    public string s_impact;
    public string s_duree_degats;
    public string[] arr_ameliorations;
    public string[,] arr_ameliorations2D;
}
#endregion

#region o_tours_feu
[Serializable]
public class Objet_tours_feu
{
    public O_tours_feu o_tours_feu;
}

[Serializable]
public class O_tours_feu
{
    public string s_forme;
    public string s_portee;
    public string s_cadence;
    public string s_puissance;
    public string s_impact;
    public string s_duree_degats;
    public string[] arr_ameliorations;
    public string[,] arr_ameliorations2D;
}
#endregion

#region o_tours_electrique
[Serializable]
public class Objet_tours_electrique
{
    public O_tours_electrique o_tours_electrique;
}

[Serializable]
public class O_tours_electrique
{
    public string s_forme;
    public string s_portee;
    public string s_cadence;
    public string s_puissance;
    public string s_impact;
    public string s_duree_degats;
    public string[] arr_ameliorations;
    public string[,] arr_ameliorations2D;
}
#endregion

#region o_tours_laser
[Serializable]
public class Objet_tours_laser
{
    public O_tours_laser o_tours_laser;
}

[Serializable]
public class O_tours_laser
{
    public string s_forme;
    public string s_portee;
    public string s_cadence;
    public string s_puissance;
    public string s_impact;
    public string s_duree_degats;
    public string[] arr_ameliorations;
    public string[,] arr_ameliorations2D;
}
#endregion

#region o_tours_tromblon
[Serializable]
public class Objet_tours_tromblon
{
    public O_tours_tromblon o_tours_tromblon;
}

[Serializable]
public class O_tours_tromblon
{
    public string s_forme;
    public string s_portee;
    public string s_cadence;
    public string s_puissance;
    public string s_impact;
    public string s_duree_degats;
    public string[] arr_ameliorations;
    public string[,] arr_ameliorations2D;
}
#endregion

#region o_unites_monstres_valeurs_textuelles_vitalite
[Serializable]
public class Objet_unites_monstres_valeurs_textuelles_vitalite
{
    public O_unites_monstres_valeurs_textuelles_vitalite o_unites_monstres_valeurs_textuelles_vitalite;
}

[Serializable]
public class O_unites_monstres_valeurs_textuelles_vitalite
{
    public int _1pv;
    public int tres_faible;
    public int faible;
    public int normale;
    public int forte;
    public int tres_forte;
    public int enorme;
}
#endregion

#region o_unites_monstres_valeurs_textuelles_armure
[Serializable]
public class Objet_unites_monstres_valeurs_textuelles_armure
{
    public O_unites_monstres_valeurs_textuelles_armure o_unites_monstres_valeurs_textuelles_armure;
}

[Serializable]
public class O_unites_monstres_valeurs_textuelles_armure
{
    public int aucune;
    public int tres_faible;
    public int faible;
    public int normale;
    public int forte;
    public int tres_forte;
    public int enorme;
}
#endregion

#region o_unites_monstres_valeurs_textuelles_initiative
[Serializable]
public class Objet_unites_monstres_valeurs_textuelles_initiative
{
    public O_unites_monstres_valeurs_textuelles_initiative o_unites_monstres_valeurs_textuelles_initiative;
}

[Serializable]
public class O_unites_monstres_valeurs_textuelles_initiative
{
    public int dernier;
    public int tres_lente;
    public int lente;
    public int normale;
    public int rapide;
    public int tres_rapide;
    public int premier;
}
#endregion

#region o_unites_monstres_valeurs_textuelles_degats
[Serializable]
public class Objet_unites_monstres_valeurs_textuelles_degats
{
    public O_unites_monstres_valeurs_textuelles_degats o_unites_monstres_valeurs_textuelles_degats;
}

[Serializable]
public class O_unites_monstres_valeurs_textuelles_degats
{
    public int aucun;
    public int tres_faibles;
    public int faibles;
    public int normaux;
    public int forts;
    public int tres_forts;
    public int enormes;
    public int one_shot;
}
#endregion

#region o_unites_monstres_valeurs_textuelles_portee
[Serializable]
public class Objet_unites_monstres_valeurs_textuelles_portee
{
    public O_unites_monstres_valeurs_textuelles_portee o_unites_monstres_valeurs_textuelles_portee;
}

[Serializable]
public class O_unites_monstres_valeurs_textuelles_portee
{
    public int courte;
    public int longue;
    public int longue_traversante;
    public int tres_longue;
    public int lointaine;
    public int tres_lointaine;
}
#endregion

#region o_unites_soldat
[Serializable]
public class Objet_unites_soldat
{
    public O_unites_soldat o_unites_soldat;
}

[Serializable]
public class O_unites_soldat
{
    public string s_vitalite;
    public string s_armure;
    public string s_initiative;
    public string s_degats;
    public string s_portee;
}
#endregion

#region o_unites_chevalier
[Serializable]
public class Objet_unites_chevalier
{
    public O_unites_chevalier o_unites_chevalier;
}

[Serializable]
public class O_unites_chevalier
{
    public string s_vitalite;
    public string s_armure;
    public string s_initiative;
    public string s_degats;
    public string s_portee;
}
#endregion

#region o_unites_assassin
[Serializable]
public class Objet_unites_assassin
{
    public O_unites_assassin o_unites_assassin;
}

[Serializable]
public class O_unites_assassin
{
    public string s_vitalite;
    public string s_armure;
    public string s_initiative;
    public string s_degats;
    public string s_portee;
}
#endregion

#region o_unites_bagarreur
[Serializable]
public class Objet_unites_bagarreur
{
    public O_unites_bagarreur o_unites_bagarreur;
}

[Serializable]
public class O_unites_bagarreur
{
    public string s_vitalite;
    public string s_armure;
    public string s_initiative;
    public string s_degats;
    public string s_portee;
}
#endregion

#region o_unite_barbare
[Serializable]
public class Objet_unites_barbare
{
    public O_unites_barbare o_unites_barbare;
}

[Serializable]
public class O_unites_barbare
{
    public string s_vitalite;
    public string s_armure;
    public string s_initiative;
    public string s_degats;
    public string s_portee;
}
#endregion

#region o_unites_lancier
[Serializable]
public class Objet_unites_lancier
{
    public O_unites_lancier o_unites_lancier;
}

[Serializable]
public class O_unites_lancier
{
    public string s_vitalite;
    public string s_armure;
    public string s_initiative;
    public string s_degats;
    public string s_portee;
}
#endregion

#region o_unites_archer
[Serializable]
public class Objet_unites_archer
{
    public O_unites_archer o_unites_archer;
}

[Serializable]
public class O_unites_archer
{
    public string s_vitalite;
    public string s_armure;
    public string s_initiative;
    public string s_degats;
    public string s_portee;
}
#endregion

#region o_unites_mage
[Serializable]
public class Objet_unites_mage
{
    public O_unites_mage o_unites_mage;
}

[Serializable]
public class O_unites_mage
{
    public string s_vitalite;
    public string s_armure;
    public string s_initiative;
    public string s_degats;
    public string s_portee;
}
#endregion

#region o_monstres_basique
[Serializable]
public class Objet_monstres_basique
{
    public O_monstres_basique o_monstres_basique;
}

[Serializable]
public class O_monstres_basique
{
    public string s_vitalite;
    public string s_armure;
    public string s_initiative;
    public string s_degats;
    public string s_portee;
}
#endregion

#region o_monstres_costaud
[Serializable]
public class Objet_monstres_costaud
{
    public O_monstres_costaud o_monstres_costaud;
}

[Serializable]
public class O_monstres_costaud
{
    public string s_vitalite;
    public string s_armure;
    public string s_initiative;
    public string s_degats;
    public string s_portee;
}
#endregion

#region o_monstres_boxeur
[Serializable]
public class Objet_monstres_boxeur
{
    public O_monstres_boxeur o_monstres_boxeur;
}

[Serializable]
public class O_monstres_boxeur
{
    public string s_vitalite;
    public string s_armure;
    public string s_initiative;
    public string s_degats;
    public string s_portee;
}
#endregion