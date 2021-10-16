using System.Collections.Generic;
using UnityEngine;

namespace Core.Utils
{
    public class ObjectPoolingService : MonoBehaviour
    {
        [HideInInspector] public GameService Game;
        public List<ProjectileData> pools;
        public Dictionary<ProjectileData, Queue<GameObject>> PoolDictionary;

        public void Initialize()
        {
            InitializeRequirements();
            InitializePool();
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

        public Queue<GameObject> GetProjectilesFromPool(ProjectileData tag)
        {
            if (!PoolDictionary.ContainsKey(tag))
                return null;

            return PoolDictionary[tag];          
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

        public void DespawnProjectilesFromPool()
        {
            foreach (var asteroid in PoolDictionary[pools[0]])
            {
                asteroid.SetActive(false);
            }
        }
    }
}
