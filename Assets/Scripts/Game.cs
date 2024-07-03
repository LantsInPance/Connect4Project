using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] PlayerSettings playerSettings;
    [SerializeField] Board gameBoard;
    [SerializeField] AIOpponent ai;
    [SerializeField] GameEndDisplay gameEndDisplay;
    [SerializeField] ScreenFader screenFader;
    [SerializeField] AudioFader audioFader;
    [SerializeField] GameDisplay gameDisplay;
    [SerializeField] Image tableImage;
    [SerializeField] Image boardImage;

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

        gameDisplay.ApplySettings(playerSettings);
        screenFader.Fade();

        audioFader.Fade(playerSettings);
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
            gameEndDisplay.ShowWinDisplay();
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
            gameEndDisplay.ShowLoseDisplay();
            return;
        }

        CheckForDraw();
    }

    void CheckForDraw()
    {
        if (gameBoard.IsFull())
        {
            gameEndDisplay.ShowDrawDisplay();
        }
    }
}
