using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Xiangling : Character
{
    public Xiangling() : base("xiangling")
    {
        NAFrames = new List<int> { 12, 38 - 12, 72 - 38, 141 - 72, 167 - 141 };
        SkillFrame = 18;
        BurstFrame = 99;
    }

    protected override void castSkill(int level, float holdt)
    {
        ElementSkill.Cast();
        float rate = Convert.ToSingle(eTable["Flame DMG"][level]);
        var guoba = new Guoba(this, rate);
        GameManager.GetInstance().AddBuff(guoba);
    }
}

