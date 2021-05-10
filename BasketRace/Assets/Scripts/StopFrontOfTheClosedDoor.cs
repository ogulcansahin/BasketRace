using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopFrontOfTheClosedDoor : MonoBehaviour
{   
    //Startdooranim kapý prefablarýnda yer alýyor. 
    //Ýçerisinde sol kapý ve sað kapýnýn animasyon dosyalarý ve kapýnýn açýk veya kapalý olup olmadýðý bilgisi yer alýyor. Bunu yakaladýk çünkü player kapý kapalýysa
    //kapýnýn önünde açýlana dek beklemeli açýksa devam etmeli. 
    startdooranim doorstatus; 

    Animator[] PlayerAnimations; //Karakterin top sektirme ve koþma animasyonlarý alýndý. Array olarak yaratýldý çünkü iki animasyon dosyasý var.

    //Mainplayerýn alýnmasýnýn sebebi: Karakterin pozisyonunun ileri doðru deðiþimi main playerdan kontrol ediliyor.
    //Burada mainplayer çekilerek kapý kapalýyken durmasý kapý açýldýðýnda hareket etmesi saðlanýr.
    MainPlayerController mainplayer;

    //GameManager scriptinde top sayýsý yer alýyor. Eðer kapý kapalý ve kapýnýn önüne gelindiðinde top sayýsý sýfýrsa game over olmasý lazým.
    //Bu ayarýn kontrolü için gameManager alýndý. 
    GameManager gameManager; 

    bool isRunning; //Mainplayerdaki hareket durumunu burada bir bool deðiþkene atamak için oluþturduk.
    int numberOfBall; //GameManagerdaki top sayýsýný burada bir deðere atamak için kullandýk. 
    void Start()
    {
        doorstatus = GetComponentInParent<startdooranim>(); //Kapýnýn açýk olup olmadýðýyla ilgili script çekildi.
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>(); //Top sayýsýný yönetmek için gameManager scripti çekildi.
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision) //Ýlk çarpýþma anýnda basket olmadan animasyonlar durdurulsun ve çarpýþanýn main player olduðu kontrol edilsin diye
        //Örneðin rakibin attýðý top da collisiona girebilir.
    {
        if (collision.gameObject.tag == "MainPlayer")
        {
            mainplayer = collision.gameObject.GetComponent<MainPlayerController>(); //Topun kalmadýysa reklam alaný geldiðinde arkada sürekli koþmaya devam etmesin dursun.
            PlayerAnimations = collision.gameObject.GetComponentsInChildren<Animator>();
        }
        
        numberOfBall = gameManager.getBallCount(); //gameManager'a bir get methodu oluþturuldu. Böylelikle private deðeri get ile buradaki numberofball'a çekiyoruz.
        if (doorstatus.actualDoorStatus == false && collision.gameObject.tag == "MainPlayer" && numberOfBall>0) //Eðer kapý kapalý ve kapýnýn önüne mainplayer geldiyse
            // top sayýsýný kontrol et. Eðer topu kaldýysa henüz elenmemiþ. Þut çekmesini bekle.
        {
            
            PlayerAnimations[0].SetTrigger("Stop"); //MainPlayerdaki koþma animasyonunu durdurup idle animasyonunu baþlatýr. Condition'ýn adý stop yoksa default biþi yok.
            PlayerAnimations[1].SetTrigger("Stop"); //MainPlayerdaki top sektirme animasyonunu durdurur. Bu da default deðil transitionlar arasýndaki condition.
            

        }
    }

    private void OnCollisionStay(Collision collision) //Kapýnýn önünde beklerken top sayýsý sürekli kontrol edilsin. Eðer yeterli top yoksa game over.
    {
        numberOfBall = gameManager.getBallCount();
        if (doorstatus.actualDoorStatus == false && collision.gameObject.tag == "MainPlayer" && numberOfBall >0) //kapý kapalý, topum var
        {
            isRunning = false; //hareket etme.
            mainplayer.SetIsRunning(isRunning);
        }

        

        else if (doorstatus.actualDoorStatus == false && collision.gameObject.tag == "MainPlayer" && numberOfBall == 0) //kapý kapalý, topum yok, game over
        {
            
            Debug.Log("GameOver");
            isRunning = false;
            mainplayer.SetIsRunning(isRunning);
            PlayerAnimations[0].SetTrigger("Stop");  
            
        }

        if (doorstatus.actualDoorStatus == true && collision.gameObject.tag == "MainPlayer")    //kapý açýlýrsa koþmaya baþla. 
        {
            
            PlayerAnimations[0].SetTrigger("RunCondition"); //MainPlayerdaki koþma animasyonunu baþlatýr.
            if (numberOfBall != 0)              //topum varsa
            {
                PlayerAnimations[1].SetTrigger("StartDripling"); //MainPlayerdaki top sektirme animasyonunu baþlatýr. 
            }
            
            isRunning = true;
            mainplayer.SetIsRunning(isRunning);
            
        }

        //Sorun tek topun  varken basket atsan dahi game over olman. 
    }

}
