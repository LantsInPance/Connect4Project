using UnityEngine;

public class GameEndDisplay : MonoBehaviour
{
    [SerializeField] GameObject winDisplay;
    [SerializeField] GameObject loseDisplay;
    [SerializeField] GameObject drawDisplay;

    public void Clear()
    {
        winDisplay.SetActive(false);
        loseDisplay.SetActive(false);
        drawDisplay.SetActive(false);
    }

    public void ShowWinDisplay()
    {
        Show(winDisplay);
    }

    public void ShowLoseDisplay()
    {
        Show(loseDisplay);
    }

    public void ShowDrawDisplay()
    {
        Show(drawDisplay);
    }

    void Show(GameObject display)
    {
        Clear();
        display.SetActive(true);
    }
}
