using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketDetector : MonoBehaviour
{

    private ParticleSystem basketEffect;
    startdooranim TheScript;
    private AudioSource sound;
    
    private void Start()
    {
        
        sound = gameObject.GetComponent<AudioSource>();
        TheScript = GetComponentInChildren<startdooranim>();
        basketEffect = gameObject.GetComponentInChildren<ParticleSystem>();
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(sound != null && !other.CompareTag("BasketballOfEnemyPlayer"))
        {
            sound.Play();
        }
        
        basketEffect.Play();        
        if (TheScript.actualDoorStatus == false)    //Baþlangýçta kapý kapalýysa ve OnTriggerdan dolayý basket olduðunu anlýyor, basket olduysa giriyor.
        {
            
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
            
            TheScript.LeftDoor.SetBool("OpenOrClose", false);
            TheScript.RightDoor.SetBool("OpenOrClose", false);
            TheScript.actualDoorStatus = false;
            TheScript.LeftDoor.SetInteger("Stop", 1);
            TheScript.RightDoor.SetInteger("Stop", 1);
        }

     }
}
