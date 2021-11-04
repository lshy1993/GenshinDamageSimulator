using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class WeaponBox : MonoBehaviour
{
    public Image IconImage;
    public Image RareImage;
    public TextMeshProUGUI NameLabel;

    public void InitBox(string wpname)
    {
        WeaponBase wp = DataManager.GetInstance().WeaponDataList[wpname];
        Color col;
        ColorUtility.TryParseHtmlString(GetRareColor(wp.Rare), out col);
        RareImage.color = col;
        IconImage.sprite = ResourceManager.LoadWeaponIcon(wpname);
        //NameLabel.text = ResourceManager.LoadCharacterName(wpname);
    }

    private string GetRareColor(int x)
    {
        switch (x)
        {
            case 1:
                return "#838f99";
            case 2:
                return "#5e966c";
            case 3:
                return "#499fb3";
            case 4:
                return "#b886ca";
            case 5:
                return "#e6ac54";
            default:
                return "#FFFFFF";
        }
    }
}

