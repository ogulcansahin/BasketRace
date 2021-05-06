using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallForce : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody a;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        a = GetComponent<Rigidbody>();
        a.AddForce(new Vector3(0f, 8f, 15f) * 80);
    }
}
