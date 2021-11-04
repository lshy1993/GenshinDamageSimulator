using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Bennett : Character
{
    public Bennett() : base("bennett")
    {
        NAFrames = new List<int> { 12, 32 - 12, 63 - 32, 118 - 63, 167 - 118 };
        ElementSkill.HoldMaxTime = 2;
        SkillFrame = 52;
        BurstFrame = 51;
    }

    protected override void castSkill(int level, float t)
    {
        if (t <= 1)
        {
            ElementSkill.CD = 5;
            ElementSkill.Cast();
            // 造成伤害
            var dmg = Convert.ToSingle(eTable["Press DMG"][level]);
            var sk = new DamageBase("PassionOverload", dmg, Vision, 2);
            GameManager.GetInstance().DealDamage(this, sk);
            var seed = UnityEngine.Random.Range(0, 3);
            int n = seed < 1 ? 3 : 2;
            for (int i = 0; i < n; i++) GameManager.GetInstance().GetElementParticle(ELEMENT.PYRO);
        }
        else
        {
            ElementSkill.CD = t < 2.5 ? 7.5f : 10;
            ElementSkill.Cast();
            // 造成伤害
            var data = t <= 2.5 ? eTable["Charge Level 1 DMG"][level] : eTable["Charge Level 2 DMG"][level];
            foreach (var ch in data.Split('+'))
            {
                float rate = Convert.ToSingle(ch);
                var skk = new DamageBase("PassionOverload", rate, Vision, 1);
                GameManager.GetInstance().DealDamage(this, skk);
            }
            var dmg = Convert.ToSingle(eTable["Explosion DMG"][level]);
            var sk = new DamageBase("PassionOverload", dmg, Vision, 1);
            GameManager.GetInstance().DealDamage(this, sk);
            for (int i = 0; i < 3; i++) GameManager.GetInstance().GetElementParticle(ELEMENT.PYRO);
        }
    }

    protected override void castBurst(int level)
    {
        var rate = GetBaseATK() * Convert.ToSingle(qTable["ATK Bonus Ratio"][level]);
        var duration = Convert.ToSingle(qTable["Duration"][level]);
        ELEMENT infuse = Constellations >= 6 ? ELEMENT.PYRO : ELEMENT.NONE;
        var inspirationfield = new InspirationField(this, rate, duration, infuse);
        GameManager.GetInstance().AddBuff(inspirationfield);
    }
}
