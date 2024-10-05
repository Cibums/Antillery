using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "Tower")]
public class Tower : ScriptableObject
{
    public string towerName;
    public Sprite sprite;
    public float radius;
    public ITowerAbility towerAbility;
}
