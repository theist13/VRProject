using UnityEngine;

public class PlotOwnership : MonoBehaviour
{
    private GridPlot ownerPlot;
    private bool released;

    public void Setup(GridPlot plot)
    {
        ownerPlot = plot;
    }

    private void OnDestroy()
    {
        if (released)
            return;

        released = true;
        if (ownerPlot != null)
            ownerPlot.ReleasePlot();
    }
}
