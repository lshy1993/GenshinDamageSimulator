using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Xingqiu : Character
{
    public Xingqiu() : base("xingqiu")
    {
        NAFrames = new List<int> { 9, 34, 59 - 34, 116 - 59, 160 - 116 };
        SkillFrame = 82;
        BurstFrame = 29;
    }

    protected override void castSkill(int level, float holdt)
    {
        base.castSkill(level, holdt);
        for (int i = 0; i < 5; i++) GameManager.GetInstance().GetElementParticle(ELEMENT.HYDRO);
    }

    protected override void castBurst(int level)
    {
        var dmg = Convert.ToSingle(qTable["Sword Rain DMG"][level]);
        var duration = Convert.ToSingle(qTable["Duration"][level]);

        var raincutter = new RainCutter(this, dmg, duration);
        GameManager.GetInstance().AddBuff(raincutter);
    }

}
