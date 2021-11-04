using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class BodyBase
{
    public string Name { get; protected set; }
    public int Level { get; protected set; } = 1;

    public float CurHP = 1000;
    protected float BaseHP = 1000;
    protected float BaseATK = 100;
    protected float BaseDEF = 100;

    protected float ElementalMastery = 0;
    protected float CritRate = 0.05f;
    protected float CritDamage = 0.5f;
    protected float HealingBonus = 0;
    protected float IncomingHealingBonus = 0;
    protected float EnergyRecharge = 1f;
    protected float CooldownReduction = 0;
    protected float ShieldStrength = 0;

    public float DefenceReduction = 0;

    protected float PyroBonus = 0;
    protected float PyroResist = 0;
    protected float HydroBonus = 0;
    protected float HydroResist = 0;
    protected float DendroBonus = 0;
    protected float DendroResist = 0;
    protected float ElectroBonus = 0;
    protected float ElectroResist = 0;
    protected float AnemoBonus = 0;
    protected float AnemoResist = 0;
    protected float CryoBonus = 0;
    protected float CryoResist = 0;
    protected float GeoBonus = 0;
    protected float GeoResist = 0;
    protected float PhysicalBonus = 0;
    protected float PhysicalResist = 0;

    public List<ElementBase> InfectedElements;
    private Dictionary<string, ElementBase> elementCD;

    public BodyBase()
    {
        InfectedElements = new List<ElementBase>();
        elementCD = new Dictionary<string, ElementBase>();
    }

    public virtual void Update(float dt)
    {
        for (int i = InfectedElements.Count - 1; i >= 0; i--)
        {
            ElementBase eb = InfectedElements[i];
            eb.Update(dt);
            if (eb.Amount <= 0) InfectedElements.Remove(eb);
        }
        foreach (var kv in elementCD)
        {
            kv.Value.Update(dt);
        }
    }

    public virtual float GetHP()
    {
        return BaseHP;
    }

    public virtual float GetHPProgress()
    {
        return CurHP / GetHP();
    }

    public float GetResist(ELEMENT ele)
    {
        switch (ele)
        {
            case ELEMENT.PYRO:
                return PyroResist;
            case ELEMENT.HYDRO:
                return HydroResist;
            case ELEMENT.DENDRO:
                return DendroResist;
            case ELEMENT.ANEMO:
                return AnemoResist;
            case ELEMENT.CRYO:
                return CryoResist;
            case ELEMENT.ELECTRO:
                return ElectroResist;
            case ELEMENT.GEO:
                return GeoResist;
            //case ELEMENT.PHYSICAL:
            case ELEMENT.NONE:
                return PhysicalResist;
            default:
                return 1;
        }
    }

    public float GetBonus(ELEMENT ele)
    {
        switch (ele)
        {
            case ELEMENT.PYRO:
                return PyroBonus;
            case ELEMENT.HYDRO:
                return HydroBonus;
            case ELEMENT.DENDRO:
                return DendroBonus;
            case ELEMENT.ANEMO:
                return AnemoBonus;
            case ELEMENT.CRYO:
                return CryoBonus;
            case ELEMENT.ELECTRO:
                return ElectroBonus;
            case ELEMENT.GEO:
                return GeoBonus;
            //case ELEMENT.PHYSICAL:
            case ELEMENT.NONE:
                return PhysicalBonus;
            default:
                return 1;
        }
    }

    public REACTION ElementReactionCheck(ElementBase eb)
    {
        // 检查附着cd
        if (elementCD.ContainsKey(eb.From))
        {
            if (!elementCD[eb.From].CanAttach())
            {
                // 计数器-1
                elementCD[eb.From].HitCount -= 1;
                return REACTION.NONE;
            }
            else
            {
                // 可以再次附着
                elementCD[eb.From].Reset();
                attachElement(eb);
            }
        }
        else
        {
            elementCD.Add(eb.From, eb);// 追加记录cd            
            elementCD[eb.From].Reset();
            attachElement(eb);
        }
        if (InfectedElements.Count <= 1 || InfectedElements[0].Amount <= 0) return REACTION.NONE;
        // 后手元素 和 先手元素反应
        REACTION final = REACTION.NONE;
        switch (InfectedElements[1].Type)
        {
            case ELEMENT.CRYO:
                if (InfectedElements[0].Type == ELEMENT.PYRO)
                {
                    // 融化
                    final = REACTION.MELT;
                }
                else if (InfectedElements[0].Type == ELEMENT.HYDRO)
                {
                    // 冻结
                    final = REACTION.FREEZE;
                }
                else if(InfectedElements[0].Type == ELEMENT.ELECTRO)
                {
                    final = REACTION.SUPERCONDUCT;
                }
                break;
            case ELEMENT.PYRO:
                if (InfectedElements[0].Type == ELEMENT.HYDRO)
                {
                    // 蒸发
                    final = REACTION.VAPORIZE;
                }
                else if (InfectedElements[0].Type == ELEMENT.CRYO)
                {
                    // 融化
                    final = REACTION.MELT;
                }
                else if(InfectedElements[0].Type == ELEMENT.ELECTRO)
                {
                    // 超载
                    final = REACTION.OVERLOAD;
                }
                break;
            case ELEMENT.HYDRO:
                if (InfectedElements[0].Type == ELEMENT.PYRO)
                {
                    // 蒸发
                    final = REACTION.VAPORIZE;
                }
                else if(InfectedElements[0].Type == ELEMENT.ELECTRO)
                {
                    // 感电
                    final = REACTION.ELECTROCHARGED;
                }
                break;
            case ELEMENT.ELECTRO:
                if (InfectedElements[0].Type == ELEMENT.PYRO)
                {
                    // 超载
                    final = REACTION.OVERLOAD;
                }
                else if(InfectedElements[0].Type == ELEMENT.HYDRO)
                {
                    final = REACTION.ELECTROCHARGED;
                }
                else if(InfectedElements[0].Type == ELEMENT.CRYO)
                {
                    final = REACTION.SUPERCONDUCT;
                }
                break;
            case ELEMENT.ANEMO:
                // 向周边扩散
                return REACTION.SWIRL;
        }
        if (final != REACTION.NONE)
        {
            // 先手元素减去后手的量
            InfectedElements[0].Amount -= ReactionMultipler(InfectedElements[0].Type, InfectedElements[1].Type) * InfectedElements[1].Amount;
            // 后手元素消失
            InfectedElements.RemoveAt(1);
            if (InfectedElements[0].Amount <= 0) InfectedElements.RemoveAt(0);
        }
        return final;
    }

    private void attachElement(ElementBase eb)
    {
        if (InfectedElements.Count >= 1)
        {
            if (InfectedElements[0].Type == eb.Type)
            {
                InfectedElements[0].AddAmount(eb.Amount * 0.8f);
            }
            else if (InfectedElements.Count >= 2 && InfectedElements[1].Type == eb.Type)
            {
                InfectedElements[1].AddAmount(eb.Amount * 0.8f);
            }
            else
            {
                InfectedElements.Add(eb);
            }
        }
        else
        {
            //Debug.Log(eb.Type);
            InfectedElements.Add(eb);
        }
    }

    private float ReactionMultipler(ELEMENT a, ELEMENT b)
    {
        // 1.水克制火 2.火克制冰 3.水 / 雷 / 冰 / 火克制风 / 岩
        switch (b)
        {
            case ELEMENT.PYRO:
                if (a == ELEMENT.HYDRO) return 0.5f;
                if (a == ELEMENT.CRYO) return 2;
                if (a == ELEMENT.ANEMO || a == ELEMENT.GEO) return 2;
                break;
            case ELEMENT.CRYO:
                if (a == ELEMENT.PYRO) return 0.5f;
                if (a == ELEMENT.ANEMO || a == ELEMENT.GEO) return 2;
                break;
            case ELEMENT.HYDRO:
                if (a == ELEMENT.PYRO) return 2;
                if (a == ELEMENT.ANEMO || a == ELEMENT.GEO) return 2;
                break;
            case ELEMENT.ELECTRO:
                if (a == ELEMENT.ANEMO || a == ELEMENT.GEO) return 2;
                break;
            case ELEMENT.ANEMO:
            case ELEMENT.GEO:
                if (a == ELEMENT.HYDRO) return 0.5f;
                if (b == ELEMENT.PYRO) return 0.5f;
                if (b == ELEMENT.CRYO) return 0.5f;
                if (b == ELEMENT.ELECTRO) return 0.5f;
                break;
            default:                
                break;
        }
        return 1; 
    }

    public void GetDamage(int dmg)
    {
        CurHP -= dmg;
    }
}