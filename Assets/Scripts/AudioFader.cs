using System.Collections;
using UnityEngine;

public class AudioFader : MonoBehaviour
{
    [SerializeField] AudioSource ambientAudio;
    [SerializeField] float audioFadeTime = 4.0f;

    WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    public void Fade(PlayerSettings settings)
    {
        ambientAudio.clip = settings.GetAmbientAudio();
        StartCoroutine(FadeInAudio());
    }

    IEnumerator FadeInAudio()
    {
        ambientAudio.Play();

        float currentTime = 0.0f;
        while (currentTime < audioFadeTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime > audioFadeTime)
            {
                currentTime = audioFadeTime;
            }

            ambientAudio.volume = currentTime / audioFadeTime;
            yield return waitForEndOfFrame;
        }
    }
}
