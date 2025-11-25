using UnityEngine;

public class DisparoJEFE2 : MonoBehaviour
{
    [Header("Disparo - Referencias")]
    public Transform controladorDisparo;
    public bool useDistanceDetection = true;

    [Header("Ajustes de línea")]
    public float distanciaLinea;
    public LayerMask capaHelicoptero;

    [Header("Estado")]
    public bool helicopteroEnRango;
    
    [Header("Dirección")]
    public bool haciaIzquierda = true;
    
    [Header("Firing")]
    public bool autoFire = false;
    public GameObject balaPrefab;
    public float velocidadBala = 5f;
    public float lifetimeBala = 5f;
    public float fireRate = 1f;
    public int bulletDamage = 1;

    [Header("Animator")]
    public Animator animator;

    private float nextFireTime = 0f;
    private GameObject cachedHelicoptero;

    void Update()
    {
        if (controladorDisparo == null)
        {
            helicopteroEnRango = false;
            return;
        }

        if (cachedHelicoptero == null)
            cachedHelicoptero = GameObject.Find("Helicoptero");

        Vector2 direccion = Vector2.right;
        if (controladorDisparo != null)
            direccion = controladorDisparo.right;
        else
            direccion = transform.right;

        if (haciaIzquierda) direccion = -direccion;

        bool detectedByRay = false;
        RaycastHit2D hit = Physics2D.Raycast(controladorDisparo.position, direccion, distanciaLinea, capaHelicoptero);
        detectedByRay = hit.collider != null;

        bool detectedByDistance = false;
        if (useDistanceDetection && cachedHelicoptero != null)
        {
            float dist = Vector2.Distance(controladorDisparo.position, cachedHelicoptero.transform.position);
            detectedByDistance = dist <= distanciaLinea;
        }

        helicopteroEnRango = detectedByRay || detectedByDistance;

        if (autoFire && helicopteroEnRango && Time.time >= nextFireTime)
        {
            if (balaPrefab != null)
            {
                Disparar();
            }
        }
    }

    public void Disparar()
    {
        if (Time.time < nextFireTime)
        {
            return;
        }

        if (balaPrefab == null)
        {
            Debug.LogWarning("DisparoJEFE2: balaPrefab no asignado.");
            return;
        }

        if (controladorDisparo == null)
        {
            Debug.LogWarning("DisparoJEFE2: controladorDisparo no asignado.");
            return;
        }

        if (animator != null)
        {
            animator.SetTrigger("fire");
        }

        GameObject balaGO = Instantiate(balaPrefab, controladorDisparo.position, Quaternion.identity);
        BalaJEFE2 bala = balaGO.GetComponent<BalaJEFE2>();
        
        if (bala != null)
        {
            GameObject helicopteroGO = cachedHelicoptero;
            if (helicopteroGO == null)
                helicopteroGO = GameObject.Find("Helicoptero");
            
            Vector2 dir;
            if (helicopteroGO != null)
            {
                dir = (helicopteroGO.transform.position - controladorDisparo.position);
            }
            else
            {
                dir = controladorDisparo.right;
                if (haciaIzquierda) dir = -dir;
            }

            bala.direccion = dir.normalized;
            bala.velocidad = velocidadBala;
            bala.lifetime = lifetimeBala;
            bala.useRigidbody = true;
            bala.daño = bulletDamage;
        }
        else
        {
            Debug.LogWarning("DisparoJEFE2: el prefab de bala no contiene BalaJEFE2.");
            Destroy(balaGO);
        }

        nextFireTime = Time.time + fireRate;
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


