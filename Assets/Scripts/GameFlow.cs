using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameFlow : MonoBehaviour
{
    [SerializeField] PlayerSettings playerSettings;
    [SerializeField] Board gameBoard;
    [SerializeField] AIOpponent ai;
    [SerializeField] GameObject winDisplay;
    [SerializeField] GameObject loseDisplay;
    [SerializeField] GameObject drawDisplay;
    [SerializeField] AudioSource ambientAudio;
    [SerializeField] Image screenFader;
    [SerializeField] float screenFadeTime = 1.0f;
    [SerializeField] float audioFadeTime = 4.0f;

    [SerializeField] Image backgroundImage;
    [SerializeField] Image tableImage;
    [SerializeField] Image boardImage;

    WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Clear()
    {
        winDisplay.SetActive(false);
        loseDisplay.SetActive(false);
        drawDisplay.SetActive(false);
    }

    void Start()
    {
        if (!playerSettings.IsInitialized)
        {
            LoadMenu();
            return;
        }

        if (gameBoard != null)
        {
            gameBoard.onPlayerTurnComplete += OnPlayerTurnComplete;
            gameBoard.onAITurnComplete += OnAITurnComplete;
        }
        else
        {
            Debug.LogError("No gameBoard assigned to GameFlow.", this);
        }

        if (ai == null)
        {
            Debug.LogError("No ai assigned to GameFlow.", this);
        }

        backgroundImage.sprite = playerSettings.GetBackgroundSprite();
        tableImage.sprite = playerSettings.GetTableSprite();
        boardImage.sprite = playerSettings.GetBoardSprite();

        StartCoroutine(FadeInScreen());
        StartCoroutine(FadeInAudio());
    }

    void OnDestroy()
    {
        if (gameBoard != null)
        {
            gameBoard.onPlayerTurnComplete -= OnPlayerTurnComplete;
            gameBoard.onAITurnComplete -= OnAITurnComplete;
        }
    }

    void OnPlayerTurnComplete()
    {
        if (gameBoard.ContainsMatch)
        {
            winDisplay.SetActive(true);
            return;
        }
        
        CheckForDraw();

        if (!gameBoard.IsFull())
        {
            ai.TakeTurn();
        }
    }

    void OnAITurnComplete()
    {
        if (gameBoard.ContainsMatch)
        {
            loseDisplay.SetActive(true);
            return;
        }

        CheckForDraw();
    }

    void CheckForDraw()
    {
        if (gameBoard.IsFull())
        {
            drawDisplay.SetActive(true);
        }
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

    IEnumerator FadeInAudio()
    {
        ambientAudio.clip = playerSettings.GetAmbientAudio();
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
