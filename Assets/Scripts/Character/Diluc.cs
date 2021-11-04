using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Diluc : Character
{
    private int eHit = 1;
    private float infuseTime = 0;

    public Diluc() : base("diluc")
    {
        NAFrames = new List<int> { 24, 77 - 24, 115 - 77, 181 - 115 };
        SkillFrame = 45;
        BurstFrame = 145;
    }

    public override void CastNormalAttack()
    {
        NormalAttack.Cast();
        animecd = NAFrames[hitnum - 1] / 60f;
        hitnum = naTable.ContainsKey($"{hitnum + 1}-Hit DMG") ? hitnum + 1 : 1;
        int level = NormalAttack.Level;// + passive ? 1 : 0;
        ELEMENT infused = GetInfuse();
        if (infuseTime > 0) infused = ELEMENT.PYRO;
        castNormalAttack(level, infused);
    }

    public override void Update(float dt)
    {
        if (infuseTime > 0) infuseTime -= dt;
        base.Update(dt);
    }

    protected override void castSkill(int level, float holdt)
    {
        //ElementSkill.CD = 0;
        float rate = Convert.ToSingle(eTable[$"{eHit}-Hit DMG"][level]);
        eHit++;
        if (eHit == 4)
        {
            eHit = 1;
            ElementSkill.CD = 10;
            ElementSkill.Cast();
        }
        // 造成伤害
        var sk = new DamageBase($"SearingOnslaught{eHit}", rate, Vision, 1);
        GameManager.GetInstance().DealDamage(this, sk);
        int seed = UnityEngine.Random.Range(0, 3);
        int n = seed < 1 ? 2 : 1;
        for (int i = 0; i < n; i++) GameManager.GetInstance().GetElementParticle(Vision);
    }

    protected override void castBurst(int level)
    {
        float rate = Convert.ToSingle(qTable[$"Slashing DMG"][level]);
        var sk = new DamageBase("ElementBurst", rate, Vision, 2);
        GameManager.GetInstance().DealDamage(this, sk);
        
        float rate1 = Convert.ToSingle(qTable[$"Explosion DMG"][level]);
        var sk1 = new DamageBase("ElementBurst", rate1, Vision, 2);
        GameManager.GetInstance().DealDamage(this, sk1);
        // 附魔
        infuseTime = 8;
    }
}
