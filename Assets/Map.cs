using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map", menuName = "Map")]
public class Map : ScriptableObject
{
    public string MapName;
    public Vector2Int[] MapPath;
    public MapObstacle[] MapObstacles;

    private bool ValidateMapPath()
    {
        if (MapPath == null) return false;

        if (MapPath.GroupBy(v => v).Any(g => g.Count() > 1))
        {
            Debug.LogError("Map not valid: There cannot be duplicate elements ín the map's path");
            return false;
        }

        Vector2Int? previous = null;

        foreach (Vector2Int element in MapPath)
        {
            if (previous == null)
            {
                previous = element;
                continue;
            }

            if (element.x == previous?.x || element.y == previous?.y)
            {
                previous = element;
                continue;
            }

            Debug.LogError("Map not valid: One coordinate in each element has to be the same as the last one");

            previous = element;
            return false;
        }

        return true;
    }

    public Vector2Int[] GetFullMapPath()
    {
        if (!ValidateMapPath())
        {
            throw new System.Exception("Map is not valid!");
        }

        List<Vector2Int> newMapPath = new List<Vector2Int>();

        Vector2Int? previous = null;

        foreach (Vector2Int element in MapPath)
        {
            if (previous.HasValue)
            {
                if (element.x == previous.Value.x)
                {
                    int yDiff = element.y - previous.Value.y;
                    int yDiffAbs = Mathf.Abs(yDiff);
                    int direction = yDiff / yDiffAbs;

                    for (int yOffset = 1; yOffset < yDiffAbs; yOffset++)
                    {
                        int x = element.x;
                        int y = previous.Value.y + (yOffset * direction);

                        newMapPath.Add(new Vector2Int(x,y));
                    }
                }
                
                if (element.y == previous.Value.y)
                {
                    int xDiff = element.x - previous.Value.x;
                    int xDiffAbs = Mathf.Abs(xDiff);
                    int direction = xDiff / xDiffAbs;

                    for (int xOffset = 1; xOffset < xDiffAbs; xOffset++)
                    {
                        int y = element.y;
                        int x = previous.Value.x + (xOffset * direction);

                        newMapPath.Add(new Vector2Int(x, y));
                    }
                }
            }

            newMapPath.Add(element);
            previous = element;
        }

        return newMapPath.ToArray();
    }
}
