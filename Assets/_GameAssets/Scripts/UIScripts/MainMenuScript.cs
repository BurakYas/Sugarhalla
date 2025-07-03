using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    // Oyuna başla butonu için
    public void StartButton()
    {
        // "Game" sahnesinin adını kendi sahne adınla değiştir
        SceneManager.LoadScene("StartGameMenu");
    }

    // Oyundan çık butonu için
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Oyun kapatıldı!");
    }

    // Ayarlar butonu için (varsa)
    public void OpenSettings()
    {
        // Ayarlar panelini açacak kodu buraya yazabilirsin
        Debug.Log("Ayarlar açıldı!");
    }

    public void NewGame()
    {
        // Yeni oyun başlatma işlemleri
        Debug.Log("Yeni oyun başlatıldı!");
        SceneManager.LoadScene("Tutorial");
    }
    public void LoadGame()
    {
        // Kaydedilmiş oyunu yükleme işlemleri
        Debug.Log("Kaydedilmiş oyun yüklendi!");
        // Burada kaydedilmiş oyunu yüklemek için gerekli kodları ekleyebilirsin
    }
}
