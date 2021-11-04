using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HuTao : Character
{
    private float skilltime = 0;
    private float atkincrease = 0;
    private float icd = 0;// 产球

    public HuTao() : base("hutao")
    {
        NAFrames = new List<int> { 13, 29 - 13, 54 - 29, 90 - 54, 120 - 90, 173 - 120 };
        SkillFrame = 42;
        BurstFrame = 130;
    }

    public override void Update(float dt)
    {
        if (skilltime > 0) skilltime -= dt;
        if (skilltime <= 0) atkincrease = 0;
        if (icd > 0) icd -= dt;
        base.Update(dt);
    }

    public override float GetATK()
    {
        float a = GetBaseATK();
        float b = atkincrease * GetHP();
        b = Math.Min(b, 4 * a);
        return base.GetATK() + b;
    }

    public override void CastNormalAttack()
    {
        NormalAttack.Cast();
        animecd = NAFrames[hitnum - 1] / 60f;
        hitnum = naTable.ContainsKey($"{hitnum + 1}-Hit DMG") ? hitnum + 1 : 1;
        int level = NormalAttack.Level;// + passive ? 1 : 0;
        ELEMENT infused = GetInfuse();
        if (skilltime > 0) infused = ELEMENT.PYRO;
        castNormalAttack(level, infused);
        // 产球
        if(icd <= 0)
        {

        }
    }

    protected override void castSkill(int level, float holdt)
    {
        ElementSkill.Cast();
        // HP
        CurHP *= 0.7f;
        // 自身附魔
        skilltime = 9;
        atkincrease = Convert.ToSingle(eTable["ATK Increase"][level]);
    }

    protected override void castBurst(int level)
    {
        float rate = Convert.ToSingle(qTable["Skill DMG"][level]);
        if(CurHP < GetHP() * 0.5)
        {
            rate = Convert.ToSingle(qTable["Low HP Skill DMG"][level]);
        }
        var dmg = new DamageBase("SpiritSoother", rate, Vision, 2, 5);
        GameManager.GetInstance().DealDamage(this, dmg);
    }
}
