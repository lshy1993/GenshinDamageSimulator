using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public enum SUBBOX
    {
        CHARA,WEAPON,ARTIFACT
    }
    public GameObject CharaSelectPanel, WeaponSelectPanel, ArtifactSelectPanel, DetailPanel;
    public CharacterManager[] csm;

    private Character[] teams { get { return GameManager.GetInstance().teams; } }

    private int curChara;
    private int curArtifact;

    void OnEnable()
    {
        DataManager.GetInstance().GamePlaying = false;
    }
    void OnDisable()
    {
        DataManager.GetInstance().GamePlaying = true;
    }

    private void SwtichModalBox(SUBBOX t)
    {
        CharaSelectPanel.SetActive(t == SUBBOX.CHARA);
        WeaponSelectPanel.SetActive(t == SUBBOX.WEAPON);
        ArtifactSelectPanel.SetActive(t == SUBBOX.ARTIFACT);
    }

    public void OpenCharacterBox(int cid)
    {
        curChara = cid;
        SwtichModalBox(SUBBOX.CHARA);
        CharaSelectPanel.GetComponent<CharacterSelectManager>().InitPos(cid);
    }
    public void CloseCharacterBox()
    {
        CharaSelectPanel.SetActive(false);
    }
    public void ChangeCharacter(string cname)
    {
        // teams[curChara].ChangeChara(cname);
        csm[curChara].InitCharacterUI();
        Debug.Log($"change CH{curChara} to {cname}");
    }

    public void OpenWeaponBox(int cid)
    {
        curChara = cid;
        var wt = teams[curChara].WeaponType;
        WeaponSelectPanel.GetComponent<WeaponSelectManager>().WeaponType = wt;
        WeaponSelectPanel.GetComponent<WeaponSelectManager>().cid = cid;
        SwtichModalBox(SUBBOX.WEAPON);
    }
    public void CloseWeaponBox()
    {
        WeaponSelectPanel.SetActive(false);
    }
    public void ChangeWeapon(string wname)
    {
        teams[curChara].ChangeWeapon(wname);
        csm[curChara].InitWeaponUI();
        Debug.Log($"change CH{curChara} Weapon to {wname}");
    }

    public void OpenArtifactBox(int cid, int apos)
    {
        curChara = cid;
        curArtifact = apos;
        var d = teams[curChara].Artifacts[curArtifact];
        ArtifactSelectPanel.GetComponent<ArtifactSelectManager>().cid = cid;
        ArtifactSelectPanel.GetComponent<ArtifactSelectManager>().apos = (ArtifactBase.ARTIFACTPOS)apos;
        ArtifactSelectPanel.GetComponent<ArtifactSelectManager>().SetArtifact(d);
        SwtichModalBox(SUBBOX.ARTIFACT);
    }
    public void CloseArtifactBox()
    {
        ArtifactSelectPanel.SetActive(false);
    }
    public void ChangeArtifact(ArtifactBase ad)
    {
        csm[curChara].InitArtifactUI(curArtifact, ad);
        teams[curChara].Artifacts[curArtifact] = ad;
        Debug.Log($"change CH{curChara} Artifact{curArtifact} to {ad.Name}");
    }

    public void ShowDetail(int id)
    {
        curChara = id;
        DetailPanel.GetComponent<CharacterDetailManager>().SetID(curChara);
        DetailPanel.SetActive(true);
    }
    public void CloseDetail()
    {
        DetailPanel.SetActive(false);
    }
}
