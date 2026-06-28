using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public AudioSource clickSource;
    public AudioClip clickClip;

    public void PlayClick()
    {
        // 🔥 GLOBAL AUDIO CHECK (REVISI DOSEN)
        if (AudioManager.instance != null && !AudioManager.instance.soundOn)
            return;

        if (clickSource != null && clickClip != null)
        {
            clickSource.PlayOneShot(clickClip);
        }
        else
        {
            Debug.LogWarning("Click Source atau Click Clip belum diisi");
        }
    }
}