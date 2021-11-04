using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class EnemyBox : MonoBehaviour
{
    public Monster mon;
    public Image Icon;
    public TextMeshProUGUI NameLabel;
    public Slider HPSlider;
    public GameObject ElementPanel;

    public GameObject prefab;

    void Update()
    {
        if (mon == null) return;
        NameLabel.text = mon.Name;
        HPSlider.value = mon.GetHPProgress();
        foreach (Transform tr in ElementPanel.transform) Destroy(tr.gameObject);
        foreach (ElementBase ele in mon.InfectedElements)
        {
            var go = Instantiate(prefab, ElementPanel.transform);
            go.GetComponent<Image>().sprite = ResourceManager.LoadElementIcon(ele.Type);
        }
    }

}
