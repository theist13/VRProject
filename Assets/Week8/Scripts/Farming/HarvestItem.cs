using UnityEngine;

public class HarvestItem : MonoBehaviour
{
    [SerializeField, Min(0)] private int sellPrice;

    public int SellPrice => sellPrice;

    public void Setup(int price)
    {
        sellPrice = Mathf.Max(0, price);
    }
}
