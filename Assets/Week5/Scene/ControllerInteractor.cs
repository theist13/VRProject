using UnityEngine;
// ตัว haptics เราจะใช้ผ่าน XRBaseInputInteractor (base class ของ XRDirectInteractor)
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ControllerInteractor : MonoBehaviour
{
    [Header("Filter")]
    [SerializeField] string targetTag = "VRButton"; // ตั้งแท็กนี้บนวัตถุปุ่ม

    [Header("Haptics")]
    [SerializeField] bool playHaptics = true;

    [SerializeField, Range(0f, 1f)]
    float amplitude = 0.4f;          // ความแรง 0–1

    [SerializeField]
    float duration = 0.05f;          // ระยะเวลาสั่น (วินาที)

    [Header("XR Interactor (มือข้างนี้)")]
    [SerializeField]
    XRBaseInputInteractor xrInteractor; // ใส่ XRDirectInteractor ของมือข้างนี้

    void Awake()
    {
        // ถ้าไม่ได้ลากใน Inspector ให้ลองหาในตัวเอง
        if (xrInteractor == null)
            xrInteractor = GetComponent<XRBaseInputInteractor>();
    }

    void OnTriggerEnter(Collider other)
    {
        // ฟิลเตอร์ tag
        if (!string.IsNullOrEmpty(targetTag) && !other.CompareTag(targetTag))
            return;

        // หา IInteractable บน object นั้น หรือบน parent
        var interactable =
            other.GetComponentInParent<IInteractable>() ??
            other.GetComponent<IInteractable>();

        if (interactable != null)
        {
            // ส่ง reference controller เข้าไปให้ด้วย เผื่อปุ่มอยากรู้ว่าใครกด
            interactable.Interact(gameObject);

            // สั่งจอยสั่น
            TryHaptics();
        }
    }

    void TryHaptics()
    {
        if (!playHaptics)
            return;

        if (xrInteractor == null)
            return;

        // ใช้เมธอดของ XRBaseInputInteractor (XRDirectInteractor สืบทอดมาจากตัวนี้)
        // amplitude = 0–1, duration = วินาที
        xrInteractor.SendHapticImpulse(amplitude, duration);
    }
}
