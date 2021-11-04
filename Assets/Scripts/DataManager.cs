using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using UnityEngine;

using Newtonsoft.Json;


public class DataManager
{
    private static DataManager instance = new DataManager();

    public static DataManager GetInstance()
    {
        return instance;
    }

    public Dictionary<string, Character> CharaDataList;
    public Dictionary<string, WeaponBase> WeaponDataList;
    //public Dictionary<string, List<string>> WeaponNameList;
    public Dictionary<string, List<string>> ArtifactNameList;

    /// <summary>
    /// 翻译脚本
    /// </summary>
    public Dictionary<string, List<string>> TranslatedScripts;

    public bool GamePlaying = true;

    public DataManager()
    {
        //Init();
    }

    public void Init()
    {
        LoadTranslateList();
        LoadCharaData();
        LoadWeaponData();
        LoadArtifactData();
        Debug.Log("DM init");
    }

    private void LoadTranslateList()
    {
        TranslatedScripts = new Dictionary<string, List<string>>();
        TextAsset tx = Resources.Load<TextAsset>("translate");
        StringReader reader = new StringReader(tx.text);
        try
        {
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                var data = new List<string>(line.Split(','));
                string key = data[0];
                data.RemoveAt(0);
                TranslatedScripts.Add(key, data);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        finally
        {
            reader.Close();
        }
        Debug.Log("translate script readed");
    }

    private void LoadArtifactData()
    {
        string path = "json/ArtifactList";
        try
        {
            TextAsset json = Resources.Load<TextAsset>(path);
            ArtifactNameList = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json.text);
            Debug.Log("ArtifactList Load");
        }
        catch (Exception e)
        {
            Debug.LogError($"{path} :{e.Message}");
            ArtifactNameList = new Dictionary<string, List<string>>();
        }
    }

    private void LoadCharaData()
    {
        string path = "json/CharacterData";
        try
        {
            TextAsset json = Resources.Load<TextAsset>(path);
            CharaDataList = JsonConvert.DeserializeObject<Dictionary<string, Character>>(json.text);
            Debug.Log("CharaData Load");
        }
        catch (Exception e)
        {
            Debug.LogError($"{path} :{e.Message}");
            CharaDataList = new Dictionary<string, Character>();
        }
    }

    private void LoadWeaponData()
    {
        string path = "json/WeaponData";
        try
        {
            TextAsset json = Resources.Load<TextAsset>(path);
            WeaponDataList = JsonConvert.DeserializeObject<Dictionary<string, WeaponBase>>(json.text);
            Debug.Log("WeaponData Load");
        }
        catch (Exception e)
        {
            Debug.LogError($"{path} :{e.Message}");
            WeaponDataList = new Dictionary<string, WeaponBase>();
        }
    }

    public Dictionary<string, List<string>> GetCharacterBase(string chname)
    {
        var fname = "json/chara/" + Regex.Replace(chname, "traveler(.*)", "traveler").ToLower();
        Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
        try
        {
            TextAsset json = Resources.Load<TextAsset>(fname);
            dic = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json.text);
        }
        catch (Exception e)
        {
            Debug.LogError($"{fname}, {e.Message}");
            dic = new Dictionary<string, List<string>>();
        }
        return dic;
    }

    public Dictionary<string, List<string>> GetSkillBase(string skname)
    {
        var fname = "json/skill/" + skname;
        Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
        try
        {
            TextAsset json = Resources.Load<TextAsset>(fname);
            dic = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json.text);
        }
        catch (Exception e)
        {
            Debug.LogError($"{fname}: {e.Message}");
            dic = new Dictionary<string, List<string>>();
        }
        return dic;
    }

    public Dictionary<string, List<string>> GetWeaponBase(string wpname)
    {
        var fname = "json/weapon/" + wpname.Replace(' ', '_').ToLower();
        Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
        try
        {
            TextAsset json = Resources.Load<TextAsset>(fname);
            dic = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json.text);
        }
        catch (Exception e)
        {
            Debug.LogError($"{fname}: {e.Message}");
            dic = new Dictionary<string, List<string>>();
        }
        return dic;
    }

    public List<string> GetWeaponByType(WEAPONTYPE type)
    {
        var res = new List<string>();
        foreach(var kv in WeaponDataList)
        {
            if (kv.Value.WeaponType == type) res.Add(kv.Key);
        }
        return res;
    }

    //public List<string> GetCharaByType(WEAPONTYPE type)
    //{
    //    var res = new List<string>();
    //    foreach (var kv in WeaponDataList)
    //    {
    //        if (kv.Value.WeaponType == type) res.Add(kv.Key);
    //    }
    //    return res;
    //}

    public string GetTranslatedUIText(string code)
    {
        string outs = code;
        if (string.IsNullOrEmpty(code)) return code;

        if( Application.systemLanguage == SystemLanguage.Chinese)
        {
            int n = 0;
            if (TranslatedScripts.ContainsKey(code))
            {
                var list = TranslatedScripts[code];
                if (n >= list.Count) n = 0;
                outs = list[n];
                if (string.IsNullOrEmpty(outs)) outs = code;
                //Debug.Log($"key: {code},translation: {outs}");
            }
            else
            {
                Debug.LogWarning($"Translation key: {code} not found.");
            }
        }

        return outs;
    }
}
