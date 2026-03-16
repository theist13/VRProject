using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GridPlot : MonoBehaviour
{
    [SerializeField] private Transform snapPoint;

    private bool isOccupied;
    private CropDefinitionSO activeCrop;
    private Coroutine growthRoutine;

    public bool IsOccupied => isOccupied;

    private void OnTriggerEnter(Collider other)
    {
        if (isOccupied)
            return;

        SeedItem seed = other.GetComponent<SeedItem>();
        if (seed == null || seed.CropDefinition == null)
            return;

        XRGrabInteractable grab = other.GetComponent<XRGrabInteractable>();
        if (grab != null && grab.isSelected)
            return;

        PlantSeed(seed);
    }

    private void PlantSeed(SeedItem seed)
    {
        activeCrop = seed.CropDefinition;
        isOccupied = true;

        Vector3 targetPosition = snapPoint != null ? snapPoint.position : transform.position;
        Quaternion targetRotation = snapPoint != null ? snapPoint.rotation : transform.rotation;

        Destroy(seed.gameObject);

        if (growthRoutine != null)
            StopCoroutine(growthRoutine);

        growthRoutine = StartCoroutine(GrowCrop(targetPosition, targetRotation));
    }

    private IEnumerator GrowCrop(Vector3 spawnPosition, Quaternion spawnRotation)
    {
        yield return new WaitForSeconds(activeCrop.seedToSeedlingSeconds);

        GameObject seedlingInstance = null;
        if (activeCrop.seedlingPrefab != null)
            seedlingInstance = Instantiate(activeCrop.seedlingPrefab, spawnPosition, spawnRotation, transform);

        yield return new WaitForSeconds(activeCrop.seedlingToMatureSeconds);

        if (seedlingInstance != null)
            Destroy(seedlingInstance);

        if (activeCrop.maturePrefab != null)
        {
            GameObject mature = Instantiate(activeCrop.maturePrefab, spawnPosition, spawnRotation);
            HarvestItem harvest = mature.GetComponent<HarvestItem>();
            if (harvest == null)
                harvest = mature.AddComponent<HarvestItem>();

            harvest.Setup(activeCrop.sellPrice);

            PlotOwnership ownership = mature.GetComponent<PlotOwnership>();
            if (ownership == null)
                ownership = mature.AddComponent<PlotOwnership>();

            ownership.Setup(this);
        }

        activeCrop = null;
        growthRoutine = null;
    }

    public void ReleasePlot()
    {
        isOccupied = false;
    }
}
