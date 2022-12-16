using UnityEngine;

[CreateAssetMenu(fileName = "AnimationSettings", menuName = "ScriptableObjects/AnimationSettings")]
public class AnimationSettings : ScriptableObject
{
    public AnimationCurve pieceFallCurve;
    public float pieceFallTime = 1.0f;
    public float pieceLandMinVolume = 0.5f;
    public float pieceLandMaxVolume = 1.0f;
}