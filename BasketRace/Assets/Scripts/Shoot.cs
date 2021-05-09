using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shoot : MonoBehaviour
{

    public GameObject basketballPrefab;
    private Rigidbody basketballRb;
    private Vector3 firstPressLocation;
    private Vector3 lastPressLocation;
    private float dragZ;
    private Vector3 ballSpawnPos;
    private Vector3 aimLine;
    private float speed = 5f;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    private void OnMouseUp()
    {
        lastPressLocation = Input.mousePosition;
        aimLine = firstPressLocation - lastPressLocation;

        //Spawnlacak topun tam olarak nerden spawnlanacagini belirlemek icin
        ballSpawnPos = transform.position + new Vector3(0, 0.3f, 0.025f);

        //Topu spawnladik
        basketballPrefab = Instantiate(basketballPrefab, ballSpawnPos, transform.rotation);
        basketballRb = basketballPrefab.GetComponent<Rigidbody>();

        //Basip cektigimiz uzunluk olculuyor
        float dragingMag = aimLine.magnitude;
        
        //Basip cektigimiz yon olculuyor.
        Vector3 dragDir = aimLine.normalized;

        //Ekran 2 boyutlu nesne oldugu icin y eksenine uygulanan kuvvettin belirli bi oranini z'ye de atadim
        dragDir[1] = dragDir[1] * 2;
        dragZ = dragDir[1];
        dragDir[2] = dragZ;

        //Top istenilen yonde ve kuvvette firlatilir
        basketballRb.AddForce(dragDir * dragingMag * speed);
        basketballRb.AddTorque(dragDir * speed);
        Debug.Log("top attýk");
        
    }
    private void OnMouseDown()
    {
        firstPressLocation = Input.mousePosition;
    }
    
}
