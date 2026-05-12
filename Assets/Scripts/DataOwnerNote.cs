using UnityEngine;

public sealed class DataOwnerNote : MonoBehaviour
{
    [Header("Asset Metadata")]
    public string DataOwner;
    [TextArea(2, 6)] public string Notes;
}
