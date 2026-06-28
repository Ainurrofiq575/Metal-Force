using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject optionsPanel;
    public GameObject playGuidePanel;

    private void Start()
    {
        // 🔥 pastikan time normal saat balik ke menu
        Time.timeScale = 1f;
    }

    // PLAY BUTTON (tampilkan guide dulu)
    public void PlayGame()
    {
        playGuidePanel.SetActive(true);
    }

    // START GAME BUTTON
    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    // EXIT BUTTON
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game Ditutup");
    }

    // OPEN SETTINGS
    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }

    // CLOSE SETTINGS
    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }

    // CLOSE GUIDE
    public void ClosePlayGuide()
    {
        playGuidePanel.SetActive(false);
    }

    // 🔥 SOUND TOGGLE HOOK (REVISI DOSEN SYSTEM)
    public void ToggleSound(bool value)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetSound(value);
        }
    }
}