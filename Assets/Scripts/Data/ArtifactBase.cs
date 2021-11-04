using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ArtifactBase
{
    public enum ARTIFACTPOS
    {
        FLOWER, PLUME, SANDS, GOBLET, CIRCLET
    }
    public ARTIFACTPOS Pos;
    public string Name;
    public string[] Status = new string[5] { "HP", "HP%", "DEF", "DEF%", "EleMastery" };
    public float[] Nums = new float[5] { 0, 0, 0, 0, 0 };

    public ArtifactBase(ARTIFACTPOS x)
    {
        Pos = x;
        Name = "Artifact";
        switch (Pos)
        {
            case ARTIFACTPOS.FLOWER:
                Status[0] = "HP";
                break;
            case ARTIFACTPOS.PLUME:
                Status[0] = "ATK";
                break;
            case ARTIFACTPOS.SANDS:
                Status[0] = Const.option3[UnityEngine.Random.Range(0, Const.option3.Count - 1)];
                break;
            case ARTIFACTPOS.GOBLET:
                Status[0] = Const.option4[UnityEngine.Random.Range(0, Const.option4.Count - 1)];
                break;
            case ARTIFACTPOS.CIRCLET:
                Status[0] = Const.option5[UnityEngine.Random.Range(0, Const.option5.Count - 1)];
                break;
        }
        for (int i = 0; i < 5; i++)
        {
            if (Status[i].Contains("%")) Nums[i] = UnityEngine.Random.Range(0, 0.5f);
            else Nums[i] = UnityEngine.Random.Range(0, 100f);
        }
    }

    public override string ToString()
    {
        string str = "";
        for (int i = 0; i < 5; i++)
        {
            if (Status[i].Contains("%"))
            {
                str += string.Format("{0} +{1:P1}", Status[i], Nums[i]);
            }
            else
            {
                str += string.Format("{0} +{1:N0}", Status[i], Nums[i]);
            }
            str += "\n";
        }
        return str;
    }

    public string ToScore()
    {
        return "";
    }
}