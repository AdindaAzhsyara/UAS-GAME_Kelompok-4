using UnityEngine;
using UnityEngine.UI;

public class MiniMapGenerator : MonoBehaviour
{
    public RectTransform miniMapPanel; // Panel minimap (UI)
    public GameObject iconPrefab;      // Prefab ikon titik
    public Vector2[] itemPositions;    // Posisi item (samakan dengan spawnPositions)
    public float scale = 10f;          // Skala posisi ke UI minimap

    void Start()
    {
        foreach (Vector2 pos in itemPositions)
        {
            GameObject icon = Instantiate(iconPrefab, miniMapPanel);
            RectTransform iconRect = icon.GetComponent<RectTransform>();

            // Atur posisi ikon di dalam panel (dikonversi dari world ke UI)
            Vector2 minimapPos = pos * scale;
            iconRect.anchoredPosition = minimapPos;
        }
    }
}
