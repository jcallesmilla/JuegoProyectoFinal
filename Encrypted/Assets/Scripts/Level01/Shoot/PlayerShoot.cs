using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour
{
    [Header("Configuración de Disparo")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float shootCooldown = 0.25f;
    [SerializeField] private float animationDelay = 0.1f;

    [Header("Referencias")]
    [SerializeField] private Animator animator;

    private bool canShoot = true;
    private bool isShooting = false;
    private const string SHOOT_PARAM = "shoot";

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (firePoint == null)
        {
            GameObject fp = new GameObject("FirePoint");
            fp.transform.SetParent(transform);
            fp.transform.localPosition = new Vector3(-3.0f, 1.5f, 0f);
            firePoint = fp.transform;
        }

        if (animator != null)
        {
            Debug.Log("Animator encontrado correctamente");
        }
        else
        {
            Debug.LogError("No se encontró el Animator. Asegúrate de que el Player tiene un hijo con Animator.");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canShoot && !isShooting)
        {
            Debug.Log("Click detectado - Iniciando disparo");
            StartCoroutine(ShootCoroutine());
        }
    }

    private IEnumerator ShootCoroutine()
    {
        isShooting = true;
        canShoot = false;

        if (animator != null)
        {
            Debug.Log("Activando animación de disparo");
            animator.SetBool(SHOOT_PARAM, true);
        }

        yield return new WaitForSeconds(animationDelay);

        SpawnBullet();

        if (animator != null)
        {
            animator.SetBool(SHOOT_PARAM, false);
        }

        yield return new WaitForSeconds(shootCooldown);

        canShoot = true;
        isShooting = false;
    }

    private void SpawnBullet()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("No hay BulletPrefab asignado en el Inspector");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogWarning("No hay FirePoint asignado");
            return;
        }

        int direction = transform.localScale.x > 0 ? 1 : -1;

        Debug.Log($"Disparando bala en dirección: {direction}");
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.Initialize(direction, bulletSpeed);
        }
        else
        {
            Debug.LogError("El prefab de bala no tiene el script Bullet.cs");
        }
    }
}
