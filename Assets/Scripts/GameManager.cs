using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public enum ELEMENT
{
    NONE, ANEMO, GEO, DENDRO, ELECTRO, HYDRO, CRYO, PYRO //, PHYSICAL
}

public enum WEAPONTYPE
{
    SWORD, CLAYMORE, POLEARM, BOW, CATALYST
}

public enum REACTION
{
    NONE, MELT, VAPORIZE, FREEZE, OVERLOAD, ELECTROCHARGED, SUPERCONDUCT, SWIRL
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager GetInstance()
    {
        if (instance == null) instance = GameObject.Find("GameManager").GetComponent<GameManager>();
        return instance;
    }

    public SkillManager skm;
    public StatusManager stm;
    public LogManager lgm;
    public GameObject DmgNumPanel;
    public GameObject prefab;

    public Character[] teams = new Character[4];
    public List<Monster> monsters = new List<Monster>();
    public List<Buff> envs = new List<Buff>();

    private int curChara = 0;
    public bool InfiniteHP;
    public bool InfiniteEnergy;

    private float swithcd = 0;
    private float keyHold = 0;

    private void Awake()
    {
        DataManager.GetInstance().Init();
        //teams[0] = new Amber();
        teams[0] = new HuTao();
        teams[1] = new Kaeya();
        teams[2] = new Qiqi();
        teams[3] = new Ayaka();

        monsters.Add(new Monster("123"));

        InfiniteEnergy = true;
        InfiniteHP = true;

        Debug.Log("GM Init");
    }

    void Update()
    {
        if (!DataManager.GetInstance().GamePlaying) return;
        for (int i = envs.Count - 1; i >= 0; i--)
        {
            envs[i].Update(Time.deltaTime);
            //if (envs[i].GetProgress() <= 0) envs.RemoveAt(i);
        }
        foreach (var chh in teams)
        {
            chh.Update(Time.deltaTime);
        }
        foreach(var mon in monsters)
        {
            mon.Update(Time.deltaTime);
        }

        swithcd -= Time.deltaTime;
        if (swithcd < 0) swithcd = 0;

        if (keyHold > 0) keyHold += Time.deltaTime;
        if (teams[curChara].ElementSkill.HoldMaxTime !=0 && keyHold >= teams[curChara].ElementSkill.HoldMaxTime)
        {
            CastSkill();
        }

        KeyBinding();
    }

    private void KeyBinding()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            HoldSkill();
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            CastSkill();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            CastBurst();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwichChara(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwichChara(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwichChara(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwichChara(3);
        }
    }

    public void NormalAttack()
    {
        var ch = teams[curChara];
        if (ch.CanCastNormalAttack())
        {
            ch.CastNormalAttack();
            foreach(var buff in envs)
            {
                buff.AttachAttack();
            }
        }
    }

    private void HoldSkill()
    {
        keyHold += Time.deltaTime;
    }

    private void CastSkill()
    {
        if (keyHold == 0) return;
        var ch = teams[curChara];
        if (ch.CanCastSkill())
        {
            //Debug.Log(keyHold);
            ch.CastSkill(keyHold);
        }
        keyHold = 0;
    }

    private void CastBurst()
    {
        var ch = teams[curChara];
        if (ch.CanCastBurst())
        {
            ch.CastBurst();
        }
    }

    private void SwichChara(int x)
    {
        if (curChara == x) return;
        if (swithcd > 0) return;
        if (!teams[curChara].CanSwitch()) return;
        curChara = x;
        skm.id = x;
        swithcd = 1;
    }

    public void SwitchInfiniteHP(bool flag)
    {
        InfiniteHP = flag;
        Debug.Log($"Infinite HP {flag}");
    }
    public void SwitchInfiniteEnergy(bool flag)
    {
        InfiniteEnergy = flag;
        Debug.Log($"Infinite Energy {flag}");
    }

    public void AddBuff(Buff buff)
    {
        envs.Add(buff);
    }

    public void DealDamage(Character ch, DamageBase sk)
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            if (sk.TargetNum != -1 && i >= sk.TargetNum) continue;
            Monster Enemy = monsters[i];
            float dmg = ch.GetATK() * sk.SkillRate * (1 + ch.GetBonus(sk.Element));
            float seed = UnityEngine.Random.Range(0, 1f);
            bool critflag = seed <= ch.GetCritRate();
            if (critflag) dmg *= (1 + ch.GetCritDamage());
            dmg *= (100 + ch.Level) / ((100 + ch.Level) + (100 + Enemy.Level) * (1 - Enemy.DefenceReduction));
            var res = Enemy.GetResist(sk.Element);
            if (res > 0.75f)
            {
                dmg *= 1 / (1 + 4 * res);
            }
            else if (res >= 0)
            {
                dmg *= 1 - res;
            }
            else
            {
                dmg *= 1 - res / 2;
            }
            //dmg * =(Bonus DMG % taken by opponents);
            REACTION elereaction = REACTION.NONE;
            if(sk.Element != ELEMENT.NONE)
            {
                float t = Const.GetElementTime(sk.EleAmout);
                var eb = new ElementBase(sk.Element, sk.EleAmout, t, ch.Name + sk.Name);
                elereaction = Enemy.ElementReactionCheck(eb);
                int EM = Mathf.FloorToInt(ch.GetElementalMastery());
                float LM = Const.GetLevelMultiplier(ch.Level);
                // 反应区
                switch (elereaction)
                {
                    case REACTION.MELT:
                        if (sk.Element == ELEMENT.PYRO)
                        {
                            // 火->融化                            
                            dmg *= 2f * (1 + 2.78f * EM / (1400 + EM));
                        }
                        else if (sk.Element == ELEMENT.CRYO)
                        {
                            // 冰->融化
                            dmg *= 1.5f * (1 + 2.78f * EM / (1400 + EM));
                        }
                        break;
                    case REACTION.VAPORIZE:
                        if (sk.Element == ELEMENT.HYDRO)
                        {
                            // 水 -> 蒸发
                            dmg *= 2f * (1 + 2.78f * EM / (1400 + EM));
                        }
                        else if (sk.Element == ELEMENT.PYRO)
                        {
                            // 火 -> 蒸发
                            dmg *= 1.5f * (1 + 2.78f * EM / (1400 + EM));
                        }
                        break;
                }
                Damage(ch, Enemy, sk.Name, sk.Element, dmg, critflag, elereaction);
                // 增幅反应的额外伤害
                switch (elereaction)
                { 
                    case REACTION.OVERLOAD:
                        // 超载
                        float odmg = 4 * (1 + 16 * EM / (2000 + EM)) * LM;
                        TransformativeDamage(ch, "OVERLOAD", ELEMENT.PYRO, odmg, -1);
                        break;
                    case REACTION.ELECTROCHARGED:
                        // 感电
                        float edmg = 3 * (1 + 16 * EM / (2000 + EM)) * LM;
                        TransformativeDamage(ch, "ELECTROCHARGED", ELEMENT.ELECTRO, edmg, -1);
                        break;
                    case REACTION.SUPERCONDUCT:
                        float sudmg = 1 * (1 + 16 * EM / (2000 + EM)) * LM;
                        TransformativeDamage(ch, "SUPERCONDUCT", ELEMENT.CRYO, sudmg, 1);
                        break;
                    case REACTION.SWIRL:
                        // 扩散
                        float swdmg = 1.2f * (1 + 16 * EM / (2000 + EM)) * LM;
                        break;
                }
            }
            else
            {
                // 碎冰处理
                Damage(ch, Enemy, sk.Name, sk.Element, dmg, critflag, elereaction);
            }            
        }
    }

    private void Damage(Character ch, Monster Enemy, string dmgname, ELEMENT dmgele, float dmg, bool critflag, REACTION elereaction )
    {
        int final = Mathf.RoundToInt(dmg);
        if (!InfiniteHP) Enemy.GetDamage(final);
        var go = Instantiate(prefab, DmgNumPanel.transform);
        go.GetComponent<DamageNum>().ShowNum(dmgele, final, elereaction);
        lgm.Log(ch, dmgname, dmgele, elereaction, critflag, final);
    }

    private void TransformativeDamage(Character ch, string dmgname, ELEMENT dmgele, float dmg, int targetNum, int flag = -1)
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            if (targetNum != -1 && i >= targetNum) continue;
            Monster Enemy = monsters[i];
            int final = Mathf.RoundToInt(dmg);
            if (!InfiniteHP) Enemy.GetDamage(final);
            var go = Instantiate(prefab, DmgNumPanel.transform);
            go.GetComponent<DamageNum>().ShowNum(dmgele, final, REACTION.NONE);
            lgm.Log(ch, dmgname, dmgele, REACTION.NONE, false, final);
        }
    }


    public void GetElementParticle(ELEMENT type, bool Orb = false)
    {
        float offfield = 0.6f; // 根据人数改变 0.7 0.8
        for(int i = 0; i < 4; i++)
        {
            float energy = 1;
            if (type == ELEMENT.NONE) energy = 2;
            else if (teams[i].Vision == type) energy = 3;
            if (Orb) energy *= 3;
            if (i != curChara) energy *= offfield;
            teams[i].Charge(energy);
        }
    }
}
