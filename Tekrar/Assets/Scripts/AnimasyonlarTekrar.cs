using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimasyonlarTekrar : MonoBehaviour
{
    private Color renk;
    private MeshRenderer meshimiz;
    void Start()
    {
        meshimiz = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void renkdegistir() // Bunu add event ile animasyona ekledik belirli bir anda kübün rengi deðiþiyor. 
    {
        meshimiz.material.color = new Color(0.5f, 1, 1);
    }
}
