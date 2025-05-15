using UnityEngine;

public class StickyPlaneScript : MonoBehaviour
{
   
    private Rigidbody playerRigidbody; // Oyuncunun Rigidbody bileşeni
    private NewControlHandler playerController; // Oyuncunun kontrol scripti
    private float originalTorqueMultiplier; 
    private float originalAngular; // Karakterin orijinal hızı

    private void OnTriggerEnter(Collider other)
    {
        // Eğer "Player" tagine sahip bir obje bu alana girerse
        if (other.CompareTag("Player"))
        {
            // Oyuncunun Rigidbody ve kontrol scriptine erişim sağla
            playerRigidbody = other.GetComponent<Rigidbody>();
            playerController = other.GetComponent<NewControlHandler>();
            if (playerRigidbody != null && playerController != null)
            {
                // Zıplamayı devre dışı bırak
                playerController.canJump = false;

                // Orijinal hızı kaydet ve hızı düşür
                originalTorqueMultiplier = playerController.torqueMultiplier;

                playerController.torqueMultiplier = 1.2f;
                playerController.maxAngularVelocity = 5f;

                Debug.Log("Karakter yapışkan alana girdi! Zıplama devre dışı bırakıldı ve hareket hızı düşürüldü.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Eğer "Player" tagine sahip bir obje bu alandan çıkarsa
        if (other.CompareTag("Player"))
        {
            if (playerRigidbody != null && playerController != null)
            {
                // Zıplamayı tekrar etkinleştir
                playerController.canJump = true;

                // Hızı eski haline döndür
               playerController.torqueMultiplier = 1.5f;
               playerController.maxAngularVelocity = 10f;

                Debug.Log("Karakter yapışkan alandan çıktı! Zıplama tekrar etkinleştirildi ve hareket hızı normale döndü.");
            }

            
        }
    }

   
}
