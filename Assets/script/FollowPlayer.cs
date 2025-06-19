using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;       // Objek pemain
    public Vector3 offset = new Vector3(0f, 0.1f, -2f); // Jarak kamera terhadap pemain (sesuaikan untuk zoom)

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.LookAt(target);
        }
    }
}
