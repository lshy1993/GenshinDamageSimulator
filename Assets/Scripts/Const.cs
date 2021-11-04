using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public static class Const
{
    public static List<int> lvmin = new List<int> { 1, 20, 40, 50, 60, 70, 80 };
    public static List<int> lvmax = new List<int> { 20, 40, 50, 60, 70, 80, 90 };

    public static List<string> extension_artifact = new List<string> { "flower", "plume", "sands", "goblet", "circlet" };
    public static List<string> option0 = new List<string> { "HP", "HP%", "ATK", "ATK%", "DEF", "DEF%", "EleCharge%", "EleMastery", "Crit%", "CritDMG%" };
    public static List<string> option3 = new List<string> { "ATK%", "DEF%", "HP%", "EleCharge%", "EleMastery" };
    public static List<string> option4 = new List<string> { "ATK%", "DEF%", "HP%", "PyroDMG%", "HydroDMG%", "CryoDMG%", "ElectroDMG%", "AnemoDMG%", "GeoDMG %", "PhyDMG%", "EleMastery" };
    public static List<string> option5 = new List<string> { "ATK%", "DEF%", "HP%", "Crit%", "CritDMG%", "Heal%", "EleMastery" };


    public static Dictionary<WEAPONTYPE, string> defaultweapon = new Dictionary<WEAPONTYPE, string> {
        { WEAPONTYPE.SWORD, "Dull Blade"},
        { WEAPONTYPE.CLAYMORE, "Waster Greatsword" },
        { WEAPONTYPE.POLEARM, "Beginners Protector" },
        { WEAPONTYPE.BOW,"Hunters Bow" },
        { WEAPONTYPE.CATALYST, "Apprentices Notes"}
    };

    public static int RELU(int value, int min, int max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    public static string GetElementColor(ELEMENT ele)
    {
        switch (ele)
        {
            case ELEMENT.ANEMO:
                return "#66ffcc";
            case ELEMENT.CRYO:
                return "#99ccff";
            case ELEMENT.HYDRO:
                return "#0099ff";
            case ELEMENT.PYRO:
                return "#ff6600";
            case ELEMENT.GEO:
                return "#ffcc66";
            case ELEMENT.ELECTRO:
                return "#cc80ff";
            case ELEMENT.DENDRO:
                return "#33cc33";
            case ELEMENT.NONE:
            default:
                break;
        }
        return "#000000";
    }

    public static float GetLevelMultiplier(int lv)
    {
        if (lv < 60)
        {
            return 0.0002325f * Mathf.Pow(lv, 3) + 0.05547f * Mathf.Pow(lv, 2) - 0.2523f * lv + 14.47f;
        }
        else
        {
            return 0.00194f * Mathf.Pow(lv, 3) - 0.319f * Mathf.Pow(lv, 2) + 30.7f * lv - 868;
        }
    }

    public static float GetElementTime(int amt)
    {
        if (amt == 1) return 9.5f;
        if (amt == 2) return 12;
        if (amt == 4) return 17;
        return 0;
    }
}
