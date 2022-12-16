using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WinLine", menuName = "ScriptableObjects/WinLine")]
public class WinLine : ScriptableObject
{
    public Direction[] directions;
}

[Serializable]
public class Direction
{
    [Range(-1, 1)] public int stepX;
    [Range(-1, 1)] public int stepY;

    public bool IsZero
    {
        get
        {
            return stepX == 0 && stepY == 0;
        }
    }
}