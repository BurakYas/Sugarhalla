using UnityEngine;

public class InteractableObjects : MonoBehaviour, IInteractable
{
    [Header("Hareket Ettirilecek Obje")]
    public MoveBetweenPoint targetObject; // Inspector'dan ata

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Bu fonksiyon E tuşuna basıldığında çağrılır
    public void Interact()
    {
        Debug.Log($"{gameObject.name} ile etkileşime girildi!");
        if (targetObject != null)
        {
            targetObject.Interact();
        }
    }
}
