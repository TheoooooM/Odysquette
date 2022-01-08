using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Transform spawn;
    public Transform circle;
    public Transform square;
    
    
    private LineRenderer line;
    
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, spawn.position);
        Vector2 dir = (GameManager.Instance.mousepos - new Vector2(spawn.position.x, spawn.position.y));
        RaycastHit2D hit = Physics2D.Raycast(spawn.position, dir.normalized);
        line.SetPosition(1, hit.point);

        if (GameManager.Instance.firstEffect == GameManager.Effect.bouncing ||
            GameManager.Instance.secondEffect == GameManager.Effect.bouncing)
        {
            line.positionCount = 3;
            Vector2 secondDir = Vector2.Reflect(dir.normalized, hit.normal);
            RaycastHit2D secondHit = Physics2D.Raycast(hit.point, secondDir.normalized);
            line.SetPosition(2, secondHit.point);

            square.position = secondHit.point;
        }
        else line.positionCount = 2;
        
        
        circle.position = hit.point;
    }
}
