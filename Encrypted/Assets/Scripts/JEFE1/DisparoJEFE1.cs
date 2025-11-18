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
    [Tooltip("Si está activado, disparará automáticamente cuando el jugador esté en rango")]
    public bool autoFire = false;
    [Tooltip("Prefab de la bala a instanciar (debe contener el script BalaJEFE1)")]
    public GameObject balaPrefab;
    // La variable 'dañoBala' y su Tooltip han sido eliminados.
    [Tooltip("Velocidad que tendrá la bala")]
    public float velocidadBala = 5f;
    [Tooltip("Tiempo de vida de la bala (s)")]
    public float lifetimeBala = 5f;
    [Tooltip("Cadencia de disparo (disparos por segundo)")]
    public float fireRate = 1f;

    [Header("Animator")]
    [Tooltip("Animator del jefe. Se lanzará el trigger 'Disparar' antes de instanciar la bala (debes añadir ese parámetro en el Animator Controller).")]
    public Animator animator;

    private float fireCooldown = 0f;
    private GameObject cachedPlayer;

    void Update()
    {
        if (controladorDisparo == null)
        {
            jugadorEnRango = false;
            return;
        }

        if (cachedPlayer == null)
            cachedPlayer = GameObject.FindWithTag("Player");

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

        if (autoFire && jugadorEnRango)
        {
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

    public void Disparar()
    {
        if (fireRate <= 0f)
        {
            Debug.LogWarning("DisparoJEFE1: fireRate must be > 0 to fire.");
            return;
        }

        if (fireCooldown > 0f)
        {
            return;
        }

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

        GameObject balaGO = Instantiate(balaPrefab, controladorDisparo.position, Quaternion.identity);
        BalaJEFE1 bala = balaGO.GetComponent<BalaJEFE1>();
        if (bala != null)
        {
            GameObject playerGO = GameObject.FindWithTag("Player");
            Vector2 dir;
            if (playerGO != null)
            {
                dir = (playerGO.transform.position - controladorDisparo.position);
            }
            else
            {
                dir = controladorDisparo.right;
                if (haciaIzquierda) dir = -dir;
            }

            if (bala != null)
{
    bala.direccion = dir.normalized;
    bala.velocidad = velocidadBala;
    bala.lifetime = lifetimeBala;
    bala.useRigidbody = true;
    bala.daño = 1; // O crea una variable pública en DisparoJEFE1 para controlarlo
}
        }
        else
        {
            Debug.LogWarning("DisparoJEFE1: el prefab de bala no contiene BalaJEFE1.");
        }

        fireCooldown = 1f / fireRate;
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

