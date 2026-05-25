using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int point = 0;
    public int maxPoint = 5;

    public int checkpoint = 0;
    public int maxCheckpoint = 3;

    public TextMeshProUGUI pointText;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdatePointText();
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
        pointText.text = "Point: " + point + "/" + maxPoint;
    }
}