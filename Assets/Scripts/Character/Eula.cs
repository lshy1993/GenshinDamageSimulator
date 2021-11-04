using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Eula : Character
{
    public Eula() : base("eula")
    {
        NAFrames = new List<int> { 30, 58 - 30, 126 - 58, 161 - 126, 250 - 161 };
        ElementSkill.HoldMaxTime = 1.0f;
        SkillFrame = 65;
        BurstFrame = 125;
    }

    private int grimheart = 0;

    protected override void castSkill(int level, float t)
    {
        if (t <= 1)
        {
            SkillFrame = 65;
            ElementSkill.CD = 4;
            ElementSkill.Cast();
            // 造成伤害
            float dmg = Convert.ToSingle(eTable["Press DMG"][level]);
            var sk = new DamageBase("IcetideVortex", dmg, Vision, 1);
            GameManager.GetInstance().DealDamage(this, sk);
            // 堆叠冷酷之心
            grimheart = Math.Min(2, grimheart + 1);
            // 产球
            int seed = UnityEngine.Random.Range(0, 2);
            int n = seed < 1 ? 1 : 2;
            for (int i = 0; i < n; i++) GameManager.GetInstance().GetElementParticle(Vision);
        }
        else
        {
            SkillFrame = 89;
            ElementSkill.CD = 10;
            ElementSkill.Cast();
            // 造成伤害
            float dmg = Convert.ToSingle(eTable["Hold DMG"][level]);
            var sk = new DamageBase("IcetideVortex", dmg, Vision, 1);
            GameManager.GetInstance().DealDamage(this, sk);
            // 冷酷之心效果
            float ddmg = Convert.ToSingle(eTable["Icewhirl Brand DMG"][level]);
            var ssk = new DamageBase("IcewhirlBrand", dmg, Vision, 1);
            for (int i = 0; i < grimheart; i++) GameManager.GetInstance().DealDamage(this, ssk);
            grimheart = 0;
            // 产球
            int seed = UnityEngine.Random.Range(0, 2);
            int n = seed < 1 ? 2 : 3;
            for (int i = 0; i < n; i++) GameManager.GetInstance().GetElementParticle(Vision);
        }
    }

    protected override void castBurst(int level)
    {
        // 伤害
        float dmg = Convert.ToSingle(qTable["Skill DMG"][level]);
        var sk = new DamageBase("GlacialIllumination", dmg, Vision, 2);
        GameManager.GetInstance().DealDamage(this, sk);
        // 光剑
        float basedmg = Convert.ToSingle(qTable["Lightfall Sword Base DMG"][level]);
        float stackdmg = Convert.ToSingle(qTable["DMG Per Stack"][level]);
        var lightfallsword = new LightfallSword(this, basedmg, stackdmg);
        GameManager.GetInstance().AddBuff(lightfallsword);
    }
}
