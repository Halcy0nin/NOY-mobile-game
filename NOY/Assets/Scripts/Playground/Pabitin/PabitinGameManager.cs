using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PabitinGameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private GameObject setPanel;
    [SerializeField] private TextMeshProUGUI setText;

    [Header("Gameplay Settings")]
    [SerializeField] private float collectionTime = 5f;
    [SerializeField] private float raiseDuration = 2f;
    [SerializeField] private float waitBeforeNextRound = 2f;
    [SerializeField] private int maxSets = 5;
    [SerializeField] private float raiseOffset = 10f;

    [Header("References")]
    [SerializeField] private List<Transform> ropeAnchors;
    [SerializeField] private PabitinPrizeSpawner spawner;
    [SerializeField] private SaveManager saveManager;

    [Header("Countdown")]
    [SerializeField] private GameObject startCountdownPanel;
    [SerializeField] private TextMeshProUGUI startCountdownText;

    private List<float> loweredYPositions = new List<float>();
    private List<float> raisedYPositions = new List<float>();

    private int score = 0;
    private int currentSet = 0;
    private float timer = 0f;
    private bool isCollecting = false;
    private int highScore = 0;

    public void InitializePabitinScore(int highScore)
    {
        this.highScore = highScore;
    }

    private void Awake()
    {
        CacheAnchorHeights();
    }

    private void Start()
    {
        PabitinScoreSaveData PabitinData = saveManager.LoadPabitinHighScore();
        if (PabitinData != null)
            InitializePabitinScore(PabitinData.pabitinHighScore);
        else
            highScoreText.text = "0";
    }

    private void CacheAnchorHeights()
    {
        loweredYPositions.Clear();
        raisedYPositions.Clear();

        foreach (Transform anchor in ropeAnchors)
        {
            float lowY = anchor.position.y;
            loweredYPositions.Add(lowY);
            raisedYPositions.Add(lowY + raiseOffset);
        }
    }

    private IEnumerator StartGameRoutine()
    {
        score = 0;
        currentSet = 0;
        isCollecting = false;

        UpdateScoreUI();
        gameOverPanel.SetActive(false);
        timerText.text = "Time: 0";

        // Show countdown UI
        startCountdownPanel.SetActive(true);

        float countdown = 3f;
        while (countdown > 0)
        {
            startCountdownText.text = $"Game starts in {Mathf.CeilToInt(countdown)}";
            countdown -= Time.deltaTime;
            yield return null;
        }

        startCountdownPanel.SetActive(false);

        MoveAnchorsInstantly(true); // ensure ropes are up before game starts
        yield return new WaitForSeconds(0.2f); // brief pause before first lowering

        StartCoroutine(HandleNextSet());
    }

    private void Update()
    {
        if (!isCollecting) return;

        timer -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.CeilToInt(timer).ToString();

        if (timer <= 0f)
        {
            isCollecting = false;
            StartCoroutine(HandleSetTransition());
        }
    }

    private IEnumerator HandleNextSet()
    {
        currentSet++;

        if (CheckForGameEnd()) yield break;

        spawner.CheckAndRefillPrizes();
        yield return StartCoroutine(MoveAnchorsSmoothly(false)); // Lower

        timer = collectionTime;
        isCollecting = true;
    }

    private IEnumerator HandleSetTransition()
    {
        // Show set panel immediately
        if (currentSet < maxSets)
            setPanel.SetActive(true);

        float remaining = waitBeforeNextRound;

        // Start raising ropes in background
        StartCoroutine(MoveAnchorsSmoothly(true));

        // Countdown on set panel
        while (remaining > 0f)
        {
            setText.text = $"Set {currentSet} complete! Next set coming in {remaining:F1} seconds.";
            remaining -= Time.deltaTime;
            yield return null;
        }

        setPanel.SetActive(false);

        StartCoroutine(HandleNextSet());
    }

    private IEnumerator MoveAnchorsSmoothly(bool raising)
    {
        float elapsed = 0f;
        float duration = raiseDuration;
        Vector3[] startPositions = new Vector3[ropeAnchors.Count];

        for (int i = 0; i < ropeAnchors.Count; i++)
            startPositions[i] = ropeAnchors[i].position;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            for (int i = 0; i < ropeAnchors.Count; i++)
            {
                float targetY = raising ? raisedYPositions[i] : loweredYPositions[i];
                Vector3 newPos = startPositions[i];
                newPos.y = Mathf.Lerp(startPositions[i].y, targetY, t);
                ropeAnchors[i].position = newPos;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < ropeAnchors.Count; i++)
        {
            Vector3 pos = ropeAnchors[i].position;
            pos.y = raising ? raisedYPositions[i] : loweredYPositions[i];
            ropeAnchors[i].position = pos;
        }
    }

    public void MoveAnchorsInstantly(bool raising)
    {
        for (int i = 0; i < ropeAnchors.Count; i++)
        {
            Vector3 pos = ropeAnchors[i].position;
            pos.y = raising ? raisedYPositions[i] : loweredYPositions[i];
            ropeAnchors[i].position = pos;
        }
    }

    private IEnumerator ShowSetCompleteMessage(int setNumber, float waitTime)
    {
        float remaining = waitTime;

        while (remaining > 0f)
        {
            setText.text = $"Set {setNumber} complete. Please wait for {remaining:F1} secs.";
            yield return null;
            remaining -= Time.deltaTime;
        }
    }

    private bool CheckForGameEnd()
    {
        if (currentSet > maxSets)
        {
            EndGame();
            return true;
        }
        return false;
    }

    public void AddScore(int amount)
    {
        if (!isCollecting) return;

        score += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }

    private void EndGame()
    {
        isCollecting = false;

        if (score > highScore)
        {
            highScore = score;
            SavePabitinHighScore();
        }

        finalScoreText.text = "" + score;
        highScoreText.text = "" + highScore;
        gameOverPanel.SetActive(true);
    }

    private void SavePabitinHighScore()
    {
        saveManager.SavePabitinHighScore(score);
    }
    public void OnStartButtonPressed()
    {
        StartCoroutine(StartGameRoutine());
    }

}
