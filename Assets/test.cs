using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class test : MonoBehaviour
{
    [SerializeField] private GameObject cube;
    [SerializeField] private float speed = 50f;

    private Vector3 bonsoirrrrr;
    private bool bon;

    private float angle = 180;

    private float angleDivision = 1;

    // Start is called before the first frame update
    
    private GameObject cubee;

    void Start()
    {


        /*   for (int i = 0; i < angleDivision+2; i++)
           {
               GameObject bullet = Instantiate(cube, transform.position, transform.rotation);
               Vector3 currentBasePosition = new Vector3();
               float currentAngle = -angle / 2 + (angle / angleDivision) * i;
               Vector3 rotation = Quaternion.Euler(0, currentAngle, 0) * transform.forward;
   
               currentBasePosition = bullet.transform.position;
   
              
               bullet.GetComponent<Rigidbody>().AddForce(rotation * speed);
               bon = false;
           }*/  for (int i = 0; i< 3; i++)
                  {
                     
                      StartCoroutine(catastrof());
                      Debug.Log("bonsoir" + i);
                  }
       
    }
//voir avec les pool poru mettre un objet or de la pool et lorsqu'il est dedans il peut se mettre dans un objet
    // Update is called once per frame
  


public  virtual void bonsoir()
    {
       
    }

  void addforce(GameObject bonsoir)
  {
      if (bon == true)
      {
            bonsoir.GetComponent<Rigidbody>().AddForce(transform.forward*speed);
           
      }
   
  }

  IEnumerator catastrof()
  {
     
       
          yield return new WaitForSeconds(2f);
      
  }
}
