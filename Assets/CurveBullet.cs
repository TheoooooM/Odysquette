using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveBullet : ScriptBullet
{
    private List<PointsForBezierCurve> pointsForBezierCurve;
    public List<Vector3> trajectories = new List<Vector3>();
    public int stepOfCurve;
    private int currentWaypoint;
    private  int  currentStepPoint;
    private List<Vector3> currentListWaypoint = new List<Vector3>();
    Vector3 currentDirection = new Vector3();
   public float speed;
  private void Start()
  {
      pointsForBezierCurve = new List<PointsForBezierCurve>();
      for (int i = 1; i < trajectories.Count+1; i += 2)
      {
          Vector3 Start = trajectories[i - 1];
          Vector3 End = trajectories[i + 1];
          for (int j = 1; j <= stepOfCurve; 
              j++) {
              Vector3 StepPoint = ExtensionMethods.Bezier(Start , trajectories[i], End, (j / (float) stepOfCurve)) ;
             pointsForBezierCurve[i].pointsForBezierCurve.Add(StepPoint);

          }

               

      }
      currentListWaypoint.AddRange(pointsForBezierCurve[currentWaypoint].pointsForBezierCurve);
      currentWaypoint = 1 ;
      currentStepPoint = 0;
      currentDirection = new Vector3(currentListWaypoint[currentStepPoint].x - currentListWaypoint[currentStepPoint - 1].x,
              currentListWaypoint[currentStepPoint].y - currentListWaypoint[currentStepPoint - 1].y,
              currentListWaypoint[currentStepPoint].z - currentListWaypoint[currentStepPoint - 1].z).normalized;


  }

  private void Update()
  {
      if (Vector3.Distance(transform.position, currentListWaypoint[currentStepPoint]) < 0.3f)
      {
          if (Vector3.Distance(transform.position, trajectories[currentWaypoint]) < 0.3f)
          {
            
              currentWaypoint++;
              currentStepPoint = 0;  
              currentListWaypoint.Clear();
              currentListWaypoint.AddRange(pointsForBezierCurve[currentWaypoint].pointsForBezierCurve);
              currentDirection = new Vector3(currentListWaypoint[currentStepPoint].x - currentListWaypoint[currentStepPoint - 1].x,
                  currentListWaypoint[currentStepPoint].y - currentListWaypoint[currentStepPoint - 1].y,
                  currentListWaypoint[currentStepPoint].z - currentListWaypoint[currentStepPoint - 1].z).normalized;
              
          }
          else
          {
              currentStepPoint++;
              currentDirection = new Vector3(currentListWaypoint[currentStepPoint].x - currentListWaypoint[currentStepPoint - 1].x,
                  currentListWaypoint[currentStepPoint].y - currentListWaypoint[currentStepPoint - 1].y,
                  currentListWaypoint[currentStepPoint].z - currentListWaypoint[currentStepPoint - 1].z).normalized;
           
          }
          
      }
  
  }

  private void FixedUpdate()
  {
      rb .velocity = currentDirection * speed;
  }
}
