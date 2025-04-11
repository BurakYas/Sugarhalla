using UnityEngine;

public class WaterDamageHandler : MonoBehaviour
{
    [SerializeField] private PlayerHealthController playerHealthController; // Sağlık kontrol scriptine referans
    [SerializeField] private int waterDamage = 10; // Suyun içinde alınacak hasar miktarı
    [SerializeField] private float damageInterval = 1.5f; // Hasar alma aralığı (saniye)
    [SerializeField] private float initialDelay = 5f; // İlk hasar için bekleme süresi (saniye)

    private bool isInWater = false; // Karakterin suyun içinde olup olmadığını kontrol eder

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            if (playerHealthController != null)
            {
                isInWater = true;
                StartCoroutine(ApplyWaterDamage());
            }
            else
            {
                Debug.LogWarning("PlayerHealthController referansı atanmadı!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            isInWater = false; // Sudan çıktığında hasar almayı durdur
            StopCoroutine(ApplyWaterDamage());
        }
    }

    private System.Collections.IEnumerator ApplyWaterDamage()
    {
        // İlk 5 saniye bekle (hasar almadan)
        Debug.Log("Suyun içinde, hasar almadan bekleniyor...");
        yield return new WaitForSeconds(initialDelay);

        // 5 saniyeden sonra sürekli hasar uygula
        while (isInWater)
        {
            if (playerHealthController != null)
            {
                // Su hasarını uygula ve DeathCause.Water gönder
                playerHealthController.TakeDamage(waterDamage, PlayerHealthController.DeathCause.Water);
                Debug.Log("Suyun içinde! Sağlık: " + playerHealthController.GetCurrentHealth());
            }
            yield return new WaitForSeconds(damageInterval); // Hasar alma aralığı kadar bekle
        }
    }
}
