using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;

    public int itemCount = 0; // jumlah item yang dikumpulkan
    private Vector2 startPosition;

    Vector2 movement;

    void Start()
    {
        startPosition = transform.position; // simpan posisi awal
    }

    void Update()
    {
        // Ambil input dari keyboard
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Set parameter animator
        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);
        bool isMoving = movement.sqrMagnitude > 0.1f;
        animator.SetBool("IsMoving", isMoving);
        Debug.Log("IsMoving: " + isMoving);

        // Simpan arah terakhir hanya jika sedang bergerak
        if (isMoving)
        {
            animator.SetFloat("LastMoveX", movement.x);
            animator.SetFloat("LastMoveY", movement.y);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Guard"))
        {
            Debug.Log("Tertangkap penjaga!");
            ResetPlayerPosition();
            DecreaseItem();
        }
    }

    void FixedUpdate()
    {
        // Gerakkan karakter
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void ResetPlayerPosition()
    {
        rb.position = startPosition;
    }

    public void DecreaseItem()
    {
        itemCount = Mathf.Max(0, itemCount - 1); // ini bisa tetap disimpan kalau kamu mau punya variabel lokal
        Debug.Log("Item berkurang. Sisa item (lokal): " + itemCount);

        ItemManager manager = FindObjectOfType<ItemManager>();
        if (manager != null)
        {
            manager.DecreaseCollected();
        }
    }

}
