using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Stormeye : Buff
{
    private int hit = 0;
    private int infusehit = 0;
    private ELEMENT core = ELEMENT.ANEMO;
    private float dotrate, elerate;
    private float interval = 8 / 20f;
    private float curTime = 0;

    public Stormeye(Character ch, float dotrate, float addrate) : base(ch, "Stormeye", 8)
    {
        this.dotrate = dotrate;
        this.elerate = addrate;
    }

    protected override void updateEvent(float dt)
    {
        curTime -= dt;
        if(curTime <= 0)
        {
            var dmg = new DamageBase("Stormeye Dmg", dotrate, ELEMENT.ANEMO, 0, -1);
            if (hit % 3 == 0) dmg.EleAmout = 1; // 每3次挂风
            GameManager.GetInstance().DealDamage(Parent, dmg);
            hit++;
            if (hit > 4 && core == ELEMENT.ANEMO)
            {
                // 染色判定
            }
            // 染色后的附加伤害
            if (isInfused(core))
            {
                var eledmg = new DamageBase("Stormeye AdditionElement Dmg", elerate, core, 0, -1);
                if (infusehit % 3 == 0) eledmg.EleAmout = 1;
                GameManager.GetInstance().DealDamage(Parent, eledmg);
                infusehit++;
            }
            curTime = interval;
        }
    }

    private bool isInfused(ELEMENT core)
    {
        return core == ELEMENT.CRYO || core == ELEMENT.PYRO || core == ELEMENT.HYDRO || core == ELEMENT.ELECTRO;
    }
}

