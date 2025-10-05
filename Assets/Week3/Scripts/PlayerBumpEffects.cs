using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PlayerBumpEffects : MonoBehaviour
{
    [Header("Scoring")]
    public string targetTag = "ScoreItem";
    public int pointsPerHit = 1;

    [Header("Haptics")]
    public XRDirectInteractor leftHand;   // ลากคอมโพเนนต์ XR Direct Interactor ของมือซ้ายมาใส่
    public XRDirectInteractor rightHand;  // ลากของมือขวามาใส่
    [Range(0f, 1f)] public float hapticAmplitude = 0.4f;
    [Range(0f, 1f)] public float hapticDuration = 0.08f;

    private readonly HashSet<Collider> _cooldown = new HashSet<Collider>();
    public float scoreCooldownPerCollider = 0.5f;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!string.IsNullOrEmpty(targetTag) && !hit.collider.CompareTag(targetTag))
            return;
        if (_cooldown.Contains(hit.collider)) return;

        _cooldown.Add(hit.collider);
        StartCoroutine(Clear(hit.collider));

        // +คะแนน
        ScoreManager.Instance?.Add(pointsPerHit);
        Destroy(hit.collider.gameObject);
        // สั่น (จะสั่นทั้งสองมือหรือเลือกมือเดียวก็ได้)
        SendHaptics(leftHand, hapticAmplitude, hapticDuration);
        SendHaptics(rightHand, hapticAmplitude, hapticDuration);
    }

    IEnumerator Clear(Collider c) { yield return new WaitForSeconds(scoreCooldownPerCollider); _cooldown.Remove(c); }

    void SendHaptics(XRDirectInteractor hand, float amp, float dur)
    {
        if (hand?.xrController != null)
            hand.xrController.SendHapticImpulse(amp, dur);
    }
}
