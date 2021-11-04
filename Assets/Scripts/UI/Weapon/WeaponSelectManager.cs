using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectManager : MonoBehaviour
{
    public TeamManager cm;
    public GameObject ModalBox;
    public GameObject WeaponListContent;
    public GameObject prefab;

    public WEAPONTYPE WeaponType = WEAPONTYPE.SWORD;
    public int cid;

    private void OnEnable()
    {
        ModalBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(200 + cid * 480, -260);

        foreach (Transform tr in WeaponListContent.transform)
        {
            Destroy(tr.gameObject);
        }

        var weaponList = DataManager.GetInstance().GetWeaponByType(WeaponType);
        foreach(string wp in weaponList)
        {
            var go = Instantiate(prefab, WeaponListContent.transform);
            go.GetComponent<WeaponBox>().InitBox(wp);
            go.GetComponent<Button>().onClick.AddListener(() => { cm.ChangeWeapon(wp); gameObject.SetActive(false); });
        }
    }

}
