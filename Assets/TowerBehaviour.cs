using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class TowerBehaviour : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform rangeCircle;
    public Transform sizeCircle;

    public float SupportMultiplier = 1.0f;

    public Tower tower;

    private void Update()
    {
        if (tower != null)
        {
            ShowCircles(!EnemyController.waveIsOngoing);
        }
    }

    public void UpdateTower()
    {
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        collider.radius = tower.towerSize;

        GetComponent<SpriteRenderer>().sprite = tower.sprite;

        gameObject.name = $"{tower.towerName}_Tower";

        if (tower is SupportiveTower)
        {
            Debug.Log("Supporting");
            IncreaseOfNearbyTowersBy((tower as SupportiveTower));
        }
    }

    void IncreaseOfNearbyTowersBy(SupportiveTower tower)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, tower.range);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Tower"))
            {
                TowerBehaviour towerBehaviour = collider.gameObject.GetComponent<TowerBehaviour>();

                if (towerBehaviour.tower is SupportiveTower)
                {
                    continue;
                }

                Debug.Log($"Increasing speed for: {towerBehaviour.gameObject}");
                towerBehaviour.SupportMultiplier += tower.supportRate;
            }
        }
    }

    IEnumerator StartProjectileClock(ProjectileTower projectileTower)
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f / (projectileTower.shootingSpeed * SupportMultiplier));

            if (EnemyController.waveIsOngoing)
            {
                Debug.Log("Trying to shoot");
                ShootNearbyEnemy(projectileTower);
            }
        }
    }

    private void ShowCircles(bool state)
    {
        if (state)
        {
            rangeCircle.localScale = new Vector3(tower.range * 2, tower.range * 2, 1);
            rangeCircle.gameObject.SetActive(true);

            sizeCircle.localScale = new Vector3(tower.towerSize * 2, tower.towerSize * 2, 1);
            sizeCircle.gameObject.SetActive(true);

            return;
        }

        rangeCircle.gameObject.SetActive(false);
        sizeCircle.gameObject.SetActive(false);
    }

    private void ShootNearbyEnemy(ProjectileTower projectileTower)
    {
        var enemy = FindNearestEnemy(projectileTower.range);

        if (enemy == null)
        {
            return;
        }

        Vector2 direction = (enemy.transform.position - transform.position).normalized;
        float distanceToTarget = Vector2.Distance(transform.position, enemy.transform.position);

        GetComponent<SpriteRenderer>().flipX = enemy.transform.position.x > transform.position.x ? true : false;

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
        AudioController.PlaySound(1, 0.3f);

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

    public void OnPlace(Tower tower)
    {
        this.tower = tower;

        switch (tower.GetType().Name)
        {
            case nameof(ProjectileTower):
                StartCoroutine(StartProjectileClock(tower as ProjectileTower));
                break;
        }
    }
}
