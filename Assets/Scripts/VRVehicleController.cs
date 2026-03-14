using System.Collections;
using UnityEngine;

public class VRVehicleController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform playerRigRoot;
    [SerializeField] Transform seatPoint;
    [SerializeField] Transform exitPoint;

    [Header("Reset")]
    [SerializeField] bool resetVehicleOnExit = true;
    [SerializeField] Transform resetPoint;

    [Header("Step Movement")]
    [SerializeField, Min(0.01f)] float moveStep = 0.5f;
    [SerializeField, Min(0.01f)] float moveDuration = 0.15f;
    [SerializeField] Vector3 forwardAxis = Vector3.forward;
    [SerializeField] Vector3 verticalAxis = Vector3.up;

    Transform _playerOriginalParent;
    Vector3 _initialVehiclePosition;
    Quaternion _initialVehicleRotation;
    bool _isPlayerInside;
    bool _isMoving;

    void Awake()
    {
        _initialVehiclePosition = transform.position;
        _initialVehicleRotation = transform.rotation;
    }

    public void EnterVehicle()
    {
        if (playerRigRoot == null || seatPoint == null)
            return;

        if (_isPlayerInside)
            return;

        _playerOriginalParent = playerRigRoot.parent;
        playerRigRoot.SetParent(seatPoint, true);
        playerRigRoot.position = seatPoint.position;
        playerRigRoot.rotation = seatPoint.rotation;

        _isPlayerInside = true;
    }

    public void ExitVehicle()
    {
        if (playerRigRoot == null || !_isPlayerInside)
            return;

        playerRigRoot.SetParent(_playerOriginalParent, true);

        if (exitPoint != null)
        {
            playerRigRoot.position = exitPoint.position;
            playerRigRoot.rotation = exitPoint.rotation;
        }

        _isPlayerInside = false;
        _isMoving = false;
        StopAllCoroutines();

        if (resetVehicleOnExit)
            ResetVehicle();
    }

    public void MoveForward()
    {
        MoveVehicle(forwardAxis);
    }

    public void MoveBackward()
    {
        MoveVehicle(-forwardAxis);
    }

    public void MoveUp()
    {
        MoveVehicle(verticalAxis);
    }

    public void MoveDown()
    {
        MoveVehicle(-verticalAxis);
    }

    void MoveVehicle(Vector3 axis)
    {
        if (!_isPlayerInside)
            return;

        if (_isMoving)
            return;

        if (axis.sqrMagnitude < 0.001f)
            return;

        Vector3 direction = axis.normalized;
        Vector3 targetPosition = transform.position + direction * moveStep;

        StartCoroutine(SmoothStepMove(targetPosition));
    }

    IEnumerator SmoothStepMove(Vector3 targetPosition)
    {
        _isMoving = true;

        Vector3 startPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;

            // ทำให้ต้นช้า-ปลายช้า ดูนุ่มกว่า lerp ตรงๆ
            t = Mathf.SmoothStep(0f, 1f, t);

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition;
        _isMoving = false;
    }

    public void ResetVehicle()
    {
        StopAllCoroutines();
        _isMoving = false;

        if (resetPoint != null)
        {
            transform.position = resetPoint.position;
            transform.rotation = resetPoint.rotation;
            return;
        }

        transform.position = _initialVehiclePosition;
        transform.rotation = _initialVehicleRotation;
    }
}