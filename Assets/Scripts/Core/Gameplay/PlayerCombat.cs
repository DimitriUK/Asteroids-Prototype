using UnityEngine;
using Core.Utils;

public class PlayerCombat : MonoBehaviour
{
    private ObjectPoolingService objectPoolingService;

    [SerializeField] private GameObject PlayerGunBarrel;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        objectPoolingService = FindObjectOfType<ObjectPoolingService>();
    }

    public void FireProjectile()
    {
        objectPoolingService.SpawnProjectileFromPool(objectPoolingService.pools[1], PlayerGunBarrel.transform.position, PlayerGunBarrel.transform.rotation);
    }
}
