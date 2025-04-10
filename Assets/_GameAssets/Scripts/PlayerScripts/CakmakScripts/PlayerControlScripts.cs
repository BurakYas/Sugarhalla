using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerControlScripts : MonoBehaviour
{
    // Küp kontrol ve hareketleri için script dosyasıdır.

    [Header("References")]
    private Rigidbody _rigidBody;
    [SerializeField] private Transform _cameraTransform; // Kameranın Transform referansı

    [Header("Movement Settings")]
    [SerializeField] private float _rollingSpeed = 5f; // Küpün yuvarlanma hızı
    [SerializeField] private float _jumpForce = 5f; // Zıplama kuvveti
    [SerializeField] private LayerMask _groundLayer; // Zemin kontrolü için layer
    [SerializeField] private float _airControlMultiplier = 0.5f; // Havadayken hareket hızı çarpanı

    private bool _isMoving = false; // Hareket kontrolü
    private bool _canJump = true; // Zıplama kontrolü
    private Coroutine _currentRollCoroutine; // Mevcut coroutine referansı

    void Awake()
    {
        // Rigidbody bileşenini al
        _rigidBody = GetComponent<Rigidbody>();
        if (_rigidBody == null)
        {
            Debug.LogError("Rigidbody bileşeni bulunamadı!");
        }

        // Kamera referansını kontrol et
        if (_cameraTransform == null)
        {
            _cameraTransform = Camera.main.transform; // Eğer atanmadıysa Main Camera'yı kullan
        }
    }

    void Update()
    {
        // Eğer hareket ediyorsa diğer girişleri engelle
        if (_isMoving) return;

        // Hareket girişleri
        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        if (inputDirection.magnitude > 0.1f)
        {
            // Kameraya göre hareket yönünü hesapla
            Vector3 moveDirection = GetCameraRelativeDirection(inputDirection);

            // Karakterin yönünü hareket yönüne çevir
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * 10f);

            // Hareketi başlat
            StartRolling(moveDirection, onGround() ? _rollingSpeed : _rollingSpeed * _airControlMultiplier);
        }
        else
        {
            // Tuşa basılmadığında hareketi durdur
            StopRolling();
        }

        // Zıplama girişleri
        if (Input.GetKeyDown(KeyCode.Space) && _canJump && onGround())
        {
            PlayerJump();
        }
    }

    private Vector3 GetCameraRelativeDirection(Vector3 inputDirection)
    {
        // Kameranın yönüne göre hareket yönünü hesapla
        Vector3 cameraForward = _cameraTransform.forward;
        Vector3 cameraRight = _cameraTransform.right;

        // Y eksenini sıfırla (yatay düzlemde hareket)
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        return (cameraForward.normalized * inputDirection.z + cameraRight.normalized * inputDirection.x).normalized;
    }

    private void StartRolling(Vector3 dir, float speed)
    {
        if (_currentRollCoroutine != null)
        {
            StopCoroutine(_currentRollCoroutine); // Mevcut coroutine'i durdur
        }

        _currentRollCoroutine = StartCoroutine(Roll(dir, speed));
    }

    private void StopRolling()
    {
        if (_currentRollCoroutine != null)
        {
            StopCoroutine(_currentRollCoroutine); // Mevcut coroutine'i durdur
            _currentRollCoroutine = null;
            _isMoving = false; // Hareketi durdur
        }
    }

    IEnumerator Roll(Vector3 dir, float speed)
    {
        _isMoving = true;
        var anchor = transform.position + (Vector3.down + dir) * 0.5f;
        var axis = Vector3.Cross(Vector3.up, dir);

        for (int i = 0; i < (90 / speed); i++)
        {
            transform.RotateAround(anchor, axis, speed);
            yield return new WaitForSeconds(0.01f);
        }

        _isMoving = false;
    }

    private void PlayerJump()
    {
        // Mevcut dikey hızını sıfırla
        _rigidBody.linearVelocity = new Vector3(_rigidBody.linearVelocity.x, 0f, _rigidBody.linearVelocity.z);

        // Yukarı doğru kuvvet uygula
        _rigidBody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);

        // Zıplama kontrolü
        _canJump = false;
        Invoke(nameof(ResetJump), 0.5f); // 0.5 saniye sonra tekrar zıplayabilir
    }

    private void ResetJump()
    {
        _canJump = true; // Zıplama izni ver
    }

    private bool onGround()
    {
        // Zemin kontrolü için raycast
        return Physics.Raycast(transform.position, Vector3.down, 1f, _groundLayer);
    }
}
