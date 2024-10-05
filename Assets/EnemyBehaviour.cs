using System.Linq;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private Enemy enemy;
    private Vector2Int[] path = null;
    private int currentTargetPositionIndex = 1;
    public float arrivalThreshold = 0.1f;

    public void StartEnemy(Enemy enemy)
    {
        this.enemy = enemy;
        path = MapController.currentMap.MapPath;
        transform.position = new Vector3(path[0].x, path[0].y, 0);
    }

    void Update()
    {
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        float step = enemy.Speed * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, path[currentTargetPositionIndex], step);

        if (Vector2.Distance(transform.position, path[currentTargetPositionIndex]) < arrivalThreshold)
        {
            OnReachTarget();
        }
    }

    private void OnReachTarget()
    {
        Debug.Log("Reached the target position!");

        if (currentTargetPositionIndex >= path.Count() - 1)
        {
            Debug.Log("Reached the last element in the path.");

            PlayerStats.Health--;

            GameController.instance.OnHealthUpdates();

            Destroy(gameObject);

            return;
        }

        currentTargetPositionIndex++;
    }
}
