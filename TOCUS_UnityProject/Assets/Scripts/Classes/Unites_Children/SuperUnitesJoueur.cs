using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperUnitesJoueur : UnitesJoueur
{

   public SuperUnitesJoueur(Sprite _maSpriteBase, Vector2 _SizeBoxCollider, Vector2 _ScaleCanvasHealthBar, Vector2 _PositionCanvasHealthBar,string _MonTag) : base (_maSpriteBase, _SizeBoxCollider, _ScaleCanvasHealthBar, _PositionCanvasHealthBar,_MonTag)
    {
        ConstructeurSuperUnites(_maSpriteBase, _SizeBoxCollider, _ScaleCanvasHealthBar, _PositionCanvasHealthBar, _MonTag);
    }

    public void ConstructeurSuperUnites(Sprite _maSpriteBase, Vector2 _SizeBoxCollider, Vector2 _ScaleCanvasHealthBar, Vector2 _PositionCanvasHealthBar,string _MonTag)
    {
        ConstructeurUnitesJoueur(_maSpriteBase, _SizeBoxCollider, _ScaleCanvasHealthBar, _PositionCanvasHealthBar, _MonTag);

        monGO.AddComponent<CallBacksSuperUnitesJoueur>();
        monGO.GetComponent<CallBacksSuperUnitesJoueur>().maSuperUniteJoueur = this;
    }


    public void SuperUniteJoueurAmeliorationStatistiques(CallBacksSuperUnitesJoueur _MaSuperUniteJoueur)
    {
        if (_MaSuperUniteJoueur.maSuperUniteJoueur.uniteVitalite > 0)
        {
            _MaSuperUniteJoueur.maSuperUniteJoueur.uniteVitalite--;
        }
        if (_MaSuperUniteJoueur.maSuperUniteJoueur.uniteInitiative > 0)
        {
            _MaSuperUniteJoueur.maSuperUniteJoueur.uniteInitiative--;
        }
        if (_MaSuperUniteJoueur.maSuperUniteJoueur.uniteDegats > 0)
        {
            _MaSuperUniteJoueur.maSuperUniteJoueur.uniteDegats--;
        }
        _MaSuperUniteJoueur.maSuperUniteJoueur.AttributionCaracteristiques();
        _MaSuperUniteJoueur.maSuperUniteJoueur.HealthBar_MaJ();
    }

}
