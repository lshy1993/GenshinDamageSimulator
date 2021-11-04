using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class FieryRain : Buff
{
    private DamageBase waveDmg;
    private float interval;
    private int waveNum;

    public FieryRain(Character ch, float wdmg, float tdmg, float duration) : base(ch, "FieryRain", duration)
    {
        waveNum = Mathf.FloorToInt(tdmg / wdmg);
        interval = duration / waveNum;
        waveDmg = new DamageBase(Name, wdmg, ELEMENT.PYRO, 1, 1);
    }

    protected override void updateEvent(float dt)
    {
        if(Mathf.CeilToInt(RestTime / interval) < waveNum)
        {
            waveNum--;
            GameManager.GetInstance().DealDamage(Parent, waveDmg);
        }
    }
}

