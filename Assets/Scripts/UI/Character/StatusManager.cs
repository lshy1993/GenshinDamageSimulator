using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class StatusManager : MonoBehaviour
{
    public int id;
    public Slider HPSlider;
    public TextMeshProUGUI HPLabel;
    public GameObject ElementPanel,BuffPanel;
    public GameObject eleprefab, buffprefab;

    private List<Buff> envs { get { return GameManager.GetInstance().envs; } }

    private void Start()
    {
        Debug.Log("Status Init");
        foreach (Transform tr in BuffPanel.transform) Destroy(tr.gameObject);
        
    }

    void Update()
    {
        Character ch = GameManager.GetInstance().teams[id];
        if (ch == null) return;
        HPSlider.value = ch.GetHPProgress();
        HPLabel.text = ch.GetHP().ToString("N0");

        foreach (Transform tr in ElementPanel.transform) Destroy(tr.gameObject);
        for (int i = ch.InfectedElements.Count - 1; i >= 0; i--)
        {
            ElementBase ele = ch.InfectedElements[i];
            var go = Instantiate(eleprefab, ElementPanel.transform);
            go.GetComponent<Image>().sprite = ResourceManager.LoadElementIcon(ele.Type);
        }

        for (int i = envs.Count - 1; i >= 0; i--)
        {
            Buff buff = envs[i];
            if (!buff.active)
            {
                Destroy(BuffPanel.transform.GetChild(i).gameObject);
                envs.RemoveAt(i);
            }
            else
            {
                GameObject go;
                if(i >= BuffPanel.transform.childCount) go = Instantiate(buffprefab, BuffPanel.transform);
                else go = BuffPanel.transform.GetChild(i).gameObject;
                go.GetComponent<Slider>().value = buff.GetProgress();
                go.transform.Find("Name_Text").GetComponent<TextMeshProUGUI>().text = string.Format("{0} {1:F1}", buff.Name, buff.RestTime);
            }
        }
    }

}

