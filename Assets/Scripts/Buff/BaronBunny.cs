using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BaronBunny : Buff
{
    private DamageBase finalDmg;

    public BaronBunny(Character ch, float rate) : base(ch, "BaronBunny", 5)
    {
        finalDmg = new DamageBase(Name, rate, ELEMENT.PYRO, 2, -1);
    }

    protected override void finishEvent()
    {
        GameManager.GetInstance().DealDamage(Parent, finalDmg);
    }
}

