using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject bullet;
    public string bulletKey = "bullet";
    public float shootInterval = 3f;
    public float bulletSpeed = 5f;
    public float spreadAngle = 15f;

    void Start()
    {
        StartCoroutine(ShootRoutine());
    }

    IEnumerator ShootRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootInterval);

            var targets = FindObjectsOfType<ZombieMove>();
            if (targets.Length == 0) continue;

            ZombieMove target = targets[Random.Range(0, targets.Length)];
            Vector3 targetDir = (target.transform.position - transform.position).normalized;

            ShootBullet(targetDir);
            ShootBullet(Quaternion.Euler(0, 0, spreadAngle) * targetDir);
            ShootBullet(Quaternion.Euler(0, 0, -spreadAngle) * targetDir); 
        }
    }

    void ShootBullet(Vector3 direction)
    {
        GameObject instance = PoolManager.Instance.GetObject(bulletKey, bullet, transform);
        Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }
    }
}
