using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsHandler : MonoBehaviour
{
    public Toggle musicToggle;
    public Slider volumeSlider;
    private AudioSource musicSource;

    void Start()
    {
        // Load saved settings (if any)
        LoadSettings();
    }

    void Awake()
    {
        // Find and initialize the music source
        GameObject backgroundMusic = GameObject.Find("BackgroundMusic");
        if (backgroundMusic != null)
        {
            musicSource = backgroundMusic.GetComponent<AudioSource>();
        }
        else
        {
            Debug.Log("Could not find game object");
        }
    }

    void LoadSettings()
    {
        // Load settings from PlayerPrefs
        if (PlayerPrefs.HasKey("MusicEnabled"))
        {
            bool musicEnabled = PlayerPrefs.GetInt("MusicEnabled") == 1;
            if (musicToggle != null) // Check if musicToggle is assigned
            {
                musicToggle.isOn = musicEnabled;
            }

            if (musicEnabled && musicSource != null)
            {
                musicSource.Play();
            }
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float volume = PlayerPrefs.GetFloat("MusicVolume");
            if (volumeSlider != null) // Check if volumeSlider is assigned
            {
                volumeSlider.value = volume; //THIS IS LINE 48
            }

            if (musicSource != null)
            {
                musicSource.volume = volume;
            }
        }
    }


    public void ToggleMusic()
    {
        if (musicSource == null)
        {
            Debug.Log("Missing musicSource");
            return;
        }
        if (musicToggle == null)
        {
            Debug.Log("Missing musicToggle");
            return;
        }

        if (musicToggle.isOn)
        {
            musicSource.Play();
        }
        else
        {
            musicSource.Stop();
        }

        // Save music toggle state
        PlayerPrefs.SetInt("MusicEnabled", musicToggle.isOn ? 1 : 0);
    }

    public void AdjustVolume()
    {
        if (musicSource == null)
        {
            Debug.Log("Missing musicSource");
            return;
        }
        if (volumeSlider == null)
        {
            Debug.Log("Missing volumeSlider");
            return;
        }

        musicSource.volume = volumeSlider.value;

        // Save volume settings
        PlayerPrefs.SetFloat("MusicVolume", volumeSlider.value);
    }

    public void BackToStartMenu()
    {
        // Save settings before going back to the start menu
        SaveSettings();

        // Load the start menu scene
        SceneManager.LoadScene("StartMenu");
    }

    void SaveSettings()
    {
        // Save settings to PlayerPrefs
        if (musicToggle != null)
        {
            PlayerPrefs.SetInt("MusicEnabled", musicToggle.isOn ? 1 : 0);
        }

        if (volumeSlider != null)
        {
            PlayerPrefs.SetFloat("MusicVolume", volumeSlider.value);
        }
    }
}
