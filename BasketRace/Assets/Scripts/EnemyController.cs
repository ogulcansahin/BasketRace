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
    int impactChance;
    public GameObject basketballForShooting;
    private Vector3 shootVector;
    public static EnemyController Instance;
    RaycastHit stageHit;
    RaycastHit stageHitMainPlayer;
    Vector3 startPositionOfBall;
    Vector3 endPositionOfBall;
    bool isBallSpawned = true;
    float speed = .5f;
    AudioSource sound;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        enemyPlayer = GameObject.FindWithTag("EnemyPlayer");
        road = GameObject.FindWithTag("EnemyFloor");
        road_x = (road.transform.position - (road.transform.localScale * 0.235f)).x;
        enemyPlayerAnimator = gameObject.GetComponentsInChildren<Animator>();
        powerUpRunParticle = gameObject.GetComponentsInChildren<ParticleSystem>();
        sound = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        
        if (gameManager.GetisGameStarted() == true)
        {
            ShootBalltoMainPlayerMovingWay();
            ShoottoMovingway();
            ShootBall();
            ShootBalltoMainPlayerDoor();
            ControlSwitchLane();
            if (IsRunning && IsPowerUpRun != true && !IsImpact)
            {
                SwitchLane();
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
        transform.Translate(transform.forward * Time.deltaTime * speed);
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
        yield return new WaitForSeconds(1f);
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
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Box")
        {
            int chance = Random.Range(1, 6); //Eðer floatsa max deðer inclusive, deðilse max deðerin bir altý inclusive oluyormuþ.
            
            if (chance == 1 || chance == 2 || chance == 3)
            {
                Destroy(collision.gameObject);
            }

            if (chance == 4) // Kutuya takýlsýn.
            {
                IsImpact = true;
                Destroy(collision.gameObject);
            }

            if (chance == 5) // Hýzlansýn, hiçbir þeye çarpmasýn ve 3 top toplasýn.
            {
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
            sound.Play();
            IsImpact = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Fastener")
        {
            speed = 1.0f;

        }

        else if (other.gameObject.tag == "Slower")
        {
            speed = .25f;
        }
        if (other.gameObject.tag == "EnemyFinish")
        {
            gameManager.GameOver();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Fastener") || other.gameObject.CompareTag("Slower"))
        {
            speed = .5f;
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

    private void SwitchLane()
    {
        RaycastHit obstacleHit;
        int layerMaskObstacle = 1 << 8;
        RaycastHit boxHit;
        int layerMaskBox = 1 << 9;


        //Parametreler: Orijin yani ýþýnýn baþlayacaðý nokta, ýþýðýn hangi yönde ilerleyeceði, ýþýðýn çarptýðý objenin bilgileri hitte tutulur, 
        if (Physics.Raycast (new Vector3 (transform.position.x, transform.position.y+0.1f, transform.position.z), transform.TransformDirection (Vector3.forward),out obstacleHit, 0.2f, layerMaskObstacle)){
            StartCoroutine(chanceMaker());
            if(impactChance != 1)
            {
                
                ChangeLine();
            }
            
            
        }

        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), transform.TransformDirection(new Vector3(-2f,0f,1f)), out boxHit, 0.5f, layerMaskBox))
        {
            ChangeLine();


        }


        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), transform.TransformDirection(new Vector3(2f, 0f, 1f)), out boxHit, 0.5f, layerMaskBox))
        {
            
            ChangeLine();
            


        }



    }

    private IEnumerator chanceMaker()
    {
        yield return new WaitForSeconds(1f);
        impactChance = Random.Range(1, 4); //%33 ihtimalle takýlýyor %66 ihtimalle geçiyor.
        
    }

    private void ShootBall()
    {

        int layerMaskForStage = 1 << 10;
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.265f, transform.position.z+0.125f), transform.TransformDirection(new Vector3(0f,1f,4f)), out stageHit, 2.5f, layerMaskForStage))
        {
            startPositionOfBall = new Vector3(transform.position.x, transform.position.y + 0.265f, transform.position.z + 0.125f);
            endPositionOfBall = stageHit.transform.position;
            if (isBallSpawned)
            {
                if(stageHit.transform.parent.parent.GetComponentInChildren<startdooranim>().actualDoorStatus == false)
                {
                    StartCoroutine(SpawnBall());
                    isBallSpawned = false;
                }
                
            }
        }

    }

    private void ShootBalltoMainPlayerDoor()
    {

        int layerMaskForMainPlayerStage = 1 << 12;
        if (isRight)
        {
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.265f, transform.position.z + 0.125f), transform.TransformDirection(new Vector3(2.5f, 2f, 10f)), out stageHitMainPlayer, 2.5f, layerMaskForMainPlayerStage))
            {

                startPositionOfBall = new Vector3(transform.position.x, transform.position.y + 0.265f, transform.position.z + 0.125f);
                endPositionOfBall = stageHitMainPlayer.transform.position;
                if (isBallSpawned)
                {
                    if (stageHitMainPlayer.transform.parent.parent.GetComponentInChildren<startdooranim>().actualDoorStatus == true)
                    {
                        StartCoroutine(SpawnBall());
                        isBallSpawned = false;
                    }

                }
            }

        }
        
        else
        {
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.265f, transform.position.z + 0.125f), transform.TransformDirection(new Vector3(6f, 4f, 15f)), out stageHitMainPlayer, 2.5f, layerMaskForMainPlayerStage))
            {

                startPositionOfBall = new Vector3(transform.position.x, transform.position.y + 0.265f, transform.position.z + 0.125f);
                endPositionOfBall = stageHitMainPlayer.transform.position;
                if (isBallSpawned)
                {
                    if (stageHitMainPlayer.transform.parent.parent.GetComponentInChildren<startdooranim>().actualDoorStatus == true)
                    {
                        StartCoroutine(SpawnBall());
                        isBallSpawned = false;
                    }

                }
            }

        }


    }

    private void ShootBalltoMainPlayerMovingWay()
    {

        int layerMaskForMainPlayerMovingWay = 1 << 13;
        if (isRight)
        {
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.265f, transform.position.z + 0.125f), transform.TransformDirection(new Vector3(2.5f, 2f, 10f)), out stageHitMainPlayer, 2.5f, layerMaskForMainPlayerMovingWay))
            {

                startPositionOfBall = new Vector3(transform.position.x, transform.position.y + 0.265f, transform.position.z + 0.125f);
                endPositionOfBall = stageHitMainPlayer.transform.position;
                if (isBallSpawned)
                {
                    if (!stageHitMainPlayer.transform.parent.parent.GetChild(7).GetChild(0).gameObject.activeSelf)
                    {
                        StartCoroutine(SpawnBallforMovingway());
                        isBallSpawned = false;
                    }
                }
            }

        }

        else
        {
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.265f, transform.position.z + 0.125f), transform.TransformDirection(new Vector3(6f, 4f, 15f)), out stageHitMainPlayer, 2.5f, layerMaskForMainPlayerMovingWay))
            {

                startPositionOfBall = new Vector3(transform.position.x, transform.position.y + 0.265f, transform.position.z + 0.125f);
                endPositionOfBall = stageHitMainPlayer.transform.position;
                if (isBallSpawned)
                {
                    if (!stageHitMainPlayer.transform.parent.parent.GetChild(7).GetChild(0).gameObject.activeSelf)
                    {
                        StartCoroutine(SpawnBallforMovingway());
                        isBallSpawned = false;
                    }

                }
            }


        }


    }

    private void ShoottoMovingway()
    {
        int layerMaskForWalkingWay = 1 << 11;
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.265f, transform.position.z + 0.125f), transform.TransformDirection(new Vector3(0f, 1f, 4f)), out stageHit, 2.5f, layerMaskForWalkingWay))
        {
            startPositionOfBall = new Vector3(transform.position.x, transform.position.y + 0.265f, transform.position.z + 0.125f);
            endPositionOfBall = stageHit.transform.position;
            if (isBallSpawned && stageHit.transform.parent.parent.GetChild(7).GetChild(0).gameObject.activeSelf)
            {
                StartCoroutine(SpawnBallforMovingway());
                isBallSpawned = false;
            }
        }
    }


    private IEnumerator SpawnBall()
    {
        yield return new WaitForSeconds(0.5f);
        enemyPlayerAnimator[0].SetTrigger("ShootCondition");
        Instantiate(basketballForShooting, startPositionOfBall, Quaternion.identity);
        shootVector = endPositionOfBall - startPositionOfBall;
        yield return new WaitForSeconds(1.5f);
        isBallSpawned = true;
    }

    private IEnumerator SpawnBallforMovingway()
    {
        yield return new WaitForSeconds(0.5f);
        enemyPlayerAnimator[0].SetTrigger("ShootCondition");
        Instantiate(basketballForShooting, startPositionOfBall, Quaternion.identity);
        shootVector = endPositionOfBall - startPositionOfBall;
        yield return new WaitForSeconds(2f);
        isBallSpawned = true;
    }

    public Vector3 getShootVector()
    {
        return shootVector;
    }

    private void ControlSwitchLane()
    {
        if (isRight)
        {
            if (transform.position.x > -4.935f)
            {
                transform.position = new Vector3(-4.935f, transform.position.y, transform.position.z);
            }
        }

        else if (!isRight)
        {
            if (transform.position.x < -5.214f)
            {
                transform.position = new Vector3(-5.214f, transform.position.y, transform.position.z);
            }
        }
    }
}
