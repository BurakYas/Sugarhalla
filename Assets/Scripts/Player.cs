using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float rollSpeed = 5;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float dashSpeed = 20;
    [SerializeField] private float dashDuration = 0.2f;
    private bool isMoving;
    private bool isGrounded;
    private bool isDashing;
    private Rigidbody rb;
    private Vector3 currentDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 1; // Add some drag to smooth out the movement
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

            if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
            {
                StartCoroutine(Dash());
            }
        }
    }

    private void Assemble(Vector3 dir)
    {
        if (isMoving) return;

        var anchor = transform.position + (Vector3.down + dir) * 0.5f; // Center of the cube
        var axis = Vector3.Cross(Vector3.up, dir); // Axis of rotation
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
            Vector3 jumpVelocity = currentDirection * rollSpeed + Vector3.up * jumpForce;
            rb.linearVelocity = jumpVelocity;
            isGrounded = false;
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            rb.linearVelocity = currentDirection * dashSpeed;
            yield return null;
        }

        rb.linearVelocity = Vector3.zero;
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
