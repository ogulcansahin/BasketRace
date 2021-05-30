using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetCoinLevelCompleted : MonoBehaviour
{
    private TextMeshProUGUI scoreText;

    void Start()
    {
        scoreText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void SetCoin(int coin)
    {
        scoreText.text = "Reward " + coin.ToString();
    }
}
