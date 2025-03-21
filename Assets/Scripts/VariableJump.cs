using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableJump : MonoBehaviour
{
    [SerializeField] private float rollSpeed = 5;
    private bool isMoving;
    public Rigidbody rb;
    public float buttonTime = 0.5f;
    public float jumpHeight = 5;
    public float cancelRate = 100;
    float jumpTime;
    bool jumping;
    bool jumpCancelled;

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Assemble(Vector3.left);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Assemble(Vector3.right);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            Assemble(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Assemble(Vector3.back);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float jumpForce = Mathf.Sqrt(jumpHeight * -2 * Physics.gravity.y);
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);

            jumping = true;
            jumpCancelled = false;
            jumpTime = 0;
        }

        if (jumping)
        {
            jumpTime += Time.deltaTime;
            if (Input.GetKeyUp(KeyCode.Space))
            {
                jumpCancelled = true;
            }

            if (jumpTime > buttonTime)
            {
                jumping = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (jumpCancelled && jumping && rb.linearVelocity.y > 0)
        {
            rb.AddForce(Vector3.down * cancelRate);
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
}
