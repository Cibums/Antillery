using UnityEngine;

public class Tower : ScriptableObject
{
    public string towerName;
    public Sprite sprite;
    public float towerSize;
}

[CreateAssetMenu(fileName = "New Projectile Tower", menuName = "Tower/Projectile")]
public class ProjectileTower : Tower
{
    public float shootingRange;
    public float shootingSpeed;
    public float attackDamage;
}
