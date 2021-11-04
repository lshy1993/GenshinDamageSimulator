using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;

public class ResourceManager
{
    private static ResourceManager instance = new ResourceManager();

    public static ResourceManager GetInstance()
    {
        if (instance == null)
        {
            instance = new ResourceManager();
        }
        return instance;
    }

    public static Sprite LoadArtifactSprite(string setname, ArtifactBase.ARTIFACTPOS pos)
    {
        string extension = pos.ToString().ToLower();//  DataManager.GetInstance().extension_artifact[(int)pos];
        string fname = $"artifacts/{setname}_{extension}";
        var sp = Resources.Load<Sprite>(fname);
        return sp;
    }

    public static Sprite LoadWeaponIcon(string chname)
    {
        chname = chname.Replace(' ', '_').ToLower();
        string fname = $"weapons/{chname}";
        var sp = Resources.Load<Sprite>(fname);
        return sp;
    }

    public static Sprite LoadCharacterIcon(string chname)
    {
        string fname = $"characters/{chname}";
        var sp = Resources.Load<Sprite>(fname);
        return sp;
    }

    public static Sprite LoadElementIcon(ELEMENT elename)
    {
        string fname = $"elements/{elename.ToString().ToLower()}";
        var sp = Resources.Load<Sprite>(fname);
        return sp;
    }

    public static Sprite LoadSkillIcon(string chara, string skname)
    {
        chara = Regex.Replace(chara, "boy_|girl_", "");
        string fname = $"skills/{chara}/{skname}";
        var sp = Resources.Load<Sprite>(fname);
        return sp;
    }

    public static string LoadCharacterName(string chara)
    {
        chara = Regex.Replace(chara, "traveler(.*)", "traveler");
        return chara;
    }
}
