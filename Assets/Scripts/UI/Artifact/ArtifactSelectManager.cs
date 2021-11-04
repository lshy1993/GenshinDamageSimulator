using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class ArtifactSelectManager : MonoBehaviour
{
    public TeamManager cm;
    public GameObject ModalBox;
    public GameObject ArtifactListContent;
    public GameObject prefab;

    public TMP_Dropdown[] Dropdown;
    public TMP_InputField[] Level;
    public ArtifactBase.ARTIFACTPOS apos = 0;
    public int cid = 0;
    private ArtifactBase curArtifact;

    void Awake()
    {
        Debug.Log("Awake");
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable");
        ModalBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(150 + cid * 480, -515 - 55 * (int)apos);
        foreach (Transform tr in ArtifactListContent.transform)
        {
            Destroy(tr.gameObject);
        }
        var gp = GetComponent<ToggleGroup>();
        var pp = apos.ToString().ToLower();
        foreach (string ch in DataManager.GetInstance().ArtifactNameList[pp])
        {
            var go = Instantiate(prefab, ArtifactListContent.transform);
            go.name = ch;
            go.transform.Find("Main_Image").GetComponent<Image>().sprite = ResourceManager.LoadArtifactSprite(ch, apos);
            go.GetComponent<Toggle>().group = gp;
            go.GetComponent<Toggle>().onValueChanged.AddListener((x) => { if (x) SelectArtifact(ch); });
            bool flag = curArtifact != null && ch == curArtifact.Name;
            go.GetComponent<Toggle>().SetIsOnWithoutNotify(flag);
        }
        InitNumber();
    }

    private void OnDisable()
    {
    }

    private void InitNumber()
    {
        // 如果null则随机一个
        if (curArtifact == null) curArtifact = new ArtifactBase(apos);
        // 根据圣遗物部位apos决定主词条可选范围
        switch (apos)
        {
            case ArtifactBase.ARTIFACTPOS.FLOWER:
                Dropdown[0].ClearOptions();
                Dropdown[0].AddOptions(new List<string> { "HP" });
                break;
            case ArtifactBase.ARTIFACTPOS.PLUME:
                Dropdown[0].ClearOptions();
                Dropdown[0].AddOptions(new List<string> { "ATK" });
                break;
            case ArtifactBase.ARTIFACTPOS.SANDS:
                Dropdown[0].ClearOptions();
                Dropdown[0].AddOptions(Const.option3);
                break;
            case ArtifactBase.ARTIFACTPOS.GOBLET:
                Dropdown[0].ClearOptions();
                Dropdown[0].AddOptions(Const.option4);
                break;
            case ArtifactBase.ARTIFACTPOS.CIRCLET:
                Dropdown[0].ClearOptions();
                Dropdown[0].AddOptions(Const.option5);
                break;
        }
        StartCoroutine(AutoSelect());
    }

    private IEnumerator AutoSelect()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < 5; i++)
        {
            try
            {
                var d = Dropdown[i].options.FindIndex((x) => x.text == curArtifact.Status[i]);
                //Debug.Log($"{i}, {d} {curArtifact.Status[i]}");
                Dropdown[i].SetValueWithoutNotify(d);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
                Dropdown[i].SetValueWithoutNotify(0);
            }
            if (curArtifact.Status[i].Contains("%"))
            {
                Level[i].text = (curArtifact.Nums[i] * 100).ToString("F1");
            }
            else
            {
                Level[i].text = curArtifact.Nums[i].ToString("N0");
            }
        }
    }

    private void SelectArtifact(string setname)
    {
        curArtifact.Pos = apos;
        curArtifact.Name = setname;
    }

    public void SetArtifact(ArtifactBase d)
    {
        curArtifact = d;
    }

    public void ChangeStatus(int x)
    {
        var d = Dropdown[x];
        var key = curArtifact.Status[x];
        var next = d.options[d.value].text;
        curArtifact.Status[x] = next;
        Debug.Log($"change Artifact{apos}, {curArtifact.ToString()}");
    }

    public void ChangeNums(int x)
    {
        var key = curArtifact.Status[x];
        var next = Convert.ToSingle(Level[x].text);
        if (key.Contains("%")) next *= 0.01f;
        curArtifact.Nums[x] = next ;
        Debug.Log($"change Artifact{apos}, {key} {next}");
    }

    public void Save()
    {
        cm.ChangeArtifact(curArtifact);
        cm.CloseArtifactBox();
    }
}
