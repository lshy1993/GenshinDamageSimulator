using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class CharacterBox : MonoBehaviour
{
    public Image IconImage;
    public Image EleImage;
    public TextMeshProUGUI NameLabel;

    public void InitBox(string name)
    {
        Character ch = new Character(name);
        Color col;
        ColorUtility.TryParseHtmlString(ch.Rare > 4 ? "#FFB13F" : "#D28FD6", out col);
        GetComponent<Image>().color = col;
        IconImage.sprite = ResourceManager.LoadCharacterIcon(ch.Name);
        EleImage.sprite = ResourceManager.LoadElementIcon(ch.Vision);
        NameLabel.text = ResourceManager.LoadCharacterName(ch.Name);
        //go.GetComponent<Image>().SetNativeSize();
    }
}

