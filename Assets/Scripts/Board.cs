using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class Board : MonoBehaviour
{
    [SerializeField] Column[] columns;
    [SerializeField] WinLine[] winLines;
    [SerializeField] Image fallingPiece;
    [SerializeField] PlayerSettings playerSettings;

    public int requiredMatchLength = 4;

    public UnityAction onPlayerTurnComplete;
    public UnityAction onAITurnComplete;

    public bool ContainsMatch { get; private set; }

    CanvasGroup canvasGroupComponent;
    bool IsGameFinished 
    {
        get
        {
            return ContainsMatch || IsFull();
        }
    }

    public void DropPlayerPiece(int columnIndex)
    {
        canvasGroupComponent.interactable = false;

        DropPiece(columnIndex, PlayerID.PlayerOne, () =>
        {
            EndPlayerTurn();
        });
    }

    public int[] GetAvailableColumns()
    {
        List<int> availableColumns = new List<int>();
        for (int i = 0; i < columns.Length; i++)
        {
            Column column = columns[i];
            if (!column.IsFull)
            {
                availableColumns.Add(i);
            }
        }

        return availableColumns.ToArray();
    }

    public void DropPiece(int columnIndex, PlayerID playerID, UnityAction onComplete)
    {
        if (IsGameFinished)
        {
            return;
        }

        fallingPiece.sprite = playerSettings.GetPlayerPieceSprite(playerID);

        Column column = columns[columnIndex];
        if (column != null)
        {
            column.DropPiece(playerID, () =>
            {
                OnFallComplete(column, columnIndex, onComplete);
            });
        }
        else
        {
            Debug.LogError("Column " + columnIndex + " is null", this);
        }
    }

    public void EndAITurn()
    {
        onAITurnComplete?.Invoke();

        if (!IsGameFinished)
        {
            canvasGroupComponent.interactable = true;
        }
    }

    public bool IsFull()
    {
        for (int i = 0; i < columns.Length; i++)
        {
            if (!columns[i].IsFull)
            {
                return false;
            }
        }

        return true;
    }

    public bool WouldDropWin(PlayerID player, int columnIndex)
    {
        Column column = columns[columnIndex];
        int indexOfNextSpot = column.GetIndexOfLowestUnownedSpot();
        Spot testSpot = column.GetSpotAtIndex(indexOfNextSpot);
        if (testSpot != null)
        {
            testSpot.SetOwner(player);
            CheckConnections(columnIndex, indexOfNextSpot, testSpot);
            testSpot.Clear();
            
            if (ContainsMatch)
            {
                Debug.Log("Found match for " + player.ToString() + " at column " + columnIndex);
                ContainsMatch = false;
                return true;
            }
        }

        return false;
    }

    void Awake()
    {
        canvasGroupComponent = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        fallingPiece.gameObject.SetActive(false);
        Clear();

        for (int lineIndex = 0; lineIndex < winLines.Length; lineIndex++)
        {
            WinLine winLine = winLines[lineIndex];
            for (int directionIndex = 0; directionIndex < winLine.directions.Length; directionIndex++)
            {
                Direction direction = winLine.directions[directionIndex];
                if (direction.IsZero)
                {
                    Debug.LogWarning("Direction " + directionIndex + " of WinLine " + lineIndex + " is zero.", this);
                }
            }
        }
    }

    void Clear()
    {
        ContainsMatch = false;

        for (int i = 0; i < columns.Length; i++)
        {
            Column column = columns[i];
            column.Clear();
            column.FallingPiece = fallingPiece.transform;
        }
    }

    void EndPlayerTurn()
    {
        onPlayerTurnComplete?.Invoke();
    }

    void CheckConnections(int columnIndex, int spotID, Spot spotToTest)
    {
        List<Spot> matchingSpots = new List<Spot>();
        for (int lineIndex = 0; lineIndex < winLines.Length; lineIndex++)
        {
            matchingSpots.Clear();
            matchingSpots.Add(spotToTest);
            
            WinLine line = winLines[lineIndex];
            for (int directionIndex = 0; directionIndex < line.directions.Length; directionIndex++)
            {
                Direction direction = line.directions[directionIndex];
                CheckForMatches(ref matchingSpots, columnIndex, spotID, direction);
            }

            if (matchingSpots.Count >= requiredMatchLength)
            {
                Debug.Log("Match found for win line " + line.name);
                ContainsMatch = true;
            }
        }
    }

    void CheckForMatches(ref List<Spot> matches, int columnIndex, int spotIndex, Direction direction)
    {
        int columnToTest = columnIndex + direction.stepX;
        int spotToTest = spotIndex + direction.stepY;

        if (columnToTest >= 0 && columnToTest < columns.Length)
        {
            Column column = columns[columnToTest];
            Spot spot = column.GetSpotAtIndex(spotToTest);
            if (spot != null)
            {
                if (spot.OwnerID == matches[0].OwnerID)
                {
                    matches.Add(spot);
                    CheckForMatches(ref matches, columnToTest, spotToTest, direction);
                }
            }
        }
    }

    void OnFallComplete(Column column, int columnIndex, UnityAction onComplete)
    {
        int indexOfNewSpot = column.GetIndexOfHighestOwnedSpot();
        Spot newSpot = column.GetSpotAtIndex(indexOfNewSpot);
        CheckConnections(columnIndex, indexOfNewSpot, newSpot);
        onComplete?.Invoke();
    }
}





//  x x x x x x x
//  x x x x x x x
//  x x x x x x x
//  x x Y Y x x x
//  x R R Y Y x R
//  x Y R R R Y R


// starting at bottom right, check left, then up-left, then up. 
//    for 

// x x x x x x x
// x x x x x x x
// x x x x x R x
// x x x Y x Y x
// x x x R Y R x
// x x R R Y Y Y
