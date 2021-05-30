using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SetLevelToCanvas : MonoBehaviour
{
    private string currentScene;
    private int currentLevel;
    TextMeshProUGUI levelText;

    private void Start()
    {
        levelText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void SetLevel(int level)
    {
        currentLevel = level;
        currentScene = currentLevel.ToString();
        levelText.text = "Level " + currentScene;

    }
}
