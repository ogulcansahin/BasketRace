using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTrajectory : MonoBehaviour
{
    

    [SerializeField]
    private LineRenderer _lineRenderer; //çizgi çekmek için kullanacaðýz.
    private int _lineSegmentCount = 200; //Aimdeki doðrusal çizgi sayýsý.
    private int _linePointCount =199;
    private List<Vector3> _linePoints = new List<Vector3>(); //çizgideki noktalarýn koordinatlarý

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
        float FlightDuration = (2 * velocity.y) / Physics.gravity.y; //Uçuþ süresi formülü
        float stepTime = FlightDuration / _lineSegmentCount; //Aimin bir noktasýndan bir noktaya olan çizgiyi ne kadar sürede geçeceðini hesaplýyor.
        _linePoints.Clear(); //Ýlk atýþta içi boþ olduðu için önemli deðil sonraki atýþlarda bir önceki atýþtan kalan nokta koordinatlarýný temizler.
        _linePoints.Add(startingPoint);

        

        for (int i=1; i<_linePointCount; i++)
        {
            float stepTimePassed = stepTime * i; //zaman içindeki deðiþimi
            
            Vector3 MovementVector = new Vector3(
                velocity.x * stepTimePassed,
                velocity.y * stepTimePassed - 0.5f * Physics.gravity.y * stepTimePassed * stepTimePassed,
                velocity.z * stepTimePassed*0.6f);


            Vector3 NewPointOnline = startingPoint - MovementVector ;

            RaycastHit hit;
            // Raycast(orjin,gideceði nokta, bir collidera çarparsa onu farkettiriyor, maksimum range)
            // Raycast kýsmý sadece line rendererýn ucunun bir colliderla çarpýþtýðý zaman kesilme iþlemini gerçekleþtiriyor.
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
