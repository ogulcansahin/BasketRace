using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerController : MonoBehaviour
{
    GameObject mainPlayer;
    GameObject road;
    float road_x;
    bool IsRunning = true;
    bool IsPowerUpRun = false;
    bool isRight = true;
    bool IsImpact = false;
    private GameManager gameManager;
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;
    private float startTime;
    private float passingTime;
    private Animator [] MainPlayerAnimator;
    private int dieCondition = 0; // Die condition sürekli triggerlanmasýn diye.
    private ParticleSystem [] powerUpRunParticle;
    float speed = 0.5f;

    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        mainPlayer = GameObject.FindWithTag("MainPlayer");
        road = GameObject.FindWithTag("MainPlayerFloor");
        road_x = (road.transform.position - (road.transform.localScale * 0.215f)).x;
        MainPlayerAnimator = gameObject.GetComponentsInChildren<Animator>();
        powerUpRunParticle = gameObject.GetComponentsInChildren<ParticleSystem>();
        
    }
    private void Update()
    {
        Swipe();

    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        if(gameManager.GetisGameStarted() == true)
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
            road_x = (road.transform.position + (road.transform.localScale * 0.225f)).x;
            isRight = false;     
        }
      
        else if (!isRight)
        {
            while (transform.position.x < road_x) 
            {
                transform.Translate(transform.right * Time.deltaTime * 2.9f);
                yield return new WaitForSeconds(0.005f);
            }            
            road_x = (road.transform.position - (road.transform.localScale * 0.215f)).x;
            isRight = true;
        }
     
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Fastener")
        {
            speed = 1.0f;
        }

        else if (collision.gameObject.tag == "Slower")
        {
            speed = .25f;
        }

        else if (collision.gameObject.tag == "MainPlayerFloor")
        {
            speed = .5f;
        }

        if (collision.gameObject.tag == "Basketball_Skill")
        {
            gameManager.updateBallCount(1);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Box")
        {
            int chance = Random.Range(1, 6); //Eðer floatsa max deðer inclusive, deðilse max deðerin bir altý inclusive oluyormuþ.

            if(chance == 1 || chance == 2 || chance == 3)
            {
                int random = Random.Range(-2, 3);
                gameManager.updateBallCount(random);
                Destroy(collision.gameObject);
            }

            if(chance == 4) // Kutuya takýlsýn.
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerFinish")
        {
            gameManager.GoNextLevel();
        }
    }

    public bool GetIsRunning()
    {
        return IsRunning;
    }

    public void SetIsRunning (bool IsRunning)
    {
        this.IsRunning = IsRunning;
    }

    public void Swipe()
    {
        if (Input.touches.Length > 0)
        {
            if(gameManager.GetisGameStarted() == false)
            {
                gameManager.SetisGameStarted(true); 
            }
            

            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                startTime = Time.time;
                //save began touch 2d point
                firstPressPos = new Vector2(t.position.x, t.position.y);
            }
            passingTime = Time.time - startTime;
            if (t.phase == TouchPhase.Ended && passingTime < 0.35f)
            {
                //save ended touch 2d point
                secondPressPos = new Vector2(t.position.x, t.position.y);

                //create vector from the two points
                currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                //normalize the 2d vector
                currentSwipe.Normalize();

                //swipe left
                if (currentSwipe.x < 0)
             {
                    if (isRight == true)
                    {
                        ChangeLine();
                    }
                }
                //swipe right
                if (currentSwipe.x > 0)
             {
                    if(isRight == false)
                    {
                        ChangeLine();
                    }
                }
            }
        }
    }

    private IEnumerator PowerUpRun()
    {
        MainPlayerAnimator[0].SetFloat("RunSpeed", 2f);
        MainPlayerAnimator[1].SetFloat("DriplingSpeed", 2f);
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
        MainPlayerAnimator[0].SetFloat("RunSpeed", 1f);
        MainPlayerAnimator[1].SetFloat("DriplingSpeed", 1.5f);

    }

    private IEnumerator Impact()
    {
        if (dieCondition == 0)
            MainPlayerAnimator[0].SetTrigger("DieCondition");
        dieCondition = 1;
        MainPlayerAnimator[0].SetFloat("RunSpeed", 0f);
        MainPlayerAnimator[1].SetFloat("DriplingSpeed", 0f);
        if (!powerUpRunParticle[1].isPlaying)
        {
            powerUpRunParticle[1].Play();
        }
        transform.Translate(transform.forward * -1 * Time.deltaTime * 0.2f);
        yield return new WaitForSeconds(1f);
        powerUpRunParticle[1].Stop();
        dieCondition = 0;
        IsImpact = false;
        MainPlayerAnimator[0].SetFloat("RunSpeed", 1f);
        MainPlayerAnimator[1].SetFloat("DriplingSpeed", 1.5f);


    }
}
