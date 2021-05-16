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
    private Animator playerAnimations;
    private GameObject basketball_of_player;
    private Vector3 touchReleasePos;
    private int BallCount;
    Vector3 forceInit;
    private float startTime;
    private float passingTime;
    private Camera cam;
    private Vector3 ScreenLimitation;
    private Vector3 ScreenLimitationAsWorldSpace;

    private void Start()
    {
        playerAnimations = GameObject.FindWithTag("MainPlayer").GetComponentInChildren<Animator>();
        basketball_of_player = GameObject.FindWithTag("BasketballOfPlayer");
        rb = GetComponent<Rigidbody>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        ScreenLimitation = new Vector3((Screen.width)/4,(Screen.height)/4, (Screen.height) / 4);
        ScreenLimitationAsWorldSpace = cam.ScreenToWorldPoint(ScreenLimitation);
        
        
    }

    private void Update()
    {

        BallCount = gameManager.getBallCount();

        if (Input.touches.Length > 0 && BallCount > 0)
        {

            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                startTime = Time.time;
                TouchPressDown = new Vector2(t.position.x, t.position.y);

            }

            passingTime = Time.time - startTime;
            if (t.phase == TouchPhase.Moved && passingTime > 0.35f)
            {

                Vector3 curPosition = t.position;
                forceInit = (curPosition - TouchPressDown);
                if (forceInit.y > (-ScreenLimitationAsWorldSpace).y * 2f)
                {
                    forceInit.y = (ScreenLimitationAsWorldSpace).y * -2f;
                }

                if (forceInit.y < (-ScreenLimitationAsWorldSpace).y / 3f)
                {
                    forceInit.y = (ScreenLimitationAsWorldSpace).y / -3f;
                }

                if (forceInit.x > (-ScreenLimitationAsWorldSpace).x / 2f && forceInit.x > 0)
                {
                    forceInit.x = (-ScreenLimitationAsWorldSpace).x / 2f;
                }

                if (forceInit.x < (ScreenLimitationAsWorldSpace).x / 2f && forceInit.x < 0)
                {
                    forceInit.x = (ScreenLimitationAsWorldSpace).x / 2f;
                }


                //Z eksenine de y ekseni atandý.
                forceInit = (new Vector3(forceInit.x, forceInit.y, forceInit.y));

                if (!isShoot)
                    DrawTrajectory.Instance.UpdateTrajectory(forceInit, rb, transform.position);
            }
            if (t.phase == TouchPhase.Stationary && passingTime > 0.35f)
            {
                forceInit = (new Vector3(forceInit.x, forceInit.y, forceInit.y));
                if (forceInit.y > (-ScreenLimitationAsWorldSpace).y * 2f)
                {
                    forceInit.y = (ScreenLimitationAsWorldSpace).y * -2f;
                }

                if (forceInit.y < (-ScreenLimitationAsWorldSpace).y / 3f)
                {
                    forceInit.y = (ScreenLimitationAsWorldSpace).y / -3f;
                }

                if (forceInit.x > (-ScreenLimitationAsWorldSpace).x / 2f && forceInit.x > 0)
                {
                    forceInit.x = (-ScreenLimitationAsWorldSpace).x / 2f;
                }

                if (forceInit.x < (ScreenLimitationAsWorldSpace).x / 2f && forceInit.x < 0)
                {
                    forceInit.x = (ScreenLimitationAsWorldSpace).x / 2f;
                }

                if (!isShoot)
                    DrawTrajectory.Instance.UpdateTrajectory(forceInit, rb, transform.position);
            }
            if (t.phase == TouchPhase.Ended && passingTime > 0.35f)
            {
                DrawTrajectory.Instance.HideLine();
                touchReleasePos = t.position;

                atisegimi = (touchReleasePos - TouchPressDown);//2f; //Ekranýn sensitivitesini ayarladýk.
                if (atisegimi.y > (-ScreenLimitationAsWorldSpace).y * 2f)
                {
                    atisegimi.y = (ScreenLimitationAsWorldSpace).y * -2f;
                }

                if (atisegimi.y < (-ScreenLimitationAsWorldSpace).y / 3f)
                {
                    atisegimi.y = (ScreenLimitationAsWorldSpace).y / -3f;

                }

                if (atisegimi.x > (-ScreenLimitationAsWorldSpace).x / 2f && atisegimi.x > 0)
                {
                    atisegimi.x = (-ScreenLimitationAsWorldSpace).x / 2f;
                }

                if (atisegimi.x < (ScreenLimitationAsWorldSpace).x / 2f && atisegimi.x < 0)
                {
                    atisegimi.x = (ScreenLimitationAsWorldSpace).x / 2f;
                }

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
        transform.parent = null;
        gameManager.updateBallCount(-1);
    }
}
