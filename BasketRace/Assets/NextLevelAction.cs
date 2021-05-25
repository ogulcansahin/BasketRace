using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextLevelAction : MonoBehaviour
{
    Button levelCompletedButton;
    private GameManager gameManager;
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        levelCompletedButton = gameObject.GetComponentInChildren<Button>();
        levelCompletedButton.onClick.AddListener(OnNextLevelButtonTap);
    }

    // Update is called once per frame

    public void OnNextLevelButtonTap()
    {
        gameManager.loadNextScene();
    }
}
