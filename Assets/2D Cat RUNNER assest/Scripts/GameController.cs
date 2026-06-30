using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    #region Singleton
    public static GameController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }     
    }
    #endregion
    [Header("GameStats")]
    [SerializeField] GameObject player;
    [Header("GameStats")]
    [SerializeField] public float CurrentScore;
    [SerializeField] public bool isPlaying = false;


    [Header("GameUI")]
    [SerializeField] private GameObject StartGameUI;
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private TextMeshProUGUI EndScore;


    private void Start()
    {
  
    }

    private void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            isPlaying = true;
        }

        if (!isPlaying) { return; }

        CurrentScore += Time.deltaTime;//score
        UpdateScore();

    }

    public void StartGame()
    {
        player.SetActive(true);
        StartGameUI.SetActive(false);
        GameOverUI.SetActive(false);
        isPlaying = true;
    }

    public void OpenGameOverUI()
    {
        StartGameUI.SetActive(false);
        GameOverUI.SetActive(true);
    }


    public string PrettyScore()
    {
        return Mathf.RoundToInt(CurrentScore).ToString();
    }

    public void UpdateScore()
    {
        
        ScoreText.text = "Score : " + PrettyScore();
    }

    public void GameOver()
    {
        OpenGameOverUI();
        EndScore.text = "Score : " + PrettyScore();
        CurrentScore = 0;
        isPlaying = false;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        Time.timeScale = 1f;
    }


}
