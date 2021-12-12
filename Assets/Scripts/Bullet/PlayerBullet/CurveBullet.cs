using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CurveBullet : Bullet {
    public List<PointsForBezierCurve> pointsForBezierCurve;
    public List<Vector3> trajectories;
    public int stepOfCurve;
    public int currentWaypoint;
    public float speedBounce;
    private int currentStepPoint;
    public List<Vector3> currentListWaypoint = new List<Vector3>();

    public float speed;
    public bool isCurve;
    public bool isParameterTrajectories;
    private Vector2 currentDirection;

    public override void OnEnable() {
        base.OnEnable();
        for (int i = 0; i < trajectories.Count; i++) {
            trajectories[i] += transform.position;
        }

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

    private void OnDisable() {
        isCurve = false;
        trajectories = new List<Vector3>();

        pointsForBezierCurve = new List<PointsForBezierCurve>();


        currentListWaypoint = new List<Vector3>();
    }

    public override void Update() {
        base.Update();
        if (!isBounce && !colliding) {
            if (isCurve) {
               
                if (Vector3.Distance(transform.position, currentListWaypoint[currentStepPoint]) < 0.1f) {
                  
                    if (currentListWaypoint[currentStepPoint] == currentListWaypoint[currentListWaypoint.Count - 1] && pointsForBezierCurve[currentWaypoint] != pointsForBezierCurve[pointsForBezierCurve.Count - 1]) {
                        currentWaypoint += 2; 
                        currentStepPoint = 1;
                    
                        currentListWaypoint.Clear();
                        currentListWaypoint.AddRange(pointsForBezierCurve[currentWaypoint].pointsForBezierCurve);
                    }

                    else {
                      
                        if (currentListWaypoint[currentStepPoint] == currentListWaypoint[currentListWaypoint.Count - 1]) {
                            speed = 0;
                            Debug.Log("end curve");

                         
                            if (rateMode == StrawSO.RateMode.Ultimate)
                                PoolManager.Instance.poolDictionary[GameManager.Instance.actualStraw][1].Enqueue(gameObject);
                            else {
                                PoolManager.Instance.poolDictionary[GameManager.Instance.actualStraw][0].Enqueue(gameObject);
                            }

                            trajectories = new List<Vector3>();

                            pointsForBezierCurve = new List<PointsForBezierCurve>();


                            currentListWaypoint = new List<Vector3>();  gameObject.SetActive(false);
                            return;
                        }
                        else {
                            currentStepPoint++;
                            
                          
                        
                         
                        }
                    }
                   
                }
                if(((Vector2)currentListWaypoint[currentStepPoint]-rb.position).magnitude > 0.3f)
                    currentDirection = ((Vector2)currentListWaypoint[currentStepPoint]-rb.position).normalized;
                 rb.position = Vector2.MoveTowards(rb.position, currentListWaypoint[currentStepPoint], speed );
                 
            }
        }
      
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        colliding = true;
      
        if (_bounceCount > 0 && other.gameObject.CompareTag("Walls")) {
            
            _bounceCount--;
            
            Debug.Log(currentDirection);

            var direction = Vector3.Reflect(currentDirection, other.contacts[0].normal);
            rb.velocity = direction * Mathf.Max(speedBounce, 0f);
        
            var angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            transform.rotation = Quaternion.Euler(0, 0, angle);
            

            isBounce = true;
   
            PoolManager.Instance.SpawnImpactPool(transform);
        }
        else {
            DesactiveBullet();
        }
    }
}