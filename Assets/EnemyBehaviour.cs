using System.Linq;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private Enemy enemy;
    private Vector2Int[] path = null;
    private int currentTargetPositionIndex = 1;
    public float arrivalThreshold = 0.1f;
    public int Health = 100;

    private int speedMultiplier = 1;

    private Transform graphics;

    public void StartEnemy(Enemy enemy)
    {
        this.enemy = enemy;
        Health = enemy.Health;
        path = MapController.currentMap.MapPath;
        transform.position = new Vector3(path[0].x, path[0].y, 0);

        graphics = Instantiate(enemy.Graphics, transform).transform;
    }

    void Update()
    {
        UpdateGraphicsOrder();
        MoveTowardsTarget();
    }

    private void UpdateGraphicsOrder()
    {
        graphics.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1000 + Mathf.RoundToInt(transform.position.y * 10);
    }

    public void DamageEnemy()
    {
        Health--;
        PlayerStats.Money += 2;
        AudioController.PlaySound(2);
        OnEnemyHealthChanged();
    }

    private void MoveTowardsTarget()
    {
        float step = enemy.Speed * Time.deltaTime * speedMultiplier;

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

            PlayerStats.Health -= Health;
            GameController.instance.OnPlayerHealthChanges();

            EnemyController.instance.CheckIfWaveEnded();

            Destroy(gameObject);

            return;
        }

        currentTargetPositionIndex++;
    }

    public void OnEnemyHealthChanged()
    {
        Debug.Log($"Health changed to {Health} on {gameObject.name}");

        if (Health <= 0)
        {
            EnemyController.instance.CheckIfWaveEnded();
            AudioController.PlaySound(3);
            Destroy(gameObject); 
            return;
        }
    }
}
