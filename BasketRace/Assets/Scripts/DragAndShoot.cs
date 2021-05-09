using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndShoot : MonoBehaviour
{
    private Vector3 mousePressDownPos;
    private Vector3 mouseReleasePos;
    private Vector3 atisegimi;

    private Rigidbody rb;
    private bool isShoot=false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

    }
    
    private void OnMouseDown()
    {
        mousePressDownPos = Input.mousePosition;
    }
    private void OnMouseDrag() //Mouse colliderýn üzerine týklý bekliyor. Daha kaldýrmamýþ.
    {
        

        Vector3 forceInit = (Input.mousePosition - mousePressDownPos);
        Vector3 forceV = (new Vector3(forceInit.x, forceInit.y, forceInit.y)) * forceMultiplier;

        if (!isShoot)
            DrawTrajectory.Instance.UpdateTrajectory(forceV, rb, transform.position);
    }

    private void OnMouseUp() //fareyi býraktýðýnda
    {
        

        DrawTrajectory.Instance.HideLine();
        mouseReleasePos = Input.mousePosition;
        atisegimi = mouseReleasePos - mousePressDownPos;
        
        if (atisegimi.y < 0)
        {
            atisegimi.y = atisegimi.y * -1;
        }
        //atisegimi.x = atisegimi.x * -1;
        
        Shoot(atisegimi);

    }

    
    private float forceMultiplier = 1;

    void Shoot (Vector3 Force)
    {
        if (isShoot)
            return;

        rb.AddForce(new Vector3 (Force.x, (Force.y*1.05f), (Force.y*0.6f)) * forceMultiplier);
        isShoot = true;
        Spawner.Instance.NewSpawnRequest();

    }
    
}
