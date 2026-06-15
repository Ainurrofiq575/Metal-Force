using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public AudioSource clickSource;
    public AudioClip clickClip;

    public void PlayClick()
    {
        if (clickSource != null && clickClip != null)
        {
            clickSource.PlayOneShot(clickClip);
            Debug.Log("Click sound played");
        }
        else
        {
            Debug.LogWarning("Click Source atau Click Clip belum diisi");
        }
    }
}