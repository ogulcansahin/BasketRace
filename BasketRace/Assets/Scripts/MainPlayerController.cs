using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerController : MonoBehaviour
{
    GameObject mainPlayer;
    GameObject road;
    float road_x;
    bool IsRunning = true;
    bool isRight = true;
    private GameManager gameManager;
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;
    private float startTime;
    private float passingTime;

    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        mainPlayer = GameObject.FindWithTag("MainPlayer");
        road = GameObject.FindWithTag("MainPlayerFloor");
        road_x = (road.transform.position - (road.transform.localScale * 0.215f)).x;
        
    }
    private void Update()
    {
        Swipe();

    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (IsRunning)
        {
            run();
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
            gameManager.updateBallCount(1);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Box")
        {
            int random = Random.Range(-2, 3);
            gameManager.updateBallCount(random);
            Destroy(collision.gameObject);
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
}
