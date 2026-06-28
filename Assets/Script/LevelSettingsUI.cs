using UnityEngine;

public class LevelSettingsUI : MonoBehaviour
{
    [Header("Panel")]
    public GameObject settingsPanel;

    private void Start()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    // Buka settings
    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    // Tutup settings
    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    // Toggle sound dari settings level
    public void ToggleSound(bool value)
    {
        if (AudioManager.instance != null)
            AudioManager.instance.SetSound(value);
    }
}
