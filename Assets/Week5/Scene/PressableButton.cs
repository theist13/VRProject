using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
public class PressableButton : MonoBehaviour, IInteractable
{
    [Header("Visual Press")]
    [SerializeField] Transform pressTarget;      // ชิ้นส่วนที่ขยับตอนถูกกด (ไม่ตั้ง = ใช้ตัวเอง)
    [SerializeField] float pressDepth = 0.02f;   // ระยะกด (เมตร)
    [SerializeField] float pressTime = 0.06f;   // เวลากดยุบ/เด้ง

    [Header("Direction")]
    [Tooltip("ทิศทางการกด (เช่น X=1,0,0 คือไปทางขวา / 0,-1,0 คือกดลง)")]
    [SerializeField] Vector3 pressDirection = new Vector3(0, -1, 0); // ค่าเริ่มต้น = กดลง
    [Tooltip("ตีความทิศทางใน Local Space (ตามแกนของปุ่ม) หรือ World Space")]
    [SerializeField] Space directionSpace = Space.Self; // Self=Local, World=World

    [Header("Behavior")]
    [Tooltip("ชื่อฉากที่จะโหลด (ต้องอยู่ใน Build Settings) เว้นว่าง = ไม่เปลี่ยนฉาก")]
    [SerializeField] string sceneToLoad = "";
    [SerializeField] UnityEvent onPressed;   // เรียกตอนกดยุบ
    [SerializeField] UnityEvent onReleased;  // เรียกตอนเด้งกลับ

    [Header("Safety")]
    [SerializeField] float debounceSeconds = 0.3f;

    Vector3 _initialLocalPos;
    bool _busy;
    float _lastPressedTime;

    void Awake()
    {
        if (pressTarget == null) pressTarget = transform;
        _initialLocalPos = pressTarget.localPosition;
    }

    public void Interact(GameObject interactor)
    {
        if (_busy) return;
        if (Time.time - _lastPressedTime < debounceSeconds) return;

        _lastPressedTime = Time.time;
        StartCoroutine(DoPress());
    }

    System.Collections.IEnumerator DoPress()
    {
        _busy = true;

        // คำนวณตำแหน่งปลายทางตาม "ทิศทางที่กำหนด"
        Vector3 fromLocal = pressTarget.localPosition;
        Vector3 toLocal = ComputePressedLocalPosition();

        // กดยุบ
        yield return MoveLocal(pressTarget, fromLocal, toLocal, pressTime);
        onPressed?.Invoke();

        // เด้งกลับ
        yield return MoveLocal(pressTarget, pressTarget.localPosition, _initialLocalPos, pressTime);
        onReleased?.Invoke();

        // เปลี่ยนฉากถ้าระบุ
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }

        _busy = false;
    }

    // แปลงทิศทาง + ระยะ ให้เป็นตำแหน่ง local ปลายทาง
    Vector3 ComputePressedLocalPosition()
    {
        // กันกรณีใส่ (0,0,0)
        Vector3 dir = pressDirection.sqrMagnitude < 1e-8f ? Vector3.down : pressDirection.normalized;

        if (directionSpace == Space.Self)
        {
            // ทิศทางตามแกนของปุ่ม (Local)
            return _initialLocalPos + (dir * pressDepth);
        }
        else
        {
            // ทิศทางตามแกนโลก (World) แปลงกลับมาเป็น Local ของ parent
            Transform p = pressTarget;
            Vector3 worldFrom = p.position;
            Vector3 worldTo = worldFrom + dir * pressDepth;

            if (p.parent != null)
                return p.parent.InverseTransformPoint(worldTo);
            else
                return worldTo; // ไม่มีพาเรนต์ ก็ถือ world=local ไปเลย
        }
    }

    IEnumerator MoveLocal(Transform t, Vector3 from, Vector3 to, float duration)
    {
        if (duration <= 0f) { t.localPosition = to; yield break; }
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float k = Mathf.Clamp01(elapsed / duration);
            float e = Mathf.SmoothStep(0f, 1f, k);           // ease in-out
            t.localPosition = Vector3.LerpUnclamped(from, to, e);
            yield return null;
        }
        t.localPosition = to;
    }

#if UNITY_EDITOR
    // ช่วยมองทิศทางตอนเลือกวัตถุใน Scene
    void OnDrawGizmosSelected()
    {
        Transform t = pressTarget ? pressTarget : transform;
        Vector3 origin = t.position;

        Vector3 dir = pressDirection.sqrMagnitude < 1e-8f ? Vector3.down : pressDirection.normalized;
        Vector3 worldDir = (directionSpace == Space.Self) ? t.TransformDirection(dir) : dir;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(origin, origin + worldDir * pressDepth);
        Gizmos.DrawSphere(origin + worldDir * pressDepth, 0.005f);
    }
#endif
}