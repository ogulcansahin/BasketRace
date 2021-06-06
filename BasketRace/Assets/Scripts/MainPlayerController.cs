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
    public bool levelIsFinished = false;
    private GameManager gameManager;
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;
    private float startTime;
    private float passingTime;
    private Animator [] MainPlayerAnimator;
    private int dieCondition = 0; // Die condition sürekli triggerlanmasýn diye.
    private ParticleSystem [] powerUpRunParticle;
    float speed = .5f;
    private AudioSource[] sounds;

    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        mainPlayer = GameObject.FindWithTag("MainPlayer");
        road = GameObject.FindWithTag("MainPlayerFloor");
        road_x = (road.transform.position - (road.transform.localScale * 0.215f)).x;
        MainPlayerAnimator = gameObject.GetComponentsInChildren<Animator>();
        powerUpRunParticle = gameObject.GetComponentsInChildren<ParticleSystem>();
        sounds = gameObject.GetComponents<AudioSource>();
        sounds[2].time = sounds[2].clip.length * .5f;

    }
    private void Update()
    {
        Swipe();
        ControlSwitchLane();
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

        if (collision.gameObject.tag == "Basketball_Skill")
        {
            sounds[0].Play();
            gameManager.updateBallCount(1);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Box")
        {
            int chance = Random.Range(1, 6); //Eðer floatsa max deðer inclusive, deðilse max deðerin bir altý inclusive oluyormuþ.
            
            if(chance == 1 || chance == 2 || chance == 3)
            {
                int random = Random.Range(-2, 3);
                if(random < 0)
                {
                    sounds[3].Play();
                }

                else if(random >= 0)
                {
                    sounds[0].Play();
                }
                gameManager.updateBallCount(random);
                Destroy(collision.gameObject);
            }

            if(chance == 4) // Kutuya takýlsýn.
            {
                sounds[1].Play();
                IsImpact = true;
                Destroy(collision.gameObject);
            }

            if (chance == 5) // Hýzlansýn, hiçbir þeye çarpmasýn ve 3 top toplasýn.
            {
                sounds[2].Play();
                gameManager.updateBallCount(3);
                IsPowerUpRun = true;
                Destroy(collision.gameObject);
            }

        }
        if (collision.gameObject.tag == "Obstacle")
        {
            sounds[1].Play();
            IsImpact = true;
            Destroy(collision.gameObject);
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
        if (other.gameObject.tag == "PlayerFinish")
        {
            levelIsFinished = true;
            gameManager.LevelFinished();
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

    public void SetIsRunning (bool IsRunning)
    {
        this.IsRunning = IsRunning;
    }

    public void Swipe()
    {
        if (Input.touches.Length > 0)
        {
            if(gameManager.GetisGameStarted() == false && levelIsFinished == false)
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
                        sounds[3].Play();
                        ChangeLine();
                    }
                }
                //swipe right
                if (currentSwipe.x > 0)
             {
                    if(isRight == false)
                    {
                        sounds[4].Play();
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
        yield return new WaitForSeconds(1f);
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

    private void ControlSwitchLane()
    {
        if (isRight)
        {
            if (transform.position.x > -4.26f)
            {
                transform.position = new Vector3 (-4.26f,transform.position.y,transform.position.z);
            }
        }

        else if (!isRight)
        {
            if (transform.position.x < -4.57f)
            {
                transform.position = new Vector3(-4.57f, transform.position.y, transform.position.z);
            }
        }
    }
}
