using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject optionsPanel;

    [Header("Audio")]
    public AudioSource menuMusic;

    // PLAY
    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
    }

    // EXIT
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game Ditutup");
    }

    // HOW TO PLAY PANEL
    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }

    // MUSIC TOGGLE
public void ToggleMusic(bool isOn)
{
    if (menuMusic == null) return;

    if (isOn)
    {
        menuMusic.mute = false;

        if (!menuMusic.isPlaying)
        {
            menuMusic.Play();
        }
    }
    else
    {
        menuMusic.mute = true;
    }

    Debug.Log("Music: " + (isOn ? "ON" : "OFF"));
}
}