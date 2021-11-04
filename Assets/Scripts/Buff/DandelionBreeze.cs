using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DandelionBreeze : Buff
{
    private DamageBase borderDmg;

    public DandelionBreeze(Character ch, float rate) : base(ch, "DandelionBreeze", 10)
    {
        borderDmg = new DamageBase("DandelionBreeze", rate, ELEMENT.ANEMO, 2);
    }
}
