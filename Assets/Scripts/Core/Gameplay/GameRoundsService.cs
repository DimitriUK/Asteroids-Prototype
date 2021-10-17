using UnityEngine;

namespace Core.Gameplay {
    public class GameRoundsService : MonoBehaviour
    {
        public enum GameRounds
        {
            One = 5,
            Two = 7,
            Three = 9,
            Four = 12,
            Five = 15,
            Six = 17,
            Seven = 19,
            Eight = 20,
            Nine = 25,
            Ten = 30
        }

        public GameRounds CurrentLevel;

        private GameService gameService;     

        public void SetRoundToNewGame()
        {
            CurrentLevel = GameRounds.One;
        }

        public GameRounds GetCurrentRound()
        {
            return CurrentLevel;
        }

        public GameRounds GetNextRound()
        {
            return CurrentLevel.Next();
        }

        public void SetupRoundAndStartRound(int asteroidsToSpawn)
        {
            gameService.ObjectPoolingService.GetAsteroidsFromPoolAndSpawn(asteroidsToSpawn);
        }

        private void Awake()
        {
            Initialise();
        }

        private void Initialise()
        {
            gameService = GetComponent<GameService>();
        }
    }
}
