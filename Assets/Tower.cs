using UnityEngine;

public class Tower : ScriptableObject
{
    public string towerName;
    public float range;
    public Sprite sprite;
    public float towerSize;
    public int Cost;
}

[CreateAssetMenu(fileName = "New Projectile Tower", menuName = "Tower/Projectile")]
public class ProjectileTower : Tower
{
    public float shootingSpeed;
    public float attackDamage;
}

[CreateAssetMenu(fileName = "New Supportive Tower", menuName = "Tower/Supportive")]
public class SupportiveTower : Tower
{
    public float supportRate;
}
