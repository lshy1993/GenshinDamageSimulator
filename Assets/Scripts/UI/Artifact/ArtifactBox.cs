using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class ArtifactBox : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI DetailLabel;
    public TextMeshProUGUI ScoreLabel;

    public void Init(ArtifactBase artifact)
    {
        if (artifact == null)
        {
            Icon.sprite = Resources.Load<Sprite>("组 363");
            DetailLabel.text = "";
            ScoreLabel.text = "";
        }
        else
        {
            Icon.sprite = ResourceManager.LoadArtifactSprite(artifact.Name, artifact.Pos);
            DetailLabel.text = artifact.ToString();
            ScoreLabel.text = artifact.ToScore();
        }
    }
}
