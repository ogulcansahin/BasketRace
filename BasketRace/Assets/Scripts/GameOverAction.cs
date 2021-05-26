using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverAction : MonoBehaviour
{
    Button gameOverButton;
    private GameManager gameManager;
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        gameOverButton = gameObject.GetComponentInChildren<Button>();
        gameOverButton.onClick.AddListener(OnRetryButtonTapped);
    }

    // Update is called once per frame

    public void OnRetryButtonTapped()
    {
        gameManager.loadCurrentScene();
    }
}
