using UnityEngine;

public class FireDamageHandler : MonoBehaviour
{
    [SerializeField] private PlayerHealthController playerHealthController; // Sağlık kontrol scriptine referans
    [SerializeField] private int fireDamage = 20; // Ateşten alınacak hasar miktarı
    [SerializeField] private float damageInterval = 1f; // Hasar alma aralığı (saniye)

    private bool isInFire = false; // Karakterin ateşin içinde olup olmadığını kontrol eder

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Fire"))
        {
            if (playerHealthController != null)
            {
                isInFire = true;
                StartCoroutine(ApplyFireDamage());
            }
            else
            {
                Debug.LogWarning("PlayerHealthController referansı atanmadı!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Fire"))
        {
            isInFire = false; // Ateşten çıktığında hasar almayı durdur
            StopCoroutine(ApplyFireDamage());
        }
    }

    private System.Collections.IEnumerator ApplyFireDamage()
    {
        while (isInFire)
        {
            if (playerHealthController != null)
            {
                // Ateş hasarını uygula ve DeathCause.Fire gönder
                playerHealthController.TakeDamage(fireDamage, PlayerHealthController.DeathCause.Fire);
                Debug.Log("Ateşin içinde! Sağlık: " + playerHealthController.GetCurrentHealth());
            }
            yield return new WaitForSeconds(damageInterval); // Hasar alma aralığı kadar bekle
        }
    }
}