using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void LoadLevel(string namaScene)
    {
        SceneManager.LoadScene(namaScene);
    }
}
