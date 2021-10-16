using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile", menuName = "Projectile Data/New Projectile", order = 51)]
public class ProjectileData : ScriptableObject
{
    public enum PoolTags
    {
        Asteroid,    
        Bullet
    }

    public PoolTags tag;
    public GameObject prefab;
    public int size;
}
