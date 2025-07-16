using UnityEngine;

public class DashSphere : MonoBehaviour
{
    [Header("Dash Ayarları")]
    public float dashForce = 18f;
    public float dashDuration = 0.18f;
    public float dashCooldown = 1f;

    private Rigidbody rb;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float lastDashTime = -Mathf.Infinity;
    private Vector3 dashDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Dash(Vector3 direction)
    {
        if (Time.time < lastDashTime + dashCooldown || isDashing)
            return;

        isDashing = true;
        dashTimer = 0f;
        lastDashTime = Time.time;

        // Yalnızca yatay düzlemde dash
        direction.y = 0f;
        dashDirection = direction.normalized;

        // rb.linearVelocity = Vector3.zero; // Bunu kaldır!
        rb.AddForce(dashDirection * dashForce, ForceMode.Force); // VelocityChange yerine Force kullan
    }

    void Update()
    {
        if (isDashing)
        {
            dashTimer += Time.deltaTime;
            if (dashTimer >= dashDuration)
            {
                isDashing = false;
                // Dash bitince ekstra bir şey yapmak istersen buraya ekle
            }
        }
    }
}
