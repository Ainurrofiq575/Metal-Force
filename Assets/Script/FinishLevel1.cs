using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel1 : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject levelCompletePanel;

    private bool levelFinished = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player") && !levelFinished)
        {
            CheckFinishCondition();
        }
    }

    void CheckFinishCondition()
    {
        if (GameManager.instance == null)
        {
            Debug.LogWarning("GameManager belum ditemukan!");
            return;
        }

        if (GameManager.instance.timeOver)
        {
            Debug.Log("WAKTU HABIS! Tidak bisa finish.");
            return;
        }

        if (GameManager.instance.point < GameManager.instance.maxPoint)
        {
            Debug.Log("Point belum lengkap!");
            return;
        }

        if (GameManager.instance.checkpoint < GameManager.instance.maxCheckpoint)
        {
            Debug.Log("Checkpoint belum lengkap!");
            return;
        }

        LevelComplete();
    }

    void LevelComplete()
    {
        levelFinished = true;

        GameManager.instance.StopTimer();

        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
        }

        Time.timeScale = 0f;

        Debug.Log("LEVEL 1 COMPLETE!");
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level2");
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}