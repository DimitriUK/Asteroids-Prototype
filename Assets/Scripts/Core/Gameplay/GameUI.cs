using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject[] PlayerLivesImages;

    [SerializeField] private Text PlayerScore;
    [SerializeField] private Text PlayerFinalScore;

    [Header("UI Text Popups")]
    [SerializeField] private Text PressStartScreen;
    [SerializeField] private Text GameOverScreen;

    private void Awake()
    {
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

    public void ResetLivesUI()
    {
        for (int i = 0; i < PlayerLivesImages.Length; i++)
        {
            PlayerLivesImages[i].SetActive(true);
        }
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

    private void InitializeUIScreens()
    {
        PressStartScreen.gameObject.SetActive(true);
        GameOverScreen.gameObject.SetActive(true);

        PressStartScreen.CrossFadeAlpha(1, 1, true);
        GameOverScreen.CrossFadeAlpha(0, 1, true);
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
}
