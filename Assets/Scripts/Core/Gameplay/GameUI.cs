using UnityEngine;
using UnityEngine.UI;

namespace Core.Gameplay
{
    public class GameUI : MonoBehaviour
    {
        private GameService gameService;

        [SerializeField] private GameObject[] PlayerLivesImages = null;

        [Header("UI Text Score")]
        [SerializeField] private Text PlayerScore = null;
        [SerializeField] private Text PlayerFinalScore = null;

        [Header("UI Text Popups")]
        [SerializeField] private Text PressStartScreen = null;
        [SerializeField] private Text GameOverScreen = null;

        public void SetupUIForNewGame()
        {
            ResetLivesUI();
            SetStartGameScreen(false);
            SetGameOverScreen(false);
        }

        public void SetStartGameScreen(bool isActive)
        {
            if (isActive)
            {
                PressStartScreen.CrossFadeAlpha(1, 0.5f, true);
            }
            else
            {
                PressStartScreen.CrossFadeAlpha(0, 0.1f, true);
            }
        }

        public void SetGameOverScreen(bool isActive)
        {
            if (isActive)
            {
                GameOverScreen.CrossFadeAlpha(1, 0.5f, true);
            }
            else
            {
                GameOverScreen.CrossFadeAlpha(0, 0.1f, true);
            }
        }

        public void SetScoreTextToCurrentScore(int currentScore)
        {
            PlayerScore.text = currentScore.ToString();
        }

        public void SetFinalScoreTextToCurrentScore(int currentScore)
        {
            PlayerFinalScore.text = currentScore.ToString();
        }

        public void SetupTextUIForNewGame()
        {
            SetScoreTextToCurrentScore(gameService.GameScore.TotalScore);
            SetFinalScoreTextToCurrentScore(gameService.GameScore.TotalScore);

            SetStartGameScreen(false);
            SetGameOverScreen(false);

            ResetLivesUI();
        }

        private void ResetLivesUI()
        {
            for (int i = 0; i < PlayerLivesImages.Length; i++)
            {
                PlayerLivesImages[i].SetActive(true);
            }
        }

        private void Awake()
        {
            InitializeDependencies();
            InitializeUIScreens();
        }

        private void OnEnable()
        {
            Actions.OnLiveLost += RemoveLiveCounterUI;
        }
        private void OnDisable()
        {
            Actions.OnLiveLost -= RemoveLiveCounterUI;
        }

        private void RemoveLiveCounterUI()
        {
            for (int i = PlayerLivesImages.Length - 1; i > -1; i--)
            {
                if (PlayerLivesImages[i].activeSelf)
                {
                    PlayerLivesImages[i].SetActive(false);
                    break;
                }
            }
        }

        private void InitializeDependencies()
        {
            gameService = GetComponent<GameService>();
        }

        private void InitializeUIScreens()
        {
            PressStartScreen.gameObject.SetActive(true);
            GameOverScreen.gameObject.SetActive(true);

            PressStartScreen.CrossFadeAlpha(1, 1, true);
            GameOverScreen.CrossFadeAlpha(0, 1, true);
        }
    }
}
