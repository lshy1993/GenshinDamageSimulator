using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Amber : Character
{
    public Amber() : base("amber")
    {
        NAFrames = new List<int> { 15, 33 - 15, 72 - 33, 113 - 72, 144 - 113 };
        SkillFrame = 35;
        BurstFrame = 135;
    }

    protected override void castSkill(int level, float t)
    {
        ElementSkill.Cast();
        // bunny
        float rate = Convert.ToSingle(eTable["Explosion DMG"][level]);
        var bunny = new BaronBunny(this, rate);
        GameManager.GetInstance().AddBuff(bunny);
        // 产球
        for (int i = 0; i < 4; i++) GameManager.GetInstance().GetElementParticle(ELEMENT.PYRO);
    }

    protected override void castBurst(int level)
    {
        float wdmg = Convert.ToSingle(qTable["Fiery Rain DMG Per Wave"][level]);
        float tdmg = Convert.ToSingle(qTable["Total Fiery Rain DMG"][level]);
        float duration = Convert.ToSingle(qTable["Duration"][level]);

        var fieryrain = new FieryRain(this, wdmg, tdmg, duration);
        GameManager.GetInstance().AddBuff(fieryrain);
    }
}
