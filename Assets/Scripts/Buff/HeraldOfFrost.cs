using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HeraldOfFrost : Buff
{
    private DamageBase hitdmg;
    private float interval = 1f;
    private float curtime = 1f;

    public HeraldOfFrost(Character ch, float dmg) : base(ch, "HeraldofFrost", 15)
    {
        hitdmg = new DamageBase("HeraldofFrost Hit", dmg, ch.Vision, 1, -1);
    }

    protected override void updateEvent(float dt)
    {
        curtime -= dt;
        if (curtime <= 0)
        {
            GameManager.GetInstance().DealDamage(Parent, hitdmg);
            curtime = interval;
        }
    }
}
