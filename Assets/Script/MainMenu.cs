using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject optionsPanel;
    public GameObject playGuidePanel;

    // PLAY BUTTON
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

    // SETTINGS PANEL
    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }

    // CLOSE PLAY GUIDE PANEL
    public void ClosePlayGuide()
    {
        playGuidePanel.SetActive(false);
    }
}