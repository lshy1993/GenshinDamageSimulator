using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ChonghuaLayeredFrost : Buff
{
    public ChonghuaLayeredFrost(Character ch, float duration) : base(ch, "ChonghuaLayeredFrost", duration)
    {
        Infuse = ch.Vision;
    }

    public override string GetBuffedName()
    {
        return "ATKSPD";
    }

    public override float GetBuffedNum()
    {
        return Parent.Star >= 5 ? 0.08f : 0;
    }
}
