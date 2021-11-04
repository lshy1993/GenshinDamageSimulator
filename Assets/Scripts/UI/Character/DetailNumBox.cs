using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class DetailNumBox : MonoBehaviour
{
    public TextMeshProUGUI BaseLabel, BiasLabel;

    public void SetNum(float a, float b, bool percentage = false)
    {
        if (percentage)
        {
            BaseLabel.text = string.Format("{0:P1}", a);
        }
        else
        {
            BaseLabel.text = string.Format("{0:N0}", a);
        }
        BiasLabel.text = string.Format("+{0:N0}", b);
    }
}