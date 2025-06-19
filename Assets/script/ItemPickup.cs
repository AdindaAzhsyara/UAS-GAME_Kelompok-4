using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private bool isCollected = false;
    private ItemInstance itemInstance;

    private void Awake()
    {
        itemInstance = GetComponent<ItemInstance>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isCollected && other.CompareTag("Player"))
        {
            isCollected = true;

            if (itemInstance != null)
            {
                FindObjectOfType<ItemManager>().CollectItem(transform.position);
                Debug.Log($"Item {itemInstance.itemData.itemName} diambil oleh {other.name}");
            }

            Destroy(gameObject); // Hapus objek setelah diambil
        }
    }
}