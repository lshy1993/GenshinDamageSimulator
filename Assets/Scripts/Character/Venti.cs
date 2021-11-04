using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Venti : Character
{
    public Venti() : base("venti")
    {
        NAFrames = new List<int> { 21, 44 - 21, 90 - 44, 123 - 90, 140 - 123, 191 - 140 };
        SkillFrame = 20;
        BurstFrame = 94;
    }

    protected override void castSkill(int level, float holdt)
    {
        if(holdt < 1)
        {
            SkillFrame = 20;
            ElementSkill.CD = 6;
            ElementSkill.Cast();
            float rate = Convert.ToSingle(eTable["Press DMG"][level]);
            var dmg = new DamageBase("", rate, Vision, 2, -1);
            GameManager.GetInstance().DealDamage(this, dmg);
            for (int i = 0; i < 3; i++) GameManager.GetInstance().GetElementParticle(ELEMENT.ANEMO);
        }
        else
        {
            SkillFrame = 70;
            ElementSkill.CD = 15;
            ElementSkill.Cast();
            float rate = Convert.ToSingle(eTable["Hold DMG"][level]);
            var dmg = new DamageBase("", rate, Vision, 2, -1);
            GameManager.GetInstance().DealDamage(this, dmg);
            for (int i = 0; i < 4; i++) GameManager.GetInstance().GetElementParticle(ELEMENT.ANEMO);
        }
    }

    protected override void castBurst(int level)
    {
        float dot = Convert.ToSingle(qTable["DoT"][level]);
        float addrate = Convert.ToSingle(qTable["Additional Elemental DMG"][level]);
        var stormeye = new Stormeye(this, dot, addrate);
        GameManager.GetInstance().AddBuff(stormeye);
    }
}
