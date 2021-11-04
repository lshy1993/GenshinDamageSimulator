using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// 作为游戏内所有按钮的基类
/// </summary>
public class RadioButton : MonoBehaviour
{
    public Sprite Normal, Hightlight, Selectd;

    public void SetToggle(bool pressed)
    {
        if (pressed) SetTogglePressed();
        else SetToggleAvailable();
    }

    /// <summary>
    /// 将目标设置为 已按下
    /// </summary>
    private void SetTogglePressed()
    {
        GetComponent<Button>().interactable = false;
        GetComponent<Image>().sprite = Selectd;
    }

    /// <summary>
    /// 将目标设置为 可以按下
    /// </summary>
    private void SetToggleAvailable()
    {
        GetComponent<Button>().interactable = true;
        GetComponent<Image>().sprite = Normal;
    }

}
