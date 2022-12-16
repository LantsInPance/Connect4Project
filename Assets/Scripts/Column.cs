using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Column : MonoBehaviour
{
    [SerializeField] AnimationSettings animationSettings;
    [SerializeField] PlayerSettings playerSettings;
    [SerializeField] Spot[] spots;
    [SerializeField] Button dropButton;
    [SerializeField] AudioSource pieceLandSound;

    public bool IsFull { get; private set; }
    public Transform FallingPiece { get; set; }

    WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    public void Clear()
    {
        for (int i = 0; i < spots.Length; i++)
        {
            Spot spot = spots[i];
            spot.Clear();
        }

        IsFull = false;
        if (dropButton != null)
        {
            dropButton.interactable = true;
        }
    }

    public void DropPiece(PlayerID playerID, UnityAction onComplete)
    {
        FallingPiece.position = dropButton.transform.position;
        FallingPiece.gameObject.SetActive(true);
        for (int i = spots.Length - 1; i >= 0; i--)
        {
            Spot spot = spots[i];
            if (spot != null)
            {
                if (spot.IsEmpty)
                {
                    // Start falling to this spot, then do the following and call the onComplete
                    StartCoroutine(AnimateFall(spot, playerID, onComplete));

                    if (i == 0)
                    {
                        IsFull = true;
                        if (dropButton != null)
                        {
                            dropButton.interactable = false;
                        }
                    }

                    break;
                }
            }
            else
            {
                Debug.LogError("Spot at index " + i + " is null", this);
            }
        }
    }

    public int GetIndexOfHighestOwnedSpot()
    {
        for (int i = 0; i < spots.Length; i++)
        {
            if (!spots[i].IsEmpty)
            {
                return i;
            }
        }

        return -1;
    }

    public int GetIndexOfLowestUnownedSpot()
    {
        for (int i = spots.Length - 1; i >= 0; i--)
        {
            if (spots[i].IsEmpty)
            {
                return i;
            }
        }

        return -1;
    }

    public Spot GetSpotAtIndex(int index)
    {
        if (index < spots.Length && index >= 0)
        {
            return spots[index];
        }
    
        return null;
    }

    private void Start()
    {
        dropButton.image.sprite = playerSettings.GetPlayerPieceSprite(PlayerID.PlayerOne);
    }

    IEnumerator AnimateFall(Spot spot, PlayerID playerID, UnityAction onComplete)
    {
        if (FallingPiece != null && animationSettings != null)
        {
            Vector3 currentPosition = dropButton.transform.position;
            float startY = currentPosition.y;
            float lowestY = spots[spots.Length-1].transform.position.y;
            float targetPositionY = spot.transform.position.y;
            FallingPiece.transform.position = currentPosition;

            float currentTime = 0.0f;
            while (currentPosition.y > targetPositionY)
            {
                currentTime += Time.deltaTime;
                float t = currentTime / animationSettings.pieceFallTime;
                float adjustedT = animationSettings.pieceFallCurve.Evaluate(t);
                currentPosition.y = Mathf.Lerp(startY, lowestY, adjustedT);
                currentPosition.y = Mathf.Max(currentPosition.y, targetPositionY);
                FallingPiece.transform.position = currentPosition;
                yield return waitForEndOfFrame;
            }

            float fullFallDistance = lowestY - startY;
            float actualFallDistance = targetPositionY - startY;
            float volumeT = actualFallDistance / fullFallDistance;
            pieceLandSound.volume = Mathf.Lerp(animationSettings.pieceLandMinVolume, animationSettings.pieceLandMaxVolume, volumeT);
            pieceLandSound.Play();
            FallingPiece.gameObject.SetActive(false);
        }

        spot.SetOwner(playerID);
        onComplete?.Invoke();
    }
}
