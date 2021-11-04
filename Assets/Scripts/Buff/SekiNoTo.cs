using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SekiNoTo : Buff
{
    private DamageBase cutdmg, bloomdmg;
    private float interval = 5f / 19;
    private float curtime = 5f / 19;

    public SekiNoTo(Character ch, float cdmg, float bdmg) : base(ch, "SekiNoTo", 5)
    {
        cutdmg = new DamageBase("SekiNoTo Cut", cdmg, ch.Vision, 1, -1);
        bloomdmg = new DamageBase("SekiNoTo Bloom", bdmg, ch.Vision, 1, -1);
    }

    protected override void updateEvent(float dt)
    {
        curtime -= dt;
        if (curtime <= 0)
        {
            GameManager.GetInstance().DealDamage(Parent, cutdmg);
            curtime = interval;
        }
    }

    protected override void finishEvent()
    {
        GameManager.GetInstance().DealDamage(Parent, bloomdmg);
    }
}
