using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public static TowerController instance;
    public Tower[] AllTowers;

    [HideInInspector]
    public static bool isPlacing = false;

    public Transform rangeCircle;
    public Transform sizeCircle;

    private int selectedTowerIndex = 0;

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
        if (isPlacing)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            worldPosition.z = 0;
            ShowCirclesAtPosition(true, new Vector2(worldPosition.x, worldPosition.y));
        }

        if (Input.GetMouseButtonDown(0) && isPlacing && !GameController.isGameOver)
        {
            PlaceTower(selectedTowerIndex);
        }
    }

    public void StartPlacement(TowerBuyButtonBehaviour button)
    {
        selectedTowerIndex = button.towerIndex;

        if (AllTowers[selectedTowerIndex].Cost > PlayerStats.Money || GameController.isGameOver)
        {
            return;
        }

        isPlacing = true;
        
        InterfaceController.Instance.SetBuyPanelState(false);
    }

    private void ShowCirclesAtPosition(bool state, Vector2? pos = null)
    {
        if (state && pos.HasValue)
        {
            Tower tower = AllTowers[selectedTowerIndex];

            rangeCircle.parent.position = pos.Value;

            rangeCircle.localScale = new Vector3(tower.range * 2, tower.range * 2, 1);
            rangeCircle.gameObject.SetActive(true);

            sizeCircle.localScale = new Vector3(tower.towerSize * 2, tower.towerSize * 2, 1);
            sizeCircle.gameObject.SetActive(true);

            return;
        }

        rangeCircle.gameObject.SetActive(false);
        sizeCircle.gameObject.SetActive(false);
    }

    public void StopPlacement()
    {
        isPlacing = false;
        ShowCirclesAtPosition(false);
        InterfaceController.Instance.SetBuyPanelState(true);
    }

    void PlaceTower(int index)
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0;

        if (TowerIsPlaceable(index, worldPosition))
        {
            var tower = Instantiate(towerPrefab, worldPosition, Quaternion.identity).GetComponent<TowerBehaviour>();

            tower.OnPlace(AllTowers[index]);

            var allTowers = GameObject.FindGameObjectsWithTag("Tower");

            foreach (GameObject obj in allTowers)
            {
                TowerBehaviour found = obj.GetComponent<TowerBehaviour>();
                found.SupportMultiplier = 1.0f;
            }

            foreach (GameObject obj in allTowers)
            {
                TowerBehaviour found = obj.GetComponent<TowerBehaviour>();
                found.UpdateTower();
            }

            AudioController.PlaySound(0);
            PlayerStats.Money -= AllTowers[selectedTowerIndex].Cost;
            StopPlacement();
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

        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, AllTowers[towerIndex].towerSize);

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
