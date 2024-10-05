using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

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

    public void OnHealthUpdates()
    {
        Debug.Log($"Health is {PlayerStats.Health}");

        if (PlayerStats.Health <= 0)
        {
            //Die
        }
    }
}
