using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int ballCount=10;
    public TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    private void Start()
    {
        scoreText.text = ballCount.ToString();
    }
    public void updateBallCount(int count)
    {
        ballCount += count;
        scoreText.text = ballCount.ToString();
    }
}
