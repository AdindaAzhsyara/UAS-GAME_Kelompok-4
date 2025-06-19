using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GuardPatrol2D : MonoBehaviour
{
    public Transform[] waypoints;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 3.5f;
    public float chaseDuration = 10f;
    public float avoidCheckDistance = 0.5f;
    public LayerMask wallLayer;

    private int currentIndex = 0;
    private Animator animator;
    private Transform player;
    private float chaseTimer = 0f;
    private bool isChasing = false;

    private bool lostSight = false;
    private Transform altTarget = null;

    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    int GetNearestWaypointIndex()
    {
        float shortestDistance = Mathf.Infinity;
        int nearestIndex = 0;

        for (int i = 0; i < waypoints.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, waypoints[i].position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestIndex = i;
            }
        }

        return nearestIndex;
    }

    void FixedUpdate()
    {
        if (isChasing && player != null)
        {
            ChasePlayerAvoidingWalls();
            chaseTimer -= Time.fixedDeltaTime;

            if (chaseTimer <= 0f)
            {
                StopChasing();
            }
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentIndex];
        Vector2 direction = (target.position - transform.position).normalized;

        rb.MovePosition(rb.position + direction * patrolSpeed * Time.fixedDeltaTime);

        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            currentIndex = (currentIndex + 1) % waypoints.Length;
        }
    }

    void ChasePlayerAvoidingWalls()
    {
        if (lostSight && altTarget != null)
        {
            Vector2 dir = (altTarget.position - transform.position).normalized;
            rb.MovePosition(rb.position + dir * chaseSpeed * Time.fixedDeltaTime);

            animator.SetFloat("moveX", dir.x);
            animator.SetFloat("moveY", dir.y);

            if (Vector2.Distance(transform.position, altTarget.position) < 0.2f)
            {
                lostSight = false;
                altTarget = null;
            }

            return;
        }

        Vector2 targetDirection = (player.position - transform.position).normalized;

        // Check apakah ada tembok di depan
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, avoidCheckDistance, wallLayer);

        if (hit.collider == null)
        {
            // Tidak ada tembok, langsung kejar
            rb.MovePosition(rb.position + targetDirection * chaseSpeed * Time.fixedDeltaTime);
            animator.SetFloat("moveX", targetDirection.x);
            animator.SetFloat("moveY", targetDirection.y);
        }
        else
        {
            // Tabrak tembok, cari alternatif arah horizontal/vertikal
            Vector2 horizontal = new Vector2(targetDirection.x, 0).normalized;
            Vector2 vertical = new Vector2(0, targetDirection.y).normalized;

            bool canGoHorizontally = !Physics2D.Raycast(transform.position, horizontal, avoidCheckDistance, wallLayer);
            bool canGoVertically = !Physics2D.Raycast(transform.position, vertical, avoidCheckDistance, wallLayer);

            if (canGoHorizontally)
            {
                rb.MovePosition(rb.position + horizontal * chaseSpeed * Time.fixedDeltaTime);
                animator.SetFloat("moveX", horizontal.x);
                animator.SetFloat("moveY", 0);
            }
            else if (canGoVertically)
            {
                rb.MovePosition(rb.position + vertical * chaseSpeed * Time.fixedDeltaTime);
                animator.SetFloat("moveX", 0);
                animator.SetFloat("moveY", vertical.y);
            }
            else
            {
                altTarget = GetNearestWaypointToPlayer();
                lostSight = true;
            }
        }
    }

    Transform GetNearestWaypointToPlayer()
    {
        float shortestDistance = Mathf.Infinity;
        Transform nearest = null;

        foreach (Transform wp in waypoints)
        {
            float distance = Vector3.Distance(player.position, wp.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = wp;
            }
        }

        return nearest;
    }

    void StopChasing()
    {
        isChasing = false;
        player = null;
        currentIndex = GetNearestWaypointIndex();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isChasing && other.CompareTag("Player"))
        {
            player = other.transform;
            isChasing = true;
            chaseTimer = chaseDuration;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isChasing && collision.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<ItemManager>().PenalizePlayer();
            StopChasing();
        }
    }
}
