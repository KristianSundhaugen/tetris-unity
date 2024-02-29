using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsHandler : MonoBehaviour
{
    public Toggle musicToggle;
    public Slider volumeSlider;
    public AudioSource musicSource;

    void Awake()
    {
        // Load saved settings (if any)
        // Initialize UI elements based on saved settings
        // For example, you could load the volume level and music state from PlayerPrefs
        musicSource = GameObject.FindGameObjectWithTag("BackgroundMusic").GetComponent<AudioSource>(); 
        volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f); // Default to maximum volume
        musicToggle.isOn = PlayerPrefs.GetInt("MusicEnabled", 1) == 1; // Default to music enabled
        ApplySettings();
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

    public void BackToStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
