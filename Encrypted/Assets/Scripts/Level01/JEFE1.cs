using UnityEngine;

public class JEFE1 : Entity
{
    [Header("Referencias")]
    [Tooltip("Asigna aquí el GameObject Player (arrástralo desde la Jerarquía). Si queda vacío el script intentará encontrar un objeto con la etiqueta 'Player'.")]
    public GameObject player;
    [Tooltip("Radio de detección (unidades) en el que el jefe perseguirá/atacará al jugador")]
    public float detectionRadius = 5.0f;
    // Use Entity.moveSpeed (serialized in the base class) for movement speed.
    // You can edit 'moveSpeed' in the Inspector on this component because it's serialized in the base class `Entity`.
    [Header("Disparo")]
    [Tooltip("Referencia al componente DisparoJEFE1 que gestiona la instanciación de balas. Si queda vacío el script intentará encontrar uno en este GameObject o en sus hijos.")]
    public DisparoJEFE1 disparo;

    void Start()
    {
        // Try to find a DisparoJEFE1 on this GameObject or children if not assigned
        if (disparo == null)
            disparo = GetComponentInChildren<DisparoJEFE1>();

        // If player was not assigned in the Inspector try to find it by tag
        if (player == null)
        {
            GameObject go = GameObject.FindWithTag("Player");
            if (go != null) player = go;
            else Debug.LogWarning("JEFE1: 'player' not assigned and no GameObject with tag 'Player' found.", this);
        }
        // Prevent the boss from rotating when colliding (avoid 'rolling' effect)
        // rb is initialized in Entity.Awake(), Start runs after Awake so it's safe to access.
        if (rb != null)
        {
            // Freeze rotation in Z so physics collisions don't spin the enemy
            rb.freezeRotation = true;
            // also clear any residual angular velocity
            rb.angularVelocity = 0f;
        }
    }

    // Override Entity.Update to implement enemy AI without player input
    protected override void Update()
    {
        // reuse collision handling from Entity
        HandleCollision();

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer < detectionRadius)
            {
                // Move horizontally towards the player (only X axis)
                Vector2 direction = (player.transform.position - transform.position).normalized;
                // Use the inherited moveSpeed from Entity
                rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

                // Let DisparoJEFE1 handle firing cadence; calling Disparar() is safe (it respects cooldown)
                if (disparo != null)
                    disparo.Disparar();
            }
            else
            {
                // Stop horizontal movement when player is out of detection radius
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            }
        }

        // reuse animation handling but do NOT auto-flip the transform (boss keeps fixed orientation)
        HandleAnimations();
    }
}
