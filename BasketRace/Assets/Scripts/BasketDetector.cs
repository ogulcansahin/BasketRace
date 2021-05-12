using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketDetector : MonoBehaviour
{
    
    
    startdooranim TheScript;
    
    private void Start()
    {
        
        TheScript = GetComponentInChildren<startdooranim>();
        
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (TheScript.actualDoorStatus == false)    //Ba�lang��ta kap� kapal�ysa ve OnTriggerdan dolay� basket oldu�unu anl�yor, basket olduysa giriyor.
        {
            
            //Kap�n�n a��l�p kapanmas�na dair iki farkl� transition condition var. Bunlar OpenOrClose=True ya da false. Stop ise ba�lang��taki bo� stateten di�er 
            //statelere gidip animasyon �al��arak ba�lamas�n diye. Bir kez basket olduktan sonra art�k stop tekrar 0 olam�yor. NotEquals bir kez olsun sa�lan�yor 
            //ve art�k stopun �nemi kalm�yor.
            TheScript.LeftDoor.SetBool("OpenOrClose", true); //True ise a�s�n False ise kapas�n.
            TheScript.RightDoor.SetBool("OpenOrClose", true);
            TheScript.actualDoorStatus = true;
            TheScript.LeftDoor.SetInteger("Stop", 1);
            TheScript.RightDoor.SetInteger("Stop", 1);
            
        }

        else if(TheScript.actualDoorStatus == true) //Ba�lang��ta kap� a��ksa. 
        {
            
            TheScript.LeftDoor.SetBool("OpenOrClose", false);
            TheScript.RightDoor.SetBool("OpenOrClose", false);
            TheScript.actualDoorStatus = false;
            TheScript.LeftDoor.SetInteger("Stop", 1);
            TheScript.RightDoor.SetInteger("Stop", 1);
        }

     }
}
