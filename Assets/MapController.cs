using UnityEngine;

public class MapController : MonoBehaviour
{
    public Map[] gameMaps;

    public static Map currentMap;

    public GameObject pathPrefab;

    private void Start()
    {
        Screen.SetResolution(1920, 1080, true);

        currentMap = GetRandomMap();

        GenerateMapPath(currentMap);

        EnemyController.instance.StartNextWave();
    }

    private Map GetRandomMap()
    {
        if (gameMaps.Length == 0)
        {
            Debug.LogError("No maps available!");
            return null;
        }

        int randomIndex = Random.Range(0, gameMaps.Length);

        return gameMaps[randomIndex];
    }

    public void GenerateMapPath(Map map)
    {
        Vector2Int[] mapPath = map.GetFullMapPath();

        foreach (Vector2Int element in mapPath)
        {
            Instantiate(pathPrefab, new Vector3(element.x, element.y, 0), Quaternion.identity);
        }
    }
}
