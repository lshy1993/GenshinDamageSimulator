using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Talent
{
    public string Name = "TestTalent";
    public int Level = 1;

    public float CD = 0;
    private float CurCD = 0;
    public float Energy = 0;
    private float CurEnergy = 0;

    //public float AnimationTime = 0.1f;
    public float HoldMaxTime = 0;

    public Talent() { }

    public Talent(float cd, float enegy)
    {
        CD = cd;
        Energy = enegy;
        CurEnergy = enegy;
        //Rate = rate;
    }

    public void Update(float dt)
    {
        if (CurCD <= 0) return;
        CurCD = Math.Max(0, CurCD - dt);
    }

    public float GetCDProgress()
    {
        return CurCD / CD;
    }

    public float GetEnergyProgress()
    {
        return CurEnergy / Energy;
    }

    public string GetStringCD()
    {
        return CurCD > 0 ? CurCD.ToString("F1") : "";
    }

    public bool CanCast()
    {
        return CurCD <= 0 && CurEnergy >= Energy;
    }

    public void Cast()
    {
        CurEnergy = GameManager.GetInstance().InfiniteEnergy ? Energy : 0;
        CurCD = CD;
    }

    public void Charge(float t)
    {
        CurEnergy = Math.Min(Energy, CurEnergy + t);
    }
}

