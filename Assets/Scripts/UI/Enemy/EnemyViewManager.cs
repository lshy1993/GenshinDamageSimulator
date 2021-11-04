using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class EnemyViewManager : MonoBehaviour
{
    public GameObject EnemyContent;
    public GameObject prefab;

    private void Start()
    {
        UIFresh();
        Debug.Log("Enemy Init");
    }

    public void UIFresh()
    {
        foreach(Transform tr in EnemyContent.transform)
        {
            Destroy(tr.gameObject);
        }
        foreach (Monster mon in GameManager.GetInstance().monsters)
        {
            var go = Instantiate(prefab, EnemyContent.transform);
            go.GetComponent<EnemyBox>().mon = mon;
        }

    }
}

