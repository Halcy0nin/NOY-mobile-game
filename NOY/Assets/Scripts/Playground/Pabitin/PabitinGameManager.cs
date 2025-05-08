using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PabitinGameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private Button retryButton;

    [Header("Gameplay")]
    [SerializeField] private float gameDuration = 30f;
    [SerializeField] private PabitinPrizeSpawner spawner;

    private float remainingTime;
    private int score;
    private bool isGameActive;
    public SaveManager saveManager;

    void Start()
    {
        retryButton.onClick.AddListener(OnRetryButtonPressed);
        StartGame();
    }

    private void StartGame()
    {
        score = 0;
        remainingTime = gameDuration;
        isGameActive = true;
        gameOverPanel.SetActive(false);
        UpdateScoreText();
        spawner.SpawnAllPrizes(this);
    }

    void Update()
    {
        if (!isGameActive) return;

        remainingTime -= Time.deltaTime;
        timerText.text = Mathf.CeilToInt(remainingTime).ToString();

        if (remainingTime <= 0)
        {
            EndGame();
        }
    }

    public void AddScore(int amount)
    {
        if (!isGameActive) return;
        score += amount;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    private void EndGame()
    {
        isGameActive = false;
        finalScoreText.text = "Score: " + score;

        int highScore = saveManager?.LoadPabitinHighScore().pabitinHighScore ?? 0;
        if (score > highScore)
        {
            highScore = score;
            saveManager?.SavePabitinHighScore(highScore);
        }

        highScoreText.text = "High Score: " + highScore;
        gameOverPanel.SetActive(true);
    }

    public void OnRetryButtonPressed()
    {
        StartGame();
    }
}
