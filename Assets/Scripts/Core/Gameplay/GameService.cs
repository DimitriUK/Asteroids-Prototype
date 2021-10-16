using UnityEngine;
using Core.Utils;

public enum GameRounds
{
    One = 5,
    Two = 7,
    Three = 9,
    Four = 12,
    Five = 15
}

public class GameService : MonoBehaviour
{
    [Header("Dependencies")]
    [HideInInspector] public GameScore GameScore;
    [HideInInspector] public GameMusic GameMusic;
    [HideInInspector] public GameUI GameUI;
    [HideInInspector] public ObjectPoolingService ObjectPoolingService;
    private Player player;

    [Header("Spawnable Player Prefab")]
    [SerializeField] private GameObject PlayerPrefab;

    public GameRounds CurrentLevel;

    private int PlayerLivesLeft;
    private int PlayerTargetScore;

    private int AsteroidsCurrentAmount;

    private bool isGameActive;

    private void Awake()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        InitializeDependencies();
        InitializeStartingConditions();
        InitializePoolService();
    }

    private void InitializeDependencies()
    {
        GameScore = GetComponent<GameScore>();
        GameMusic = FindObjectOfType<GameMusic>();
        GameUI = GetComponent<GameUI>();
        ObjectPoolingService = GetComponent<ObjectPoolingService>();
    }

    private void InitializeStartingConditions()
    {
        CurrentLevel = GameRounds.One;

        PlayerLivesLeft = GameConstants.PLAYER_LIVES_DEFAULT;
        PlayerTargetScore = (int)CurrentLevel * GameConstants.TOTAL_ASTEROIDS_PER_GROUP;
    }

    private void InitializePlayer()
    {
        player = Instantiate(PlayerPrefab).GetComponent<Player>();
    }

    private void InitializePoolService()
    {
        ObjectPoolingService.Initialize();
    }

    private void OnEnable()
    {
        Actions.OnAsteroidDestroyed += DestroyTargetObjectiveCounter;
        Actions.OnLiveLost += RemovePlayerLive;
    }
    private void OnDisable()
    {
        Actions.OnAsteroidDestroyed -= DestroyTargetObjectiveCounter;
        Actions.OnLiveLost -= RemovePlayerLive;
    }

    private void Update()
    {
        if (isGameActive)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetupPoolForNewGame();
            StartGameWithConditions();
        }
    }

    private void SetupPoolForNewGame()
    {
        ObjectPoolingService.DespawnProjectilesFromPool();
    }

    private void StartGameWithConditions()
    {
        InitializeStartingConditions();
        InitializePlayer();
        TogglePlayer(true, true);
        StartRound((int)CurrentLevel);
        SetupGameUIForNewGame();
        SetupScoreForNewGame();
        SetupAudioForNewGame();
    }

    public void StartRound(int asteroidsToSpawn)
    {
        for (int i = 0; i < asteroidsToSpawn; i++)
        {
            ObjectPoolingService.SpawnProjectileFromPool(ObjectPoolingService.pools[0], transform.position, Quaternion.identity);
        }
        SetGameActiveAndPlayerCanSpawn();
    }
    private void SetupGameUIForNewGame()
    {
        GameUI.ResetLivesUI();
        GameUI.SetStartGameScreen(false);
        GameUI.SetGameOverScreen(false);
    }

    private void SetupScoreForNewGame()
    {
        GameScore.TotalScore = 0;
        GameUI.SetScoreTextToCurrentScore(GameScore.TotalScore);
        GameUI.SetFinalScoreTextToCurrentScore(GameScore.TotalScore);
    }

    private void SetupAudioForNewGame()
    {
        GameMusic.SetupMusicTimeSubtractor(PlayerTargetScore);
        GameMusic.ResetMusic();
        GameMusic.StartMusicForNewGame();
    }

    private void SetGameActiveAndPlayerCanSpawn()
    {
        isGameActive = true;
        player.canSpawn = true;
    }

    public void DestroyTargetObjectiveCounter(AsteroidProjectile asteroidReference)
    {
        PlayerTargetScore--;

        if (PlayerTargetScore == 0)
            SetupNewRoundConditions();
    }
    private void RemovePlayerLive()
    {
        PlayerLivesLeft--;

        if (PlayerLivesLeft == 0)
            SetupGameOver();
    }

    private void SetupNewRoundConditions()
    {
        CurrentLevel = CurrentLevel.Next();
        PlayerTargetScore = (int)CurrentLevel * GameConstants.TOTAL_ASTEROIDS_PER_GROUP;
        SetupNextRound(CurrentLevel);
        SetupAudioForNewGame();
        SetupNextRoundMusic();
    }
    private void SetupNextRoundMusic()
    {
        SetupAudioForNewGame();
    }

    public void SetupNextRound(GameRounds nextRound)
    {
        StartRound((int)nextRound);
    }

    private void SetupGameOver()
    {
        isGameActive = false;

        Debug.Log("GAME OVER");

        TogglePlayer(false, false);
        GameUI.SetGameOverScreen(true);
        GameUI.SetStartGameScreen(true);
        GameUI.SetFinalScoreTextToCurrentScore(GameScore.TotalScore);
        GameMusic.StopMusic();

        ObjectPoolingService.DespawnProjectilesFromPool();
    }

    private void TogglePlayer(bool isPlayerActive, bool isGameStarted)
    {
        if (!isGameStarted)
        {
            player.canSpawn = false;
            Debug.Log("GAME OVER, REMOVE PLAYER");
            return;
        }
        else
        {
            player.canSpawn = true;
        }

        if (isPlayerActive)
            player.ActivatePlayer();
        else player.DeactivatePlayer();
    }
}