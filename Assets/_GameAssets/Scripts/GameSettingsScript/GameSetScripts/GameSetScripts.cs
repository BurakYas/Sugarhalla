using UnityEngine;

public class GameSetScripts : MonoBehaviour
{
    public static GameSetScripts Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void FinishLevel()
    {
        Debug.Log("Bölüm bitti!");
        // İleride ana menüye geçiş veya başka işlemler buraya eklenir
    }

}
