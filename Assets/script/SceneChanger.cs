using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void GoToLevelScene()
    {
        SceneManager.LoadScene("Levell"); // Ganti dengan nama scene tujuan
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game exited"); // Ini hanya terlihat di editor
    }
}

