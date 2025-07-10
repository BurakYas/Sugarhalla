using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio; // AudioMixer için ekledik
using DG.Tweening;

public class MainMenuScript : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;   // Ana menü paneli (Inspector'dan ata)
    public GameObject otherMenuPanel;  // Geçiş yapmak istediğin diğer menü (Inspector'dan ata)

    public GameObject settingsPanel; // Ayarlar paneli (varsa)
  [Header("Sliders")]
    public Slider musicSlider; // Inspector'dan ata
    public Slider soundSlider; // Inspector'dan ata

  [Header("Audio Sources")]
    public AudioSource musicSource; // Arka plan müziği için
    public AudioSource soundSource; // Efekt sesi için (örnek)
    public AudioSource hoverSource; // Inspector'dan ata (hover sesi için ayrı bir AudioSource)
    public AudioSource buttonClickSource;

    [Header("Icons")]
    public Image musicOnIcon;   // Inspector'dan ata
    public Image musicOffIcon;  // Inspector'dan ata
    public Image soundOnIcon;   // Inspector'dan ata
    public Image soundOffIcon;  // Inspector'dan ata

    public AudioMixer audioMixer; // Inspector’dan ata

    private void Start()
    {
        mainMenuPanel.SetActive(true);
        otherMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);

        // Varsayılan başlangıç değeri (ör: 0.5)
        float defaultValue = 0.5f;

        // Müzik
        if (musicSlider != null)
        {
            musicSlider.value = defaultValue;
            SetMusicVolume(defaultValue); // Mixer ve ikonları da günceller
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        // Ses
        if (soundSlider != null)
        {
            soundSlider.value = defaultValue;
            SetSoundVolume(defaultValue); // Mixer ve ikonları da günceller
            soundSlider.onValueChanged.AddListener(SetSoundVolume);
        }
    }

    public void SetMusicVolume(float value)
    {
        // Slider 0-1 arası, Mixer dB ister. 0 olduğunda -80, 1 olduğunda 0 dB olmalı.
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("MusicVolume", dB);

        // İkonları güncelle
        if (musicOnIcon != null && musicOffIcon != null)
        {
            bool kapali = value <= 0.01f;
            musicOnIcon.gameObject.SetActive(!kapali);
            musicOffIcon.gameObject.SetActive(kapali);
        }
    }

    public void SetSoundVolume(float value)
    {
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("SFXVolume", dB);

        if (soundOnIcon != null && soundOffIcon != null)
        {
            bool kapali = value <= 0.01f;
            soundOnIcon.gameObject.SetActive(!kapali);
            soundOffIcon.gameObject.SetActive(kapali);
        }
    }

    // Oyuna başla butonu için
    public void StartButton()
    {
        mainMenuPanel.SetActive(false);    // Ana menüyü kapat
        otherMenuPanel.SetActive(true);    // Diğer menüyü aç
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
        mainMenuPanel.SetActive(false); // Ana menüyü kapat
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true); // Ayarlar panelini aç   
        }
    }

    public void NewGame()
    {
        Debug.Log("Yeni oyun başlatıldı!");
         DOTween.KillAll();
        SceneManager.LoadScene("Tutorial");
    }

    public void LoadGame()
    {
        Debug.Log("Kaydedilmiş oyun yüklendi!");
    }

    public void BackToMainMenu()
    {
        // Alt menüyü kapat, ana menüyü aç
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        otherMenuPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void ToggleMusic()
    {
        if (musicSource != null)
        {
            bool kapali = musicSource.volume > 0.01f;
            musicSource.volume = kapali ? 0f : 1f;
            if (musicSlider != null)
                musicSlider.value = musicSource.volume;
            // İkonları güncelle
            if (musicOnIcon != null && musicOffIcon != null)
            {
                musicOnIcon.gameObject.SetActive(!kapali);
                musicOffIcon.gameObject.SetActive(kapali);
            }
        }
    }

    public void ToggleSound()
    {
        if (soundSource != null)
        {
            bool kapali = soundSource.volume > 0.01f;
            soundSource.volume = kapali ? 0f : 1f;
            if (soundSlider != null)
                soundSlider.value = soundSource.volume;
            // İkonları güncelle
            if (soundOnIcon != null && soundOffIcon != null)
            {
                soundOnIcon.gameObject.SetActive(!kapali);
                soundOffIcon.gameObject.SetActive(kapali);
            }
        }
    }

    public void PlayButtonClickSound()
    {
        buttonClickSource.PlayOneShot(buttonClickSource.clip);
    }

    public void PlayHoverSound()
    {
       hoverSource.PlayOneShot(hoverSource.clip);
    }
}
