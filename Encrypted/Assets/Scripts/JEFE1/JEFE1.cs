using UnityEngine;

public class JEFE1 : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 5f;
    public float speed = 2f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isRunning;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // 🔒 Evita que el jefe se voltee o rote por las físicas
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            if (direction.x > 0)
        {
                transform.localScale = new Vector3(-2, 2, 1);
        }
            if (direction.x < 0)
        {
                transform.localScale = new Vector3(2, 2, 1);
        }
            movement = new Vector2(direction.x, 0);

            isRunning = true;
            
        }
        else
        {
            movement = Vector2.zero;
            isRunning = false;
        }

        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);

        animator.SetBool("isRunning", isRunning);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

