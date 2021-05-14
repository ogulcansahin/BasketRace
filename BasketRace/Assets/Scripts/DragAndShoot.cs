using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndShoot : MonoBehaviour
{
    private Vector3 TouchPressDown;
    private Vector3 atisegimi;
    private GameManager gameManager;
    private Rigidbody rb;
    private bool isShoot=false;
    private Vector3 maximumForce;
    private Animator playerAnimations;
    private GameObject basketball_of_player;
    private Vector3 touchReleasePos;
    private int BallCount;
    Vector3 forceInit;
    private float startTime;
    private float passingTime;

    //private float maxdistance_y_for1920_1080 = 760f;
    //private float mindistance_y_for1920_1080 = 240f;

    private void Start()
    {
        playerAnimations = GameObject.FindWithTag("MainPlayer").GetComponentInChildren<Animator>();
        basketball_of_player = GameObject.FindWithTag("BasketballOfPlayer");
        rb = GetComponent<Rigidbody>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {

        BallCount = gameManager.getBallCount();
        
        if (Input.touches.Length > 0 && BallCount>0)
        {
            
            Touch t = Input.GetTouch(0);

            if(t.phase == TouchPhase.Began )
            {
                startTime = Time.time;
                TouchPressDown = new Vector2(t.position.x, t.position.y);
                
            }

            passingTime = Time.time - startTime;
            if (t.phase == TouchPhase.Moved && passingTime > 0.35f)
            {
                
                Vector3 curPosition = t.position;
                forceInit = (curPosition - TouchPressDown);
                

                //Z eksenine de y ekseni atandý.
                forceInit = (new Vector3(forceInit.x, forceInit.y, forceInit.y));

                if (!isShoot)
                    DrawTrajectory.Instance.UpdateTrajectory(forceInit, rb, transform.position);
            }
            if(t.phase == TouchPhase.Stationary && passingTime > 0.35f)
            {
                forceInit = (new Vector3(forceInit.x, forceInit.y, forceInit.y));


                if (!isShoot)
                    DrawTrajectory.Instance.UpdateTrajectory(forceInit, rb, transform.position);
            }
            if(t.phase == TouchPhase.Ended && passingTime > 0.35f)
            {
                DrawTrajectory.Instance.HideLine();
                touchReleasePos = t.position;

                atisegimi = (touchReleasePos - TouchPressDown);//2f; //Ekranýn sensitivitesini ayarladýk.

                atisegimi = atisegimi / 2;

                if (atisegimi.y < 0)
                {
                    atisegimi.y = atisegimi.y * -1;
                }
                
                playerAnimations.SetTrigger("ShootCondition");
                basketball_of_player.GetComponent<MeshRenderer>().enabled = false;
                gameObject.GetComponent<MeshRenderer>().enabled = true;
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                Shoot(atisegimi);

            }
        }
    }


    void Shoot (Vector3 Force)
    {
        if (isShoot)
            return;
        rb.AddForce(new Vector3(Force.x, (Force.y * 1.035f), (Force.y * 1f)));
        isShoot = true;
        Spawner.Instance.NewSpawnRequest();
        gameManager.updateBallCount(-1);
    }
}
