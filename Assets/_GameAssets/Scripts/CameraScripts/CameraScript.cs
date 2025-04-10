using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 100f; // Kameranın dönüş hızı

    void Update()
    {
        // Q tuşuna basıldığında sola döndür
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0f, -_rotationSpeed * Time.deltaTime, 0f, Space.World);
        }

        // E tuşuna basıldığında sağa döndür
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0f, _rotationSpeed * Time.deltaTime, 0f, Space.World);
        }
    }
}
