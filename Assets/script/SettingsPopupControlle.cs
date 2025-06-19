using UnityEngine;
#if UNITY_EDITOR
using UnityEditor; // penting untuk akses EditorApplication
#endif

public class SettingsPopupController : MonoBehaviour
{
    public GameObject settingsPanel;

    public void ShowPopup()
    {
        settingsPanel.SetActive(true);
    }

    public void HidePopup()
    {
        settingsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("Game exited");

        // Keluar aplikasi jika dibuild, atau stop play mode jika di editor
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
