using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ElementBase
{
    /// <summary>
    /// 元素量
    /// </summary>
    public float Amount = 1;
    /// <summary>
    /// 持续时间
    /// </summary>
    public float Time = 9.5f;
    public ELEMENT Type;
    public string From;

    // 刷新机制
    public int HitCount = 1;
    public float CD = 2.5f;

    private float spd;

    public ElementBase(ELEMENT tp, float amt, float time, string fm)
    {
        Type = tp;
        Amount = amt;
        Time = time;
        spd = Amount / time;
        From = fm;
    }

    public void Update(float dt)
    {
        Amount -= dt * spd;
        CD -= dt;
    }

    public void AddAmount(float amt)
    {
        Amount = Math.Max(Amount, amt);
    }

    public bool CanAttach()
    {
        return CD <= 0 || HitCount == 0;
    }

    public void Reset()
    {
        CD = 2.5f;
        HitCount = 4;
    }
}
