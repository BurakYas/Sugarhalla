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
    private bool canDoubleJump;
    private Rigidbody rb;
    private Vector3 currentDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Physics.gravity = new Vector3(0, -15f, 0);
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartCoroutine(Dash());
            }
        }
    }

    void FixedUpdate()
    {
        if (!isGrounded && !isDashing)
        {
            if (rb.linearVelocity.y < 0) // Updated from 'velocity' to 'linearVelocity'
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
            }
            else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space)) // Updated from 'velocity' to 'linearVelocity'
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
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
        if (isGrounded)
        {
            canDoubleJump = true;
            PerformJump();
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            PerformJump();
        }
    }

    private void PerformJump()
    {
        transform.position += Vector3.up * 0.1f;

        Vector3 jumpVelocity = rb.linearVelocity + (currentDirection * rollSpeed * 0.75f) + Vector3.up * jumpForce; // Updated from 'velocity' to 'linearVelocity'
        rb.linearVelocity = jumpVelocity; // Updated from 'velocity' to 'linearVelocity'

        isGrounded = false;
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            if (Input.GetKey(KeyCode.A)) currentDirection = Vector3.left;
            if (Input.GetKey(KeyCode.D)) currentDirection = Vector3.right;
            if (Input.GetKey(KeyCode.W)) currentDirection = Vector3.forward;
            if (Input.GetKey(KeyCode.S)) currentDirection = Vector3.back;

            Vector3 dashVelocity = currentDirection * dashSpeed;
            rb.linearVelocity = new Vector3(dashVelocity.x, rb.linearVelocity.y, dashVelocity.z); // Updated from 'velocity' to 'linearVelocity'
            yield return null;
        }

        rb.linearVelocity = Vector3.zero; // Updated from 'velocity' to 'linearVelocity'
        isDashing = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
