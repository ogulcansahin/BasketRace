using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnObject;
    private float newSpawnDuration = 0.5f;
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
        transform.parent = mainplayer.transform;

    }

    void SpawnNewObject ()
    {
        
        spawn_position = mainplayer.transform.position + new Vector3(0f, +0.2f, +0.1f);
        Instantiate(spawnObject, spawn_position, Quaternion.identity);
        basketballofplayer.GetComponent<MeshRenderer>().enabled = true;
    }

    public void NewSpawnRequest ()
    {
        Invoke("SpawnNewObject", newSpawnDuration);
    }
    
}
