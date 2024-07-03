using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] Image screenFader;
    [SerializeField] float screenFadeTime = 1.0f;

    WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    public void Fade()
    {
        StartCoroutine(FadeInScreen());
    }

    IEnumerator FadeInScreen()
    {
        Color color = screenFader.color;
        float currentTime = screenFadeTime;
        while (currentTime > 0.0f)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0.0f)
            {
                currentTime = 0.0f;
            }

            color.a = currentTime / screenFadeTime;
            screenFader.color = color;
            yield return waitForEndOfFrame;
        }
    }
}
