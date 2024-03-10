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
        LoadSettings();
    }

    void Awake()
    {
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
        if (PlayerPrefs.HasKey("MusicEnabled"))
        {
            bool musicEnabled = PlayerPrefs.GetInt("MusicEnabled") == 1;
            if (musicToggle != null)
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
            if (volumeSlider != null)
            {
                volumeSlider.value = volume;
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

        PlayerPrefs.SetFloat("MusicVolume", volumeSlider.value);
    }

    public void BackToStartMenu()
    {
        SaveSettings();
        SceneManager.LoadScene("StartMenu");
    }

    void SaveSettings()
    {
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
