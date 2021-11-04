using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class InspirationField : Buff
{
    private float atkIns;

    public InspirationField(Character ch, float atk, float duration, ELEMENT infuse) : base(ch, "InspirationField", duration)
    {
        atkIns = atk;
        Infuse = infuse;
    }

    public override string GetBuffedName()
    {
        return "ATK";
    }

    public override float GetBuffedNum()
    {
        return atkIns;
    }
}
