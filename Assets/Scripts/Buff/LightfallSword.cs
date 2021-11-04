using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LightfallSword : Buff
{
    private float baseDmg, stackDmg;
    private int stackNum = 5;
    private float interval = 0.1f;

    public LightfallSword(Character ch, float bdmg, float sdmg) : base(ch, "LightfallSword", 10)
    {
        baseDmg = bdmg;
        stackDmg = sdmg;
    }

    protected override void updateEvent(float dt)
    {
        interval -= dt;
    }

    public override void AttachAttack()
    {
        if (interval > 0) return;
        interval = 0.1f;
        stackNum = Math.Min(30, stackNum + 1);
    }

    protected override void finishEvent()
    {
        float fdmg = baseDmg + stackDmg * stackNum;
        DamageBase finalDmg = new DamageBase(Name, fdmg, ELEMENT.NONE, 1, -1);
        GameManager.GetInstance().DealDamage(Parent, finalDmg);
    }
}

