using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowerTextureOffSet : MonoBehaviour
{
    private float ScrollX = 1f;
    void Update()
    {
        float OffsetX = Time.time * ScrollX;
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(OffsetX, 0);
    }
}
