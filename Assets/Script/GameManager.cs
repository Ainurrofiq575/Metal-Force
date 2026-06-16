using TMPro;
using UnityEngine;

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
    public float timeRemaining = 180f; // 180 detik = 3 menit
    public bool timerRunning = false;
    public bool timeOver = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdatePointText();
        UpdateTimeText();
    }

    void Update()
    {
        if (timerRunning && !timeOver)
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

                Debug.Log("WAKTU HABIS!");
            }
        }
    }

    public void StartTimer()
    {
        if (!timerRunning && !timeOver)
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

        UpdatePointText();

        Debug.Log("Point: " + point);
    }

    public void AddCheckpoint()
    {
        checkpoint++;

        Debug.Log("Checkpoint: " + checkpoint);
    }

    void UpdatePointText()
    {
        pointText.text = "POINT: " + point + "/" + maxPoint;
    }

    void UpdateTimeText()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timeText.text = "TIME: " + minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}