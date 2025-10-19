// HapticOnContactSimple.cs
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// ต้องอยู่บน Player ที่มี CharacterController
[RequireComponent(typeof(CharacterController))]
public class HapticOnContactSimple : MonoBehaviour
{
    [Header("Controllers to vibrate (assign in Inspector)")]
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor leftController;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor rightController;

    [Header("Filter by Tag (leave empty = any)")]
    public string requiredTag = "HapticTarget";

    [Header("Haptic")]
    [Range(0f, 1f)] public float amplitude = 0.55f;
    [Range(0f, 1f)] public float duration = 0.10f;
    [Range(0f, 2f)] public float cooldown = 0.25f;

    [Header("Debug")]
    public bool debugAllHits = true;

    private float _nextTime;

    // ยิงเมื่อ CharacterController ของ Player ชน collider (non-trigger)
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (Time.time < _nextTime) return;

        if (hit.collider.CompareTag(requiredTag))
        {
            Vibrate(leftController);
            Vibrate(rightController);

            _nextTime = Time.time + cooldown;

        }

    }

    void Vibrate(UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor hand)
    {
        if (hand == null)
        {
            if (debugAllHits) Debug.Log("[Haptic] Skip: hand is null");
            return;
        }

        var ctrl = hand.xrController;
        if (ctrl == null)
        {
            if (debugAllHits) Debug.Log($"[Haptic] Skip: xrController is null on '{hand.name}'");
            return;
        }

        ctrl.SendHapticImpulse(Mathf.Clamp01(amplitude), Mathf.Max(0f, duration));
        Debug.Log("[Haptic] Haptic Controller on contact!!");
    }
}
