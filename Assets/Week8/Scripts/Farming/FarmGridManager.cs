using System.Collections.Generic;
using UnityEngine;

public class FarmGridManager : MonoBehaviour
{
    [SerializeField] private List<GridPlot> plots = new();

    private void Awake()
    {
        if (plots.Count == 0)
            plots.AddRange(GetComponentsInChildren<GridPlot>());
    }

    public bool TryGetNearestFreePlot(Vector3 position, out GridPlot nearestPlot)
    {
        nearestPlot = null;
        float bestDistance = float.MaxValue;

        foreach (GridPlot plot in plots)
        {
            if (plot == null || plot.IsOccupied)
                continue;

            float distance = (plot.transform.position - position).sqrMagnitude;
            if (distance < bestDistance)
            {
                bestDistance = distance;
                nearestPlot = plot;
            }
        }

        return nearestPlot != null;
    }
}
