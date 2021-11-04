using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class DamageNum : MonoBehaviour
{
    private float t = 1f;
    private float speed = 250;

    private void Update()
    {
        if (t > 0)
        {
            t -= Time.deltaTime;
            var pos = GetComponent<RectTransform>().localPosition;
            pos.y += speed * Time.deltaTime;
            GetComponent<RectTransform>().localPosition = pos;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ShowNum(ELEMENT ele, int dmg, REACTION reaction)
    {
        speed = UnityEngine.Random.Range(50, 150);
        var dmgstr = dmg.ToString();
        if (reaction != REACTION.NONE) dmgstr = reaction.ToString() + " " + dmgstr;
        GetComponent<TextMeshProUGUI>().text = dmgstr;
        Color col = Color.white;
        ColorUtility.TryParseHtmlString(Const.GetElementColor(ele), out col);
        GetComponent<TextMeshProUGUI>().color = col;
        int rectx = 400;
        int recty = 0;
        var x = UnityEngine.Random.Range(-rectx, rectx);
        var y = UnityEngine.Random.Range(-recty, recty);
        GetComponent<RectTransform>().localPosition = new Vector2(x, y);
    }
}

