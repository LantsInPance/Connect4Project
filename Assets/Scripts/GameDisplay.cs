using UnityEngine;
using UnityEngine.UI;

public class GameDisplay : MonoBehaviour
{
    [SerializeField] Image backgroundImage;
    [SerializeField] Image tableImage;
    [SerializeField] Image boardImage;

    public void ApplySettings(PlayerSettings settings)
    {
        backgroundImage.sprite = settings.GetBackgroundSprite();
        tableImage.sprite = settings.GetTableSprite();
        boardImage.sprite = settings.GetBoardSprite();
    }
}
