using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CurveBullet : Bullet {
    public List<PointsForBezierCurve> pointsForBezierCurve;
    public List<Vector3> trajectories;
    public int stepOfCurve;
    public int currentWaypoint;

    private int currentStepPoint;
    public List<Vector3> currentListWaypoint = new List<Vector3>();
    Vector3 currentDirection = new Vector3();
    public float speed;
    public bool isCurve;
    public bool isParameterTrajectories;


    public override void OnEnable() {
        base.OnEnable();
        for (int i = 0; i < trajectories.Count; i++) {
            trajectories[i] += transform.position;
        }

        lastpos = transform.position;

        transform.position = trajectories[0];

        for (int i = 0; i < trajectories.Count; i++) {
            if (i + 1 < trajectories.Count) {
                i++;
            }
            else {
                break;
            }

            PointsForBezierCurve a = new PointsForBezierCurve();
            PointsForBezierCurve b = new PointsForBezierCurve();
            a.pointsForBezierCurve = new List<Vector3>();
            b.pointsForBezierCurve = new List<Vector3>();
            pointsForBezierCurve.Add(a);
            pointsForBezierCurve.Add(b);

            Vector3 Start = trajectories[i - 1];

            Vector3 End = trajectories[i + 1];

            for (int j = 0;
                j < stepOfCurve;
                j++) {
                Vector3 StepPoint =
                    ExtensionMethods.Bezier(Start, trajectories[i], End, (j / (float) stepOfCurve));
                Start = StepPoint;

                pointsForBezierCurve[i].pointsForBezierCurve.Add(StepPoint);
            }
        }

        currentWaypoint = 1;
        currentStepPoint = 1;
        currentListWaypoint.AddRange(pointsForBezierCurve[currentWaypoint].pointsForBezierCurve);

        currentDirection = new Vector3(currentListWaypoint[currentStepPoint].x - currentListWaypoint[currentStepPoint - 1].x,
            currentListWaypoint[currentStepPoint].y - currentListWaypoint[currentStepPoint - 1].y,
            currentListWaypoint[currentStepPoint].z - currentListWaypoint[currentStepPoint - 1].z).normalized;

        isCurve = true;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos() {
        for (int i = 1; i < pointsForBezierCurve[1].pointsForBezierCurve.Count; i++) {
            Handles.DrawLine(pointsForBezierCurve[1].pointsForBezierCurve[i - 1],
                pointsForBezierCurve[1].pointsForBezierCurve[i]);
        }

        for (int i = 1; i < pointsForBezierCurve[3].pointsForBezierCurve.Count; i++) {
            Handles.DrawLine(pointsForBezierCurve[3].pointsForBezierCurve[i - 1],
                pointsForBezierCurve[3].pointsForBezierCurve[i]);
        }

        Vector3 vector3 = transform.position;
        Handles.DrawLine(transform.position, new Vector3(transform.position.x + rb.velocity.x, transform.position.y + rb.velocity.y, 0));
    }
#endif
    void FixedUpdate()
    {  if (isBounce) return;
        if (!isCurve) return;
        rb.velocity = (currentDirection) * speed;
    }
    private void OnDisable() {
        isCurve = false;
        trajectories = new List<Vector3>();

        pointsForBezierCurve = new List<PointsForBezierCurve>();


        currentListWaypoint = new List<Vector3>();
    }

    private Vector2 lastpos;
    public override void Update() {
        base.Update(); 
        if (isBounce) return;
        if (!isCurve) return;
        CheckWayPointInterpolate();
        
        lastpos = rb.position;
    }

    void GenerateInterpolate(out List<Vector2> interPolate)
    {
         interPolate = new List<Vector2>();
        var direction = (Vector2)transform.position - lastpos;
        int interval = (int)(direction.magnitude / 0.005f);
        for (int i = 0; i < interval; i++)
        {
           // Debug.Log(lastpos+i*direction/interval);
            interPolate.Add(lastpos+i*direction/interval);
            
        }  
       
    }

    void CheckWayPointInterpolate()
    {
        GenerateInterpolate(out List<Vector2> interPolate);
        for (int i = 0; i < interPolate.Count-1; i++)
        {
            
            Debug.Log(i);
           if( CheckWaypoint(interPolate[i]))
           {
               break;

           }
               
        }
       
        
    }
    
     bool CheckWaypoint(Vector2 currentInterpolate)
    {
        
                var _nextStepPoint = currentListWaypoint[currentStepPoint];   
                var _currentStepPoint = currentListWaypoint[currentStepPoint-1];
                if(Vector2.Distance(currentInterpolate, _nextStepPoint) < 0.1f)
                Debug.Log($"distanceStepPoint {Vector2.Distance(currentInterpolate, _nextStepPoint)}");
                if (Vector2.Distance(currentInterpolate, _nextStepPoint) < 0.05f) {
                    Debug.LogWarning($"distanceDetection {Vector2.Distance(currentInterpolate, _nextStepPoint)}");
                    
                            if (_nextStepPoint == currentListWaypoint[currentListWaypoint.Count - 1] && pointsForBezierCurve[currentWaypoint] != pointsForBezierCurve[pointsForBezierCurve.Count - 1]) {
                                currentWaypoint += 2;
                                currentStepPoint = 1;
                                currentListWaypoint.Clear();
                                currentListWaypoint.AddRange(pointsForBezierCurve[currentWaypoint].pointsForBezierCurve);
                                currentDirection = new Vector3(_nextStepPoint.x - _currentStepPoint.x,
                                    _nextStepPoint.y - _currentStepPoint.y,
                                    _nextStepPoint.z - _currentStepPoint.z).normalized;
                                rb.position = _currentStepPoint;
                                return true;


                            }
        
                            else {
                                if (_nextStepPoint == currentListWaypoint[currentListWaypoint.Count - 1]) {
                                    speed = 0;
                                    
                                    gameObject.SetActive(false);
                                 
                                    if (rateMode == StrawSO.RateMode.Ultimate)
                                        PoolManager.Instance.poolDictionary[GameManager.Instance.actualStraw][1].Enqueue(gameObject);
                                    else {
                                        PoolManager.Instance.poolDictionary[GameManager.Instance.actualStraw][0].Enqueue(gameObject);
                                    }
        
                                    trajectories = new List<Vector3>();
        
                                    pointsForBezierCurve = new List<PointsForBezierCurve>();
        
        
                                    currentListWaypoint = new List<Vector3>();
                                }
                                else {
                                    currentStepPoint++;
                                    Debug.Log("testje lis ça");
                                    rb.position = _currentStepPoint;
                                    currentDirection = new Vector3(_nextStepPoint.x - _currentStepPoint.x,
                                        _nextStepPoint.y - _currentStepPoint.y,
                                        _nextStepPoint.z - _currentStepPoint.z).normalized;
                                    return true;
                                   
                                }
                                
                            }
                        }
                
                    
                 
                        return false;         
                               
                             
                         

    }
}
