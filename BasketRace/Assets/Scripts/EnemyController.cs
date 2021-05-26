using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    GameObject enemyPlayer;
    GameObject road;
    float road_x;
    bool IsRunning = true;
    bool IsPowerUpRun = false;
    bool isRight = true;
    bool IsImpact = false;
    private GameManager gameManager;
    private Animator[] enemyPlayerAnimator;
    private ParticleSystem[] powerUpRunParticle;
    private int dieCondition = 0;

    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        enemyPlayer = GameObject.FindWithTag("EnemyPlayer");
        road = GameObject.FindWithTag("EnemyFloor");
        road_x = (road.transform.position - (road.transform.localScale * 0.235f)).x;
        enemyPlayerAnimator = gameObject.GetComponentsInChildren<Animator>();
        powerUpRunParticle = gameObject.GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (gameManager.GetisGameStarted() == true)
        {
            if (IsRunning && IsPowerUpRun != true && !IsImpact)
            {
                run();
            }

            if (IsPowerUpRun && !IsImpact)
            {
                StartCoroutine(PowerUpRun());
            }

            if (IsImpact)
            {
                StartCoroutine(Impact());
            }
        }

    }
    private void run()
    {
        transform.Translate(transform.forward * Time.deltaTime * 0.5f);
    }

    private void ChangeLine()
    {
        StartCoroutine(LaneChangingRouitne());

    }

    IEnumerator LaneChangingRouitne()
    {
        if (isRight)
        {
            while (transform.position.x > road_x)
            {
                transform.Translate(transform.right * -1 * Time.deltaTime * 2.9f);
                yield return new WaitForSeconds(0.005f);
            }
            road_x = (road.transform.position + (road.transform.localScale * 0.235f)).x;
            isRight = false;
        }

        else if (!isRight)
        {
            while (transform.position.x < road_x)
            {
                transform.Translate(transform.right * Time.deltaTime * 2.9f);
                yield return new WaitForSeconds(0.005f);
            }
            road_x = (road.transform.position - (road.transform.localScale * 0.235f)).x;
            isRight = true;
        }

    }

    private IEnumerator PowerUpRun()
    {
        enemyPlayerAnimator[0].SetFloat("RunSpeed", 2f);
        enemyPlayerAnimator[1].SetFloat("DriplingSpeed", 2f);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        transform.Translate(transform.forward * Time.deltaTime * 1.5f);
        if (!powerUpRunParticle[0].isPlaying)
        {
            powerUpRunParticle[0].Play();
        }
        yield return new WaitForSeconds(2.5f);
        powerUpRunParticle[0].Stop();
        gameObject.GetComponent<BoxCollider>().enabled = true;
        IsPowerUpRun = false;
        enemyPlayerAnimator[0].SetFloat("RunSpeed", 1f);
        enemyPlayerAnimator[1].SetFloat("DriplingSpeed", 1.5f);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Basketball_Skill")
        {
            gameManager.updateBallCount(1);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Box")
        {
            int chance = Random.Range(1, 6); //Eðer floatsa max deðer inclusive, deðilse max deðerin bir altý inclusive oluyormuþ.

            if (chance == 1 || chance == 2 || chance == 3)
            {
                int random = Random.Range(-2, 3);
                gameManager.updateBallCount(random);
                Destroy(collision.gameObject);
            }

            if (chance == 4) // Kutuya takýlsýn.
            {
                IsImpact = true;
                Destroy(collision.gameObject);
            }

            if (chance == 5) // Hýzlansýn, hiçbir þeye çarpmasýn ve 3 top toplasýn.
            {
                gameManager.updateBallCount(3);
                IsPowerUpRun = true;
                Destroy(collision.gameObject);
            }

        }
        if (collision.gameObject.tag == "Obstacle")
        {
            IsImpact = true;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "BasketballOfPlayer")
        {
            IsImpact = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyFinish")
        {
            gameManager.GameOver();
        }
    }

    public bool GetIsRunning()
    {
        return IsRunning;
    }

    public void SetIsRunning(bool IsRunning)
    {
        this.IsRunning = IsRunning;
    }

    private IEnumerator Impact()
    {
        if (dieCondition == 0)
            enemyPlayerAnimator[0].SetTrigger("DieCondition");
        dieCondition = 1;
        enemyPlayerAnimator[0].SetFloat("RunSpeed", 0f);
        enemyPlayerAnimator[1].SetFloat("DriplingSpeed", 0f);
        if (!powerUpRunParticle[1].isPlaying)
        {
            powerUpRunParticle[1].Play();
        }
        transform.Translate(transform.forward * -1 * Time.deltaTime * 0.2f);
        yield return new WaitForSeconds(1f);
        powerUpRunParticle[1].Stop();
        dieCondition = 0;
        IsImpact = false;
        enemyPlayerAnimator[0].SetFloat("RunSpeed", 1f);
        enemyPlayerAnimator[1].SetFloat("DriplingSpeed", 1.5f);


    }
}
