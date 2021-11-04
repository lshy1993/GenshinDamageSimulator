using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class Fischl : Character
{
    public Fischl() : base("fischl")
    {
        NAFrames = new List<int> { 10, 28 - 10, 61 - 28, 102 - 61, 131 - 102 };
        SkillFrame = 18;
        BurstFrame = 20;
    }

    protected override void castSkill(int level, float holdt)
    {
        ElementSkill.Cast();
        float dmg = Convert.ToSingle(eTable["Summoning DMG"][level]);
        var sk = new DamageBase("Nightrider", dmg, Vision, 1);
        GameManager.GetInstance().DealDamage(this, sk);

        float ozdmg = Convert.ToSingle(eTable["Ozs ATK DMG"][level]);
        var oz = new Oz(this, ozdmg, 10);
        GameManager.GetInstance().AddBuff(oz);
    }

    protected override void castBurst(int level)
    {
        float dmg = Convert.ToSingle(eTable["Falling Thunder DMG"][level]);
        var sk = new DamageBase("MidnightPhantasmagoria", dmg, Vision, 2);
        GameManager.GetInstance().DealDamage(this, sk);
        
    }
}

