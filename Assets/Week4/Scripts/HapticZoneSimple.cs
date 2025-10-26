using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticZoneSimple : MonoBehaviour
{
    [Header("Who is the Player?")]
    [Tooltip("ตั้ง Tag บน XR Origin/Player แล้วระบุชื่อ Tag ตรงนี้")]
    public string playerTag = "Player";
    [Tooltip("ถ้าตั้งไม่ใช้ Tag ให้ลาก Root ของ Player มาแทนได้ (เช่น XR Origin)")]
    public Transform playerRoot;

    [Header("Controllers to vibrate (assign manually)")]
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor leftController;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor rightController;

    [Header("Haptic (pulse while inside)")]
    [Range(0f, 1f)] public float amplitude = 0.45f;
    [Range(0f, 2f)] public float duration = 0.08f;
    [Range(0f, 2f)] public float pulseInterval = 0.20f;

    private int _playerCount;          // รองรับกรณีโซนซ้อนกัน/คอลลายเดอร์หลายชิ้นของ Player
    private Coroutine _loop;

    void OnTriggerEnter(Collider other)
    {
        if (!IsPlayer(other)) return;
        _playerCount++;
        if (_loop == null) _loop = StartCoroutine(PulseLoop());
    }

    void OnTriggerExit(Collider other)
    {
        if (!IsPlayer(other)) return;
        _playerCount = Mathf.Max(0, _playerCount - 1);
        if (_playerCount == 0 && _loop != null)
        {
            StopCoroutine(_loop);
            _loop = null;
        }
    }

    IEnumerator PulseLoop()
    {
        while (_playerCount > 0)
        {
            Send(leftController);
            Send(rightController);
            Debug.Log("Haptoc Conroller!!");
            yield return new WaitForSeconds(Mathf.Max(0f, pulseInterval));
        }
        _loop = null;
    }

    bool IsPlayer(Collider col)
    {
        if (playerRoot != null) return col.transform.IsChildOf(playerRoot);
        return col.CompareTag(playerTag);
    }

    void Send(UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor hand)
    {
        if (hand == null || hand.xrController == null) return;
        hand.xrController.SendHapticImpulse(Mathf.Clamp01(amplitude), Mathf.Max(0f, duration));
    }
}
