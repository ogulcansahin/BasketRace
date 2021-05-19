using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int ballCount=5;

    public Image playerIndicator;
    public Slider playerProgressBar;
    public Slider enemyProgressBar;
    public TextMeshProUGUI scoreText;

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

        levelLength = playerFinish.transform.position.z - playerStart.transform.position.z;
        playerStartZ = playerStart.transform.position.z;
        enemyStartZ = enemyStart.transform.position.z;

        if (ballCount == 0) //Baþlangýçta topun yoksa karakterin elindeki basketbol topu görünmesin.
        {
            BasketballOfPlayer.SetActive(false);

        }
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



  
    
}
