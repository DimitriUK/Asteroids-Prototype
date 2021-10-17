using UnityEngine;
using Core.Pooling;

namespace Core.Gameplay
{
    public class Gun : MonoBehaviour
    {
        public bool CanFire = false;

        public float nextShot = 0f;
        public float fireRate = 0.1f;

        private ObjectPoolingService objectPoolingService;

        [SerializeField] private GameObject playerGunBarrel = null;

        public void EnableGun()
        {
            CanFire = true;
        }

        public void DisableGun()
        {
            CanFire = false;
        }

        public void CheckTriggerAndFireGun()
        {
            if (!CanFire)
                return;

            objectPoolingService.SpawnProjectileFromPool(objectPoolingService.pools[1], playerGunBarrel.transform.position, playerGunBarrel.transform.rotation);
            Actions.OnBulletFired?.Invoke();
        }

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            objectPoolingService = FindObjectOfType<ObjectPoolingService>();
        }
    }
}