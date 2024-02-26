using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsHandler : MonoBehaviour
{
    public Toggle musicToggle;
    public Slider volumeSlider;

    public AudioSource musicSource;

    void Start()
    {
        // Load saved settings (if any)
        // Initialize UI elements based on saved settings
    }

    public void ToggleMusic()
    {
        if (musicToggle.isOn)
        {
            // Play music
            musicSource.Play();
        }
        else
        {
            // Stop music
            musicSource.Stop();
        }
    }

    public void AdjustVolume()
    {
        musicSource.volume = volumeSlider.value;
    }

    public void BackToStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
