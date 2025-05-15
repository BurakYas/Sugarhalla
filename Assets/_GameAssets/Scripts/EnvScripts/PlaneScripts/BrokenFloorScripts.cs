using UnityEngine;

public class BrokenFloorScripts : MonoBehaviour
{
    public float breakDelay = 1.5f; // Kaç saniye sonra kırılacak
    public float respawnDelay = 3f; // Kaç saniye sonra tekrar gelsin
    private bool isBreaking = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!isBreaking && collision.collider.CompareTag("Player"))
        {
            isBreaking = true;
            Invoke(nameof(BreakFloor), breakDelay);
        }
    }

    void BreakFloor()
    {
        gameObject.SetActive(false);
        Invoke(nameof(RespawnFloor), respawnDelay);
    }

    void RespawnFloor()
    {
        gameObject.SetActive(true);
        isBreaking = false;
    }
}
