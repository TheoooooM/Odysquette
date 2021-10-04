using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    [SerializeField] float MouvementSpeed = 0.01f;
    public GameObject gun;
    

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.D))
        {
            GetComponent<Rigidbody2D>().position += new Vector2(MouvementSpeed*0.2f, 0);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            GetComponent<Rigidbody2D>().position += new Vector2(-MouvementSpeed*0.2f, 0);
        }
        if (Input.GetKey(KeyCode.Z))
        {
            GetComponent<Rigidbody2D>().position += new Vector2(0, MouvementSpeed*0.2f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            GetComponent<Rigidbody2D>().position += new Vector2(0, -MouvementSpeed*0.2f);
        }

    }
}
