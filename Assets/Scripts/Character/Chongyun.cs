using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Chongyun : Character
{
    public Chongyun() : base("chongyun")
    {
        NAFrames = new List<int> { 24, 62 - 24, 124 - 62, 204 - 124 };
        SkillFrame = 57;
        BurstFrame = 135;
    }

    protected override void castSkill(int level, float holdt)
    {
        ElementSkill.Cast();
        var dmg = Convert.ToSingle(eTable["Skill DMG"][level]);
        var sk = new DamageBase("SpiritBlade", dmg, Vision, 2, -1);
        GameManager.GetInstance().DealDamage(this, sk);
        for (int i = 0; i < 4; i++) GameManager.GetInstance().GetElementParticle(ELEMENT.CRYO);
        // 领域
        var duration = Convert.ToSingle(eTable["Field Duration"][level]);
        var chonghualayeredfrost = new ChonghuaLayeredFrost(this, duration);
        GameManager.GetInstance().AddBuff(chonghualayeredfrost);
    }

    protected override void castBurst(int level)
    {
        var dmg = Convert.ToSingle(qTable["Skill DMG"][level]);
        for (int i = 1; i <= 3; i++)
        {
            var sk = new DamageBase($"CloudPartingStar-{i}", dmg, Vision, 1);
            GameManager.GetInstance().DealDamage(this, sk);
        }
    }

}
