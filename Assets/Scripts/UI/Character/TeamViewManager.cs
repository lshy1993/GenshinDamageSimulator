using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class TeamViewManager : MonoBehaviour
{
    public TextMeshProUGUI[] CharacterLabels;

    void Update()
    {
        for(int i = 0; i < 4; i++)
        {
            Character ch = GameManager.GetInstance().teams[i];
            CharacterLabels[i].text = ResourceManager.LoadCharacterName(ch.Name);
        }
    }
}

