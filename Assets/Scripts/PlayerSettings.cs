using System;
using UnityEngine;

public enum PlayerID
{
    None = -1,
    PlayerOne = 0,
    PlayerTwo = 1
}

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public Scenery[] availableSceneries;
    public ColorScheme[] availablePiecePairs;

    ColorScheme selectedColorScheme;
    Scenery selectedScenery;
    public bool IsInitialized
    {
        get
        {
            return selectedColorScheme != null && selectedScenery != null;
        }
    }

    public void SetScenery(int index)
    {
        selectedScenery = availableSceneries[index];
    }

    public void SetColorScheme(int index)
    {
        selectedColorScheme = availablePiecePairs[index];
    }

    public Sprite GetPlayerPieceSprite(PlayerID playerID)
    {
        if (playerID == PlayerID.PlayerOne)
        {
            return selectedColorScheme.pieceA;
        }
        else if (playerID == PlayerID.PlayerTwo)
        {
            return selectedColorScheme.pieceB;
        }

        return null;
    }

    public Sprite GetBackgroundSprite()
    {
        return selectedScenery.background;
    }

    public Sprite GetTableSprite()
    {
        return selectedScenery.table;
    }

    public Sprite GetBoardSprite()
    {
        return selectedColorScheme.board;
    }

    public AudioClip GetAmbientAudio()
    {
        return selectedScenery.ambientSound;
    }

    [Serializable]
    public class Scenery
    {
        public Sprite background;
        public Sprite table;
        public AudioClip ambientSound;
    }

    [Serializable]
    public class ColorScheme
    {
        public Sprite pieceA;
        public Sprite pieceB;
        public Sprite board;
    }
}