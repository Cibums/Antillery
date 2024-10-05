using UnityEngine;

[CreateAssetMenu(menuName = "Enemy", fileName = "New Enemy")]
public class Enemy : ScriptableObject
{
    public Sprite Graphics;
    public string EnemyName;
    public int Health;
    public float Speed;
    public bool Flying;
    public bool HasProtectiveLayer;
}
