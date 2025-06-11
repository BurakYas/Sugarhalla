using UnityEngine;

public class MoveBetweenPoint : MonoBehaviour, IInteractable
{
    [Header("Hareket Noktaları")]
    public Transform pointA; // Başlangıç noktası
    public Transform pointB; // Gideceği nokta
    public float moveSpeed = 2f;

    private bool movingToB = false;
    private bool isMoving = false;

    void Update()
    {
        if (isMoving)
        {
            Transform target = movingToB ? pointB : pointA;
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            // Hedefe ulaştıysa hareketi durdur
            if (Vector3.Distance(transform.position, target.position) < 0.01f)
            {
                isMoving = false;
            }
        }
    }

    // IInteractable'dan gelen fonksiyon
    public void Interact()
    {
        movingToB = !movingToB;
        isMoving = true;
    }
}
