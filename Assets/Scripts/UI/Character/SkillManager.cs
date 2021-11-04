using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class SkillManager : MonoBehaviour
{
    public int id = 0;
    public Slider Eslider;
    public TextMeshProUGUI ETimeLabel;
    public Slider Qslider;
    public TextMeshProUGUI QTimeLabel;

    private string curChara;

    protected virtual void Update()
    {
        Character ch = GameManager.GetInstance().teams[id];
        if (ch == null) return;
        if (ch.Name != curChara)
        {
            curChara = ResourceManager.LoadCharacterName(ch.Name);
            Eslider.transform.Find("Background").GetComponent<Image>().sprite = ResourceManager.LoadSkillIcon(ch.Name, "talent_2");
            Qslider.transform.Find("Background").GetComponent<Image>().sprite = ResourceManager.LoadSkillIcon(ch.Name, "talent_3");
            Color col;
            ColorUtility.TryParseHtmlString(Const.GetElementColor(ch.Vision)+"40", out col);
            Qslider.transform.Find("Fill Area/Fill").GetComponent<Image>().color = col;
        }

        Eslider.value = ch.ElementSkill.GetCDProgress();
        Eslider.transform.Find("Background").GetComponent<Image>().color = ch.ElementSkill.CanCast() ? Color.white : Color.gray;
        ETimeLabel.text = ch.ElementSkill.GetStringCD();

        Qslider.value = ch.ElementBurst.GetEnergyProgress();
        Qslider.transform.Find("Background").GetComponent<Image>().color = ch.ElementBurst.CanCast() ? Color.white : Color.gray;
        QTimeLabel.text = ch.ElementBurst.GetStringCD();
    }

}
