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
        }
        else
        {
            Debug.Log("Could not find game object");
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
            // Play music if the toggle is on
            musicSource.Play();
        }
        else
        {
            // Stop music if the toggle is off
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
        // Save settings
        PlayerPrefs.SetFloat("MusicVolume", volumeSlider.value);
    }

    public void BackToStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
