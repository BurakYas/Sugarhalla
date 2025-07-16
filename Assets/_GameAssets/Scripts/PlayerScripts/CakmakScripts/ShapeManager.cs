using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    [SerializeField] private GameObject cubePlayer;
    [SerializeField] private GameObject spherePlayer;

    // Ana Player objesine ekle
    [SerializeField] private GameObject activeShape; // CubePlayer veya SpherePlayer

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetShape("Cube");
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetShape("Sphere");
    }

    void LateUpdate()
    {
        if (activeShape != null)
            transform.position = activeShape.transform.position;
    }

    public void SetShape(string shape)
    {
        cubePlayer.SetActive(shape == "Cube");
        spherePlayer.SetActive(shape == "Sphere");
    }
}
