using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour
{
    public CountdownTimer countdownTimer; // Drag object CountdownTimer dari Inspector
    public ItemData[] itemDataList; // baru, isinya ScriptableObject
    public int itemCount = 5;
    public Vector2[] spawnPositions;
    public TextMeshProUGUI collectedText;
    public Transform playerTransform;
    private Vector2 playerStartPosition;
    private Stack<Vector2> collectedItemPositions = new Stack<Vector2>();
    private List<Vector2> usedPositions = new List<Vector2>();
    private List<Vector2> availablePositions = new List<Vector2>();
    private int timesCaught = 0;           // Jumlah pemain tertangkap
    public int maxCaughtAllowed = 3;       // Batas maksimal tertangkap
    public GameObject gameOverPanel;       // UI Panel untuk Game Over

    private int collected = 0;

    void Start()
    {
        SpawnItems();
        UpdateUI();
        availablePositions.AddRange(spawnPositions);
        if (playerTransform != null)
        {
            playerStartPosition = playerTransform.position;
        }
    }

    void SpawnItems()
    {
        List<Vector2> chosenPositions = new List<Vector2>();
        System.Random rand = new System.Random();

        for (int i = 0; i < itemCount; i++)
        {
            Vector2 pos;
            int attempts = 0;

            do
            {
                pos = spawnPositions[rand.Next(spawnPositions.Length)];
                attempts++;
            }
            while (chosenPositions.Exists(p => Vector2.Distance(p, pos) < 0.1f) && attempts < 100);

            chosenPositions.Add(pos);

            ItemData data = itemDataList[rand.Next(itemDataList.Length)];
            Instantiate(data.prefab, pos, Quaternion.identity);

            chosenPositions.Add(pos);
            availablePositions.Remove(pos);   // tandai posisi ini tidak tersedia
            usedPositions.Add(pos);           // tandai posisi sedang dipakai
        }
    }

    public void DecreaseCollected()
    {
        collected = Mathf.Max(0, collected - 1);
        UpdateUI();
    }

    public void CollectItem(Vector2 pos)
{
    collected++;
    UpdateUI();

    usedPositions.Remove(pos);
    if (!availablePositions.Contains(pos))
        availablePositions.Add(pos);

    collectedItemPositions.Push(pos); // simpan lokasi item

    // Jika semua item terkumpul
    if (collected >= itemCount && countdownTimer != null)
    {
        countdownTimer.ShowWinner(); // Panggil fungsi winner
    }
}

    void UpdateUI()
    {
        collectedText.text = collected + "/" + itemCount;
    }

    public void PenalizePlayer()
    {
        // Tambahkan jumlah tertangkap
        timesCaught++;

        // Cek apakah sudah melebihi batas tertangkap
        if (timesCaught >= maxCaughtAllowed)
        {
            StartCoroutine(GameOver());
            return;
        }

        // Selalu kembalikan ke posisi awal
        if (playerTransform != null)
        {
            playerTransform.position = playerStartPosition;
        }

        // Jika punya item, respawn kembali item terakhir
        if (collectedItemPositions.Count > 0)
        {
            collected--;
            UpdateUI();

            Vector2 returnPos = collectedItemPositions.Pop();
            ItemData data = itemDataList[Random.Range(0, itemDataList.Length)];
            Instantiate(data.prefab, returnPos, Quaternion.identity);


            if (!usedPositions.Contains(returnPos))
                usedPositions.Add(returnPos);
            availablePositions.Remove(returnPos);
        }
    }
    private IEnumerator GameOver()
    {
        Debug.Log("Game Over!");

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true); // Tampilkan panel Game Over
            gameOverPanel.transform.SetAsLastSibling();
        }

        Time.timeScale = 0f; // Hentikan gameplay

        // Tunggu 3 detik secara real-time
        yield return new WaitForSecondsRealtime(3f);

        Time.timeScale = 1f; // Aktifkan kembali waktu
        SceneManager.LoadScene(1); // Ganti dengan index/scene level yang benar
    }


}