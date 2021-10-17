using UnityEngine;

namespace Core.Gameplay
{
    public class GameScore : MonoBehaviour
    {
        public int TotalScore { get; set; }

        private const int BIG_ASTEROID_POINTS_VALUE = 20;
        private const int MEDIUM_ASTEROID_POINTS_VALUE = 50;
        private const int SMALL_ASTEROID_POINTS_VALUE = 100;

        private GameService gameService;

        public void ResetTotalScoreToDefault()
        {
            TotalScore = 0;
        }

        private void Awake()
        {
            Initialize();
        }

        private void OnEnable()
        {
            Actions.OnAsteroidDestroyed += AddPoints;
        }
        private void OnDisable()
        {
            Actions.OnAsteroidDestroyed -= AddPoints;
        }

        private void Initialize()
        {
            gameService = GetComponent<GameService>();
            TotalScore = 0;
        }

        private int GetPointsValueForAsteroid(Asteroids.AsteroidSize asteroid)
        {
            if (asteroid == Asteroids.AsteroidSize.AsteroidBig)
                return BIG_ASTEROID_POINTS_VALUE;
            else if (asteroid == Asteroids.AsteroidSize.AsteroidMedium)
                return MEDIUM_ASTEROID_POINTS_VALUE;
            else return SMALL_ASTEROID_POINTS_VALUE;
        }

        private void AddPoints(Asteroids.AsteroidSize asteroid)
        {
            var pointsValue = GetPointsValueForAsteroid(asteroid);

            TotalScore = TotalScore + pointsValue;
            gameService.GameUI.SetScoreTextToCurrentScore(TotalScore);
        }
    }
}
