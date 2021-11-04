using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GlacialWaltz : Buff
{
    private DamageBase finalDmg;
    private float interval = 8 / 13f;

    public GlacialWaltz(Character ch, float rate, float duration) : base(ch, "GlacialWaltz", duration)
    {
        finalDmg = new DamageBase(Name, rate, ch.Vision, 1);
    }

    protected override void updateEvent(float dt)
    {
        interval -= dt;
        if (dt <= 0)
        {
            GameManager.GetInstance().DealDamage(Parent, finalDmg);
            interval = 8 / 13f;
        }
    }
}
