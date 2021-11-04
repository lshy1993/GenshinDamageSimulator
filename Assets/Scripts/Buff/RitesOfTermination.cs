using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RitesOfTermination : Buff
{
    private DamageBase dotDmg;
    private float interval = 1.5f;
    private float curtime = 1.5f;

    public RitesOfTermination(Character ch, float rate) : base(ch, "RitesOfTermination", 8)
    {
        dotDmg = new DamageBase("RitesOfTermination", rate, ch.Vision, 1, -1);
    }

    protected override void updateEvent(float dt)
    {
        curtime -= dt;
        if (curtime <= 0)
        {
            GameManager.GetInstance().DealDamage(Parent, dotDmg);
            curtime = interval;
        }
    }
}
