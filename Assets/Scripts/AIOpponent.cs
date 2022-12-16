using System.Collections;
using UnityEngine;

public class AIOpponent : MonoBehaviour
{
    [SerializeField] Board gameBoard;
    [SerializeField] float delayTime = 1.5f;

    WaitForSeconds wait;

    public void TakeTurn()
    {
        StartCoroutine(TakeDelayedTurn());
    }

    IEnumerator TakeDelayedTurn()
    {
        yield return wait;
        int selectedColumn = DetermineColumn();
        gameBoard.DropPiece(selectedColumn, PlayerID.PlayerTwo, () =>
        {
            gameBoard.EndAITurn();
        });
    }

    void Start()
    {
        wait = new WaitForSeconds(delayTime);
    }

    int DetermineColumn()
    {
        int[] availableColumns = gameBoard.GetAvailableColumns();

        int winningMove = GetWinningMoveFor(PlayerID.PlayerTwo, availableColumns);
        if (winningMove >= 0)
        {
            return winningMove;
        }

        int blockingMove = GetWinningMoveFor(PlayerID.PlayerOne, availableColumns);
        if (blockingMove >= 0)
        {
            return blockingMove;
        }

        int randomIndex = Random.Range(0, availableColumns.Length);
        return availableColumns[randomIndex];
    }

    int GetWinningMoveFor(PlayerID player, int[] availableColumns)
    {
        for (int i = 0; i < availableColumns.Length; i++)
        {
            int testColumn = availableColumns[i];
            if (gameBoard.WouldDropWin(player, testColumn))
            {
                return testColumn;
            }
        }

        return -1;
    }
}
