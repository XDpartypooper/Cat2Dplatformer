using System.Runtime.InteropServices;
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
    [Header("GamePlayer")]
    [SerializeField] private GameObject player;
    [SerializeField] private HighScore HS;
    [Header("GameStats")]
    [SerializeField] public float CurrentScore;
    [SerializeField] public bool isPlaying = false;


    [Header("GameUI")]
    [SerializeField] private GameObject StartGameUI;
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private TextMeshProUGUI EndScore;
    [SerializeField] private TextMeshProUGUI HighScore;

    [Header("Other")]
    [SerializeField] public AudioSource CatMeow;
    private SpawnerScript SS;
    private CatRunnerScript CatRunner;      
    

    private void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        CatRunner = FindAnyObjectByType<CatRunnerScript>();

        SS = FindAnyObjectByType<SpawnerScript>();
        string LoadedData = WebSaveFile.Load("Save");

        if (LoadedData != null)
        {
            HS = JsonUtility.FromJson<HighScore>(LoadedData);
        }
        else
        {
            HS = new HighScore();
        }
       
    }

    private void Update()
    {
 
        if (!isPlaying) { return; }

        CurrentScore += Time.deltaTime;//score
        UpdateScore();

    }

    public void StartGame()
    {
        
        CurrentScore = 0;
        player.SetActive(true);
        CatRunner.GetComponentInParent<Animator>().Play("CatRun");
        StartGameUI.SetActive(false);
        GameOverUI.SetActive(false);
        isPlaying = true;
    }

    public void CatWalkAnimtion()
    {
        player.SetActive(true);
        isPlaying = false;//doesnt do anything if not playing
        CatRunner.GetComponentInParent<Animator>().Play("CatWalk");//animator set when in main menu/start menu
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
        CatMeow.Play();
        SS.ClearSpawnedObstacles();
        OpenGameOverUI();
       

        if (HS.HIGHSCORE < CurrentScore)
        {
            HS.HIGHSCORE = CurrentScore;
            EndScore.text = "Score : " + PrettyScore();
            HighScore.text = "NEW High Score !! : " + PrettyScore();
            WebSaveFile.Save("Save", JsonUtility.ToJson(HS));
        }
        else
        {
            EndScore.text = "Score : " + PrettyScore();
            HighScore.text = "High Score : " + Mathf.RoundToInt(HS.HIGHSCORE);
        }

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
