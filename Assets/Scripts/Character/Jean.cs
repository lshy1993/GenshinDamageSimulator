using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Jean : Character
{
    public Jean() : base("jean")
    {
        NAFrames = new List<int> { 14, 37 - 14, 66 - 37, 124 - 66, 159 - 124 };
        ElementSkill.HoldMaxTime = 5;
        SkillFrame = 46;
        BurstFrame = 88;
    }

    protected override void castSkill(int level, float holdt)
    {
        ElementSkill.Cast();
        // 伤害
        float rate = Convert.ToSingle(eTable["Skill DMG"][level]);
        var dmg = new DamageBase("GaleBlade", rate, Vision, 2, -1);
        // 产球
        int seed = UnityEngine.Random.Range(0, 2);
        int n = seed == 0 ? 2 : 3;
        for (int i = 0; i < n; i++) GameManager.GetInstance().GetElementParticle(Vision);
    }

    protected override void castBurst(int level)
    {
        float drate = Convert.ToSingle(qTable["Elemental Burst DMG"][level]);
        var dmg = new DamageBase("DandelionBreeze", drate, Vision, 2, -1);
        GameManager.GetInstance().DealDamage(this, dmg);
        // 生成领域
        float brate = Convert.ToSingle(qTable["Field Entering/Exiting DMG"][level]);
        var dandelionbreeze = new DandelionBreeze(this, brate);
        GameManager.GetInstance().AddBuff(dandelionbreeze);
    }
}
