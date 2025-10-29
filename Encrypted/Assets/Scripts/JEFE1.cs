using UnityEngine;

public class JEFE1 : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Assign the Player GameObject here (drag the player from the Hierarchy). If left empty the script will try to find an object with tag 'Player'.")]
    public GameObject player;
    public float detectionRadius = 5.0f;
    public float speed = 2.0f;
    private Rigidbody2D rb;
    private Vector2 movement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // If player was not assigned in the Inspector try to find it by tag
        if (player == null)
        {
            GameObject go = GameObject.FindWithTag("Player");
            if (go != null) player = go;
            else Debug.LogWarning("JEFE1: 'player' not assigned and no GameObject with tag 'Player' found.", this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            movement = Vector2.zero;
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer < detectionRadius)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;

            movement = new Vector2(direction.x, 0);

        }
        else
        {
            movement = Vector2.zero;
        }
        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
    }
}
