using Core.Gameplay;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Pooling
{
    public class ObjectPoolingService : MonoBehaviour
    {
        [HideInInspector] public GameService Game;
        public List<ProjectileData> pools;
        public Dictionary<ProjectileData, Queue<GameObject>> PoolDictionary;

        public void Initialise()
        {
            InitializeRequirements();
            InitializePool();
        }

        public GameObject SpawnProjectileFromPool(ProjectileData tag, Vector3 position, Quaternion rotation)
        {
            if (!PoolDictionary.ContainsKey(tag))
                return null;

            GameObject projectileToSpawn = PoolDictionary[tag].Dequeue();

            projectileToSpawn.SetActive(true);
            projectileToSpawn.transform.position = position;
            projectileToSpawn.transform.rotation = rotation;

            IPooledProjectile pooledProjectile = projectileToSpawn.GetComponent<IPooledProjectile>();

            if (pooledProjectile != null)
            {
                pooledProjectile.OnProjectileSpawn();
            }

            PoolDictionary[tag].Enqueue(projectileToSpawn);

            return projectileToSpawn;
        }

        public void DespawnAsteroidsFromPool()
        {
            foreach (var asteroid in PoolDictionary[pools[0]])
            {
                asteroid.SetActive(false);
            }
        }

        public void DespawnBulletsFromPool()
        {
            foreach (var bullet in PoolDictionary[pools[1]])
            {
                bullet.SetActive(false);
            }
        }

        public void GetAsteroidsFromPoolAndSpawn(int asteroidsToSpawn)
        {
            for (int i = 0; i < asteroidsToSpawn; i++)
            {
                SpawnProjectileFromPool(pools[0], transform.position, Quaternion.identity);
            }
        }

        private void InitializeRequirements()
        {
            Game = GetComponent<GameService>();
        }

        private void InitializePool()
        {
            PoolDictionary = new Dictionary<ProjectileData, Queue<GameObject>>();

            foreach (ProjectileData pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject objToSpawn = Instantiate(pool.prefab);
                    objToSpawn.SetActive(false);
                    objectPool.Enqueue(objToSpawn);
                }

                PoolDictionary.Add(pool, objectPool);
            }
        }
    }
}
