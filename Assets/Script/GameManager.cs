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
        sfxSource = GetComponent<AudioSource>();

    // 🔥 STOP ONLY BG MUSIC (SAFE VERSION)
    AudioSource[] allAudio = FindObjectsOfType<AudioSource>();

foreach (AudioSource a in allAudio)
{
    if (a.loop && a.gameObject.name.ToLower().Contains("music"))
    {
        a.Stop();
    }
}

    point = 0;
    checkpoint = 0;

    timeRemaining = startTime;
    timerRunning = false;
    timeOver = false;
    gameOver = false;

    if (gameOverPanel != null)
        gameOverPanel.SetActive(false);

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
                ShowGameOver();
            }
        }
    }

    // ✅ INI WAJIB ADA (BIAR STARTGATE TIDAK ERROR)
    public void StartTimer()
    {
        if (!timerRunning && !timeOver && !gameOver)
            timerRunning = true;
    }

    // ✅ INI WAJIB ADA (BIAR FINISHLEVEL TIDAK ERROR)
    public void StopTimer()
    {
        timerRunning = false;
    }

    public void AddPoint()
    {
        point++;

        if (AudioManager.instance != null && !AudioManager.instance.soundOn)
            return;

        PlayEggPickupSound();
        UpdatePointText();
    }

    public void AddCheckpoint()
    {
        checkpoint++;

        if (AudioManager.instance != null && !AudioManager.instance.soundOn)
            return;

        PlayEggPickupSound();
    }

    void PlayEggPickupSound()
    {
        if (AudioManager.instance != null && !AudioManager.instance.soundOn)
            return;

        if (sfxSource != null && eggPickupClip != null)
            sfxSource.PlayOneShot(eggPickupClip);
    }

    void UpdatePointText()
    {
        if (pointText != null)
            pointText.text = "POINT: " + point + "/" + maxPoint;
    }

    void UpdateTimeText()
    {
        if (timeText != null)
        {
            int m = Mathf.FloorToInt(timeRemaining / 60);
            int s = Mathf.FloorToInt(timeRemaining % 60);

            timeText.text = "TIME: " + m.ToString("00") + ":" + s.ToString("00");
        }
    }

    void ShowGameOver()
    {
        gameOver = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
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