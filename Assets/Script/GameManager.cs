using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Point")]
    public int point = 0;
    public int maxPoint = 5;

    [Header("Checkpoint")]
    public int checkpoint = 0;
    public int maxCheckpoint = 3;

    [Header("UI")]
    public TextMeshProUGUI pointText;
    public TextMeshProUGUI timeText;

    [Header("Timer")]
    public float startTime = 180f;
    public float timeRemaining;
    public bool timerRunning = false;
    public bool timeOver = false;

    [Header("Game Over")]
    public GameObject gameOverPanel;

    [Header("SFX")]
    public AudioSource sfxSource;
    public AudioClip eggPickupClip;

    private bool gameOver = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;

        if (sfxSource == null)
        {
            sfxSource = GetComponent<AudioSource>();
        }

        point = 0;
        checkpoint = 0;

        timeRemaining = startTime;
        timerRunning = false;
        timeOver = false;
        gameOver = false;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        UpdatePointText();
        UpdateTimeText();
    }

    void Update()
    {
        if (timerRunning && !timeOver && !gameOver)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimeText();
            }
            else
            {
                timeRemaining = 0;
                timerRunning = false;
                timeOver = true;

                UpdateTimeText();
                ShowGameOver();
            }
        }
    }

    public void StartTimer()
    {
        if (!timerRunning && !timeOver && !gameOver)
        {
            timerRunning = true;
            Debug.Log("Timer dimulai!");
        }
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public void AddPoint()
    {
        point++;

        PlayEggPickupSound();

        UpdatePointText();

        Debug.Log("Point: " + point);
    }

    void PlayEggPickupSound()
    {
        if (sfxSource != null && eggPickupClip != null)
        {
            sfxSource.PlayOneShot(eggPickupClip);
        }
    }

    public void AddCheckpoint()
    {
        checkpoint++;

        PlayEggPickupSound();

        Debug.Log("Checkpoint: " + checkpoint);
    }

    void UpdatePointText()
    {
        if (pointText != null)
        {
            pointText.text = "POINT: " + point + "/" + maxPoint;
        }
    }

    void UpdateTimeText()
    {
        if (timeText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);

            timeText.text = "TIME: " + minutes.ToString("00") + ":" + seconds.ToString("00");
        }
    }

    void ShowGameOver()
    {
        gameOver = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Time.timeScale = 0f;

        Debug.Log("GAME OVER! WAKTU HABIS.");
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}