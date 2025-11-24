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

    private Entity playerEntity;
    private Animator animator;
    private bool canShoot = true;
    private bool isShooting = false;
    private Camera mainCamera;

    private void Start()
    {
        playerEntity = GetComponent<Entity>();
        animator = GetComponentInChildren<Animator>();
        mainCamera = Camera.main;

        if (firePoint == null)
        {
            GameObject fp = new GameObject("FirePoint");
            fp.transform.SetParent(transform);
            fp.transform.localPosition = new Vector3(0.5f, 0.5f, 0f);
            firePoint = fp.transform;
        }
    }

    public void Shoot()
    {
        if (canShoot && !isShooting)
        {
            StartCoroutine(ShootCoroutine());
        }
    }

    private IEnumerator ShootCoroutine()
    {
        isShooting = true;
        canShoot = false;

        yield return new WaitForSeconds(animationDelay);

        SpawnBullet();

        yield return new WaitForSeconds(shootCooldown);

        canShoot = true;
        isShooting = false;
    }

    private void SpawnBullet()
    {
        if (bulletPrefab == null || firePoint == null || mainCamera == null) return;

        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)firePoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.Initialize(direction, bulletSpeed);
        }
    }

    private int GetFacingDirection()
    {
        return transform.localScale.x > 0 ? 1 : -1;
    }
}
