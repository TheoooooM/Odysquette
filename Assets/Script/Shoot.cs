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

    public float x;
    public float y;

    private void Start()
    {
        GunPos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log("rigidbody2D: "+ Parent.GetComponent<Rigidbody2D>().position + "lookdir : " + lookDir + "angle: "+ angle);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 position = new Vector3(transform.position.x+x, transform.position.y+y, 0);
            Instantiate(Bullet, Spawner.transform.position, transform.rotation);
            Debug.Log("piou");
        }
        //transform.position = GunPos;
    }

    private void FixedUpdate()
    {
        _lookDir = new Vector2(mousepos.x, mousepos.y) - Parent.GetComponent<Rigidbody2D>().position ;
        angle = Mathf.Atan2(_lookDir.y, _lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        
    }
}