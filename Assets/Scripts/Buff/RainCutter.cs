using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RainCutter : Buff
{
    private DamageBase finalDmg;
    private int wave = 0;
    private float interval = 0;

    public RainCutter(Character ch, float rate, float duration) : base(ch, "RainCutter", duration)
    {
        finalDmg = new DamageBase(Name, rate, ELEMENT.HYDRO, 1, 1);
    }

    protected override void updateEvent(float dt)
    {
        interval -= dt;
    }

    public override void AttachAttack()
    {
        if (interval > 0) return;
        interval = 1f;
        int num = wave % 2 == 0 ? 2 : 3;
        if (Parent.Constellations == 6) num = GetSwordNum(wave);
        wave += 1;
        for (int i = 0; i < num; i++) GameManager.GetInstance().DealDamage(Parent, finalDmg);
    }

    private int GetSwordNum(int wave)
    {
        switch (wave % 3)
        {
            case 0:
                return 2;
            case 1:
                return 3;
            case 2:
                return 5;
        }
        return 2;
    }
}

