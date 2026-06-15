using UnityEngine;
using UnityEngine.UI;

public class MusicToggleController : MonoBehaviour
{
    public AudioSource menuMusic;
    private Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(SetMusic);
    }

    private void Start()
    {
        SetMusic(toggle.isOn);
    }

    public void SetMusic(bool isOn)
    {
        if (menuMusic == null) return;

        menuMusic.mute = !isOn;

        if (isOn && !menuMusic.isPlaying)
        {
            menuMusic.Play();
        }

        Debug.Log("Music: " + (isOn ? "ON" : "OFF"));
    }
}