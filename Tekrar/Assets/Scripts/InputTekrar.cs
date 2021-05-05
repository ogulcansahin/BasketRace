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
        //    Debug.Log("Mouse sol t�k.");

        //}

        //if (Input.GetMouseButton(1))
        //{
        //    Debug.Log("Mouse sa� t�k.");

        //}

        //if (Input.GetMouseButton(2))
        //{
        //    Debug.Log("Mouse mis t�k.");

        //}
        //if (Input.GetKey("a")) //A ya bas�ld���nda input al�r. Bas�l� tutunca s�rekli if'in i�indeki ger�ekle�ir.
        //{
        //    Debug.Log("a ya bas�ld�");

        //}

        //if (Input.GetKeyDown("a")) //A ya bas�ld���nda input al�r. Bas�l� tuttu�unu anlamaz.
        //{
        //    Debug.Log("a ya bas�ld�");

        //}

        //if (Input.GetKeyUp("a")) //A b�rak�ld��� an �al���r bir kez �al���r.
        //{
        //    Debug.Log("a b�rak�ld�");

        //}

        //if (Input.anyKey) //Herhangi bir �eye bas�lm�� m� diye bak�yor bas�l� tutuldu�unda s�rekli yaz�yor..
        //{
        //    Debug.Log("Input al�nd�");

        //}

        if (Input.anyKeyDown)//Herhangi bir �eye bas�lm�� m� diye bak�yor. Bas�l� tutsan da ilk giri�te yaz�yor.
        {
            Debug.Log("Girisi aldim basili tutsan farketmez.");

            
        }

    }
}
