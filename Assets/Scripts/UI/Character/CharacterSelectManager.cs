using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class CharacterSelectManager : MonoBehaviour
{
    public TeamManager cm;
    public GameObject ModalBox;
    public GameObject CharaListContent;
    public GameObject prefab;

    void Awake()
    {
        foreach(Transform tr in CharaListContent.transform)
        {
            Destroy(tr.gameObject);
        }
        foreach(string ch in DataManager.GetInstance().CharaDataList.Keys)
        {
            var go = Instantiate(prefab, CharaListContent.transform);
            go.name = ch;
            go.GetComponent<CharacterBox>().InitBox(ch);
            go.GetComponent<Button>().onClick.AddListener(() => { cm.ChangeCharacter(ch); gameObject.SetActive(false); });
        }
    }

    public void InitPos(int cid)
    {
        ModalBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(200 + cid * 480, -20);
    }

}
