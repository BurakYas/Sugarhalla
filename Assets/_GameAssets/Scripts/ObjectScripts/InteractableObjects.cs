using UnityEngine;

public class InteractableObjects : MonoBehaviour, IInteractable
{
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
        // Buraya istediğin işlemi ekleyebilirsin (ör: kapı açma, ışık yakma, animasyon vs.)
    }
}
