using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Thunderbeast : Buff
{
    private DamageBase finalDmg;
    private float interval = 0;

    public Thunderbeast(Character ch, float rate, float duration) : base(ch, "Thunderbeast", duration)
    {
        finalDmg = new DamageBase(Name, rate, ELEMENT.ELECTRO, 1, 1);
    }

    protected override void updateEvent(float dt)
    {
        interval -= dt;
    }

    public override void AttachAttack()
    {
        if (interval > 0) return;
        interval = 1f;
        GameManager.GetInstance().DealDamage(Parent, finalDmg);
    }
}

