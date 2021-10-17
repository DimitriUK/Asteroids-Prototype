﻿using System;
using System.Text;
using UnityEngine;

public class AsteroidService : MonoBehaviour, IPooledProjectile
{
    public Asteroids[] Asteroids;
    public Action OnLocalAsteroidDestroyed;

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