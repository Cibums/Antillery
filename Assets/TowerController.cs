using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public static TowerController instance;
    public Tower[] Towers;

    [SerializeField]
    private GameObject towerPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlaceTower(0);
        }
    }

    void PlaceTower(int index)
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0;

        if (TowerIsPlaceable(index, worldPosition))
        {
            var tower = Instantiate(towerPrefab, worldPosition, Quaternion.identity).GetComponent<TowerBehaviour>();
            tower.UpdateTower(Towers[index]);
            AudioController.PlaySound(0);
        }
    }

    bool TowerIsPlaceable(int towerIndex, Vector2 position)
    {
        string[] targetTags = new string[]
        {
            "Obstacle",
            "Tower",
            "Path"
        };

        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, Towers[towerIndex].towerSize);

        foreach (Collider2D collider in colliders)
        {
            foreach (string tag in targetTags)
            {
                if (collider.CompareTag(tag))
                {
                    Debug.LogWarning($"Collided with {collider.gameObject.name}");
                    return false;
                }
            }
        }

        return true;
    }
}
