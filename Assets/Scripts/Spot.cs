using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Spot : MonoBehaviour
{
    [SerializeField] PlayerSettings playerSettings;

    Image imageComponent;
    public PlayerID OwnerID { get; private set; } = PlayerID.None;

    public bool IsEmpty { get { return OwnerID == PlayerID.None; } }

    public void Clear()
    {
        imageComponent.enabled = false;
        OwnerID = PlayerID.None;
    }

    public void SetOwner(PlayerID playerIndex)
    {
        OwnerID = playerIndex;
        ApplyRandomRotation();
        imageComponent.enabled = true;

        if (playerSettings != null)
        {
            imageComponent.sprite = playerSettings.GetPlayerPieceSprite(playerIndex);
        }
    }

    void Awake()
    {
        imageComponent = GetComponent<Image>();
    }

    void ApplyRandomRotation()
    {
        Vector3 rotation = transform.localEulerAngles;
        rotation.z = Random.Range(0.0f, 360.0f);
        transform.localEulerAngles = rotation;
    }
}
