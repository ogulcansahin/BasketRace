using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopFrontOfTheClosedDoorForEnemy : MonoBehaviour
{
    Animator[] enemyAnimations;
    startdooranim doorstatus;
    EnemyController enemyController;

    Vector3 enteringPosition;

    bool isRunning;  

    private ParticleSystem[] impactParticle;

    void Start()
    {
        doorstatus = GetComponentInParent<startdooranim>(); 
        impactParticle = GameObject.FindWithTag("EnemyPlayer").GetComponentsInChildren<ParticleSystem>();
    }

    private void OnCollisionEnter(Collision collision) 
                                                       
    {
        if (collision.gameObject.tag == "EnemyPlayer")
        {
            enemyController = collision.gameObject.GetComponent<EnemyController>(); 
            enemyAnimations = collision.gameObject.GetComponentsInChildren<Animator>();
            enteringPosition = collision.gameObject.transform.position;

        }

      
        if (doorstatus.actualDoorStatus == false && collision.gameObject.tag == "EnemyPlayer") 
                                                                                                                  
        {
            enemyAnimations[0].SetTrigger("Stop");
            enemyAnimations[0].SetBool("Stop", true); 
            enemyAnimations[1].SetTrigger("StopDripling"); 


        }
    }

    private void OnCollisionStay(Collision collision) 
    {
        if (doorstatus.actualDoorStatus == false && collision.gameObject.tag == "EnemyPlayer") 
        {
            enemyAnimations[0].SetBool("Stop", true);
            enemyAnimations[0].ResetTrigger("RunCondition");
            enemyAnimations[1].SetTrigger("StopDripling");
            isRunning = false; //hareket etme.
            enemyController.SetIsRunning(isRunning);
            if (collision.transform.position.z - enteringPosition.z > 0.5)
            {
                enemyAnimations[0].SetTrigger("StopTrigger");
                isRunning = false;
                enemyController.SetIsRunning(isRunning);
                enemyAnimations[0].SetTrigger("DieCondition");

                if (!impactParticle[1].isPlaying)
                {
                    impactParticle[1].Play();
                }

            }
            collision.transform.position = Vector3.Lerp(collision.transform.position, new Vector3(collision.transform.position.x, enteringPosition.y, enteringPosition.z), Mathf.PingPong(Time.time * 0.5f, 1.0f));
            enemyAnimations[0].ResetTrigger("DieCondition");
        }



        if (doorstatus.actualDoorStatus == true && collision.gameObject.tag == "EnemyPlayer")   
        {

            enemyAnimations[0].ResetTrigger("StopTrigger");
            enemyAnimations[0].SetBool("Stop", false);
            enemyAnimations[0].SetTrigger("RunCondition"); 
            enemyAnimations[1].SetTrigger("StartDripling"); 
            

            isRunning = true;
            enemyController.SetIsRunning(isRunning);

        }


    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "EnemyPlayer")
        {
            enemyAnimations[0].ResetTrigger("StopTrigger");
            enemyAnimations[0].SetBool("Stop", false);
            enemyAnimations[0].SetTrigger("RunCondition"); 
            enemyAnimations[1].SetTrigger("StartDripling"); 
            isRunning = true;
            enemyController.SetIsRunning(isRunning);
        }

    }
}
