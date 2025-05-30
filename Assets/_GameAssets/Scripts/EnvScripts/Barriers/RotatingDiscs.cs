using UnityEngine;

public class RotatingDiscs : MonoBehaviour
{
    [Header("Dönüş Ayarları")]
    public float rotationSpeed = 90f; // Derece/saniye cinsinden hız

    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Platformun açısal hızını karaktere uygula
                Vector3 platformAngularVelocity = Vector3.up * rotationSpeed * Mathf.Deg2Rad;
                Vector3 offset = collision.transform.position - transform.position;
                Vector3 velocity = Vector3.Cross(platformAngularVelocity, offset);
                rb.MovePosition(rb.position + velocity * Time.deltaTime);
            }
        }
    }
}
