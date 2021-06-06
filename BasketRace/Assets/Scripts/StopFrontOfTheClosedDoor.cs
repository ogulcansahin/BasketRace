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
    MainPlayerController mainPlayerController;
    

    //GameManager scriptinde top say�s� yer al�yor. E�er kap� kapal� ve kap�n�n �n�ne gelindi�inde top say�s� s�f�rsa game over olmas� laz�m.
    //Bu ayar�n kontrol� i�in gameManager al�nd�. 
    GameManager gameManager;

    Vector3 enteringPosition;

    bool isRunning; //Mainplayerdaki hareket durumunu burada bir bool de�i�kene atamak i�in olu�turduk.
    int numberOfBall; //GameManagerdaki top say�s�n� burada bir de�ere atamak i�in kulland�k. 

    //karakter kap�n�n �n�nde kap� a��kken ge�iyor, tekrar potaya basket at�l�rsa shootdistance �n ba��nda durmal�. Bu sebeple bool de�er
    //olu�turduk.
    bool isEnteredMainPlayer = true;

    private AudioSource[] sounds;

    private ParticleSystem[] impactParticle;
    void Start()
    {
        doorstatus = GetComponentInParent<startdooranim>(); //Kap�n�n a��k olup olmad���yla ilgili script �ekildi.
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>(); //Top say�s�n� y�netmek i�in gameManager scripti �ekildi.
        impactParticle = GameObject.FindWithTag("MainPlayer").GetComponentsInChildren<ParticleSystem>();
        sounds = GameObject.FindWithTag("MainPlayer").GetComponentsInChildren<AudioSource>();
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision) //�lk �arp��ma an�nda basket olmadan animasyonlar durdurulsun ve �arp��an�n main player oldu�u kontrol edilsin diye
        //�rne�in rakibin att��� top da collisiona girebilir.
    {
        if (collision.gameObject.tag == "MainPlayer")
        {
            mainPlayerController = collision.gameObject.GetComponent<MainPlayerController>(); //Topun kalmad�ysa reklam alan� geldi�inde arkada s�rekli ko�maya devam etmesin dursun.
            PlayerAnimations = collision.gameObject.GetComponentsInChildren<Animator>();
            if (isEnteredMainPlayer)
            {
                enteringPosition = new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y, collision.gameObject.transform.position.z +0.01f);
                isEnteredMainPlayer = false;
            }
            

        }
        
        numberOfBall = gameManager.getBallCount(); //gameManager'a bir get methodu olu�turuldu. B�ylelikle private de�eri get ile buradaki numberofball'a �ekiyoruz.
        if (doorstatus.actualDoorStatus == false && collision.gameObject.tag == "MainPlayer" && numberOfBall>0) //E�er kap� kapal� ve kap�n�n �n�ne mainplayer geldiyse
            // top say�s�n� kontrol et. E�er topu kald�ysa hen�z elenmemi�. �ut �ekmesini bekle.
        {
            PlayerAnimations[0].SetTrigger("Stop");
            PlayerAnimations[0].SetBool("Stop" , true); //MainPlayerdaki ko�ma animasyonunu durdurup idle animasyonunu ba�lat�r. Condition'�n ad� stop yoksa default bi�i yok.
            PlayerAnimations[1].SetTrigger("StopDripling"); //MainPlayerdaki top sektirme animasyonunu durdurur. Bu da default de�il transitionlar aras�ndaki condition.
            

        }
    }

    private IEnumerator OnCollisionStay(Collision collision) //Kap�n�n �n�nde beklerken top say�s� s�rekli kontrol edilsin. E�er yeterli top yoksa game over.
    {
        numberOfBall = gameManager.getBallCount();
        if (doorstatus.actualDoorStatus == false && collision.gameObject.tag == "MainPlayer" && numberOfBall >0) //kap� kapal�, topum var
        {
            PlayerAnimations[0].SetBool("Stop", true);
            PlayerAnimations[0].ResetTrigger("RunCondition");
            PlayerAnimations[1].SetTrigger("StopDripling");
            isRunning = false; //hareket etme.
            mainPlayerController.SetIsRunning(isRunning);

            if (collision.transform.position.z - enteringPosition.z > 0.5)
            {
                PlayerAnimations[0].SetTrigger("StopTrigger");

                PlayerAnimations[0].SetTrigger("DieCondition");
                    
                if (!impactParticle[1].isPlaying)
                {
                    sounds[1].Play();
                    impactParticle[1].Play();
                }
                
            }
            //collision.transform.position = Vector3.Lerp(collision.transform.position, new Vector3(collision.transform.position.x, enteringPosition.y, enteringPosition.z), Mathf.PingPong(Time.time * 0.5f, 1.0f));
            collision.transform.position = new Vector3(collision.transform.position.x, enteringPosition.y, enteringPosition.z);

            PlayerAnimations[0].ResetTrigger("DieCondition");
        }

        

        else if (doorstatus.actualDoorStatus == false && collision.gameObject.tag == "MainPlayer" && numberOfBall == 0) //kap� kapal�, topum yok, game over
        {
            yield return new WaitForSeconds(1);
            if(doorstatus.actualDoorStatus == false)
            {
                gameManager.GameOver();
                isRunning = false;
                mainPlayerController.SetIsRunning(isRunning);
            }
             
            
        }

        if (doorstatus.actualDoorStatus == true && collision.gameObject.tag == "MainPlayer")    //kap� a��l�rsa ko�maya ba�la. 
        {

            PlayerAnimations[0].ResetTrigger("StopTrigger");
            PlayerAnimations[0].SetBool("Stop", false);
            PlayerAnimations[0].SetTrigger("RunCondition"); //MainPlayerdaki ko�ma animasyonunu ba�lat�r.
            if (numberOfBall != 0)              //topum varsa
            {
                PlayerAnimations[1].SetTrigger("StartDripling"); //MainPlayerdaki top sektirme animasyonunu ba�lat�r. 
            }
            
            isRunning = true;
            mainPlayerController.SetIsRunning(isRunning);
            
        }

        //Sorun tek topun  varken basket atsan dahi game over olman. 

    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "MainPlayer")
        {
            PlayerAnimations[0].ResetTrigger("StopTrigger");
            PlayerAnimations[0].SetBool("Stop", false);
            PlayerAnimations[0].SetTrigger("RunCondition"); //MainPlayerdaki ko�ma animasyonunu ba�lat�r.
            if (numberOfBall != 0)              //topum varsa
            {
                PlayerAnimations[1].SetTrigger("StartDripling"); //MainPlayerdaki top sektirme animasyonunu ba�lat�r. 
            }

            isRunning = true;
            mainPlayerController.SetIsRunning(isRunning);
        }
        
    }


}
