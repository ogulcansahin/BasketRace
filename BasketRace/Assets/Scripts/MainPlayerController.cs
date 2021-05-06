using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerController : MonoBehaviour
{
    GameObject mainPlayer; //Karakteri aldýk.
    bool IsRunning = true;
    private GameObject road; //Flooru aldýk.
    float road_x;
    void Start()
    {
        mainPlayer = GameObject.FindWithTag("MainPlayer");
        road = GameObject.FindWithTag("MainFloor");
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeLine(); 
            

        }
        

        if (IsRunning)
        {
            run();
        }
        
        else if (!IsRunning)
        {

        }

    }
    private void run()
        {
            transform.Translate(transform.forward * Time.deltaTime * 0.8f);
        }

    private void ChangeLine()
    {

        road_x = (road.transform.position - (road.transform.localScale * 0.215f)).x;
        StartCoroutine(LaneChangingRouitne());

    }

    IEnumerator LaneChangingRouitne()
    {
        
        while (transform.position.x > road_x)
        {
            
            transform.Translate(transform.right * -1 * Time.deltaTime * 2.9f);
            yield return new WaitForSeconds(0.005f);
        }
    }
}

