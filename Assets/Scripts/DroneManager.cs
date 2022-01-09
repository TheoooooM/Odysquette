using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using Random = UnityEngine.Random;

public class DroneManager : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    private bool withParcel;
    [SerializeField] private GameObject chest; 
    [SerializeField] private int chestProcentDrop = 80;
    private Vector3 posForLaunchParcel;
    [SerializeField]
    private float minLength;
    [SerializeField]
    private float maxLength;
    private bool goToExit;
    private Vector3 destinationToExit;
    private bool nextToPlayer;
    private const string idleWithParcel = "DRONE_IDLE_PARCEL";
    private const string idleWithoutParcel = "DRONE_IDLE_NONE";
    private const string launchParcel = "DRONE_LAUNCHPARCEL";
    private void OnEnable()
    {
        Debug.Log("testaa");
        int random = Random.Range(0, 100);
        if (chestProcentDrop > random)
        {
            withParcel = true;
            animator.Play(idleWithParcel);
        }
        else
        {
            withParcel = false;
            animator.Play(idleWithoutParcel);
        }
        transform.position = SetRandomPosition();
    }

    private void OnDisable()
    {
     
    }

    Vector3 SetRandomPosition()
    {
        Debug.Log("testaa");
        Vector2 randomDirection = Random.insideUnitCircle;
        float randomLength = Random.Range(minLength, maxLength);
        return randomDirection * randomLength; 
    }

    private void Update()
    {
        if (!goToExit)
        {
            Debug.Log("testaa");
            if( GoToDestination(Playercontroller.Instance.transform.position));
            else
            {
                Debug.Log("testaa");
                CheckIfParcel();
            }
        }
        else
        {
            Debug.Log("testaa");
            if(GoToDestination(destinationToExit));
            else
            {
                Debug.Log("testaa");
                gameObject.SetActive(false);
            }
        }
    }
    

    void CheckIfParcel()
    {
        if(withParcel)
            LaunchParcel();
        else
            SetExit();
       
    }
    
   bool GoToDestination(Vector3 destination)
    {
        if (transform.position != destination)
        {
            Debug.Log("testaa");
            transform.position =
                Vector3.MoveTowards(transform.position, destination, speed);
            return true;
        }

        return false;

    }

    void LaunchParcel()
    {
        animator.Play(launchParcel);
        posForLaunchParcel = Playercontroller.Instance.transform.position;
    }

    public void InstantiateParcel()
    {
        Instantiate(chest, posForLaunchParcel, Quaternion.identity);  
        
    }
    

  public  void SetExit()
    {
        destinationToExit = SetRandomPosition();
        goToExit = true;
    }
    
  
}
