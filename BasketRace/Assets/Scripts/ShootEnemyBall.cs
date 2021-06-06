using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemyBall : MonoBehaviour
{
    Rigidbody rg;
    Vector3 shootVector;
    void Start()
    {
        rg = transform.GetComponent<Rigidbody>();
        shootVector = EnemyController.Instance.getShootVector();
        rg.AddForce(shootVector.x*75f, shootVector.y*421f, shootVector.z*83.3f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
