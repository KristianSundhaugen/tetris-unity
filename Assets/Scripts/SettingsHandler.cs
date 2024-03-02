using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsHandler : MonoBehaviour
{
    public Toggle musicToggle;
    public Slider volumeSlider;
    private AudioSource musicSource;

    void Awake()
    {
        // Load saved settings (if any)
        // Initialize UI elements based on saved settings
        // For example, you could load the volume level and music state from PlayerPrefs
        GameObject backgroundMusic = GameObject.Find("BackgroundMusic");
        if (backgroundMusic != null)
        {
            musicSource = backgroundMusic.GetComponent<AudioSource>();
            volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f); // Default to maximum volume
            musicToggle.isOn = PlayerPrefs.GetInt("MusicEnabled", 1) == 1; // Default to music enabled
        }
        else
        {
            Debug.Log("Could not find game object");
        }
    }

    public void ToggleMusic()
    {
        ApplySettings();
    }

    public void AdjustVolume()
    {
        ApplySettings();
    }

    private void ApplySettings()
    {
        if (musicSource != null)
        {
            musicSource.volume = volumeSlider.value;

            if (musicToggle.isOn)
            {
                // Play music if the toggle is on
                musicSource.Play();
            }
            else
            {
                // Stop music if the toggle is off
                musicSource.Stop();
            }

            // Save settings
            PlayerPrefs.SetFloat("MusicVolume", volumeSlider.value);
            PlayerPrefs.SetInt("MusicEnabled", musicToggle.isOn ? 1 : 0);
        }
        else
        {
            Debug.LogError("Music source is not initialized.");
        }
    }

    public void BackToStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
