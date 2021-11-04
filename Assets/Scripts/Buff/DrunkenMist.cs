using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DrunkenMist : Buff
{
    private DamageBase finalDmg;

    public DrunkenMist(Character ch, float dmg) : base(ch, "DrunkenMist", 12)
    {
        finalDmg = new DamageBase("SignatureMix", dmg, ch.Vision, 1);
    }
}

