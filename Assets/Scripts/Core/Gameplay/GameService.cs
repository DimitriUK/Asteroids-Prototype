using Core.Audio;
using Core.Controls;
using Core.Pooling;
using UnityEngine;

namespace Core.Gameplay
{
    public class GameService : MonoBehaviour
    {
        [Header("Dependencies")]
        [HideInInspector] public GameRoundsService GameRoundsService;
        [HideInInspector] public GameScore GameScore;
        [HideInInspector] public GameUI GameUI;

        [HideInInspector] public AudioService AudioService;
     
        [HideInInspector] public ObjectPoolingService ObjectPoolingService;
        [HideInInspector] public InputService InputService;

        public int GetCurrentAsteroidsInRound => asteroidsCurrentAmount;
        public int GetPlayerLivesLeftInRound => playerLivesLeft;

        [Header("Spawnable Player Prefab")]
        [SerializeField] private GameObject PlayerPrefab = null;

        private Player player;

        private bool isGameActive;

        private int playerLivesLeft;
        private int playerTargetScore;
        private int asteroidsCurrentAmount = 0;

        public void StartNewGame()
        {
            if (isGameActive)
                return;

            StartGameWithNewGameConditions();
        }

        private void Awake()
        {
            InitialiseGame();
        }

        private void InitialiseGame()
        {
            InitialiseDependencies();
            InitialiseStartingConditions();
            InitialisePoolService();
        }

        private void InitialiseDependencies()
        {
            InputService = GetComponent<InputService>();
            GameRoundsService = GetComponent<GameRoundsService>();
            GameScore = GetComponent<GameScore>();
            AudioService = FindObjectOfType<AudioService>();
            GameUI = GetComponent<GameUI>();
            ObjectPoolingService = GetComponent<ObjectPoolingService>();
        }

        private void InitialiseStartingConditions()
        {
            GameRoundsService.SetRoundToNewGame();
            playerLivesLeft = GameConstants.PLAYER_LIVES_DEFAULT;
            playerTargetScore = (int)GameRoundsService.GetCurrentRound() * GameConstants.TOTAL_ASTEROIDS_PER_GROUP;
        }

        private void InitialisePlayer()
        {
            if (player == null)
            {
                player = Instantiate(PlayerPrefab).GetComponent<Player>();
                InputService.InitialisePlayer();
            }
        }

        private void InitialisePoolService()
        {
            ObjectPoolingService.Initialise();
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

        private void StartGameWithNewGameConditions()
        {
            InitialiseStartingConditions();
            InitialisePlayer();
            SetGameActiveAndPlayerCanSpawn();
            TogglePlayer(true, true);
            GameRoundsService.SetupRoundAndStartRound((int)GameRoundsService.GetCurrentRound());
            SetupScoreAndUIForNewGame();
            SetupAudioForNewGame();
            player.ActivateRespawnShield();
        }

        private void SetupScoreAndUIForNewGame()
        {
            GameScore.ResetTotalScoreToDefault();
            GameUI.SetupTextUIForNewGame();
        }

        private void SetupAudioForNewGame()
        {
            AudioService.music.SetupMusicTimeDelay(playerTargetScore);
            AudioService.music.StopMusic();
            AudioService.StartMusic();
        }

        private void SetGameActiveAndPlayerCanSpawn()
        {
            isGameActive = true;
            player.CanSpawn = true;
        }

        public void DestroyTargetObjectiveCounter(Asteroids.AsteroidSize asteroidSize)
        {
            playerTargetScore--;

            if (playerTargetScore == 0)
                SetupNextRoundConditions();
        }
        private void RemovePlayerLive()
        {
            playerLivesLeft--;

            if (playerLivesLeft == 0)
                SetupGameOver();
        }

        private void SetupNextRoundConditions()
        {
            ObjectPoolingService.DespawnAsteroidsFromPool();
            ObjectPoolingService.DespawnBulletsFromPool();

            GameRoundsService.GameRounds nextRound = GameRoundsService.GetNextRound();
            GameRoundsService.CurrentLevel = nextRound;

            playerTargetScore = (int)nextRound * GameConstants.TOTAL_ASTEROIDS_PER_GROUP;
            GameRoundsService.SetupRoundAndStartRound((int)nextRound);

            player.ActivateRespawnShield();

            SetupAudioForNewGame();
        }

        private void SetupGameOver()
        {
            isGameActive = false;
            TogglePlayer(false, false);
            GameUI.SetGameOverScreen(true);
            GameUI.SetStartGameScreen(true);
            GameUI.SetFinalScoreTextToCurrentScore(GameScore.TotalScore);
            AudioService.music.StopMusic();
            ObjectPoolingService.DespawnAsteroidsFromPool();
            ObjectPoolingService.DespawnBulletsFromPool();
        }

        private void TogglePlayer(bool isPlayerActive, bool isGameStarted)
        {
            if (!isGameStarted)
            {
                player.CanSpawn = false;
                return;
            }
            else
            {
                player.CanSpawn = true;
            }

            if (isPlayerActive)
                player.ActivatePlayer();
            else player.DeactivatePlayer();
        }
    }
}