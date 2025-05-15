using UnityEngine;

public class NewControlHandler : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] public float moveSpeed = 5f; // Hareket hızı
    [SerializeField] private float jumpForce = 5f; // Zıplama kuvveti
    [SerializeField] private LayerMask groundLayer; // Zemin kontrolü için layer
    [SerializeField] public float torqueMultiplier = 1.5f; // Yuvarlanma için tork çarpanı
    [SerializeField] private float airControlMultiplier = 0.5f; // Havada hareket kontrolü için çarpan
    [SerializeField] public float maxAngularVelocity = 10f; // Maksimum açısal hız sınırı
    [SerializeField] private float maxSpeed = 20f; // Maksimum hız sınırı

    public bool canJump;

    public Vector3 GetInputDirection()
    {
        // Replace this with the actual logic to get the player's input direction
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        return new Vector3(horizontal, 0, vertical);
    }

    [Header("References")]
    [SerializeField] private Transform cameraTransform; // Kameranın Transform referansı
    private Rigidbody _rigidBody; // Karakterin Rigidbody bileşeni

    private bool _isGrounded = false; // Karakterin zeminde olup olmadığını kontrol eder

    void Start()
    {
        canJump = true;
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

            Jump(moveDirection);
        }

        // Havadayken hareketi kontrol et
        if (!IsGrounded())
        {
            AirMove(moveDirection);
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

    public void Jump(Vector3 direction)
    {
        if (canJump)
        {
              // Zıplama kuvvetini uygula (dikey kuvvet)
        _rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // Eğer bir yön girdisi varsa, ileri doğru bir kuvvet uygula
        if (direction != Vector3.zero)
        {
            // İleri doğru kuvvet uygula
            Vector3 jumpForceDirection = direction.normalized * moveSpeed * 0.5f; // Kuvveti azaltmak için 0.5 çarpanı
            _rigidBody.AddForce(jumpForceDirection, ForceMode.Impulse);
        }
        }
       

        // Hızı sınırla
        //LimitVelocity();
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

    private void AirMove(Vector3 direction)
    {
        // Havada hareket kontrolü için ek fonksiyon
        if (direction != Vector3.zero)
        {
            // Mevcut hızın büyüklüğünü al
            float currentSpeed = _rigidBody.linearVelocity.magnitude;

            // Hıza göre havada uygulanan kuvveti ölçekle
            float scaledAirControl = Mathf.Clamp(currentSpeed / maxSpeed, 0.5f, 1f); // 0.5 ile 1 arasında ölçekle

            // Havada hareket için kuvvet uygula
            Vector3 force = new Vector3(direction.x, 0f, direction.z) * moveSpeed * airControlMultiplier * scaledAirControl;
            _rigidBody.AddForce(force, ForceMode.Acceleration);
        }

        // Hızı sınırla
       // LimitVelocity();
    }

    /*private void LimitVelocity()
    {
        // Mevcut hızı kontrol et
        Vector3 velocity = _rigidBody.linearVelocity;

        // Havada hız sınırını X ve Z ekseninde uygula
        Vector3 horizontalVelocity = new Vector3(velocity.x, 0f, velocity.z);
        if (horizontalVelocity.magnitude > maxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
        }

        // Y eksenindeki hızı koruyarak yeni hız vektörünü uygula
        _rigidBody.linearVelocity = new Vector3(horizontalVelocity.x, velocity.y, horizontalVelocity.z);
    }
    */
}
