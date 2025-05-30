using UnityEngine;

public class LasersScripts : MonoBehaviour
{
    public string playerTag = "Player"; // Sadece Player tag'lı nesneleri etkiler
    public float rotationSpeed = 90f;   // Derece/saniye cinsinden dönme hızı (Inspector'dan ayarlanabilir)

    private void Update()
    {
        // Y ekseni etrafında sürekli döndür
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Oyuncu lazere dokundu ve öldü!");

            // Oyuncunun health scriptini al ve Crash sebebiyle öldür
            PlayerHealthController health = other.GetComponent<PlayerHealthController>();
            if (health != null)
            {
                health.Die(PlayerHealthController.DeathCause.Crash);
            }
        }
    }
}
