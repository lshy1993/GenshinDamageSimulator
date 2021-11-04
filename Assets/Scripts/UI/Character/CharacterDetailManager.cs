using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class CharacterDetailManager : MonoBehaviour
{
    public DetailNumBox HP, ATK, DEF, ElementalMastery, CritRate, CritDamage, HealingBonus, IncomingHealingBonus, EnergyRecharge, CD, ShieldStrength;
    public DetailNumBox PyroBonus, PyroResist, HydroBonus, HydroResist, DendroBonus, DendroResist, ElectroBonus, ElectroResist, AnemoBonus, AnemoResist, CryoBonus, CryoResist, GeoBonus, GeoResist, PhysicalBonus, PhysicalResist;

    private int charaID = 0;

    private void OnEnable()
    {
        //ListChild();
        UIFresh();
    }

    private void UIFresh()
    {
        Debug.Log($"CH {charaID} Detail Open");
        Character character = GameManager.GetInstance().teams[charaID];
        //if (character == null) character = new Character();

        HP.SetNum(character.GetBaseHP(), character.GetBiasHP());
        ATK.SetNum(character.GetBaseATK(), character.GetBiasATK());
        DEF.SetNum(character.GetBaseDEF(), character.GetBiasDEF());

        ElementalMastery.SetNum(character.GetElementalMastery(), 0);
        CritRate.SetNum(character.GetCritRate(), 0, true);
        CritDamage.SetNum(character.GetCritDamage(), 0, true);
        HealingBonus.SetNum(character.GetHealingBonus(), 0, true);
        IncomingHealingBonus.SetNum(character.GetIncomingHealingBonus(), 0, true);
        EnergyRecharge.SetNum(character.GetEnergyRecharge(), 0, true);
        CD.SetNum(character.GetCD(), 0, true);
        ShieldStrength.SetNum(character.GetShieldStrength(), 0, true);
        PyroBonus.SetNum(character.GetPyroBonus(), 0, true);
        PyroResist.SetNum(character.GetPyroResist(), 0, true);
        HydroBonus.SetNum(character.GetHydroBonus(), 0, true);
        HydroResist.SetNum(character.GetHydroResist(), 0, true);
        DendroBonus.SetNum(character.GetDendroBonus(), 0, true);
        DendroResist.SetNum(character.GetDendroResist(), 0, true);
        ElectroBonus.SetNum(character.GetElectroBonus(), 0, true);
        ElectroResist.SetNum(character.GetElectroResist(), 0, true);
        AnemoBonus.SetNum(character.GetAnemoBonus(), 0, true);
        AnemoResist.SetNum(character.GetAnemoResist(), 0, true);
        CryoBonus.SetNum(character.GetCryoBonus(), 0, true);
        CryoResist.SetNum(character.GetCryoResist(), 0, true);
        GeoBonus.SetNum(character.GetGeoBonus(), 0, true);
        GeoResist.SetNum(character.GetGeoResist(), 0, true);
        PhysicalBonus.SetNum(character.GetPhysicalBonus(), 0, true);
        PhysicalResist.SetNum(character.GetPhysicalResist(), 0, true);
    }

    public void SetID(int x)
    {
        charaID = x;
    }

    private void ListChild()
    {
        foreach(Transform parent in transform)
        {
            string outstr = "";
            foreach (Transform child in parent.transform)
            {
                //outstr += "public float Get" + child.name.Replace("_Box", "") + "(){\nreturn 0;\n}\n";
                var d = child.name.Replace("_Box", "");
                outstr += $"{d}.SetNum(character.Get{d}(),0,true);\n";
            }
            Debug.Log(outstr);
        }
    }
}
