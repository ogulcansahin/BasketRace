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

    //private float maxdistance_y_for1920_1080 = 760f;
    //private float mindistance_y_for1920_1080 = 240f;
    Camera cam;

    
    private void Start()
    {
        //cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        playerAnimations = GameObject.FindWithTag("MainPlayer").GetComponentInChildren<Animator>();
        basketball_of_player = GameObject.FindWithTag("BasketballOfPlayer");
        rb = GetComponent<Rigidbody>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        
        
    }

    private void Update()
    {
        BallCount = gameManager.getBallCount();
        
        if (Input.touches.Length > 0 && BallCount>0)
        {
            Touch t = Input.GetTouch(0);
            if(t.phase == TouchPhase.Began)
            {
                TouchPressDown = new Vector2(t.position.x, t.position.y);
            }
            if(t.phase == TouchPhase.Moved)
            {
                Vector3 curPosition = t.position;

                Vector3 forceInit = (curPosition - TouchPressDown);

                //Z eksenine de y ekseni atandý.
                forceInit = (new Vector3(forceInit.x, forceInit.y, forceInit.y));


                if (!isShoot)
                    DrawTrajectory.Instance.UpdateTrajectory(forceInit, rb, transform.position);
            }
            if(t.phase == TouchPhase.Ended)
            {
                DrawTrajectory.Instance.HideLine();
                touchReleasePos = t.position;

                atisegimi = (touchReleasePos - TouchPressDown);//2f; //Ekranýn sensitivitesini ayarladýk.


                if (atisegimi.y < 0)
                {
                    atisegimi.y = atisegimi.y * -1;
                }
                //atisegimi.x = atisegimi.x * -1;

                
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

        rb.AddForce(new Vector3 (Force.x, (Force.y*1.035f), (Force.y*0.6f)));
        isShoot = true;
        Spawner.Instance.NewSpawnRequest();
        gameManager.updateBallCount(-1);
    }
    
}
