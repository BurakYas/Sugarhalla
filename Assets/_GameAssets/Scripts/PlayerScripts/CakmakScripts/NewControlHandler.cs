using UnityEngine;

public class NewControlHandler : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f; // Hareket hızı
    [SerializeField] private float jumpForce = 5f; // Zıplama kuvveti
    [SerializeField] private LayerMask groundLayer; // Zemin kontrolü için layer
    [SerializeField] private float torqueMultiplier = 10f; // Yuvarlanma için tork çarpanı
    [SerializeField] private float airControlMultiplier = 0.5f; // Havada hareket kontrolü için çarpan
    [SerializeField] private float maxAngularVelocity = 10f; // Maksimum açısal hız sınırı

    [Header("References")]
    [SerializeField] private Transform cameraTransform; // Kameranın Transform referansı
    private Rigidbody _rigidBody; // Karakterin Rigidbody bileşeni

    private bool _isGrounded = false; // Karakterin zeminde olup olmadığını kontrol eder

    void Start()
    {
        // Rigidbody bileşenini al
        _rigidBody = GetComponent<Rigidbody>();
        if (_rigidBody == null)
        {
            Debug.LogError("Rigidbody bileşeni bulunamadı!");
        }

        // Kamera referansını kontrol et
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform; // Eğer atanmadıysa Main Camera'yı kullan
        }

        // Rigidbody'nin maksimum açısal hızını ayarla
        _rigidBody.maxAngularVelocity = maxAngularVelocity;
    }

    void Update()
    {
        // Hareket girişlerini al
        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        // Kameraya göre hareket yönünü hesapla
        Vector3 moveDirection = GetCameraRelativeDirection(inputDirection);

        // Hareketi uygula
        Roll(moveDirection);

        // Zıplama girişini kontrol et
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Jump();
        }
    }

    private Vector3 GetCameraRelativeDirection(Vector3 inputDirection)
    {
        // Kameranın yönüne göre hareket yönünü hesapla
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // Y eksenini sıfırla (yatay düzlemde hareket)
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        return (cameraForward.normalized * inputDirection.z + cameraRight.normalized * inputDirection.x).normalized;
    }

    private void Roll(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            // Havada mı yoksa zeminde mi olduğuna göre tork çarpanını belirle
            float currentTorqueMultiplier = IsGrounded() ? torqueMultiplier : torqueMultiplier * airControlMultiplier;

            // Hareket yönüne göre tork uygula
            Vector3 torque = new Vector3(direction.z, 0f, -direction.x) * currentTorqueMultiplier;
            _rigidBody.AddTorque(torque, ForceMode.Force);

            // Açısal hızı sınırla
            LimitAngularVelocity();
        }
    }

    private void Jump()
    {
        // Zıplama kuvvetini uygula
        _rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private bool IsGrounded()
    {
        // Zemin kontrolü için raycast
        return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    }

    private void LimitAngularVelocity()
    {
        // Açısal hızı sınırla
        if (_rigidBody.angularVelocity.magnitude > maxAngularVelocity)
        {
            _rigidBody.angularVelocity = _rigidBody.angularVelocity.normalized * maxAngularVelocity;
        }
    }
}
