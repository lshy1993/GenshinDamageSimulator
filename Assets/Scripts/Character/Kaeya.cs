using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Kaeya : Character
{
    public Kaeya() : base("kaeya")
    {
        NAFrames = new List<int>() { 14, 27, 28, 56, 48 };
        SkillFrame = 58;
        BurstFrame = 78;
    }

    protected override void castSkill(int level, float t)
    {
        ElementSkill.Cast();
        float dmg = Convert.ToSingle(eTable["Skill DMG"][level]);
        // 伤害
        var db = new DamageBase("Frostgnaw", dmg, Vision, 2, -1);
        GameManager.GetInstance().DealDamage(this, db);
        // 产球
        int seed = UnityEngine.Random.Range(0, 3);
        int n = seed < 1 ? 2 : 3;
        for (int i = 0; i < n; i++) GameManager.GetInstance().GetElementParticle(Vision);
    }

    protected override void castBurst(int level)
    {
        var dmg = Convert.ToSingle(qTable["Skill DMG"][level]);
        var duration = Convert.ToSingle(qTable["Duration"][level]);

        var glacialwaltz = new GlacialWaltz(this, dmg, duration);
        GameManager.GetInstance().AddBuff(glacialwaltz);
    }
}
