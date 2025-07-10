using UnityEngine;

public class SpinScript : MonoBehaviour
{
    private Animator animator;
    public bool isSpinning = false;
    public float spinRadius = 2f; // Inspector'dan ayarlanabilir

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Spin(float duration = 1f)
    {
        if (isSpinning) return;
        isSpinning = true;
        if (animator != null)
            animator.SetTrigger("Spin");
        Debug.Log("Spin başladı!");

        // Kırılabilir objeleri bul ve kır
        Collider[] hits = Physics.OverlapSphere(transform.position, spinRadius);
        foreach (var hit in hits)
        {
            BreakableScript breakable = hit.GetComponent<BreakableScript>();
            if (breakable != null)
            {
                breakable.Break();
            }
        }

        Invoke(nameof(EndSpin), duration);
    }

    private void EndSpin()
    {
        isSpinning = false;
        Debug.Log("Spin bitti!");
    }
}
