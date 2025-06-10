using UnityEngine;

public class NewControlHandler : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] private float minJumpForce = 3f; // Kısa basışta uygulanacak minimum kuvvet
    [SerializeField] private float maxJumpForce = 10f; // Maksimum toplam zıplama kuvveti
    [SerializeField] private float jumpHoldForce = 20f; // Basılı tutarken uygulanacak ek kuvvet (saniyede)
    [SerializeField] private float jumpHoldTimeLimit = 0.25f; // Maksimum ek kuvvet süresi (saniye)
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] public float torqueMultiplier = 1.5f;
    [SerializeField] private float airControlMultiplier = 0.5f;
    [SerializeField] public float maxAngularVelocity = 10f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float maxAirSpeed = 3f;

    public bool canJump;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    private Rigidbody _rigidBody;

    private bool _isGrounded = false;
    private Vector3 previousVelocity;

    // Variable jump değişkenleri
    private bool isJumping = false;
    private float currentJumpForce = 0f;
    private float jumpHoldTimer = 0f;

    void Start()
    {
        canJump = true;
        _rigidBody = GetComponent<Rigidbody>();
        if (_rigidBody == null)
        {
            Debug.LogError("Rigidbody bileşeni bulunamadı!");
        }

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        _rigidBody.maxAngularVelocity = maxAngularVelocity;

        // Rigidbody'nin uyumasını engelle
        _rigidBody.sleepThreshold = 0f;
    }

    void Update()
    {
        _rigidBody.WakeUp();
        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        Vector3 moveDirection = GetCameraRelativeDirection(inputDirection);

        Roll(moveDirection);

        // Zıplama başlat
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Jump(moveDirection);
            isJumping = true;
            jumpHoldTimer = 0f;
            currentJumpForce = minJumpForce;
        }

        // Zıplama tuşu basılı tutuluyorsa ve limit aşılmadıysa ek kuvvet uygula
        if (isJumping && Input.GetKey(KeyCode.Space) && jumpHoldTimer < jumpHoldTimeLimit && currentJumpForce < maxJumpForce)
        {
            float addForce = jumpHoldForce * Time.deltaTime;
            if (currentJumpForce + addForce > maxJumpForce)
                addForce = maxJumpForce - currentJumpForce;

            _rigidBody.AddForce(Vector3.up * addForce, ForceMode.Impulse);
            currentJumpForce += addForce;
            jumpHoldTimer += Time.deltaTime;
        }

        // Tuş bırakılırsa veya limit dolarsa ek kuvveti durdur
        if (isJumping && (!Input.GetKey(KeyCode.Space) || currentJumpForce >= maxJumpForce || jumpHoldTimer >= jumpHoldTimeLimit))
        {
            isJumping = false;
        }

        if (!IsGrounded())
        {
            AirMove(moveDirection);
        }
    }

    private Vector3 GetCameraRelativeDirection(Vector3 inputDirection)
    {
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        return (cameraForward.normalized * inputDirection.z + cameraRight.normalized * inputDirection.x).normalized;
    }

    private void Roll(Vector3 direction)
    {
        if (direction.magnitude > 0.05f) // Deadzone ekledik
        {
            float currentTorqueMultiplier = IsGrounded() ? torqueMultiplier : torqueMultiplier * airControlMultiplier;
            Vector3 torque = new Vector3(direction.z, 0f, -direction.x) * currentTorqueMultiplier;
            _rigidBody.AddTorque(torque, ForceMode.Force);
            LimitAngularVelocity();
        }
    }

    public void Jump(Vector3 direction)
    {
        if (canJump)
        {
            Vector3 velocity = _rigidBody.linearVelocity;
            velocity.x *= 0.3f;
            velocity.z *= 0.3f;
            velocity.y = 0f;
            _rigidBody.linearVelocity = velocity;

            _rigidBody.AddForce(Vector3.up * minJumpForce, ForceMode.Impulse);

            // Küçük de olsa ileri kuvvet uygula
            if (direction != Vector3.zero)
            {
                Vector3 jumpForward = direction.normalized * moveSpeed * 0.12f; // 0.12f ile ileri kuvveti ayarlayabilirsin
                _rigidBody.AddForce(jumpForward, ForceMode.Impulse);
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    }

    private void LimitAngularVelocity()
    {
        if (_rigidBody.angularVelocity.magnitude > maxAngularVelocity)
        {
            _rigidBody.angularVelocity = _rigidBody.angularVelocity.normalized * maxAngularVelocity;
        }
    }

    private void AirMove(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Vector3 force = new Vector3(direction.x, 0f, direction.z) * moveSpeed * airControlMultiplier * 0.1f;
            _rigidBody.AddForce(force, ForceMode.Acceleration);
        }

        Vector3 velocity = _rigidBody.linearVelocity;
        Vector3 horizontalVelocity = new Vector3(velocity.x, 0f, velocity.z);
        if (horizontalVelocity.magnitude > maxAirSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxAirSpeed;
            _rigidBody.linearVelocity = new Vector3(horizontalVelocity.x, velocity.y, horizontalVelocity.z);
        }
    }
}
