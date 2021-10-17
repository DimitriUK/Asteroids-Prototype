using System;
using UnityEngine;

namespace Core.Gameplay
{
    public class AsteroidService : MonoBehaviour, IPooledProjectile
    {
        public Asteroids[] Asteroids;

        public void OnProjectileSpawn()
        {
            ResetAsteroidForNewGame();
        }

        private void ResetAsteroidForNewGame()
        {
            Asteroids[0].gameObject.SetActive(true);

            for (int i = 1; i < Asteroids.Length; i++)
            {
                Asteroids[i].gameObject.SetActive(false);
            }
        }
    }
}