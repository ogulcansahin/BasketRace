using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnObject;
    private float newSpawnDuration = 1.0f;
    private GameObject basketballofplayer;
    private GameObject mainplayer;
    private Vector3 spawn_position;
    public static Spawner Instance;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        basketballofplayer = GameObject.FindWithTag("BasketballOfPlayer");
        mainplayer = GameObject.FindWithTag("MainPlayer");
        
        spawn_position = mainplayer.transform.position + new Vector3(0f,+0.2f,+0.1f);
        
    }

    void SpawnNewObject ()
    {
        Instantiate(spawnObject, spawn_position, Quaternion.identity);
        basketballofplayer.GetComponent<MeshRenderer>().enabled = true;
    }

    public void NewSpawnRequest ()
    {
        Invoke("SpawnNewObject", newSpawnDuration);
    }


    // Update is called once per frame
    
}
