using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndShoot : MonoBehaviour
{
    private Vector3 mousePressDownPos;
    private Vector3 mouseReleasePos;
    private Vector3 shootingDrag;
    private GameManager gameManager;
    private Rigidbody rb;
    private bool isShoot=false;
    private Vector3 maximumForce;
    private Vector3 mousePosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }
    
    private void OnMouseDown()
    {
        mousePressDownPos = Input.mousePosition;
    }
    private void OnMouseDrag() //Mouse colliderýn üzerine týklý bekliyor. Daha kaldýrmamýþ.
    {
        mousePosition = Input.mousePosition;

        if (maximumForce.y > 800f)
        {
            maximumForce.y = 800f;
        }
        
        Vector3 forceInit = (mousePosition - mousePressDownPos);
        

        Vector3 forceV = (new Vector3(forceInit.x, forceInit.y, forceInit.y)) * forceMultiplier;

        

        if (!isShoot)
            DrawTrajectory.Instance.UpdateTrajectory(forceV, rb, transform.position);
    }

    private void OnMouseUp() //fareyi býraktýðýnda
    {
        

        DrawTrajectory.Instance.HideLine();
        mouseReleasePos = Input.mousePosition;

        shootingDrag = mouseReleasePos - mousePressDownPos;
        
        if (shootingDrag.y < 0)
        {
            shootingDrag.y = shootingDrag.y * -1;
        }
        //atisegimi.x = atisegimi.x * -1;
        
        Shoot(shootingDrag);

    }

    
    private float forceMultiplier = 1;

    void Shoot (Vector3 Force)
    {
        if (isShoot)
            return;

        rb.AddForce(new Vector3 (Force.x, (Force.y*1.035f), (Force.y*0.6f)) * forceMultiplier);
        isShoot = true;
        Spawner.Instance.NewSpawnRequest();
        gameManager.updateBallCount(-1);
    }
    
}
