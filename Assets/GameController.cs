using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public static bool isGameOver = false;

    void Start()
    {
        PlayerStats.Health = 100;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void OnPlayerHealthChanges()
    {
        Debug.Log($"Health is {PlayerStats.Health}");

        if (PlayerStats.Health <= 0)
        {
            isGameOver = true;
            InterfaceController.Instance.GameOver();
        }
    }
}
