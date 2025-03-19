using UnityEngine;

public class PlayerControlScripts : MonoBehaviour
{
    // Küp kontrol ve hareketleri için deneme script dosyasıdır.

    private Rigidbody playerRigidBody;

    void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody>();
    }
}
