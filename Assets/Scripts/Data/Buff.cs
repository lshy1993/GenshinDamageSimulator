using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class Buff
{
    public string Name;
    protected float Duration;
    public float RestTime;
    public Character Parent;
    public ELEMENT Infuse = ELEMENT.NONE;

    public bool active = true;

    //public Buff(Character ch)
    //{
    //    Parent = ch;
    //}

    public Buff(Character ch, string name, float duration)
    {
        Parent = ch;
        Name = name;
        Duration = duration;
        RestTime = duration;
        active = true;
    }

    public float GetProgress()
    {
        return RestTime / Duration;
    }

    public virtual void Update(float dt)
    {
        if (!active) return;
        updateEvent(dt);
        RestTime -= dt;
        if(RestTime <= 0)
        {
            finishEvent();
            active = false;
        }
    }

    protected virtual void updateEvent(float dt) { }
    protected virtual void finishEvent() { }

    public virtual void AttachAttack() { }

    public virtual string GetBuffedName()
    {
        return "";
    }
    public virtual float GetBuffedNum()
    {
        return 0;
    }
}
