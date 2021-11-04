using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DamageBase
{
    public string Name;

    /// <summary>
    /// 技能倍率
    /// </summary>
    public float SkillRate;

    /// <summary>
    /// 元素
    /// </summary>
    public ELEMENT Element = ELEMENT.NONE;
    public int EleAmout;

    /// <summary>
    /// 打击个数
    /// </summary>
    public int TargetNum = 1;

    /// <summary>
    /// 伤害类
    /// </summary>
    /// <param name="name">名字</param>
    /// <param name="skillrate">基础伤害倍率</param>
    /// <param name="eletype">元素类型</param>
    /// <param name="eleamt">元素量</param>
    /// <param name="target">作用对象数</param>
    public DamageBase(string name, float skillrate, ELEMENT eletype, int eleamt, int target = 1)
    {
        Name = name;
        SkillRate = skillrate;
        Element = eletype;
        EleAmout = eleamt;
        TargetNum = target;
    }
}

