using UnityEngine;

public class TeleporterScripts : MonoBehaviour
{
    [Header("Diğer Teleporter Nesnesi")]
    public TeleporterScripts otherTeleporter; // Editörden sürükle-bırak ile eşleştir

    [Header("Sadece Player tag'lı nesneleri teleport et")]
    public string playerTag = "Player";

    private bool canTeleport = true; // Teleport sonrası tekrar tetiklenmeyi önler

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canTeleport && other.CompareTag(playerTag) && otherTeleporter != null)
        {
            // Diğer teleportera geçiş yap
            otherTeleporter.DisableTeleportTemporarily();
            other.transform.position = otherTeleporter.transform.position;
        }
    }

    // Teleport sonrası tekrar tetiklenmeyi önlemek için kısa süreli devre dışı bırak
    public void DisableTeleportTemporarily()
    {
        canTeleport = false;
        Invoke(nameof(EnableTeleport), 0.2f); // 0.2 saniye sonra tekrar aktif
    }

    private void EnableTeleport()
    {
        canTeleport = true;
    }
}
