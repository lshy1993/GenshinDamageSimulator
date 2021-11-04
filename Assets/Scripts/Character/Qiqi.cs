using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Qiqi : Character
{
    public Qiqi() : base("qiqi")
    {
        NAFrames = new List<int>() { 11, 29 - 11, 71 - 29, 111 - 71, 140 - 111 };
        SkillFrame = 57;
        BurstFrame = 112;
    }

    protected override void castSkill(int level, float holdt)
    {
        ElementSkill.Cast();
        // 单次伤害
        float rate = Convert.ToSingle(eTable["Skill DMG"][level]);        
        var dmg = new DamageBase("Herald of Frost", rate, Vision, 1, -1);
        GameManager.GetInstance().DealDamage(this, dmg);
        // buff
        float hitrate = Convert.ToSingle(eTable["Herald of Frost DMG"][level]);
        var heraldoffrost = new HeraldOfFrost(this, hitrate);
        GameManager.GetInstance().AddBuff(heraldoffrost);
    }

    protected override void castBurst(int level)
    {
        float rate = Convert.ToSingle(qTable["Skill DMG"][level]);
        var dmg = new DamageBase("PreserverOfFortune", rate, Vision, 2, -1);
        GameManager.GetInstance().DealDamage(this, dmg);
    }
}
