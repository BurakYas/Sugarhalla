using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float rollSpeed = 5;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float dashSpeed = 30;
    [SerializeField] private float dashDuration = 0.1f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    private bool isMoving;
    private bool isGrounded;
    private bool isDashing;
    private Rigidbody rb;
    private Vector3 currentDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Küpün daha hızlı düşmesi ve titremeyi engellemek için yerçekimini artırıyoruz.
        Physics.gravity = new Vector3(0, -15f, 0); // Normalde -9.81, daha güçlü yerçekimi verdik

        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; // Daha doğru çarpışma algılama
    }


    void Update()
    {
        if (!isDashing)
        {
            if (isGrounded)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    currentDirection = Vector3.left;
                    Assemble(Vector3.left);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    currentDirection = Vector3.right;
                    Assemble(Vector3.right);
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    currentDirection = Vector3.forward;
                    Assemble(Vector3.forward);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    currentDirection = Vector3.back;
                    Assemble(Vector3.back);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartCoroutine(Dash());
            }
        }

        // Daha hızlı düşme mekaniği
        if (!isGrounded && !isDashing)
        {
            if (rb.linearVelocity.y < 0)
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
    }

    private void Assemble(Vector3 dir)
    {
        if (isMoving) return;

        var anchor = transform.position + (Vector3.down + dir) * 0.5f;
        var axis = Vector3.Cross(Vector3.up, dir);
        StartCoroutine(Roll(anchor, axis));
    }

    private IEnumerator Roll(Vector3 anchor, Vector3 axis)
    {
        isMoving = true;
        float rotationAngle = 0;
        while (rotationAngle < 90)
        {
            float rotationStep = rollSpeed * Time.deltaTime * 90;
            transform.RotateAround(anchor, axis, rotationStep);
            rotationAngle += rotationStep;
            yield return null;
        }
        isMoving = false;
    }

    private void Jump()
    {
        if (rb != null)
        {
            transform.position += Vector3.up * 0.1f; // Küçük bir yükselme ekleyerek titremeyi azalt

            // Hareket yönüne göre ileri momentum ekleme, mevcut hızını bozmayacak şekilde ayarlandı
            Vector3 jumpVelocity = rb.linearVelocity + (currentDirection * rollSpeed * 0.75f) + Vector3.up * jumpForce;
            rb.linearVelocity = jumpVelocity;
            isGrounded = false;
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        float startTime = Time.time;

        Vector3 dashVelocity = currentDirection * dashSpeed;
        rb.linearVelocity = new Vector3(dashVelocity.x, 0, dashVelocity.z); // Y ekseninde süzülmeyi engelledik

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
