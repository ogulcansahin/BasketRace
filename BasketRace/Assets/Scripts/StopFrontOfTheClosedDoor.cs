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
    MainPlayerController mainPlayerController;

    //GameManager scriptinde top sayýsý yer alýyor. Eðer kapý kapalý ve kapýnýn önüne gelindiðinde top sayýsý sýfýrsa game over olmasý lazým.
    //Bu ayarýn kontrolü için gameManager alýndý. 
    GameManager gameManager;

    Vector3 enteringPosition;

    bool isRunning; //Mainplayerdaki hareket durumunu burada bir bool deðiþkene atamak için oluþturduk.
    int numberOfBall; //GameManagerdaki top sayýsýný burada bir deðere atamak için kullandýk. 

    private ParticleSystem[] impactParticle;
    void Start()
    {
        doorstatus = GetComponentInParent<startdooranim>(); //Kapýnýn açýk olup olmadýðýyla ilgili script çekildi.
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>(); //Top sayýsýný yönetmek için gameManager scripti çekildi.
        impactParticle = GameObject.FindWithTag("MainPlayer").GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision) //Ýlk çarpýþma anýnda basket olmadan animasyonlar durdurulsun ve çarpýþanýn main player olduðu kontrol edilsin diye
        //Örneðin rakibin attýðý top da collisiona girebilir.
    {
        if (collision.gameObject.tag == "MainPlayer")
        {
            mainPlayerController = collision.gameObject.GetComponent<MainPlayerController>(); //Topun kalmadýysa reklam alaný geldiðinde arkada sürekli koþmaya devam etmesin dursun.
            PlayerAnimations = collision.gameObject.GetComponentsInChildren<Animator>();
            enteringPosition = collision.gameObject.transform.position;

        }
        
        numberOfBall = gameManager.getBallCount(); //gameManager'a bir get methodu oluþturuldu. Böylelikle private deðeri get ile buradaki numberofball'a çekiyoruz.
        if (doorstatus.actualDoorStatus == false && collision.gameObject.tag == "MainPlayer" && numberOfBall>0) //Eðer kapý kapalý ve kapýnýn önüne mainplayer geldiyse
            // top sayýsýný kontrol et. Eðer topu kaldýysa henüz elenmemiþ. Þut çekmesini bekle.
        {
            PlayerAnimations[0].SetTrigger("Stop");
            PlayerAnimations[0].SetBool("Stop" , true); //MainPlayerdaki koþma animasyonunu durdurup idle animasyonunu baþlatýr. Condition'ýn adý stop yoksa default biþi yok.
            PlayerAnimations[1].SetTrigger("StopDripling"); //MainPlayerdaki top sektirme animasyonunu durdurur. Bu da default deðil transitionlar arasýndaki condition.
            

        }
    }

    private IEnumerator OnCollisionStay(Collision collision) //Kapýnýn önünde beklerken top sayýsý sürekli kontrol edilsin. Eðer yeterli top yoksa game over.
    {
        numberOfBall = gameManager.getBallCount();
        if (doorstatus.actualDoorStatus == false && collision.gameObject.tag == "MainPlayer" && numberOfBall >0) //kapý kapalý, topum var
        {
            PlayerAnimations[0].SetBool("Stop", true);
            PlayerAnimations[0].ResetTrigger("RunCondition");
            PlayerAnimations[1].SetTrigger("StopDripling");
            isRunning = false; //hareket etme.
            mainPlayerController.SetIsRunning(isRunning);

            if (collision.transform.position.z - enteringPosition.z > 0.5)
            {
                PlayerAnimations[0].SetTrigger("StopTrigger");
                while (collision.transform.position.z - enteringPosition.z >= 0.2f)
                {
                    Debug.Log("EnteringPosition" + enteringPosition.z);
                    Debug.Log("Transform" + collision.transform.position.z);
                    Debug.Log("Fark" + (collision.transform.position.z - enteringPosition.z));
                    isRunning = false;
                    mainPlayerController.SetIsRunning(isRunning);
                    PlayerAnimations[0].SetTrigger("DieCondition");
                    
                    if (!impactParticle[1].isPlaying)
                    {
                        impactParticle[1].Play();
                    }
                    yield return new WaitForSeconds(0.01f);
                    impactParticle[1].Stop();
                    collision.transform.Translate(collision.transform.forward * -0.5f * Time.deltaTime);
                }
            }
            
            
            PlayerAnimations[0].ResetTrigger("DieCondition");
        }

        

        else if (doorstatus.actualDoorStatus == false && collision.gameObject.tag == "MainPlayer" && numberOfBall == 0) //kapý kapalý, topum yok, game over
        {
            yield return new WaitForSeconds(1);
            if(doorstatus.actualDoorStatus == false)
            {
                Debug.Log("GameOver");
                PlayerAnimations[0].SetTrigger("Stop");
                isRunning = false;
                mainPlayerController.SetIsRunning(isRunning);
                PlayerAnimations[0].SetBool("Stop", true);
                PlayerAnimations[0].ResetTrigger("RunCondition");
            }
             
            
        }

        if (doorstatus.actualDoorStatus == true && collision.gameObject.tag == "MainPlayer")    //kapý açýlýrsa koþmaya baþla. 
        {

            PlayerAnimations[0].ResetTrigger("StopTrigger");
            PlayerAnimations[0].SetBool("Stop", false);
            PlayerAnimations[0].SetTrigger("RunCondition"); //MainPlayerdaki koþma animasyonunu baþlatýr.
            if (numberOfBall != 0)              //topum varsa
            {
                PlayerAnimations[1].SetTrigger("StartDripling"); //MainPlayerdaki top sektirme animasyonunu baþlatýr. 
            }
            
            isRunning = true;
            mainPlayerController.SetIsRunning(isRunning);
            
        }

        //Sorun tek topun  varken basket atsan dahi game over olman. 

    }
   

}
