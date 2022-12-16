using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] PlayerSettings playerSettings;
    [SerializeField] Image[] sceneryButtonImages;
    [SerializeField] Image[] colorButtonImages;
    [SerializeField] Sprite selectedButtonSprite;
    [SerializeField] Sprite darkenedButtonSprite;

    public void Quit()
    {
        Application.Quit();
    }

    public void SetScenery(int settingIndex)
    {
        SelectButton(sceneryButtonImages, settingIndex);
        playerSettings.SetScenery(settingIndex);
    }

    public void SetColorScheme(int settingIndex)
    {
        SelectButton(colorButtonImages, settingIndex);
        playerSettings.SetColorScheme(settingIndex);
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    void Start()
    {
        SetScenery(0);
        SetColorScheme(0);
    }

    void SelectButton(Image[] buttonImages, int selection)
    {
        for (int i = 0; i < buttonImages.Length; i++)
        {
            Image image = buttonImages[i];
            if (i == selection)
            {
                image.sprite = selectedButtonSprite;
            }
            else
            {
                image.sprite = darkenedButtonSprite;
            }
        }
    }
}
