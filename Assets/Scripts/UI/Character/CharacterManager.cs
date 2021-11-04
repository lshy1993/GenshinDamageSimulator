using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class CharacterManager : MonoBehaviour
{
    public TeamManager cm;
    public int ID;
    public GameObject CharaIcon, CharaAscend;
    public TMP_InputField CharaLevelInput, ConstellationInput, NormalAttackInput, ElementSkillInput, ElementBurstInput;
    public GameObject NormalAttackBias, ElementSkillBias, ElementBurstBias;
    public TextMeshProUGUI CharaMaxLabel;
    public GameObject WeaponIcon, WeaponAscend;
    public TMP_InputField WeaponLevelInput, WeaponRefineInput;
    public TextMeshProUGUI WeaponMaxLabel, WeaponAttackLabel, WeaponStatusLabel;
    public ArtifactBox[] ArtifactBoxs;

    private Character[] teams { get { return GameManager.GetInstance().teams; } }

    private void OnEnable()
    {
        // 初始角色数据
        InitCharacterUI();        
        InitWeaponUI();
        var ch = GameManager.GetInstance().teams[ID];
        for (int i = 0; i < 5; i++)
        {
            var d = ch.Artifacts[i];
            InitArtifactUI(i, d);
        }
    }

    public void OpenCharacterBox()
    {
        cm.OpenCharacterBox(ID);
    }

    public void InitCharacterUI()
    {
        Character ch = teams[ID];
        CharaIcon.GetComponent<CharacterBox>().InitBox(ch.Name);
        freshCharaLevel();
        ChangeCharacterStar(ch.Star);

        NormalAttackInput.SetTextWithoutNotify(ch.NormalAttack.Level.ToString());
        //NormalAttackBias.SetActive(ch.Constellations >= 3);
        ElementSkillInput.SetTextWithoutNotify(ch.ElementSkill.Level.ToString());
        ElementSkillBias.SetActive(ch.Constellations >= 3);
        ElementBurstInput.SetTextWithoutNotify(ch.ElementBurst.Level.ToString());
        ElementBurstBias.SetActive(ch.Constellations >= 5);
        ConstellationInput.SetTextWithoutNotify(ch.Constellations.ToString());
    }

    public void ChangeCharacterStar(int x)
    {
        var prev = teams[ID].Star;
        if (x == 1 && prev == 1) x = 0;
        for (int i = 0; i < 6; i++)
        {
            var go = CharaAscend.transform.GetChild(i);
            go.GetComponent<Image>().color = i < x ? Color.white : Color.gray;
        }
        teams[ID].ChangeStar(x);
        CharaMaxLabel.text = "/" + Const.lvmax[x].ToString();
        freshCharaLevel();
        Debug.Log($"change CH{ID} Star to {x}");
    }

    public void MinusCharaLevel()
    {
        var d = teams[ID].Level;
        teams[ID].ChangeLevel(d - 1);
        freshCharaLevel();
    }
    public void ChangeCharaLevel()
    {
        if (string.IsNullOrEmpty(CharaLevelInput.text)) return;
        var d = Convert.ToInt32(CharaLevelInput.text);
        teams[ID].ChangeLevel(d);
        freshCharaLevel();
    }
    public void PlusCharaLevel()
    {
        var d = teams[ID].Level;
        teams[ID].ChangeLevel(d + 1);
        freshCharaLevel();
    }
    private void freshCharaLevel()
    {
        var d = teams[ID].Level;
        CharaLevelInput.SetTextWithoutNotify(d.ToString());
    }

    public void MinusCharaConstellations()
    {
        var d = teams[ID].Constellations;
        changeCharaConstellations(d - 1);
    }
    public void ChangeCharaConstellations()
    {
        if (string.IsNullOrEmpty(ConstellationInput.text)) return;
        var d = Convert.ToInt32(ConstellationInput.text);
        changeCharaConstellations(d);
    }
    public void PlusCharaConstellations()
    {
        var d = teams[ID].Constellations;
        changeCharaConstellations(d + 1);
    }
    private void changeCharaConstellations(int d)
    {
        d = Const.RELU(d, 0, 6);
        ConstellationInput.SetTextWithoutNotify(d.ToString());
        ElementSkillBias.SetActive(d >= 3);
        ElementBurstBias.SetActive(d >= 5);
        teams[ID].Constellations = d;
        Debug.Log($"change CH{ID} Constellations to {d}");
    }

    public void MinusNALevel()
    {
        var d = teams[ID].NormalAttack.Level;
        changeNA(d - 1);
    }
    public void ChangeNALevel()
    {
        if (string.IsNullOrEmpty(NormalAttackInput.text)) return;
        var d = Convert.ToInt32(NormalAttackInput.text);
        changeNA(d);
    }
    public void PlusNALevel()
    {
        var d = teams[ID].NormalAttack.Level;
        changeNA(d + 1);
    }
    private void changeNA(int d)
    {
        d = Const.RELU(d, 1, 10);
        NormalAttackInput.SetTextWithoutNotify(d.ToString());
        teams[ID].NormalAttack.Level = d;
        Debug.Log($"change CH{ID} NormalAttack Lv to {d}");
    }

    public void MinusSkillLevel()
    {
        var d = teams[ID].ElementSkill.Level;
        changeSkill(d - 1);
    }
    public void ChangeSkillLevel()
    {
        if (string.IsNullOrEmpty(ElementSkillInput.text)) return;
        var d = Convert.ToInt32(ElementSkillInput.text);
        changeSkill(d);
    }
    private void changeSkill(int d)
    {
        d = Const.RELU(d, 1, 10);
        ElementSkillInput.SetTextWithoutNotify(d.ToString());
        teams[ID].ElementSkill.Level = d;
        Debug.Log($"change CH{ID} Element Skill Lv to {d}");
    }
    public void PlusSkillLevel()
    {
        var d = teams[ID].ElementSkill.Level;
        changeSkill(d + 1);
    }

    public void MinusBurstLevel()
    {
        var d = teams[ID].ElementBurst.Level;
        changeBurst(d - 1);
    }
    public void ChangeBurstLevel()
    {
        if (string.IsNullOrEmpty(ElementBurstInput.text)) return;
        var d = Convert.ToInt32(ElementBurstInput.text);
    }
    public void PlusBurstLevel()
    {
        var d = teams[ID].ElementBurst.Level;
        changeBurst(d + 1);
    }
    private void changeBurst(int d)
    {
        d = Const.RELU(d, 1, 10);
        ElementBurstInput.SetTextWithoutNotify(d.ToString());
        teams[ID].ElementBurst.Level = d;
        Debug.Log($"change CH{ID} Element Burst Lv to {d}");
    }

    public void OpenWeaponBox()
    {
        cm.OpenWeaponBox(ID);
    }
    public void InitWeaponUI()
    {
        WeaponBase weapon = teams[ID].Weapon;
        WeaponIcon.GetComponent<WeaponBox>().InitBox(weapon.Name);
        var starall = weapon.GetStarMax();
        for (int i = 0; i < 6; i++)
        {
            WeaponAscend.transform.GetChild(i).gameObject.SetActive(i < starall);
        }
        ChangeWeaponStar(weapon.Star);
        WeaponRefineInput.SetTextWithoutNotify(weapon.Refine.ToString());
    }

    public void ChangeWeaponStar(int x)
    {
        var prev = teams[ID].Weapon.Star;
        if (x == 1 && prev == 1) x = 0;
        for (int i = 0; i < 6; i++)
        {
            var go = WeaponAscend.transform.GetChild(i);
            go.GetComponent<Image>().color = i < x ? Color.white : Color.gray;
        }
        teams[ID].Weapon.ChangeStar(x);
        freshWeaponLevel();        
        Debug.Log($"change CH{ID} Weapon Star to {x}");
    }

    public void MinusWeaponLevel()
    {
        var d = teams[ID].Weapon.Level;
        teams[ID].Weapon.ChangeLevel(d - 1);
        freshWeaponLevel();
    }
    public void ChangeWeaponLevel()
    {
        if (string.IsNullOrEmpty(WeaponLevelInput.text)) return;
        int d = Convert.ToInt32(WeaponLevelInput.text);
        teams[ID].Weapon.ChangeLevel(d);
        freshWeaponLevel();
    }
    public void PlusWeaponLevel()
    {
        var d = teams[ID].Weapon.Level;
        teams[ID].Weapon.ChangeLevel(d+ 1);
        freshWeaponLevel();
    }
    private void freshWeaponLevel()
    {
        var weapon = teams[ID].Weapon;
        WeaponLevelInput.SetTextWithoutNotify(weapon.Level.ToString());
        WeaponMaxLabel.text = "/" + Const.lvmax[weapon.Star].ToString();
        WeaponAttackLabel.text = weapon.GetATK();
        WeaponStatusLabel.text = weapon.GetSubStatus();
        Debug.Log($"change CH{ID} Weapon Level to {weapon.Level}");
    }

    public void MinusWeaponRefine()
    {
        var d = teams[ID].Weapon.Refine;
        changeWeaponRefine(d - 1);
    }
    public void ChangeWeaponRefine()
    {
        if (string.IsNullOrEmpty(WeaponRefineInput.text)) return;
        var d = Convert.ToInt32(WeaponRefineInput.text);
        changeWeaponRefine(d);
    }
    private void changeWeaponRefine(int d)
    {
        d = Const.RELU(d, 1, 5);
        WeaponRefineInput.SetTextWithoutNotify(d.ToString());
        teams[ID].Weapon.Refine = d;
        Debug.Log($"change CH{ID} Weapon Refine to {d}");
    }
    public void PlusWeaponRefine()
    {
        var d = teams[ID].Weapon.Refine;
        changeWeaponRefine(d + 1);
    }

    public void OpenArtifactBox(int aid)
    {
        cm.OpenArtifactBox(ID, aid);
    }
    public void InitArtifactUI(int aid, ArtifactBase artifact)
    {
        ArtifactBoxs[aid].Init(artifact);
    }

    public void ShowDetail()
    {
        cm.ShowDetail(ID);
    }
}
