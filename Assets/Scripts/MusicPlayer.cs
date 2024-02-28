using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance;

    void Awake()
    {
        // Check if an instance of the MusicPlayer already exists
        if (instance == null)
        {
            // If not, set this instance as the singleton instance
            instance = this;
            // Mark this GameObject to not be destroyed when loading new scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy this GameObject
            Destroy(gameObject);
        }
    }
}
