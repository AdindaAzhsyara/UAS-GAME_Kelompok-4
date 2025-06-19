using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void KembaliKeHome()
    {
        SceneManager.LoadScene("Home");
    }
}
