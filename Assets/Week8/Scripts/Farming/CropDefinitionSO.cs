using UnityEngine;

[CreateAssetMenu(menuName = "Farming/Crop Definition")]
public class CropDefinitionSO : ScriptableObject
{
    [Header("Display")]
    public string cropName;

    [Header("Prefabs")]
    public GameObject seedlingPrefab;
    public GameObject maturePrefab;

    [Header("Timing")]
    [Min(0.1f)] public float seedToSeedlingSeconds = 2f;
    [Min(0.1f)] public float seedlingToMatureSeconds = 4f;

    [Header("Economy")]
    [Min(0)] public int sellPrice = 5;
}
