using UnityEngine;

public class TestScripts : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private Rigidbody _rigidbody;
    private Collider _collider;
    private void Awake() 
    {
       _rigidbody = GetComponent<Rigidbody>();
       _collider = GetComponent<Collider>();
    }
    void Start()
    {
        _rigidbody.useGravity = false;
        _collider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
