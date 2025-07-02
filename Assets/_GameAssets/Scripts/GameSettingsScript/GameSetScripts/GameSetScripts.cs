using UnityEngine;

public class GameSetScripts : MonoBehaviour
{
    public static GameSetScripts Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void FinishLevel()
    {
        Debug.Log("Bölüm bitti!");
        // Karakteri spawnPoint'e gönder
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var handler = player.GetComponent<NewControlHandler>();
            if (handler != null && handler.spawnPoint != null)
            {
                player.transform.position = handler.spawnPoint.position;
                // Rigidbody hızını da sıfırla ki fırlamasın
                var rb = player.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }
        }
    }

}
