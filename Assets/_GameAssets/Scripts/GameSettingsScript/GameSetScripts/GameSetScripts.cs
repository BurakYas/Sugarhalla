using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        
        SceneManager.LoadScene("MainMenu");
    }

}
