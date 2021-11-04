using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Oz : Buff
{
    private DamageBase ozDmg;
    private float interval = 1f;

    public Oz(Character ch, float ozdmg, float duration) : base(ch, "Oz", duration)
    {
        ozDmg = new DamageBase("OzAttack", ozdmg, ELEMENT.ELECTRO, 1);
    }

    protected override void updateEvent(float dt)
    {
        interval -= dt;
        if (interval <= 0)
        {
            interval = 1f;
            AutoAttack();
        }
    }

    public override void AttachAttack()
    {
        AutoAttack();
    }

    private void AutoAttack()
    {
        GameManager.GetInstance().DealDamage(Parent, ozDmg);
    }
}
