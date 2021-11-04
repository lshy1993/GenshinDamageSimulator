using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Diona : Character
{
    public Diona() : base("diona")
    {
        NAFrames = new List<int> { 16, 37 - 16, 67 - 37, 101 - 67, 152 - 101, 190 - 152 };
        SkillFrame = 15;
        BurstFrame = 49;
    }

    protected override void castSkill(int level, float t)
    {
        // 单个猫爪伤害
        float dmg = Convert.ToSingle(eTable["Icy Paw DMG"][level]);
        var sk = new DamageBase("IcetideVortex", dmg, Vision, 1);
        SkillFrame = t <= 1 ? 15 : 24;
        int pawall = t <= 1 ? 2 : 5; // 猫抓数
        ElementSkill.CD = t <= 1 ? 6 : 15;
        ElementSkill.Cast();
        // 造成伤害
        for (int paw = 0; paw < pawall; paw++)
        {
            GameManager.GetInstance().DealDamage(this, sk);
            // 每个猫抓都产球
            int seed = UnityEngine.Random.Range(0, 4);
            int n = seed < 1 ? 0 : 1;
            for (int i = 0; i < n; i++) GameManager.GetInstance().GetElementParticle(Vision);
        }
    }

    protected override void castBurst(int level)
    {
        // 单次伤害
        float dmg = Convert.ToSingle(qTable["Skill DMG"][level]);
        var sk = new DamageBase("SignatureMix", dmg, Vision, 1);
        // 领域
        float fdmg = Convert.ToSingle(qTable["Continuous Field DMG"][level]);
        var drunkenmist = new DrunkenMist(this, fdmg);
        GameManager.GetInstance().AddBuff(drunkenmist);
    }
}

