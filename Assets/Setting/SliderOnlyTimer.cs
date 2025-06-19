using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SliderOnlyTimer : MonoBehaviour
{
    public Slider timerSlider;
    public float gameTime = 240f; // 4 menit
    public Image whiteOverlay; // Image putih transparan

    private float startTime;
    private bool stopTimer = false;

    void Start()
    {
        startTime = Time.time;
        stopTimer = false;

        timerSlider.maxValue = gameTime;
        timerSlider.value = gameTime;

        if (whiteOverlay != null)
        {
            whiteOverlay.gameObject.SetActive(false);
            whiteOverlay.color = new Color(1, 1, 1, 0); // transparan
        }
    }

    void Update()
    {
        if (stopTimer) return;

        float elapsed = Time.time - startTime;
        float remaining = gameTime - elapsed;

        if (remaining <= 0)
        {
            remaining = 0;
            timerSlider.value = 0;
            StartCoroutine(HandleTimeEnd());
        }
        else
        {
            timerSlider.value = remaining;
        }
    }

    IEnumerator HandleTimeEnd()
    {
        stopTimer = true;

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

        // Hentikan gameplay
        Time.timeScale = 0f;

        // Tunggu 3 detik real time
        yield return new WaitForSecondsRealtime(3f);

        // Kembalikan Time.timeScale ke normal
        Time.timeScale = 1f;

        // Pindah ke scene (bisa ubah sesuai kebutuhan)
        SceneManager.LoadScene(1);
    }
}
