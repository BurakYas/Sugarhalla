using UnityEngine;
using UnityEngine.SceneManagement; // Sahne yönetimi için gerekli

public class PlayerHealthController : MonoBehaviour
{
    private int maxHealth = 100; // Maksimum sağlık
    private int currentHealth;  // Mevcut sağlık

    void Start()
    {
        currentHealth = maxHealth; // Oyuncunun sağlığını maksimuma ayarla
    }

    void Update()
    {
        // Örnek: Sağlık kontrolü için bir test
        if (Input.GetKeyDown(KeyCode.H)) // H tuşuna basıldığında sağlık azalt
        {
            TakeDamage(10);
            print("Sağlık Azaldı: " + currentHealth);
        }

        if (Input.GetKeyDown(KeyCode.J)) // J tuşuna basıldığında sağlık artır
        {
            Heal(10);
            print("Sağlık Arttı: " + currentHealth);
        }
    }

    // Sağlık azaltma fonksiyonu
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Sağlığı 0 ile maxHealth arasında sınırla
        Debug.Log("Sağlık Azaldı: " + currentHealth);

        if (currentHealth <= 0) // Eğer sağlık 0 veya daha azsa, öl
        {
            Die();
        }
    }

    // Sağlık artırma fonksiyonu
    public void Heal(int amount)
    {
        if (currentHealth == maxHealth) // Eğer sağlık zaten maksimumsa, fonksiyonu sonlandır
        {
            Debug.Log("Sağlık zaten maksimum seviyede!");
            return;
        }

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Sağlığı 0 ile maxHealth arasında sınırla
        Debug.Log("Sağlık Arttı: " + currentHealth);
    }

    // Ölüm fonksiyonu
    private void Die()
    {
        Debug.Log("Karakter öldü!");
        // Örnek: Sahneyi yeniden başlat
       // SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Alternatif: Karakteri devre dışı bırak
         gameObject.SetActive(false);
    }

    // Mevcut sağlık değerini döndüren fonksiyon
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
