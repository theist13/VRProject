using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class HarvestBasket : MonoBehaviour
{
    [SerializeField] private PlayerWallet wallet;

    private void OnTriggerEnter(Collider other)
    {
        HarvestItem harvest = other.GetComponent<HarvestItem>();
        if (harvest == null)
            return;

        XRGrabInteractable grab = other.GetComponent<XRGrabInteractable>();
        if (grab != null && grab.isSelected)
            return;

        if (wallet != null)
            wallet.AddMoney(harvest.SellPrice);

        Destroy(harvest.gameObject);
    }
}
