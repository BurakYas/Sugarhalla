using UnityEngine;
using UnityEngine.SceneManagement; // Sahne yönetimi için gerekli

public class PlayerHealthController : MonoBehaviour
{
    private int maxHealth = 100; // Maksimum sağlık
    private int currentHealth;  // Mevcut sağlık
    [SerializeField] private float fallDamageThreshold = 10f; // Düşüş hasarı için hız eşiği
    [SerializeField] private int fallDamageAmount = 20; // Düşüş hasarı miktarı

    private Rigidbody _rigidBody; // Rigidbody referansı
    [SerializeField] private LayerMask _groundLayer; // Zemin katmanı için LayerMask

    // Ölüm nedenlerini belirten enum
    public enum DeathCause
    {
        None,
        Fall,
        Fire,
        Enemy,
        Water,
    }

    void Start()
    {
        currentHealth = maxHealth; // Oyuncunun sağlığını maksimuma ayarla
        _rigidBody = GetComponent<Rigidbody>(); // Rigidbody bileşenini al
        if (_rigidBody == null)
        {
            Debug.LogError("Rigidbody bileşeni bulunamadı!");
        }
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

    private void FixedUpdate()
    {
        CheckFallDamage();
    }

    // Sağlık azaltma fonksiyonu
    public void TakeDamage(int damage, DeathCause cause = DeathCause.None)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Sağlığı 0 ile maxHealth arasında sınırla
        Debug.Log("Sağlık Azaldı: " + currentHealth);

        if (currentHealth <= 0) // Eğer sağlık 0 veya daha azsa, öl
        {
            Die(cause);
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
    private void Die(DeathCause cause)
    {
        switch (cause)
        {
            case DeathCause.Fall:
                Debug.Log("Karakter düşerek öldü!");
                break;
            case DeathCause.Fire:
                Debug.Log("Karakter yanarak öldü!");
                break;
            case DeathCause.Enemy:
                Debug.Log("Karakter bir düşman tarafından öldürüldü!");
                break;
            default:
                Debug.Log("Karakter bilinmeyen bir nedenle öldü!");
                break;
        }

        // Karakteri devre dışı bırak
        gameObject.SetActive(false);
    }

    // Mevcut sağlık değerini döndüren fonksiyon
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    // Düşüş hasarını kontrol eden fonksiyon
    private void CheckFallDamage()
    {
        // Eğer düşüş hızı eşik değeri aşıyorsa ve yere temas ediyorsa
        if (_rigidBody.linearVelocity.y < -fallDamageThreshold && onGround())
        {
            // Düşüş hızına göre hasar miktarını hesapla
            int calculatedDamage = Mathf.RoundToInt((Mathf.Abs(_rigidBody.linearVelocity.y) - fallDamageThreshold) * 2); // Hız arttıkça hasar artar
            Debug.Log($"Düşüş hasarı alındı! Hasar: {calculatedDamage}");

            // Hasarı uygula
            TakeDamage(calculatedDamage, DeathCause.Fall);
        }
    }

    private bool onGround()
    {
        // Zemin kontrolü için raycast
        return Physics.Raycast(transform.position, Vector3.down, 1f, _groundLayer);
    }

    // Ateşe temas eden bir fonksiyon
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            Debug.Log("Ateşe temas edildi!");
            TakeDamage(maxHealth, DeathCause.Fire); // Ateşe temas ederse direkt öl
        }
    }
}
