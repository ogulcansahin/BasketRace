using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;

    private Vector3 cameraOffset;

    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;
    void Start()
    {
        cameraOffset = transform.position - player.position;
    }
    

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newpos = player.position + cameraOffset;
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, newpos.y,newpos.z), SmoothFactor);
    }
}
