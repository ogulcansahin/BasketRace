using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketDetector : MonoBehaviour
{
    
    
    startdooranim TheScript;
    GameObject doorcontrol;
    private void Start()
    {
        doorcontrol = GameObject.FindGameObjectWithTag("doordetector"); //Ana kapýdaki Tag.
        TheScript = doorcontrol.GetComponent<startdooranim>();
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Selam");
        if (TheScript.actualDoorStatus == false)    //Baþlangýçta kapý kapalýysa ve OnTriggerdan dolayý basket olduðunu anlýyor, basket olduysa giriyor.
        {
            Debug.Log("Girdi");
            //Kapýnýn açýlýp kapanmasýna dair iki farklý transition condition var. Bunlar OpenOrClose=True ya da false. Stop ise baþlangýçtaki boþ stateten diðer 
            //statelere gidip animasyon çalýþarak baþlamasýn diye. Bir kez basket olduktan sonra artýk stop tekrar 0 olamýyor. NotEquals bir kez olsun saðlanýyor 
            //ve artýk stopun önemi kalmýyor.
            TheScript.LeftDoor.SetBool("OpenOrClose", true); //True ise açsýn False ise kapasýn.
            TheScript.RightDoor.SetBool("OpenOrClose", true);
            TheScript.actualDoorStatus = true;
            TheScript.LeftDoor.SetInteger("Stop", 1);
            TheScript.RightDoor.SetInteger("Stop", 1);
            
        }

        else if(TheScript.actualDoorStatus == true) //Baþlangýçta kapý açýksa. 
        {
            Debug.Log("Tekrar girdi");
            TheScript.LeftDoor.SetBool("OpenOrClose", false);
            TheScript.RightDoor.SetBool("OpenOrClose", false);
            TheScript.actualDoorStatus = false;
            TheScript.LeftDoor.SetInteger("Stop", 1);
            TheScript.RightDoor.SetInteger("Stop", 1);
        }



     }
}
