using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    GameObject enemyPlayer;
    GameObject road;
    float road_x;
    bool IsRunning = true;
    bool isRight = true;

    void Start()
    {
        enemyPlayer = GameObject.FindWithTag("EnemyPlayer");
        road = GameObject.FindWithTag("EnemyFloor");
        road_x = (road.transform.position - (road.transform.localScale * 0.235f)).x;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeLine();
        }
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
            road_x = (road.transform.position + (road.transform.localScale * 0.235f)).x;
            isRight = false;
        }

        else if (!isRight)
        {
            while (transform.position.x < road_x)
            {
                transform.Translate(transform.right * Time.deltaTime * 2.9f);
                yield return new WaitForSeconds(0.005f);
            }
            road_x = (road.transform.position - (road.transform.localScale * 0.235f)).x;
            isRight = true;
        }

    }
}
