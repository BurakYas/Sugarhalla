using UnityEngine;

public class SinkingFloorScripts : MonoBehaviour
{
    public float sinkDelay = 0.5f; // Batma gecikmesi
    public float sinkSpeed = 1f;    // Batma hızı
    public float sinkDistance = 2f;  // Batma mesafesi
    public float waitTime = 1f; // Bekleme süresi
    public float riseSpeed = 1.5f; // Yukarı çıkma hızı

    private Vector3 startPos; // Başlangıç pozisyonu
    private Vector3 targetPos; // Hedef pozisyon
    private bool isSinking = false;
    private bool isWaiting = false;
    private bool isRising = false;
    private bool playerOnPlatform = false;

    void Start()
    {
        startPos = transform.position;
        targetPos = startPos + Vector3.down * sinkDistance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerOnPlatform = true;
            if (!isSinking && !isWaiting && !isRising)
            {
                StartCoroutine(SinkRoutine());
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerOnPlatform = false;
        }
    }

    System.Collections.IEnumerator SinkRoutine()
    {
        yield return new WaitForSeconds(sinkDelay);

        // Batmaya başla
        isSinking = true;
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, sinkSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isSinking = false;

        // En aşağıda bekle
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;

        // Karakter hala üstündeyse yukarı çıkmayı bekle
        while (playerOnPlatform)
        {
            yield return null;
        }

        // Yukarı çıkmaya başla
        isRising = true;
        while (Vector3.Distance(transform.position, startPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, riseSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = startPos;
        isRising = false;
    }
}
