using System;
using System.Text;
using UnityEngine;

public class Asteroid : MonoBehaviour, IPooledProjectile
{
    public AsteroidProjectile[] Asteroids;
    public Action OnLocalAsteroidDestroyed;

    [HideInInspector] public PlayAudioClip audioLocal;

    private int AsteroidsLeft;

    private void Awake()
    {
        Initialization();
    }

    private void Initialization()
    {
        audioLocal = GetComponent<PlayAudioClip>();
        AsteroidsLeft = GetAsteroidsLength();   
    }

    private int GetAsteroidsLength()
    {
        return Asteroids.Length;
    }

    private void OnEnable()
    {
        OnLocalAsteroidDestroyed += CheckForRecyclingToPool;
    }
    private void OnDisable()
    {
        OnLocalAsteroidDestroyed -= CheckForRecyclingToPool;
    }

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

    private void CheckForRecyclingToPool()
    {
        AsteroidsLeft--;

        if (AsteroidsLeft == 0)
            gameObject.SetActive(false);
    }
}