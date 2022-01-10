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
    [SerializeField] private float maxLength;
    [SerializeField] private float yPlus;
    
    private bool goToExit;
    private Vector3 destinationToExit;
    private bool nextToPlayer;
    private const string idleWithParcel = "DRONE_IDLE_PARCEL";
    private const string idleWithoutParcel = "DRONE_IDLE_NONE";
    private const string launchParcel = "DRONE_LAUNCHPARCEL";
    private bool canMove;
    private bool beginLaunch;
    private void OnEnable()
    {
        
        AudioManager.Instance.PlayPlayerSound(AudioManager.PlayerSoundEnum.Drone);
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

        posForLaunchParcel = Playercontroller.Instance.transform.position + new Vector3(0, yPlus, 0);
        transform.position = SetRandomPosition();
    }

    private void OnDisable()
    {
        canMove = false;
        goToExit = false; 
    }

    Vector3 SetRandomPosition()
    {
        Debug.Log("testaa");
        Vector2 randomDirection = Random.insideUnitCircle;
        float randomLength = Random.Range(minLength, maxLength);
        return posForLaunchParcel + (Vector3) randomDirection * randomLength;
    }

    private void Update()
    {
        if (!goToExit)
        {
            if (!beginLaunch)
            {
                
                Debug.Log("testaa");
            if (GoToDestination(posForLaunchParcel)) ;
            else
            {
                Debug.Log("testaa");
                CheckIfParcel();
            }
        }
    }
        else
        {
            if (canMove)
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
    }
    

    void CheckIfParcel()
    {
        if(withParcel)
            LaunchParcel();
        else
        {
              SetExit();
                    canMove = true;
        }
          
       
    }
    
   bool GoToDestination(Vector3 destination)
   {
       Debug.Log(Vector3.Distance(transform.position, destination));
       if (Vector3.Distance(transform.position, destination) > .05f) 
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
        beginLaunch = true;
        animator.Play(launchParcel);
       
        
        
    }

    public void InstantiateParcel()
    {
        Instantiate(chest, posForLaunchParcel, Quaternion.identity);
        SetExit();
    }

    public void CanMove()
    {
        beginLaunch = false;
        canMove = true;
    }

  public  void SetExit()
    {
        destinationToExit = SetRandomPosition();
        goToExit = true;
        
    }
    
  
}
