using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class TowerBehaviour : MonoBehaviour
{
    private Tower tower;

    public void UpdateTower(Tower tower)
    {
        this.tower = tower;

        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        collider.radius = tower.radius;

        gameObject.name = $"{tower.towerName}_Tower";
    }
}
