using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerController : MonoBehaviour
{
    GameObject mainPlayer;

    void Start()
    {
        mainPlayer = GameObject.FindWithTag("MainPlayer");
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        
        run();

    }
    private void run()
        {
            transform.Translate(transform.forward * Time.deltaTime * 1.2f);
        }
}
