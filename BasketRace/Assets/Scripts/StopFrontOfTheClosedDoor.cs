using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopFrontOfTheClosedDoor : MonoBehaviour
{   
    //Startdooranim kap� prefablar�nda yer al�yor. 
    //��erisinde sol kap� ve sa� kap�n�n animasyon dosyalar� ve kap�n�n a��k veya kapal� olup olmad��� bilgisi yer al�yor. Bunu yakalad�k ��nk� player kap� kapal�ysa
    //kap�n�n �n�nde a��lana dek beklemeli a��ksa devam etmeli. 
    startdooranim doorstatus; 

    Animator[] PlayerAnimations; //Karakterin top sektirme ve ko�ma animasyonlar� al�nd�. Array olarak yarat�ld� ��nk� iki animasyon dosyas� var.

    //Mainplayer�n al�nmas�n�n sebebi: Karakterin pozisyonunun ileri do�ru de�i�imi main playerdan kontrol ediliyor.
    //Burada mainplayer �ekilerek kap� kapal�yken durmas� kap� a��ld���nda hareket etmesi sa�lan�r.
    MainPlayerController mainplayer;

    //GameManager scriptinde top say�s� yer al�yor. E�er kap� kapal� ve kap�n�n �n�ne gelindi�inde top say�s� s�f�rsa game over olmas� laz�m.
    //Bu ayar�n kontrol� i�in gameManager al�nd�. 
    GameManager gameManager; 

    bool isRunning; //Mainplayerdaki hareket durumunu burada bir bool de�i�kene atamak i�in olu�turduk.
    int numberOfBall; //GameManagerdaki top say�s�n� burada bir de�ere atamak i�in kulland�k. 
    void Start()
    {
        doorstatus = GetComponentInParent<startdooranim>(); //Kap�n�n a��k olup olmad���yla ilgili script �ekildi.
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>(); //Top say�s�n� y�netmek i�in gameManager scripti �ekildi.
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision) //�lk �arp��ma an�nda basket olmadan animasyonlar durdurulsun ve �arp��an�n main player oldu�u kontrol edilsin diye
        //�rne�in rakibin att��� top da collisiona girebilir.
    {
        if (collision.gameObject.tag == "MainPlayer")
        {
            mainplayer = collision.gameObject.GetComponent<MainPlayerController>(); //Topun kalmad�ysa reklam alan� geldi�inde arkada s�rekli ko�maya devam etmesin dursun.
            PlayerAnimations = collision.gameObject.GetComponentsInChildren<Animator>();
        }
        
        numberOfBall = gameManager.getBallCount(); //gameManager'a bir get methodu olu�turuldu. B�ylelikle private de�eri get ile buradaki numberofball'a �ekiyoruz.
        if (doorstatus.actualDoorStatus == false && collision.gameObject.tag == "MainPlayer" && numberOfBall>0) //E�er kap� kapal� ve kap�n�n �n�ne mainplayer geldiyse
            // top say�s�n� kontrol et. E�er topu kald�ysa hen�z elenmemi�. �ut �ekmesini bekle.
        {
            
            PlayerAnimations[0].SetTrigger("Stop"); //MainPlayerdaki ko�ma animasyonunu durdurup idle animasyonunu ba�lat�r. Condition'�n ad� stop yoksa default bi�i yok.
            PlayerAnimations[1].SetTrigger("Stop"); //MainPlayerdaki top sektirme animasyonunu durdurur. Bu da default de�il transitionlar aras�ndaki condition.
            

        }
    }

    private void OnCollisionStay(Collision collision) //Kap�n�n �n�nde beklerken top say�s� s�rekli kontrol edilsin. E�er yeterli top yoksa game over.
    {
        numberOfBall = gameManager.getBallCount();
        if (doorstatus.actualDoorStatus == false && collision.gameObject.tag == "MainPlayer" && numberOfBall >0) //kap� kapal�, topum var
        {
            isRunning = false; //hareket etme.
            mainplayer.SetIsRunning(isRunning);
        }

        

        else if (doorstatus.actualDoorStatus == false && collision.gameObject.tag == "MainPlayer" && numberOfBall == 0) //kap� kapal�, topum yok, game over
        {
            
            Debug.Log("GameOver");
            isRunning = false;
            mainplayer.SetIsRunning(isRunning);
            PlayerAnimations[0].SetTrigger("Stop");  
            
        }

        if (doorstatus.actualDoorStatus == true && collision.gameObject.tag == "MainPlayer")    //kap� a��l�rsa ko�maya ba�la. 
        {
            
            PlayerAnimations[0].SetTrigger("RunCondition"); //MainPlayerdaki ko�ma animasyonunu ba�lat�r.
            if (numberOfBall != 0)              //topum varsa
            {
                PlayerAnimations[1].SetTrigger("StartDripling"); //MainPlayerdaki top sektirme animasyonunu ba�lat�r. 
            }
            
            isRunning = true;
            mainplayer.SetIsRunning(isRunning);
            
        }

        //Sorun tek topun  varken basket atsan dahi game over olman. 
    }

}
