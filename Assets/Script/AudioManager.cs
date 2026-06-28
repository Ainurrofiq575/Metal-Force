using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Global Sound State")]
    public bool soundOn = true;

    [Header("Music")]
    public AudioSource bgMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            soundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;

            SceneManager.sceneLoaded += OnSceneLoaded;

            ApplySound();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplySound();

        if (scene.name == "Level1" || scene.name == "Level2")
        {
            StopMenuBackgroundMusic();
        }
    }

    public void SetSound(bool value)
    {
        soundOn = value;

        PlayerPrefs.SetInt("SoundOn", soundOn ? 1 : 0);
        PlayerPrefs.Save();

        ApplySound();

        Debug.Log("AUDIO STATUS: " + (soundOn ? "ON" : "OFF"));
    }

    public void ApplySound()
    {
        AudioListener.pause = !soundOn;
        AudioListener.volume = soundOn ? 1f : 0f;

        string sceneName = SceneManager.GetActiveScene().name;

        if (!soundOn)
        {
            StopMenuBackgroundMusic();
            return;
        }

        if (sceneName == "MainMenu")
        {
            PlayMusic();
        }
        else if (sceneName == "Level1" || sceneName == "Level2")
        {
            StopMenuBackgroundMusic();
        }
    }

    public void PlayMusic()
    {
        if (bgMusic != null && !bgMusic.isPlaying)
        {
            bgMusic.Play();
        }
    }

    public void StopMusic()
    {
        if (bgMusic != null)
        {
            bgMusic.Stop();
        }
    }

    public void StopMenuBackgroundMusic()
    {
        if (bgMusic != null)
        {
            bgMusic.Stop();
            Debug.Log("STOP BGM: bgMusic reference");
        }

        AudioSource[] allAudio =
            Object.FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        foreach (AudioSource audio in allAudio)
        {
            if (audio == null) continue;

            string objectName = audio.gameObject.name.ToLower();
            string clipName = audio.clip != null ? audio.clip.name.ToLower() : "";

            bool isMenuMusic =
                objectName.Contains("menu") ||
                objectName.Contains("music") ||
                objectName.Contains("bg") ||
                clipName.Contains("mountain") ||
                clipName.Contains("menu");

            if (isMenuMusic)
            {
                audio.Stop();
                Debug.Log("STOP BGM SOURCE: " + audio.gameObject.name);
            }
        }
    }

    public bool IsSoundOn()
    {
        return soundOn;
    }
}