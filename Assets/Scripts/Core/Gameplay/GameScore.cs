using UnityEngine;

public class GameScore : MonoBehaviour
{
    private GameService GameService;

    public int TotalScore { get; set; }

    private void Awake()
    {
        GameService = GetComponent<GameService>();
    }

    private void OnEnable()
    {
        Actions.OnAsteroidDestroyed += AddPoints;
    }
    private void OnDisable()
    {
        Actions.OnAsteroidDestroyed -= AddPoints;
    }

    public void Initialize()
    {
        TotalScore = 0;
    }

    public void AddPoints(AsteroidProjectile asteroidReference)
    {
        TotalScore = TotalScore + asteroidReference.pointsValue;
        GameService.GameUI.SetScoreTextToCurrentScore(TotalScore);
    }
}
