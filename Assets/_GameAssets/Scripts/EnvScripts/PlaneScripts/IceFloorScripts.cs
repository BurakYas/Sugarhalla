using UnityEngine;

public class IceFloorScripts : MonoBehaviour
{
    public float iceDrag = 0.5f; // Buz üzerindeki sürtünme (düşük olmalı)
    
    public float acceleration = 10f; // Buzda hızlanma miktarı

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearDamping = iceDrag;
                Debug.Log("Karakter buzlu zemine GİRDİ.");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");
                Vector3 inputDir = new Vector3(h, 0, v).normalized;

                Vector3 velocity = rb.linearVelocity; // veya rb.velocity kullanabilirsin
                float speed = velocity.magnitude;

                // Eğer giriş yoksa kaymaya devam et
                if (inputDir.magnitude < 0.1f && speed > 0.1f)
                {
                    rb.AddForce(velocity.normalized * (acceleration * 0.2f), ForceMode.Acceleration);
                }
                else if (speed > 0.1f && Vector3.Dot(inputDir, velocity.normalized) < -0.2f)
                {
                    // Geri tuşuna basılıyorsa, hızla orantılı yavaş fren kuvveti uygula
                    float brakeStrength = Mathf.Clamp01(-Vector3.Dot(inputDir, velocity.normalized)); // 0 ile 1 arası
                    float brakeForce = acceleration * 0.05f * brakeStrength; // 0.2f ile oynayarak fren şiddetini ayarlayabilirsin
                    rb.AddForce(-velocity.normalized * brakeForce, ForceMode.Acceleration);
                }
                else
                {
                    // Normal kaygan hızlanma
                    rb.AddForce(inputDir * acceleration, ForceMode.Acceleration);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearDamping = 0f;
                Debug.Log("Karakter buzlu zeminden ÇIKTI.");
            }
        }
    }
}
