using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTekrar : MonoBehaviour
{
    [Range(5, 35)]
    public float hiz = 15;

    private Rigidbody fizikilehareketlenme;
    void Start()
    {
        fizikilehareketlenme = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float sagsolhareketet = Input.GetAxis("Horizontal");
        float yukariasagihareket = Input.GetAxis("Vertical");
        float ziplama = Input.GetAxis("Jump");

        //transform.Translate(new Vector3(sagsolhareketet, ziplama, yukariasagihareket) * hiz * Time.deltaTime); //Fizik olmadan hareket
        //Vector3 fizik = new Vector3(sagsolhareketet, ziplama, yukariasagihareket);
        //fizikilehareketlenme.AddForce(fizik * hiz); //fizik ile hareketlenme

        //if (Input.GetMouseButton(0))
        //{
        //    Debug.Log("Mouse sol týk.");

        //}

        //if (Input.GetMouseButton(1))
        //{
        //    Debug.Log("Mouse sað týk.");

        //}

        //if (Input.GetMouseButton(2))
        //{
        //    Debug.Log("Mouse mis týk.");

        //}
        //if (Input.GetKey("a")) //A ya basýldýðýnda input alýr. Basýlý tutunca sürekli if'in içindeki gerçekleþir.
        //{
        //    Debug.Log("a ya basýldý");

        //}

        //if (Input.GetKeyDown("a")) //A ya basýldýðýnda input alýr. Basýlý tuttuðunu anlamaz.
        //{
        //    Debug.Log("a ya basýldý");

        //}

        //if (Input.GetKeyUp("a")) //A býrakýldýðý an çalýþýr bir kez çalýþýr.
        //{
        //    Debug.Log("a býrakýldý");

        //}

        //if (Input.anyKey) //Herhangi bir þeye basýlmýþ mý diye bakýyor basýlý tutulduðunda sürekli yazýyor..
        //{
        //    Debug.Log("Input alýndý");

        //}

        if (Input.anyKeyDown)//Herhangi bir þeye basýlmýþ mý diye bakýyor. Basýlý tutsan da ilk giriþte yazýyor.
        {
            Debug.Log("Girisi aldim basili tutsan farketmez.");

            
        }

    }
}
