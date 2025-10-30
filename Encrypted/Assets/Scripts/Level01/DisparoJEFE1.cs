using UnityEngine;

public class DisparoJEFE1 : MonoBehaviour
{
    [Header("Disparo - Referencias")]
    [Tooltip("Transform desde el que se traza la línea de disparo (arrastra el objeto controlador aquí)")]
    public Transform controladorDisparo;
    [Tooltip("Si está activado, el enemigo usará la distancia directa al Player para detectar en vez de sólo el raycast.")]
    public bool useDistanceDetection = true;

    [Header("Ajustes de línea")]
    [Tooltip("Longitud (en unidades) de la línea de detección")]
    public float distanciaLinea;

    [Tooltip("Capa(s) que se consideran 'Jugador' para la detección")]
    public LayerMask capaJugador;

    [Header("Estado")]
    [Tooltip("Resultado de la comprobación: true si el jugador está dentro del rango de la línea")]
    public bool jugadorEnRango;
    [Header("Dirección")]
    [Tooltip("Si está activado, la línea se dibuja hacia la izquierda del controlador; si no, hacia la derecha.")]
    public bool haciaIzquierda = true;
    [Header("Firing")]
    [Tooltip("Prefab de la bala a instanciar (debe contener el script BalaJEFE1)")]
    public GameObject balaPrefab;
    [Tooltip("Daño que aplicará cada bala")]
    public int dañoBala = 10;
    [Tooltip("Velocidad que tendrá la bala")]
    public float velocidadBala = 5f;
    [Tooltip("Tiempo de vida de la bala (s)")]
    public float lifetimeBala = 5f;
    [Tooltip("Cadencia de disparo (disparos por segundo)")]
    public float fireRate = 1f;

    private float fireCooldown = 0f;
    // cached player reference
    private GameObject cachedPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    // Update is called once per frame
    void Update()
    {
        if (controladorDisparo == null)
        {
            jugadorEnRango = false;
            return;
        }

        // Try to cache player once
        if (cachedPlayer == null)
            cachedPlayer = GameObject.FindWithTag("Player");

        // Use RaycastHit2D so we can explicitly set the bool and avoid implicit conversions
        // Determine direction: prefer the controladorDisparo.right vector if available
        Vector2 direccion = Vector2.right;
        if (controladorDisparo != null)
            direccion = controladorDisparo.right;
        else
            direccion = transform.right;

        if (haciaIzquierda) direccion = -direccion;

        bool detectedByRay = false;
        RaycastHit2D hit = Physics2D.Raycast(controladorDisparo.position, direccion, distanciaLinea, capaJugador);
        detectedByRay = hit.collider != null;

        bool detectedByDistance = false;
        if (useDistanceDetection && cachedPlayer != null)
        {
            float dist = Vector2.Distance(controladorDisparo.position, cachedPlayer.transform.position);
            detectedByDistance = dist <= distanciaLinea;
        }

        jugadorEnRango = detectedByRay || detectedByDistance;

        if (jugadorEnRango)
        {
            // aquí puedes añadir lógica cuando el jugador esté en rango
            // Disparar si toca y si ha pasado el tiempo de recarga
            if (balaPrefab != null && fireRate > 0f)
            {
                if (fireCooldown <= 0f)
                {
                    Disparar();
                    fireCooldown = 1f / fireRate;
                }
            }
        }

        if (fireCooldown > 0f) fireCooldown -= Time.deltaTime;
    }

    /// <summary>
    /// Instancia una bala desde el controladorDisparo y configura su dirección, velocidad y daño.
    /// Puedes llamar a este método desde otro script si prefieres controlar el disparo externamente.
    /// </summary>
    public void Disparar()
    {
        if (balaPrefab == null)
        {
            Debug.LogWarning("DisparoJEFE1: balaPrefab no asignado.");
            return;
        }

        if (controladorDisparo == null)
        {
            Debug.LogWarning("DisparoJEFE1: controladorDisparo no asignado.");
            return;
        }

        // Instanciar la bala en la posición del controlador
        GameObject balaGO = Instantiate(balaPrefab, controladorDisparo.position, Quaternion.identity);
        BalaJEFE1 bala = balaGO.GetComponent<BalaJEFE1>();
        if (bala != null)
        {
            // Determinar dirección hacia el GameObject tagged "Player" si existe
            GameObject playerGO = GameObject.FindWithTag("Player");
            Vector2 dir;
            if (playerGO != null)
            {
                dir = (playerGO.transform.position - controladorDisparo.position);
            }
            else
            {
                // Fallback: usar la dirección del controlador (o invertida)
                dir = controladorDisparo.right;
                if (haciaIzquierda) dir = -dir;
            }

            bala.direccion = dir.normalized;
            bala.velocidad = velocidadBala;
            bala.daño = dañoBala;
            bala.lifetime = lifetimeBala;
            // Ensure the bullet uses Rigidbody by default for physics-based movement
            bala.useRigidbody = true;
        }
        else
        {
            Debug.LogWarning("DisparoJEFE1: el prefab de bala no contiene BalaJEFE1.");
        }
    }

    private void OnDrawGizmos()
    {
        if (controladorDisparo == null) return;
        Gizmos.color = Color.red;
        Vector2 dirGizmo = controladorDisparo.right;
        if (haciaIzquierda) dirGizmo = -dirGizmo;
        Gizmos.DrawLine(controladorDisparo.position, controladorDisparo.position + (Vector3)dirGizmo * distanciaLinea);
    }
}
