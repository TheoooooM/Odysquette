using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    Vector2 _lookDir;
    public Vector2 lookDir => _lookDir;
    Vector3 GunPos;
    float angle;
    [SerializeField] GameObject Parent;
    Vector3 mousepos;
    [SerializeField] GameObject Bullet;
    [SerializeField] GameObject Spawner;
    [SerializeField] Playercontroller Playercontroller;

    [Header("Settings")]
    [SerializeField] int ShootRate;


    private void Start()
    {
        GunPos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log("rigidbody2D: "+ Parent.GetComponent<Rigidbody2D>().position + "lookdir : " + lookDir + "angle: "+ angle);

        /*if (Input.GetKey(KeyCode.Mouse0))
        {
            if (shootCooldown <= 0f)
            {
                Vector3 position = new Vector3(transform.position.x, transform.position.y, 0);
                Instantiate(Bullet, Spawner.transform.position, transform.rotation);
                Debug.Log("piou");
                shootCooldown = 1.5f;
            }
            shootCooldown -= Time.deltaTime*ShootRate;
        }*/

        //transform.position = GunPos;
    }

    private void FixedUpdate()
    {
        Vector2 Position = new Vector2(transform.position.x, transform.position.y);
        _lookDir = new Vector2(mousepos.x, mousepos.y) - Position ;
        angle = Mathf.Atan2(_lookDir.y, _lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        
    }
}