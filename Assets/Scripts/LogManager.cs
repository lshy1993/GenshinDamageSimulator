using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class LogManager : MonoBehaviour
{
    public TextMeshProUGUI TimeLabel;
    public GameObject[] TeamBox;
    public GameObject DetailPanel;
    public GameObject LogContent;
    public GameObject prefab;

    private float startTime;
    private bool started = false;
    private int[] totaldmg = new int[] { 0, 0, 0, 0, 0 };
    private List<DmgLog> logList;
    private int pos = 0;

    private int frame = 0;
    private bool hide = true;

    class DmgLog
    {
        public int dmg;
        public float time;
        public DmgLog(int dmg, float time)
        {
            this.dmg = dmg;
            this.time = time;
        }
    }

    private void Start()
    {
        // Reset();
        foreach(Transform tr in LogContent.transform)
        {
            Destroy(tr.gameObject);
        }
        Reset();
        Debug.Log("LogP Init");
    }

    private void Update()
    {
        if (!started) return;
        frame += 1;
        if(frame >= 50)
        {
            frame = 0;
            UIFresh();
        }
    }

    private void UIFresh()
    {
        TimeLabel.text = TimeSpan.FromSeconds(Time.time - startTime).ToString("mm':'ss");
        for (int i = 0; i <= 4; i++)
        {
            var DMGLabel = TeamBox[i].transform.Find("DmgNum_Text").GetComponent<TextMeshProUGUI>();
            DMGLabel.text = totaldmg[i].ToString("N0");

            var DPSLabel = TeamBox[i].transform.Find("DPS_Text").GetComponent<TextMeshProUGUI>();
            DPSLabel.text = (totaldmg[i] / (Time.time - startTime)).ToString("F2");

            //TeamBox[i].transform.Find("BattleDPS_Text").GetComponent<TextMeshProUGUI>();
            //DPSLabel.text = DPS(Time.time).ToString("F2");
        }
        
    }

    public void Log(Character ch, string dmgname, ELEMENT dmgele, REACTION reaction, bool critflag, int dmg)
    {
        if (!started) StartLog();

        if (LogContent.transform.childCount >= 200) Destroy(LogContent.transform.GetChild(0).gameObject);
        var go = Instantiate(prefab, LogContent.transform);
        var characolor = Const.GetElementColor(ch.Vision);
        var dmgcolor = Const.GetElementColor(dmgele);
        var reactstr = reaction == REACTION.NONE ? "" : reaction.ToString() + " ";
        var critstr = critflag ? "CRIT " : "";
        go.GetComponent<TextMeshProUGUI>().text = $"<color={characolor}>{ch.Name}</color> {dmgname}: {critstr}<color={dmgcolor}>{reactstr}{dmg}</color> DMG";

        totaldmg[0] += dmg;
        for(int i = 0; i < 4; i++)
        {
            if (GameManager.GetInstance().teams[i].Name == ch.Name) totaldmg[i + 1] += dmg;
        }
        logList.Add(new DmgLog(dmg, Time.time));
    }

    private float DPS(float curT)
    {
        float t = 0;
        for(int i = pos; i < logList.Count; i++)
        {
            if (logList[i].time >= curT - 10) t += logList[i].dmg;
        }
        pos = logList.Count;
        return t / 10;
    }

    public void StartLog()
    {
        started = true;
        startTime = Time.time;
        Reset();
    }

    public void Reset()
    {
        for (int i = 0; i <= 4; i++) totaldmg[i] = 0;
        logList = new List<DmgLog>();
        for (int i = 1; i <= 4; i++)
        {
            var NameLabel = TeamBox[i].transform.Find("Name_Text").GetComponent<TextMeshProUGUI>();
            NameLabel.text = GameManager.GetInstance().teams[i - 1].Name;
        }

    }

    public void Pause()
    {
        started = false;
    }

    public void Hide()
    {
        hide = !hide;
        for(int i = 1; i <= 4; i++)
        {
            TeamBox[i].SetActive(!hide);
        }
    }
}

