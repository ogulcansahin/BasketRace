using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplierBasket : MonoBehaviour
{
    private ParticleSystem basketEffect;
    private GameManager gameManager;
    private MainPlayerController mainPlayerController;
    public bool multiplierIsOver = false;
    // Start is called before the first frame update
    void Start()
    {
        basketEffect = gameObject.GetComponentInChildren<ParticleSystem>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        mainPlayerController = GameObject.FindWithTag("MainPlayer").GetComponent<MainPlayerController>();
    }
    private void Update()
    {
        if(gameManager.getBallCount() == 0)
        {
            gameManager.LevelCompleted();
        }
    }
    private void OnTriggerEnter(Collider other)
    {   
        basketEffect.Play();
        multiplierIsOver = true;
        if(gameObject.tag == "Multiplier_2")
        {
            gameManager.score *= (gameManager.getBallCount() + 1) * 2;
        }
        else if (gameObject.tag == "Multiplier_3")
        {
            gameManager.score *= (gameManager.getBallCount() + 1) * 3;
        }
        else if (gameObject.tag == "Multiplier_5")
        {
            gameManager.score *= (gameManager.getBallCount() + 1) * 5;
        }

        gameManager.LevelCompleted();
        
    }
}
