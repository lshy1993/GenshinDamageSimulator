using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Beidou : Character
{
    public Beidou() : base("beidou")
    {
        NAFrames = new List<int> { 23, 66 - 23, 134 - 66, 178 - 134, 246 - 178 };
        SkillFrame = 41;
        BurstFrame = 45;
    }

    protected override void castSkill(int level, float t)
    {
        ElementSkill.Cast();

        float rate = Convert.ToSingle(eTable["Base DMG"][level]);
        var sk = new DamageBase("ElementSkill", rate, Vision, 2);
        GameManager.GetInstance().DealDamage(this, sk);
        for (int i = 0; i < 2; i++) GameManager.GetInstance().GetElementParticle(ELEMENT.ELECTRO);
    }

    protected override void castBurst(int level)
    {
        float rate = Convert.ToSingle(qTable["Skill DMG"][level]);
        var sk = new DamageBase("ElementBurst", rate, Vision, 4);
        GameManager.GetInstance().DealDamage(this, sk);

        var dmg = Convert.ToSingle(qTable["Lightning DMG"][level]);
        var duration = Convert.ToSingle(qTable["Duration"][level]);
        var thunderbeast = new Thunderbeast(this, dmg, duration);
        GameManager.GetInstance().AddBuff(thunderbeast);
    }

}
