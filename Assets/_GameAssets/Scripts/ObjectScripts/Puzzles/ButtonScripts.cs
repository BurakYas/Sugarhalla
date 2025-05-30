using UnityEngine;

public class ButtonScripts : MonoBehaviour
{
    public string buttonType = "Red"; // Inspector'dan ayarlanabilir (ör: "Red", "Blue", "Green")
    public bool isActivated = false;  // Butonun aktif olup olmadığını gösterir
    public float resetDelay = 2f;     // Butonun tekrar aktif olacağı süre

    // Karakter butona bastığında otomatik çalışacak
    private void OnCollisionEnter(Collision collision)
    {
        if (!isActivated && collision.collider.CompareTag("Player"))
        {
            ActivateButton();
        }
    }

    // Butona basıldığında çağrılacak ana fonksiyon
    public void ActivateButton()
    {
        isActivated = true;

        switch (buttonType)
        {
            case "Red":
                RedButtonAction();
                break;
            case "Blue":
                BlueButtonAction();
                break;
            case "Green":
                GreenButtonAction();
                break;
            default:
                Debug.LogWarning("Buton tipi tanımlı değil: " + buttonType);
                break;
        }

        // Belirli bir süre sonra butonu tekrar aktif et
        Invoke(nameof(ResetButton), resetDelay);
    }

    private void ResetButton()
    {
        isActivated = false;
        Debug.Log("Buton tekrar aktif!");
        // İstersen burada görsel veya animasyon resetleyebilirsin
    }

    // Her buton için ayrı fonksiyonlar (içini sen doldurabilirsin)
    void RedButtonAction()
    {
        Debug.Log("Kırmızı buton fonksiyonu çalıştı!");
        // Buraya kırmızı butonun işlevini yaz
    }

    void BlueButtonAction()
    {
        Debug.Log("Mavi buton fonksiyonu çalıştı!");
        // Buraya mavi butonun işlevini yaz
    }

    void GreenButtonAction()
    {
        Debug.Log("Yeşil buton fonksiyonu çalıştı!");
        // Buraya yeşil butonun işlevini yaz
    }
}
