using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Rosaria : Character
{
    public Rosaria() : base("rosaria")
    {
        NAFrames = new List<int>() { 10, 36 - 10, 81 - 36, 115 - 81, 175 - 115 };
        SkillFrame = 65;
        BurstFrame = 74;
    }

    protected override void castSkill(int level, float holdt)
    {
        ElementSkill.Cast();
        // 伤害
        foreach(var ch in eTable["Skill DMG"][level].Split('+'))
        {            
            float dmg = Convert.ToSingle(ch);            
            var db = new DamageBase("RavagingConfession", dmg, Vision, 1);
            GameManager.GetInstance().DealDamage(this, db);
        }
        // 产球
        int n = 3;
        for (int i = 0; i < n; i++) GameManager.GetInstance().GetElementParticle(Vision);
    }

    protected override void castBurst(int level)
    {
        // 伤害
        foreach (var ch in eTable["Skill DMG"][level].Split('+'))
        {
            float dmg = Convert.ToSingle(ch);
            var db = new DamageBase("RavagingConfession", dmg, Vision, 1);
            GameManager.GetInstance().DealDamage(this, db);
        }
        // DOT
        float dot = Convert.ToSingle(qTable["Ice Lance DoT"][Level]);
        var ritesoftermination = new RitesOfTermination(this, dot);
        GameManager.GetInstance().AddBuff(ritesoftermination);
    }
}
