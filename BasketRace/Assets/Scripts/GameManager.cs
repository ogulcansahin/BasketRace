using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private int ballCount = 5;
    public int score = 100;

    public Image playerIndicator;
    public Slider playerProgressBar;
    public Slider enemyProgressBar;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI totalScoreText;
    public TextMeshProUGUI levelText;

    private GameObject BasketballOfPlayer;
    private GameObject playerStart;
    private GameObject playerFinish;
    private GameObject enemyStart;
    private GameObject enemyFinish;

    private GameObject mainPlayer;
    private GameObject enemyPlayer;
    private float playerLevelPercentage = 0;
    private float enemyLevelPercentage = 0;
    private float levelLength;
    private float playerStartZ;
    private float enemyStartZ;

    private Canvas TapToPlayCanvas;
    private Canvas LevelCompletedCanvas;
    private Canvas GameOverCanvas;

    private Animator[] MainPlayerAnimator;
    private Animator[] enemyPlayerAnimator;
    private MainPlayerController mainPlayerScript;
    private EnemyController enemyPlayerScript;
    private DragAndShoot basketscript;
    private MultiplierBasket multiplierBasket;

    private bool isGameStarted = false;
    SetLevelToCanvas scriptOfSetLevelToCanvas;

    // Start is called before the first frame update
    private void Start()
    {
  
        scoreText.text = ballCount.ToString();

        BasketballOfPlayer = GameObject.FindWithTag("BasketballOfPlayer");
        mainPlayer = GameObject.FindWithTag("MainPlayer");
        enemyPlayer = GameObject.FindWithTag("EnemyPlayer");
        playerStart = GameObject.FindWithTag("PlayerStart");
        playerFinish = GameObject.FindWithTag("PlayerFinish");
        enemyStart = GameObject.FindWithTag("EnemyStart");
        enemyFinish = GameObject.FindWithTag("EnemyFinish");
        mainPlayerScript = mainPlayer.GetComponent<MainPlayerController>();
        enemyPlayerScript = enemyPlayer.GetComponent<EnemyController>();
        basketscript = GameObject.FindWithTag("BasketballOfPlayer").GetComponent<DragAndShoot>();

        TapToPlayCanvas = GameObject.FindWithTag("TapToPlayCanvas").GetComponent<Canvas>();
        LevelCompletedCanvas = GameObject.FindWithTag("LevelCompletedCanvas").GetComponent<Canvas>();
        GameOverCanvas = GameObject.FindWithTag("GameOverCanvas").GetComponent<Canvas>();

        //Canvaslar görünmesin ilk baþta, sadece tap to play canvas ý görünsün.
        GameOverCanvas.enabled = false;
        LevelCompletedCanvas.enabled = false;
        TapToPlayCanvas.enabled = true;

        levelLength = playerFinish.transform.position.z - playerStart.transform.position.z;
        playerStartZ = playerStart.transform.position.z;
        enemyStartZ = enemyStart.transform.position.z;

        MainPlayerAnimator = GameObject.FindWithTag("MainPlayer").GetComponentsInChildren<Animator>();
        enemyPlayerAnimator = GameObject.FindWithTag("EnemyPlayer").GetComponentsInChildren<Animator>();

        if (ballCount == 0) //Baþlangýçta topun yoksa karakterin elindeki basketbol topu görünmesin.
        {
            BasketballOfPlayer.SetActive(false);

        }

        //Bölüm kazanýldýðýnda canvasta current scene kazanýldý yazar
        scriptOfSetLevelToCanvas = GameObject.FindWithTag("LevelCompletedCanvas").GetComponentInChildren<SetLevelToCanvas>();
        scriptOfSetLevelToCanvas.SetLevel(SceneManager.GetActiveScene().buildIndex + 1);

        Time.timeScale = 1;
    }


    private void FixedUpdate()
    {

        playerLevelPercentage = ((mainPlayer.transform.position.z - playerStartZ) /levelLength);
        playerProgressBar.value = playerLevelPercentage;

        enemyLevelPercentage = ((enemyPlayer.transform.position.z - enemyStartZ) / levelLength);
        enemyProgressBar.value = enemyLevelPercentage;
    }

    public void updateBallCount(int count)
    {
        ballCount += count;

        if (ballCount <= 0)
        {
            BasketballOfPlayer.SetActive(false);
            ballCount = 0;
            scoreText.text = ballCount.ToString();

        }

        else
        {
            if (!BasketballOfPlayer.activeSelf)
            {
                
                BasketballOfPlayer.SetActive(true);
               
            }

            scoreText.text = ballCount.ToString();
            
        }
    }

    public int getBallCount()
    {
        return ballCount;
    }

    public bool GetisGameStarted()
    {
        return isGameStarted;
    }

    public void SetisGameStarted(bool gameStarted)
    {
        isGameStarted = gameStarted;
        if(isGameStarted == true)
        {
            DisableTapToPlay();
            MainPlayerAnimator[0].SetTrigger("RunCondition");
            MainPlayerAnimator[1].SetTrigger("StartDripling");
            enemyPlayerAnimator[0].SetTrigger("RunCondition");
            enemyPlayerAnimator[1].SetTrigger("StartDripling");
        }
    }

    public void DisableTapToPlay()
    {
        TapToPlayCanvas.enabled = false;
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        GameOverCanvas.enabled = true;
        isGameStarted = false;
    }

    public void LevelFinished()
    {
        enemyPlayerAnimator[0].SetTrigger("StopTrigger");
        enemyPlayerAnimator[1].SetTrigger("StopDripling");
        MainPlayerAnimator[1].SetTrigger("StopDripling");
        MainPlayerAnimator[0].SetBool("Stop", true);
        enemyPlayerAnimator[0].SetBool("Stop", true);
        isGameStarted = false;
    }

    public void LevelCompleted()
    {
        MainPlayerAnimator[0].SetTrigger("FinishCondition");
        LevelCompletedCanvas.enabled = true;
        BasketballOfPlayer.SetActive(false);
        totalScoreText.text = "TOTAL SCORE: " + score;
        levelText.text = "LEVEL " + (SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void loadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void loadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
