using UnityEngine;

public class SeedItem : MonoBehaviour
{
    [SerializeField] private CropDefinitionSO cropDefinition;

    public CropDefinitionSO CropDefinition => cropDefinition;
}
