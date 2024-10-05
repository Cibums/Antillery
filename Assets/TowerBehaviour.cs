using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(CircleCollider2D))]
public class TowerBehaviour : MonoBehaviour
{
    public GameObject projectilePrefab;

    private Tower tower;

    public void UpdateTower(Tower tower)
    {
        this.tower = tower;

        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        collider.radius = tower.towerSize;

        gameObject.name = $"{tower.towerName}_Tower";

        switch (tower.GetType().Name)
        {
            case nameof(ProjectileTower):
                StartCoroutine(StartProjectileClock(tower as ProjectileTower));
                break;
        }
    }

    IEnumerator StartProjectileClock(ProjectileTower projectileTower)
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / projectileTower.shootingSpeed);

            if (EnemyController.waveIsOngoing)
            {
                Debug.Log("Trying to shoot");
                ShootNearbyEnemy(projectileTower);
            }
        }
    }

    private void ShootNearbyEnemy(ProjectileTower projectileTower)
    {
        var enemy = FindNearestEnemy(projectileTower.shootingRange);

        if (enemy == null)
        {
            return;
        }

        Vector2 direction = (enemy.transform.position - transform.position).normalized;
        float distanceToTarget = Vector2.Distance(transform.position, enemy.transform.position);

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, distanceToTarget);

        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject != gameObject && hit.collider.gameObject != enemy)
                {
                    Debug.Log("Hit obstacle: " + hit.collider.name);

                    switch (hit.collider.gameObject.tag)
                    {
                        case "Tower":
                            return;
                        case "Obstacle":
                            return;
                        case "Enemy":
                            return;
                        default:
                            continue;
                    }
                }
            }
        }

        StartCoroutine(LaunchProjectile(enemy.transform.position));

        Debug.Log($"{gameObject} is shooting towards {enemy}");

        var enemyBehaviour = enemy.GetComponent<EnemyBehaviour>();
        enemyBehaviour.DamageEnemy();
    }

    private IEnumerator LaunchProjectile(Vector2 targetPosition)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        while ((Vector2)projectile.transform.position != targetPosition)
        {
            projectile.transform.position = Vector2.MoveTowards(projectile.transform.position, targetPosition, 20 * Time.deltaTime);
            yield return null;
        }

        Destroy(projectile);
    }

    private GameObject FindNearestEnemy(float radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        GameObject nearestObject = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestObject = collider.gameObject;
                }
            }
        }

        Debug.Log($"Nearest object is: {nearestObject}");

        return nearestObject;
    }
}
