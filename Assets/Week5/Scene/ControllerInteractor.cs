using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Feedback;

public class ControllerInteractor : MonoBehaviour
{
    [Header("Filter")]
    [SerializeField] string targetTag = "VRButton";

    [Header("Haptic Feedback")]
    [SerializeField] bool playHaptics = true;

    [SerializeField, Range(0f, 1f)]
    float amplitude = 0.4f;

    [SerializeField]
    float duration = 0.05f;

    [Header("Feedback Provider")]
    [SerializeField]
    SimpleHapticFeedback feedback;  

    void Awake()
    {
        if (feedback == null)
            feedback = GetComponent<SimpleHapticFeedback>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{name} Hit {other.name}");

        if (!string.IsNullOrEmpty(targetTag) && !other.CompareTag(targetTag))
            return;

        var interactable =
            other.GetComponentInParent<IInteractable>() ??
            other.GetComponent<IInteractable>();

        if (interactable != null)
        {
            interactable.Interact(gameObject);
            TryHaptics();
        }
    }

    void TryHaptics()
    {
        if (!playHaptics || feedback == null)
            return;

        feedback.hapticImpulsePlayer.SendHapticImpulse(amplitude, duration);
        Debug.Log("[HAPTIC] Feedback fired.");
    }
}
