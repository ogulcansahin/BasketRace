using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int ballCount=50;

    public GameObject progressBarParticle;
    public Image playerIndicator;
    public Slider progressBar;
    public TextMeshProUGUI scoreText;

    private GameObject BasketballOfPlayer;
    private GameObject playerStart;
    private GameObject playerFinish;

    private GameObject mainPlayer;
    private float levelPercentage = 0;
    private float levelLength;
    private float startZ;

    // Start is called before the first frame update
    private void Start()
    {
  
        scoreText.text = ballCount.ToString();

        BasketballOfPlayer = GameObject.FindWithTag("BasketballOfPlayer");
        mainPlayer = GameObject.FindWithTag("MainPlayer");
        playerStart = GameObject.FindWithTag("PlayerStart");
        playerFinish = GameObject.FindWithTag("PlayerFinish");

        levelLength = playerFinish.transform.position.z - playerStart.transform.position.z;
        startZ = playerStart.transform.position.z;

        if (ballCount == 0) //Baþlangýçta topun yoksa karakterin elindeki basketbol topu görünmesin.
        {
            BasketballOfPlayer.SetActive(false);

        }
    }

    private void FixedUpdate()
    {
              
        levelPercentage = ((mainPlayer.transform.position.z - startZ) /levelLength);
        progressBar.value = levelPercentage;
           
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
