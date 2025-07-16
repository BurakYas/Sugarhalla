using UnityEngine;

public class Version2Controller : MonoBehaviour
{
    [Header("Küre Hareket Ayarları")]
    [SerializeField] private float moveForce = 10f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float gravityMultiplier = 1.5f;
    [SerializeField] private float minJumpForce = 5f;
    [SerializeField] private float maxJumpForce = 12f;
    [SerializeField] private float jumpHoldForce = 20f;
    [SerializeField] private float jumpHoldTimeLimit = 0.25f;
    [SerializeField] private float airControlMultiplier = 0.5f;
    [SerializeField] private int maxJumps = 1;

    [SerializeField] private Transform cameraTransform;
    
    private SkillManagerSphere skillManager;

    private Rigidbody rb;
    private int jumpCount = 0;
    private bool isJumping = false;
    private float currentJumpForce = 0f;
    private float jumpHoldTimer = 0f;
    private bool isDashing = false; // Dash başladığında true, bitince false

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 20f;
        skillManager = GetComponent<SkillManagerSphere>();
    }

    void Update()
    {
        if (!isDashing)
            Move();

        // Zıplama başlat
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            Jump();
            jumpCount++;
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

            rb.AddForce(Vector3.up * addForce, ForceMode.Impulse);
            currentJumpForce += addForce;
            jumpHoldTimer += Time.deltaTime;
        }

        // Tuş bırakılırsa veya limit dolarsa ek kuvveti durdur
        if (isJumping && (!Input.GetKey(KeyCode.Space) || currentJumpForce >= maxJumpForce || jumpHoldTimer >= jumpHoldTimeLimit))
        {
            isJumping = false;
        }

        // Yere yeni inildiyse jumpCount'u sıfırla
        if (IsGrounded())
        {
            jumpCount = 0;
        }

        if (Input.GetKeyDown(KeyCode.Q) && skillManager != null && !isDashing)
        {
            Vector3 dashDir = rb.linearVelocity.magnitude > 0.1f ? rb.linearVelocity.normalized : transform.forward;
            isDashing = true;
            skillManager.ActivateSkill("Dash", dashDir);
            Invoke(nameof(EndDash), 0.18f); // dashDuration kadar sonra isDashing = false
        }

        if (Input.GetKeyDown(KeyCode.R) && skillManager != null)
        {
            skillManager.ActivateSkill("Spin", Vector3.zero);
        }
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 inputDir = new Vector3(h, 0f, v);

        // Kamera yönüne göre hareket
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        Vector3 moveDir = (camForward.normalized * inputDir.z + camRight.normalized * inputDir.x).normalized;

        Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        float forceMultiplier = 1f;
        if (horizontalVel.magnitude > 1f && moveDir.magnitude > 0.1f)
        {
            // Dot product ile mevcut hız ile input yönü arasındaki ilişkiyi bul
            float dot = Vector3.Dot(horizontalVel.normalized, moveDir);
            // Eğer dot < 0 ise (ters yön), kuvveti azalt
            if (dot < 0)
            {
                forceMultiplier = Mathf.Lerp(1f, 0.2f, -dot); // Ters yönde kuvveti azalt (0.2f minimum kuvvet)
            }
        }

        // Havada hareket azaltıcı
        float finalMoveForce = IsGrounded() ? moveForce : moveForce * airControlMultiplier;

        if (moveDir.magnitude > 0.1f)
        {
            rb.AddForce(moveDir * finalMoveForce * forceMultiplier, ForceMode.Force);
        }
        else
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, new Vector3(0f, rb.linearVelocity.y, 0f), Time.deltaTime * 2f);
        }

        // Maksimum hız sınırı
        Vector3 horizontalVel2 = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (horizontalVel2.magnitude > maxSpeed)
        {
            Vector3 limitedVel = horizontalVel2.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }

        // Yer çekimi ivmesi
        if (!IsGrounded() && !isDashing)
        {
            rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
        }
    }

    private void Jump()
    {
        // Y eksenindeki mevcut hızı sıfırla ve yukarıya minimum kuvvet uygula
        Vector3 vel = rb.linearVelocity;
        vel.y = 0f;
        rb.linearVelocity = vel;
        rb.AddForce(Vector3.up * minJumpForce, ForceMode.Impulse);
    }

    private bool IsGrounded()
    {
        // Küre yere değiyor mu kontrolü (raycast ile)
        bool grounded = Physics.Raycast(transform.position, Vector3.down, 0.6f);
        return grounded;
    }

    private void EndDash()
    {
        isDashing = false;
    }
}
