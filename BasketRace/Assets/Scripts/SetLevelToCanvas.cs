using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SetLevelToCanvas : MonoBehaviour
{
    private string currentScene;
    private TextMeshProUGUI levelText;
    private int currentLevel;
    // Start is called before the first frame update
    void Start()
    {
        levelText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame

    public void SetLevel(int level)
    {
        currentLevel = level;
        currentScene = currentLevel.ToString();
        levelText.text = "Level " + currentScene;

    }
}
