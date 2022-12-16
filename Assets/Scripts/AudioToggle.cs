using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AudioToggle : MonoBehaviour
{
    [SerializeField] Sprite audioOnIcon;
    [SerializeField] Sprite audioOffIcon;

    Image imageComponent;

    public void ToggleAudio()
    {
        if (AudioListener.volume > 0.0f)
        {
            TurnAudioOff();
        }
        else
        {
            TurnAudioOn();
        }
    }

    void Start()
    {
        imageComponent = GetComponent<Image>();
        TurnAudioOn();
    }

    void TurnAudioOn()
    {
        imageComponent.sprite = audioOnIcon;
        AudioListener.volume = 1.0f;
    }

    void TurnAudioOff()
    {
        imageComponent.sprite = audioOffIcon;
        AudioListener.volume = 0.0f;
    }
}
