// HapticOnContactSimple.cs
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Feedback;

// ต้องอยู่บน Player ที่มี CharacterController
[RequireComponent(typeof(CharacterController))]
public class HapticOnContactSimple : MonoBehaviour
{
    [Header("Controllers to vibrate (assign in Inspector)")]
      public SimpleHapticFeedback leftController;
      public SimpleHapticFeedback rightController;

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

  void Vibrate(SimpleHapticFeedback hand)
    {
     if (hand == null)
        {
            if (debugAllHits)
                Debug.Log("[Haptic] Skip: SimpleHapticFeedback is null");
            return;
        }

        // ใช้ API ของ SimpleHapticFeedback
        hand.hapticImpulsePlayer.SendHapticImpulse(
            Mathf.Clamp01(amplitude),
            Mathf.Max(0f, duration)
        );

        if (debugAllHits)
            Debug.Log("[Haptic] Haptic feedback fired!");
    }
}
