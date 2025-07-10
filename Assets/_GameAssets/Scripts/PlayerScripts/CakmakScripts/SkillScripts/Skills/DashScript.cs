using UnityEngine;

public class DashScript : MonoBehaviour
{
    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private Rigidbody rb;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float lastDashTime = -Mathf.Infinity;

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

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(direction.normalized * dashForce, ForceMode.VelocityChange);
    }

    void Update()
    {
        if (isDashing)
        {
            dashTimer += Time.deltaTime;
            if (dashTimer >= dashDuration)
            {
                isDashing = false;
                // rb.linearVelocity = Vector3.zero; // Bunu kaldır!
            }
        }
        // Artık burada input kontrolü yok!
    }
}
