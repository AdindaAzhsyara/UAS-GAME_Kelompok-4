using UnityEngine;

public class MinimapArrowController : MonoBehaviour
{
    private Vector2 movement;

    void Update()
    {
        // Ambil input gerak
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.sqrMagnitude > 0.01f)
        {
            // Hitung sudut arah gerak (dalam derajat)
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;

            // Putar panah sesuai arah, dikurangi 90 derajat agar panah menghadap ke atas
            transform.localRotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }
}
