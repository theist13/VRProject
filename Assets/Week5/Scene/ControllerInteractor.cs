using UnityEngine;

public class ControllerInteractor : MonoBehaviour
{
    [Header("Filter")]
    [SerializeField] string targetTag = "VRButton"; // ตั้งแท็กนี้บนวัตถุปุ่ม

    [Header("Haptics (Optional)")]
    [SerializeField] bool playHaptics = false;
    [SerializeField, Range(0f, 1f)] float amplitude = 0.4f;
    [SerializeField] float duration = 0.05f;

    void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(targetTag) && !other.CompareTag(targetTag)) return;

        // หา IInteractable บนวัตถุเป้าหมาย (หรือบนพาเรนต์)
        var interactable = other.GetComponentInParent<IInteractable>() ?? other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactable.Interact(gameObject);
            TryHaptics();
        }
    }

    void TryHaptics()
    {
        if (!playHaptics) return;

        // ตัวอย่างง่าย ๆ: ถ้าใช้ XR Interaction Toolkit สามารถเรียกทาง XRControllerHaptics ได้
        // ที่นี่ขอเว้นไว้เพราะแต่ละโปรเจ็กต์ใช้คลาสไม่เหมือนกัน
        // ใส่โค้ดสั่นของมินท์เองตรงนี้ได้เลย
    }
}
