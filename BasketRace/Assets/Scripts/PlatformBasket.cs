using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBasket : MonoBehaviour
{
    ParticleSystem basketEffect;
    GameObject slower;
    GameObject fastener;

    // Start is called before the first frame update
    void Start()
    {
        basketEffect = gameObject.GetComponentInChildren<ParticleSystem>();
        slower = transform.GetChild(0).gameObject;
        fastener = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(slower);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "BasketballOfPlayer")
        {
            basketEffect.Play();
            if (fastener.activeSelf)
            {
                fastener.SetActive(false);
                slower.SetActive(true);
            }
            else
            {
                fastener.SetActive(true);
                slower.SetActive(false);
            }
        }
        
    }
}
