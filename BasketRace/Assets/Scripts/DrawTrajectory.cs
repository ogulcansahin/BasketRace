using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTrajectory : MonoBehaviour
{
    

    [SerializeField]
    private LineRenderer _lineRenderer; //�izgi �ekmek i�in kullanaca��z.
    private int _lineSegmentCount = 200; //Aimdeki do�rusal �izgi say�s�.
    private int _linePointCount =199;
    private List<Vector3> _linePoints = new List<Vector3>(); //�izgideki noktalar�n koordinatlar�

    #region Singleton

    public static DrawTrajectory Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion




    public void UpdateTrajectory (Vector3 forceVector, Rigidbody rigidBody, Vector3 startingPoint)
    {
       
        if (forceVector.y < 0)
        {
            forceVector.x = forceVector.x * -1;
        }
        //fnet = m.V/t = forceVector*fixedDeltaTime/m=Velocity
        Vector3 velocity = (forceVector / rigidBody.mass) * Time.fixedDeltaTime;
        float FlightDuration = (2 * velocity.y) / Physics.gravity.y; //U�u� s�resi form�l�
        float stepTime = FlightDuration / _lineSegmentCount; //Aimin bir noktas�ndan bir noktaya olan �izgiyi ne kadar s�rede ge�ece�ini hesapl�yor.
        _linePoints.Clear(); //�lk at��ta i�i bo� oldu�u i�in �nemli de�il sonraki at��larda bir �nceki at��tan kalan nokta koordinatlar�n� temizler.
        _linePoints.Add(startingPoint);

        

        for (int i=1; i<_linePointCount; i++)
        {
            float stepTimePassed = stepTime * i; //zaman i�indeki de�i�imi
            
            Vector3 MovementVector = new Vector3(
                velocity.x * stepTimePassed,
                velocity.y * stepTimePassed - 0.5f * Physics.gravity.y * stepTimePassed * stepTimePassed,
                velocity.z * stepTimePassed*0.6f);


            Vector3 NewPointOnline = startingPoint - MovementVector ;

            RaycastHit hit;
            // Raycast(orjin,gidece�i nokta, bir collidera �arparsa onu farkettiriyor, maksimum range)
            // Raycast k�sm� sadece line renderer�n ucunun bir colliderla �arp��t��� zaman kesilme i�lemini ger�ekle�tiriyor.
            if (Physics.Raycast(_linePoints[i - 1], NewPointOnline - _linePoints[i - 1], out hit, (NewPointOnline - _linePoints[i - 1]).magnitude))
            {
                _linePoints.Add(hit.point);

                break;
            }
            
                _linePoints.Add(NewPointOnline);
           
        }

        _lineRenderer.positionCount = _linePoints.Count;
        _lineRenderer.SetPositions(_linePoints.ToArray());
    }

    public void HideLine()
    {
        _lineRenderer.positionCount = 0;
    }
}
