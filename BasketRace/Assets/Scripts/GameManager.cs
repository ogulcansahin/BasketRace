using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int ballCount=5;
    public TextMeshProUGUI scoreText;
    private GameObject BasketballOfPlayer;

    // Start is called before the first frame update
    private void Start()
    {
        
        scoreText.text = ballCount.ToString();
        BasketballOfPlayer = GameObject.FindWithTag("BasketballOfPlayer");

        if (ballCount == 0) //Baþlangýçta topun yoksa karakterin elindeki basketbol topu görünmesin.
        {
            BasketballOfPlayer.SetActive(false);

        }
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
