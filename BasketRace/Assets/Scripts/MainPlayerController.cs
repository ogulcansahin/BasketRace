using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerController : MonoBehaviour
{
    GameObject mainPlayer;
    bool IsRunning = true;
    void Start()
    {
        mainPlayer = GameObject.FindWithTag("MainPlayer");
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
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
}
