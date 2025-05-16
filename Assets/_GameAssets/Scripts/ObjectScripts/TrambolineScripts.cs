using UnityEngine;

public class TrambolineScripts : MonoBehaviour
{
    [SerializeField] private float bounceMultiplier = 2f; // Zıplama kuvveti çarpanı
    [SerializeField] private float maxBounceForce = 15f;  // Maksimum zıplama kuvveti

    private void OnCollisionEnter(Collision collision)
    {
        // Çarpan objenin "Player" tagine sahip olup olmadığını kontrol et
        if (collision.collider.CompareTag("Player"))
        {
            // Oyuncunun NewControlHandler scriptine erişim sağla
            NewControlHandler playerController = collision.collider.GetComponent<NewControlHandler>();
            if (playerController != null)
            {
                // Düşüş hızını al
                Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    float fallSpeed = Mathf.Abs(rb.linearVelocity.y);

                    // Zıplama kuvvetini hesapla
                    float bounceForce = Mathf.Clamp(fallSpeed * bounceMultiplier, 0, maxBounceForce);

                    // Jump fonksiyonunu çağır ve yönü yukarıya doğru gönder
                    playerController.Jump(Vector3.up * bounceForce);

                    // Debug log ile zıplama kuvvetini göster
                    Debug.Log($"Trambolin zıplama kuvveti: {bounceForce}");
                }
            }
        }
    }
}
