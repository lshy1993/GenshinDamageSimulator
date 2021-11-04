using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using UnityEngine;

public class Character : BodyBase
{
    public int Rare = 4;
    public int Star { get; private set; } = 0;
    public ELEMENT Vision = ELEMENT.PYRO;
    public int Constellations = 0;

    protected float PercentATK = 0;
    protected float PercentHP = 0;
    protected float PercentDEF = 0;

    public WEAPONTYPE WeaponType = WEAPONTYPE.SWORD;

    public Talent NormalAttack;
    protected List<int> NAFrames;
    public Talent ElementSkill;
    public Talent ElementBurst;
    protected int SkillFrame, BurstFrame;

    public WeaponBase Weapon;
    public ArtifactBase[] Artifacts;

    protected Dictionary<string, List<string>> baseTable;
    protected Dictionary<string, List<string>> naTable;
    protected Dictionary<string, List<string>> eTable;
    protected Dictionary<string, List<string>> qTable;

    protected int hitnum = 1;
    protected float animecd = 0;

    public Character() : base()
    {
        NormalAttack = new Talent(0, 0);
        Artifacts = new ArtifactBase[5];
    }
    public Character(string name) : this()
    {
        InitChara(name);
    }

    private void InitChara(string name)
    {
        Name = name;
        NAFrames = new List<int>();
        baseTable = DataManager.GetInstance().GetCharacterBase(Name);
        naTable = DataManager.GetInstance().GetSkillBase($"{Name}_NA");
        eTable = DataManager.GetInstance().GetSkillBase($"{Name}_E");
        qTable = DataManager.GetInstance().GetSkillBase($"{Name}_Q");

        try
        {
            var ecd = Convert.ToSingle(eTable["CD"][0]);
            ElementSkill = new Talent(ecd, 0);
        }
        catch (Exception e)
        {
            Debug.LogError($"{Name} ETable:{e.Message}");
        }
        try
        {
            var qcd = Convert.ToSingle(qTable["CD"][0]);
            var qenergy = Convert.ToSingle(qTable["Energy Cost"][0]);
            ElementBurst = new Talent(qcd, qenergy);
        }
        catch (Exception e)
        {
            Debug.LogError($"{Name} QTable :{e.Message}");
        }

        Rare = DataManager.GetInstance().CharaDataList[Name].Rare;
        WeaponType = DataManager.GetInstance().CharaDataList[Name].WeaponType;
        Vision = DataManager.GetInstance().CharaDataList[Name].Vision;
        string defaultwp = Const.defaultweapon[WeaponType];
        Weapon = new WeaponBase(defaultwp);
        ReloadData();
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
        Debug.Log($"change {Name} Lv to {d}");
        ReloadData();
    }

    public void ChangeWeapon(string wpname)
    {
        Weapon = new WeaponBase(wpname);
    }

    private void ReloadData()
    {
        // 修改基础属性
        LERP(ref BaseHP, "Base HP");
        LERP(ref BaseATK, "Base ATK");
        LERP(ref BaseDEF, "Base DEF");
        LERP(ref PercentATK, "ATK");
        LERP(ref PercentHP, "HP");
        LERP(ref PercentDEF, "DEF");
        LERP(ref ElementalMastery, "Elemental Mastery");
        LERP(ref HealingBonus, "Healing Bonus");
        LERP(ref CritRate, "CRIT Rate");
        LERP(ref CritDamage, "CRIT DMG");
        LERP(ref GeoBonus, "Geo DMG Bonus");
        LERP(ref ElectroBonus, "Electro DMG Bonus");
        LERP(ref EnergyRecharge, "Energy Recharge");
        LERP(ref CryoBonus, "Cryo DMG Bonus");
        LERP(ref PyroBonus, "Pyro DMG Bonus");
        LERP(ref PhysicalBonus, "Physical DMG Bonus");
        LERP(ref AnemoBonus, "Anemo DMG Bonus");
        LERP(ref HydroBonus, "Hydro DMG Bonus");
    }

    private void LERPint(ref float obj, string colname)
    {
        //if (baseTable == null) baseTable = DataManager.GetInstance().GetCharacterBase(Name);
        if (!baseTable.ContainsKey(colname)) return;
        List<string> list = baseTable[colname];
        float min = Convert.ToSingle(list[2 * Star]);
        float max = Convert.ToSingle(list[2 * Star + 1]);
        int a = Const.lvmin[Star];
        int b = Const.lvmax[Star];
        obj = min + Mathf.RoundToInt((Level - a) / (b - a) * (max - min));
    }

    private void LERP(ref float obj, string colname)
    {
        //if (baseTable == null) baseTable = DataManager.GetInstance().GetCharacterBase(Name);
        if (!baseTable.ContainsKey(colname)) return;
        List<string> list = baseTable[colname];
        float min = Convert.ToSingle(list[2 * Star]);
        float max = Convert.ToSingle(list[2 * Star + 1]);
        int a = Const.lvmin[Star];
        int b = Const.lvmax[Star];
        obj = min + (Level - a) / (b - a) * (max - min);
    }

    public override void Update(float dt)
    {
        animecd -= dt;
        if (animecd < 0) animecd = 0;
        NormalAttack.Update(dt);
        ElementSkill.Update(dt);
        ElementBurst.Update(dt);
        base.Update(dt);
    }


    #region GET Status
    public override float GetHP()
    {
        return GetBaseHP() + GetBiasHP();
    }
    public float GetBaseHP()
    {
        // (角色突破+武器)百分比加成
        return BaseHP * (1 + PercentHP + Weapon.PercentHP);
    }
    public float GetBiasHP()
    {
        // 圣遗物加成
        float a = 0, b = 0;
        foreach(ArtifactBase art in Artifacts)
        {
            if (art == null) continue;
            for (int i=0;i<5;i++ )
            {
                if (art.Status[i] == "HP") b += art.Nums[i];
                else if (art.Status[i] == "HP%") a += art.Nums[i];
            }
        }
        return BaseHP * a + b;
    }
    public virtual float GetATK()
    {
        //Debug.Log("GetATK");
        return GetBaseATK() + GetBiasATK();
    }
    public float GetBaseATK()
    {
        // 武器加成
        return (BaseATK + Weapon.ATK) * (1 + PercentATK + Weapon.PercentATK);
    }
    public float GetBiasATK()
    {
        // 圣遗物加成
        float a = 0, b = 0;
        foreach (ArtifactBase art in Artifacts)
        {
            if (art == null) continue;
            for (int i = 0; i < 5; i++)
            {
                if (art.Status[i] == "ATK") b += art.Nums[i];
                else if (art.Status[i] == "ATK%") a += art.Nums[i];
            }
        }
        foreach (Buff ba in GameManager.GetInstance().envs)
        {
            if (ba.GetBuffedName() == "ATK")
            {
                b += ba.GetBuffedNum();
            }            
        }
        return BaseATK * a / 100 + b;
    }

    public float GetDEF()
    {
        return GetBaseDEF() + GetBiasDEF();
    }
    public float GetBaseDEF()
    {
        // (自身突破+武器)加成
        return BaseDEF * (1 + PercentDEF + Weapon.PercentDEF);
    }
    public float GetBiasDEF()
    {
        // 圣遗物加成
        float a = 0, b = 0;
        foreach (ArtifactBase art in Artifacts)
        {
            if (art == null) continue;
            for (int i = 0; i < 5; i++)
            {
                if (art.Status[i] == "DEF") b += art.Nums[i];
                else if (art.Status[i] == "DEF%") a += art.Nums[i];
            }
        }
        return BaseDEF * a + b;
    }

    public float GetElementalMastery()
    {
        float b = 0;
        foreach (ArtifactBase art in Artifacts)
        {
            if (art == null) continue;
            for (int i = 0; i < 5; i++)
            {
                if (art.Status[i] == "EleMastery") b += art.Nums[i];
            }
        }
        return ElementalMastery + Weapon.ElementalMastery + b;
    }

    public float GetCritRate()
    {
        float b = 0;
        foreach (ArtifactBase art in Artifacts)
        {
            if (art == null) continue;
            for (int i = 0; i < 5; i++)
            {
                if (art.Status[i] == "Crit%") b += art.Nums[i];
            }
        }
        return CritRate + Weapon.CritRate + b;
    }

    public float GetCritDamage()
    {
        float b = 0;
        foreach (ArtifactBase art in Artifacts)
        {
            if (art == null) continue;
            for (int i = 0; i < 5; i++)
            {
                if (art.Status[i] == "CritDMG%") b += art.Nums[i];
            }
        }
        return CritDamage + Weapon.CritDamage + b;
    }
    public float GetHealingBonus()
    {
        float b = 0;
        foreach (ArtifactBase art in Artifacts)
        {
            if (art == null) continue;
            for (int i = 0; i < 5; i++)
            {
                if (art.Status[i] == "Heal%") b += art.Nums[i];
            }
        }
        return HealingBonus + b;
    }
    public float GetIncomingHealingBonus()
    {
        return 0;
    }

    public float GetEnergyRecharge()
    {
        float b = 0;
        foreach (ArtifactBase art in Artifacts)
        {
            if (art == null) continue;
            for (int i = 0; i < 5; i++)
            {
                if (art.Status[i] == "EleCharge%") b += art.Nums[i];
            }
        }
        return EnergyRecharge + Weapon.EnergyRecharge + b;
    }
    public float GetCD()
    {
        return 0;
    }
    public float GetShieldStrength()
    {
        return ShieldStrength;
    }
    public float GetPyroBonus()
    {
        float b = 0;
        foreach (ArtifactBase art in Artifacts)
        {
            if (art == null) continue;
            for (int i = 0; i < 5; i++)
            {
                if (art.Status[i] == "PyroDMG%") b += art.Nums[i];
            }
        }
        return PyroBonus + b;
    }
    public float GetPyroResist()
    {
        return 0;
    }
    public float GetHydroBonus()
    {
        float b = 0;
        foreach (ArtifactBase art in Artifacts)
        {
            if (art == null) continue;
            for (int i = 0; i < 5; i++)
            {
                if (art.Status[i] == "HydroDMG%") b += art.Nums[i];
            }
        }
        return HydroBonus + b;
    }
    public float GetHydroResist()
    {
        return 0;
    }
    public float GetDendroBonus()
    {
        float b = 0;
        foreach (ArtifactBase art in Artifacts)
        {
            if (art == null) continue;
            for (int i = 0; i < 5; i++)
            {
                if (art.Status[i] == "DendroDMG%") b += art.Nums[i];
            }
        }
        return DendroBonus + b;
    }
    public float GetDendroResist()
    {
        return 0;
    }
    public float GetElectroBonus()
    {
        float b = 0;
        foreach (ArtifactBase art in Artifacts)
        {
            if (art == null) continue;
            for (int i = 0; i < 5; i++)
            {
                if (art.Status[i] == "ElectroDMG%") b += art.Nums[i];
            }
        }
        return ElectroBonus + b;
    }
    public float GetElectroResist()
    {
        return 0;
    }
    public float GetAnemoBonus()
    {
        float b = 0;
        foreach (ArtifactBase art in Artifacts)
        {
            if (art == null) continue;
            for (int i = 0; i < 5; i++)
            {
                if (art.Status[i] == "AnemoDMG%") b += art.Nums[i];
            }
        }
        return AnemoBonus + b;
    }
    public float GetAnemoResist()
    {
        return 0;
    }
    public float GetCryoBonus()
    {
        float b = 0;
        foreach (ArtifactBase art in Artifacts)
        {
            if (art == null) continue;
            for (int i = 0; i < 5; i++)
            {
                if (art.Status[i] == "CryoDMG%") b += art.Nums[i];
            }
        }
        return CryoBonus + b;
    }
    public float GetCryoResist()
    {
        return 0;
    }
    public float GetGeoBonus()
    {
        float b = 0;
        foreach (ArtifactBase art in Artifacts)
        {
            if (art == null) continue;
            for (int i = 0; i < 5; i++)
            {
                if (art.Status[i] == "GeoDMG%") b += art.Nums[i];
            }
        }
        return GeoBonus + b;
    }
    public float GetGeoResist()
    {
        return 0;
    }
    public float GetPhysicalBonus()
    {
        float b = 0;
        foreach (ArtifactBase art in Artifacts)
        {
            if (art == null) continue;
            for (int i = 0; i < 5; i++)
            {
                if (art.Status[i] == "PhyDMG%") b += art.Nums[i];
            }
        }
        return PhysicalBonus + Weapon.PhysicalBonus + b;
    }
    public float GetPhysicalResist()
    {
        return 0;
    }

    public ELEMENT GetInfuse()
    {
        ELEMENT final = ELEMENT.NONE;
        // 被附魔的情形
        if (WeaponType == WEAPONTYPE.CATALYST)
        {
            final = Vision;
        }
        else if(WeaponType != WEAPONTYPE.BOW)
        {
            // 近战被附魔
            foreach (var buff in GameManager.GetInstance().envs)
            {
                if (buff.Infuse > final)
                {
                    final = buff.Infuse;
                }
            }
        }
        return final;
    }

    #endregion

    public bool CanSwitch()
    {
        return animecd <= 0;
    }
    public bool CanCastNormalAttack()
    {
        return animecd <= 0 && NormalAttack.CanCast();
    }
    public bool CanCastSkill()
    {
        return animecd <= 0 && ElementSkill.CanCast();
    }
    public bool CanCastBurst()
    {
        return animecd <= 0 && ElementBurst.CanCast();
    }

    /// <summary>
    /// 重置普攻
    /// </summary>
    private void ResetNormalAttack()
    {
        hitnum = 1;
    }

    /// <summary>
    /// 普通攻击
    /// </summary>
    public virtual void CastNormalAttack()
    {
        NormalAttack.Cast();
        //Debug.Log($"hitnum: {hitnum}");
        animecd = NAFrames[hitnum - 1] / 60f;
        hitnum = naTable.ContainsKey($"{hitnum + 1}-Hit DMG") ? hitnum + 1 : 1;
        int level = NormalAttack.Level;// + passive ? 1 : 0;
        ELEMENT infused = GetInfuse();
        castNormalAttack(level, infused);
    }
    protected virtual void castNormalAttack(int level, ELEMENT infused)
    {
        var data = naTable[$"{hitnum}-Hit DMG"][level];
        if (data.Split('+').Length > 1)
        {
            var datalist = data.Split('+');
            int hitlength = datalist.Length;
            for (int i = 0; i < hitlength; i++)
            {
                float rate = Convert.ToSingle(datalist[i]);
                string dmgname = hitlength > 1 ? $"*{i + 1}" : "";
                var sk = new DamageBase($"NormalAttack {hitnum}-Hit{dmgname}", rate, infused, 1);
                GameManager.GetInstance().DealDamage(this, sk);
            }
        }
        else if (data.Split('*').Length > 1)
        {
            for (int i = 0; i < Convert.ToInt32(data.Split('*')[1]); i++)
            {
                float rate = Convert.ToSingle(data.Split('*')[0]);
                var sk = new DamageBase($"NormalAttack {hitnum}-Hit{i}", rate, infused, 1);
                GameManager.GetInstance().DealDamage(this, sk);
            }
        }
        else
        {
            float rate = Convert.ToSingle(data);
            var sk = new DamageBase($"NormalAttack {hitnum}-Hit", rate, infused, 1);
            GameManager.GetInstance().DealDamage(this, sk);
        }
    }

    /// <summary>
    /// 重击or蓄力
    /// </summary>
    public virtual void CastChargedAttack()
    {
        ResetNormalAttack();
        string hitname = "Charged Attack DMG";
        int level = NormalAttack.Level;// + passive ? 1 : 0;
        float rate = Convert.ToSingle(naTable[hitname][level]);
        var sk = new DamageBase("ChargedAttack", rate, GetInfuse(), 1);
        GameManager.GetInstance().DealDamage(this, sk);
    }
    /// <summary>
    /// 下落攻击
    /// </summary>
    public virtual void CastPlungeAttack()
    {
        ResetNormalAttack();
        string hitname = "Low Plunge DMG";
        int level = NormalAttack.Level; // + passive ? 1 : 0;
        float rate = Convert.ToSingle(naTable[hitname][level]);
        var sk = new DamageBase("Low PlungeAttack", rate, GetInfuse(), 1);
        GameManager.GetInstance().DealDamage(this, sk);
    }


    public void CastSkill(float holdt = 0)
    {
        ResetNormalAttack();
        animecd = SkillFrame / 60f;
        int level = ElementSkill.Level + Constellations >= 3 ? 3 : 0;
        castSkill(level, holdt);
    }
    protected virtual void castSkill(int level, float holdt)
    {
        ElementSkill.Cast();
        var data = eTable["Skill DMG"][level];
        try
        {
            if (data.Split('+').Length > 1)
            {
                foreach (var ss in data.Split('+'))
                {
                    float rate = Convert.ToSingle(ss);
                    var sk = new DamageBase("ElementSkill", rate, Vision, 2);
                    GameManager.GetInstance().DealDamage(this, sk);
                }
            }
            else if (data.Split('*').Length > 1)
            {
                for (int i = 0; i < Convert.ToInt32(data.Split('*')[1]); i++)
                {
                    float rate = Convert.ToSingle(data.Split('*')[0]);
                    var sk = new DamageBase("ElementSkill", rate, Vision, 2);
                    GameManager.GetInstance().DealDamage(this, sk);
                }
            }
            else
            {
                float rate = Convert.ToSingle(data);
                var sk = new DamageBase("ElementSkill", rate, Vision, 2);
                GameManager.GetInstance().DealDamage(this, sk);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"{data} :{e.Message}");
        }
    }


    public void CastBurst()
    {
        ResetNormalAttack();
        animecd = BurstFrame / 60f;
        ElementBurst.Cast();
        int level = ElementBurst.Level + Constellations >= 5 ? 3 : 0;
        castBurst(level);
    }
    protected virtual void castBurst(int level)
    {
        var data = qTable["Skill DMG"][level];
        try
        {
            float rate = Convert.ToSingle(data);
            var sk = new DamageBase("ElementBurst", rate, Vision, 1);
            GameManager.GetInstance().DealDamage(this, sk);
        }
        catch (Exception e)
        {
            Debug.LogError($"{data} :{e.Message}");
        }
    }


    public void Charge(float energy)
    {
        ElementBurst.Charge(GetEnergyRecharge() * energy);
    }
}

