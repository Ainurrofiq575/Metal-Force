using UnityEngine;
using UnityEngine.UI; // 1. Tambahkan ini agar bisa membaca komponen Toggle

public class LevelSettingsUI : MonoBehaviour
{
    [Header("Panel")]
    public GameObject settingsPanel;

    [Header("Komponen UI")]
    public Toggle musicToggle; // 2. Tambahkan variabel ini untuk kotak centangnya

    private void Start()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    // Buka settings
    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);

            // 3. --- TAMBAHAN UNTUK SINKRONISASI CENTANG ---
            // Saat panel dibuka, otomatis cek status AudioManager
            if (AudioManager.instance != null && musicToggle != null)
            {
                // SetIsOnWithoutNotify akan mengubah visual centang (hilang/muncul) 
                // tanpa memicu fungsi ToggleSound berkali-kali.
                musicToggle.SetIsOnWithoutNotify(AudioManager.instance.IsSoundOn());
            }
        }
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