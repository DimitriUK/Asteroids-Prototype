using System;

public class Actions
{
    public static Action OnLiveLost;
    public static Action OnBulletFired;
    public static Action<Asteroids.AsteroidSize> OnAsteroidDestroyed;
}
