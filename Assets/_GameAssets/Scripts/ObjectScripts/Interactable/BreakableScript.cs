using UnityEngine;

public class BreakableScript : MonoBehaviour
{
  

    public void Break()
    {
        // Burada kırılma animasyonu, efekt veya ses ekleyebilirsin
        Debug.Log(gameObject.name + " kırıldı!");
        Destroy(gameObject);
    }
}
