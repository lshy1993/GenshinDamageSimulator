using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Ayaka : Character
{
    public Ayaka() : base("ayaka")
    {
        NAFrames = new List<int> { 8, 28 - 8, 56 - 28, 98 - 56, 136 - 98 };
        SkillFrame = 56;
        BurstFrame = 95;
    }

    protected override void castSkill(int level, float holdt)
    {
        ElementSkill.Cast();
        // 伤害
        float rate = Convert.ToSingle(eTable["Skill DMG"][level]);
        var dmg = new DamageBase("Hyoka", rate, Vision, 2, -1);
        GameManager.GetInstance().DealDamage(this, dmg);
        // 产球
        int seed = UnityEngine.Random.Range(0, 2);
        int n = seed == 0 ? 4 : 5;
        for (int i = 0; i < n; i++) GameManager.GetInstance().GetElementParticle(Vision);
    }

    protected override void castBurst(int level)
    {
        float cutrate = Convert.ToSingle(qTable["Cutting DMG"][level]);
        float bloomrate = Convert.ToSingle(qTable["Bloom DMG"][level]);

        var soumetsu = new SekiNoTo(this, cutrate, bloomrate);
        GameManager.GetInstance().AddBuff(soumetsu);
    }
}
