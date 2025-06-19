using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CountdownTimer : MonoBehaviour
{
    public float totalTime = 240f; // 4 menit
    public TextMeshProUGUI timerText;
    public GameObject winnerPanel; // Panel UI untuk tampilan menang
    public GameObject gameOverImage; // Gambar Game Over
    public Image whiteOverlay; // Overlay putih transparan
    
    private float remainingTime;
    public float RemainingTime => remainingTime;
    private bool timerActive = true;
    private bool gameOverStarted = false;

    void Start()
{
    remainingTime = totalTime;
    UpdateTimerDisplay();

    if (gameOverImage != null)
        gameOverImage.SetActive(false);

    if (whiteOverlay != null)
    {
        whiteOverlay.gameObject.SetActive(false);
        whiteOverlay.color = new Color(1, 1, 1, 0);
    }

    if (winnerPanel != null)
        winnerPanel.SetActive(false); // Sembunyikan winner panel di awal
}

public void OnWinnerReply()
{
    Time.timeScale = 1f;
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}

public void OnWinnerHome()
{
    Time.timeScale = 1f;
    SceneManager.LoadScene("Home"); // atau pakai Scene index 0
}


    void Update()
    {
        if (!timerActive) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime < 0) remainingTime = 0;
            UpdateTimerDisplay();
        }
        else if (!gameOverStarted)
        {
            gameOverStarted = true;
            timerActive = false;
            StartCoroutine(HandleGameOver());
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ShowWinner()
{
    if (winnerPanel != null)
    {
        winnerPanel.SetActive(true);
        winnerPanel.transform.SetAsLastSibling(); // Pastikan panel ini di atas segalanya
    }

    Time.timeScale = 0f; // Pause game saat menang
}

    IEnumerator HandleGameOver()
    {
        // Munculkan white overlay dan fade-in
        if (whiteOverlay != null)
        {
            whiteOverlay.gameObject.SetActive(true);
            float durasi = 1f;
            float t = 0;
            while (t < durasi)
            {
                t += Time.unscaledDeltaTime;
                float alpha = Mathf.Lerp(0, 0.6f, t / durasi); // sampai 60% opacity
                whiteOverlay.color = new Color(1, 1, 1, alpha);
                yield return null;
            }
        }

        // Tampilkan gambar Game Over
        if (gameOverImage != null)
        {
            gameOverImage.SetActive(true);
            gameOverImage.transform.SetAsLastSibling(); // agar tampil paling depan
        }

        // Hentikan gameplay
        Time.timeScale = 0f;

        // Tunggu 3 detik real time
        yield return new WaitForSecondsRealtime(3f);

        // Kembalikan Time.timeScale ke normal
        Time.timeScale = 1f;

        // Pindah ke scene (pastikan index benar, bisa diganti sesuai kebutuhan)
        SceneManager.LoadScene(1);
    }
}
