using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WeaponBase
{
    public string Name { get; protected set; }
    public int Rare = 1;
    public WEAPONTYPE WeaponType = WEAPONTYPE.SWORD;

    public int Level { get; protected set; } = 1;    
    public int Star { get; protected set; } = 0;
    public int Refine = 1;

    public float ATK = 0;
    public float PercentATK = 0;
    public float PercentHP = 0;
    public float PercentDEF = 0;
    public float PhysicalBonus = 0;
    public float CritDamage = 0;
    public float CritRate = 0;
    public float ElementalMastery = 0;
    public float EnergyRecharge = 0;

    private Dictionary<string, List<string>> baseTable;

    public WeaponBase() { }

    public WeaponBase(string name)
    {
        Name = name;
        baseTable = DataManager.GetInstance().GetWeaponBase(Name);
        Rare = DataManager.GetInstance().WeaponDataList[Name].Rare;
        ReloadData();
    }

    public int GetStarMax()
    {
        return baseTable["Level"].Count == 19 ? 4 : 6;
    }
    public void ChangeStar(int d)
    {
        Star = d;
        // 自动调节等级
        ChangeLevel(Level);        
    }
    public void ChangeLevel(int d)
    {
        Level = Const.RELU(d, Const.lvmin[Star], Const.lvmax[Star]);
        ReloadData();
    }
    
    private void ReloadData()
    {
        LERP(ref ATK, "Base Atk");
        CLIP(ref PercentATK, "ATK");
        CLIP(ref PercentHP, "HP");
        CLIP(ref PercentDEF, "DEF");
        CLIP(ref PhysicalBonus, "Physical DMG Bonus");
        CLIP(ref CritDamage, "CRIT DMG");
        CLIP(ref CritRate, "CRIT Rate");
        CLIP(ref ElementalMastery, "Elemental Mastery");
        CLIP(ref EnergyRecharge, "Energy Recharge");
    }

    public string GetATK()
    {
        return string.Format("ATK: {0:N0}", ATK);
    }

    public string GetSubStatus()
    {
        if (PercentATK != 0) return string.Format("ATK: {0:P1}", PercentATK);
        if (PercentHP != 0) return string.Format("HP: {0:P1}", PercentHP);
        if (PercentDEF != 0) return string.Format("DEF: {0:P1}", PercentDEF);
        if (PhysicalBonus != 0) return string.Format("Phy DMG Bonus: {0:P1}", PhysicalBonus);
        if (CritDamage != 0) return string.Format("CRIT DMG: {0:P1}", CritDamage);
        if (CritRate != 0) return string.Format("CRIT Rate: {0:P1}", CritRate);
        if (ElementalMastery != 0) return string.Format("Elemental Mastery: {0:N0}", ElementalMastery);
        if (EnergyRecharge != 0) return string.Format("Energy Recharge: {0:P1}", EnergyRecharge);
        return "";
    }

    private void LERP(ref float obj, string colname)
    {
        if (!baseTable.ContainsKey(colname)) return;
        List<string> list = baseTable[colname];
        int t = Level / 5 + Star;
        float min = Convert.ToSingle(list[t]);
        var d = (t + 1 == list.Count) ? t : t + 1;
        float max = Convert.ToSingle(list[d]);
        obj = min + (Level % 5) / 5f * (max - min);
    }

    private void CLIP(ref float obj, string colname)
    {
        if (!baseTable.ContainsKey(colname)) return;
        List<string> list = baseTable[colname];
        int t = Level / 5 + Star;
        obj = Convert.ToSingle(list[t]);
        if (colname != "Elemental Mastery") obj /= 100;
    }

}
