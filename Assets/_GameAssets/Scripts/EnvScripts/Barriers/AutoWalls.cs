using UnityEngine;

public class AutoWalls : MonoBehaviour
{
    public Vector3 forwardOffset = new Vector3(2f, 0, 0); // İleriye ne kadar gitsin
    public float forwardSpeed = 10f; // İleri giderkenki hız
    public float backwardSpeed = 2f; // Geri dönerkenki hız
    public float waitAtFront = 1f;   // İleri gittikten sonra bekleme süresi
    public float waitAtBack = 2f;    // Geri döndükten sonra bekleme süresi

    private Vector3 startPos;
    private Vector3 targetPos;

    void Start()
    {
        startPos = transform.position;
        targetPos = startPos + forwardOffset;
        StartCoroutine(WallRoutine());
    }

    System.Collections.IEnumerator WallRoutine()
    {
        while (true)
        {
            // Hızlıca ileri git
            while (Vector3.Distance(transform.position, targetPos) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, forwardSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = targetPos;

            // İleri pozisyonda bekle
            yield return new WaitForSeconds(waitAtFront);

            // Yavaşça geri dön
            while (Vector3.Distance(transform.position, startPos) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPos, backwardSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = startPos;

            // Geri pozisyonda bekle
            yield return new WaitForSeconds(waitAtBack);
        }
    }
}
