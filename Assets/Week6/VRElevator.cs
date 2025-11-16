using System.Collections;
using UnityEngine;

public class VRElevator : MonoBehaviour
{
    [Header("Cabin & Floors")]
    [Tooltip("ตัวลิฟต์ที่จะถูกขยับ ถ้าเว้นว่างจะใช้ transform ของ object นี้")]
    public Transform cabin;

    [Tooltip("จุดตำแหน่งของแต่ละชั้น ใส่เป็น Transform ตามลำดับ index = ชั้น")]
    public Transform[] floorPoints;

    [Tooltip("ความเร็วลิฟต์ หน่วยเป็นหน่วยต่อวินาที")]
    public float moveSpeed = 2f;

    [Header("Doors")]
    [Tooltip("Animator ของประตูลิฟต์ (มี bool parameter สำหรับเปิด/ปิด)")]
    public Animator doorAnimator;

    [Tooltip("ชื่อ parameter แบบ bool ใน Animator ที่ใช้สั่งเปิด/ปิดประตู")]
    public string doorOpenParam = "IsOpen";

    [Tooltip("เวลาที่ใช้รอให้อนิเมชันประตูจบ (เปิด/ปิด)")]
    public float doorAnimTime = 1.5f;

    [Header("Player")]
    [Tooltip("Tag ของ Root Player / XR Origin")]
    public string playerTag = "Player";

    [Tooltip("Trigger ภายในลิฟต์สำหรับเช็คว่าผู้เล่นเข้ามาอยู่ในลิฟต์หรือยัง")]
    public Collider cabinTrigger;

    private int currentFloor = 0;
    private bool isMoving = false;

    // เก็บ reference root ของ player ที่กำลังอยู่ในลิฟต์ (เอาไว้ parent)
    private Transform currentPlayerRoot;

    // เก็บสถานะประตู
    private bool doorsOpen = false;

    private void Awake()
    {
        if (cabin == null)
            cabin = transform;

        if (cabinTrigger == null)
        {
            // ลองหา trigger ในลูก ๆ ให้
            cabinTrigger = GetComponentInChildren<Collider>();
            if (cabinTrigger != null)
                cabinTrigger.isTrigger = true;
        }
    }

    /// <summary>
    /// เรียกลิฟต์จาก "ข้างใน" (ปุ่มในลิฟต์)
    /// ไปยังชั้นที่ floorIndex
    /// </summary>
    public void GoToFloor(int floorIndex)
    {
        if (!ValidateFloorIndex(floorIndex)) return;

        if (isMoving)
        {
            Debug.Log("VRElevator: กำลังเคลื่อนที่อยู่ (GoToFloor)");
            return;
        }

        if (floorIndex == currentFloor)
        {
            Debug.Log("VRElevator: อยู่ชั้นนี้อยู่แล้ว (GoToFloor)");
            return;
        }

        StartCoroutine(ElevatorSequence(floorIndex));
    }

    /// <summary>
    /// เรียกลิฟต์จากปุ่ม "หน้าลิฟต์" ชั้น floorIndex
    /// </summary>
    public void CallFromOutside(int floorIndex)
    {
        if (!ValidateFloorIndex(floorIndex)) return;

        if (isMoving)
        {
            Debug.Log("VRElevator: กำลังเคลื่อนที่อยู่ (CallFromOutside)");
            return;
        }

        // ถ้าลิฟต์อยู่ชั้นเดียวกับปุ่ม
        if (floorIndex == currentFloor)
        {
            Debug.Log("VRElevator: ลิฟต์อยู่ชั้นนี้แล้ว (CallFromOutside)");

            // ถ้าประตูปิด → เปิดให้
            if (!doorsOpen)
            {
                StartCoroutine(OpenDoorsRoutine());
            }
            // ถ้าเปิดอยู่แล้ว → ไม่ต้องทำอะไร
            return;
        }

        // ถ้าอยู่คนละชั้น → ใช้ sequence ปิดประตู → วิ่ง → เปิดประตู
        StartCoroutine(ElevatorSequence(floorIndex));
    }

    private bool ValidateFloorIndex(int floorIndex)
    {
        if (floorPoints == null || floorPoints.Length == 0)
        {
            Debug.LogWarning("VRElevator: ไม่ได้ตั้งค่า floorPoints");
            return false;
        }

        if (floorIndex < 0 || floorIndex >= floorPoints.Length)
        {
            Debug.LogWarning($"VRElevator: floorIndex {floorIndex} อยู่นอกช่วง 0-{floorPoints.Length - 1}");
            return false;
        }

        return true;
    }

    private IEnumerator ElevatorSequence(int targetFloorIndex)
    {
        isMoving = true;

        // 1. ปิดประตู (ถ้าเปิดอยู่)
        if (doorsOpen)
            yield return StartCoroutine(CloseDoorsRoutine());

        // 2. ขยับลิฟต์ไปชั้นเป้าหมาย
        yield return StartCoroutine(MoveCabinRoutine(targetFloorIndex));

        currentFloor = targetFloorIndex;

        // 3. เปิดประตู
        yield return StartCoroutine(OpenDoorsRoutine());

        isMoving = false;
    }

    private IEnumerator MoveCabinRoutine(int targetFloorIndex)
    {
        Vector3 startPos = cabin.position;
        Vector3 targetPos = floorPoints[targetFloorIndex].position;

        float distance = Vector3.Distance(startPos, targetPos);
        if (distance <= 0.001f)
        {
            cabin.position = targetPos;
            yield break;
        }

        float duration = distance / moveSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            cabin.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        cabin.position = targetPos;
    }

    private IEnumerator OpenDoorsRoutine()
    {
        if (doorAnimator != null && !string.IsNullOrEmpty(doorOpenParam))
        {
            doorAnimator.SetBool(doorOpenParam, true);
        }

        doorsOpen = true;

        if (doorAnimTime > 0f)
            yield return new WaitForSeconds(doorAnimTime);
    }

    private IEnumerator CloseDoorsRoutine()
    {
        if (doorAnimator != null && !string.IsNullOrEmpty(doorOpenParam))
        {
            doorAnimator.SetBool(doorOpenParam, false);
        }

        doorsOpen = false;

        if (doorAnimTime > 0f)
            yield return new WaitForSeconds(doorAnimTime);
    }

    // -------- Parenting ผู้เล่นให้ติดไปกับลิฟต์ --------

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag))
            return;

        Transform root = GetPlayerRoot(other.transform);
        if (root != null)
        {
            currentPlayerRoot = root;
            currentPlayerRoot.SetParent(cabin, true); // true = รักษา world position
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag))
            return;

        Transform root = GetPlayerRoot(other.transform);
        if (root != null && root == currentPlayerRoot)
        {
            currentPlayerRoot.SetParent(null, true);
            currentPlayerRoot = null;
        }
    }

    /// <summary>
    /// ดึง root ของ player (เช่น XR Origin หรือ object ที่มี CharacterController)
    /// เพื่อไม่ไป parent แค่มือหรือหัว
    /// </summary>
    private Transform GetPlayerRoot(Transform t)
    {
        // ถ้ามี CharacterController ใน parent ให้ใช้ตัวนั้นเป็น root
        CharacterController cc = t.GetComponentInParent<CharacterController>();
        if (cc != null)
            return cc.transform;

        // ถ้าใช้ XR Origin ชื่อแปลก ๆ อาจตั้ง tag "Player" ไว้ที่ root อยู่แล้ว
        Transform current = t;
        while (current.parent != null)
        {
            if (current.CompareTag(playerTag))
                return current;
            current = current.parent;
        }

        return t.root;
    }
}
