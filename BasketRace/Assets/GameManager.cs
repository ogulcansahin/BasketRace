using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int ballCount=0;
    public TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    public void updateBallCount(int count)
    {
        ballCount += count;
        scoreText.text = "X " + ballCount;
    }
}
