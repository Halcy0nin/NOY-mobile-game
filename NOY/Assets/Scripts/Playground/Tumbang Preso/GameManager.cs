using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private SaveManager saveManager;
    public GameObject slipperPrefab;
    public Transform spawnPoint;
    public GameObject gameOverPanel;
    public TMP_Text scoreText;
    public TMP_Text finalScoreText;
    public TMP_Text highScoreText;
    [SerializeField] private Transform slipperParent;
    private int score = 0;
    private int highScore = 0;
    private GameObject slipper;
    public Image forceBar;
    public Image directionArrow;

    public void InitializeScore(int highScore)
    {
        this.highScore = highScore;
    }
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {   
        ScoreSaveData data = saveManager.LoadHighScore();
        if (data != null)
        {
            InitializeScore(data.highScore);
        }
        if (forceBar != null)
        forceBar.fillAmount = 0f; // Reset force bar at the start
        gameOverPanel.SetActive(false);
        UpdateScoreText();
    }

    public void SpawnSlipper()
    {
        slipper = Instantiate(slipperPrefab, spawnPoint.position, Quaternion.identity, slipperParent);
        slipper.name = "Slipper";
        SlipperThrow2D slipperScript = slipper.GetComponent<SlipperThrow2D>();
        slipperScript.SetUIReferences(directionArrow.transform, forceBar);
    }

    public void RegisterHit()
    {
        score++;
        UpdateScoreText();
        SpawnSlipper();
    }

    public void RegisterMiss()
    {
        if (score > highScore)
        {
            highScore = score;
            SaveHighScore();
        }

        finalScoreText.text = "Score: " + score;
        highScoreText.text = "High Score: " + highScore;
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        score = 0;
        gameOverPanel.SetActive(false);
        if (forceBar != null)
        forceBar.fillAmount = 0f; // Reset force bar at the start
        UpdateScoreText();

        // Destroy existing slipper if one exists
        GameObject existingSlipper = GameObject.Find("Slipper");
        if (existingSlipper != null)
        {
            Destroy(existingSlipper);
        }

        SpawnSlipper();
    }

    public void CloseGame()
    {
        score = 0;
        gameOverPanel.SetActive(false);
        UpdateScoreText();

        // Destroy existing slipper if one exists
        GameObject existingSlipper = GameObject.Find("Slipper");
        if (existingSlipper != null)
        {
            Destroy(existingSlipper);
        }
    }

    void UpdateScoreText()
    {  
        scoreText.text = "Score: " + score;
    }


    void SaveHighScore()
    {
        saveManager.SaveHighScore(score);
        
    }

    [System.Serializable]
    public class SaveData
    {
        public int highScore;
    }
}
