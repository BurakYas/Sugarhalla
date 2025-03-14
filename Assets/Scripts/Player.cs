using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        movement = movement.normalized * speed * Time.deltaTime;

        if (movement != Vector3.zero)
        {
            // Move the cube
            transform.Translate(movement, Space.World);

            // Calculate the rotation axis and angle
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, movement).normalized;
            float angle = movement.magnitude * (180 / Mathf.PI) / (transform.localScale.x / 2);

            // Apply the rotation
            transform.RotateAround(transform.position - movement / 2, rotationAxis, angle);
        }
    }
}
