using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Again : MonoBehaviour
{

    #region Variables

    private Transform[,] map;
    [SerializeField] private int mapSize;
    [SerializeField] private float padding = 1.2f;
    [SerializeField]private GameObject room;
    [SerializeField] private int walkTimes;
    private Transform currentRoom;
    private Vector2 pos;
    
    int opposite;
    
    
     
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        map = new Transform[mapSize, mapSize];
        
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                GameObject GO = Instantiate(room);
                map[x, y] = GO.transform;
                GO.transform.position = new Vector2(x * padding, y * padding);
            }
        }
        pos = new Vector2(mapSize / 2, mapSize / 2);
        currentRoom = map[(int)pos.x, (int)pos.y];
        currentRoom.GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Path(walkTimes);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPath();
        }
    }

    void ResetPath()
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                map[x,y].GetComponent<SpriteRenderer>().color = Color.white;
                pos = new Vector2(mapSize / 2, mapSize / 2);
                currentRoom = map[(int)pos.x, (int)pos.y];
                currentRoom.GetComponent<SpriteRenderer>().color = Color.yellow;
                Debug.ClearDeveloperConsole();
            }
        }
    }
    
    void Path(int walkTime)
    {
        Debug.Log("Path");
        for (int i = 0; i < walkTime; i++)
        {
            
            int dir = Random.Range(0, 3);
            if (dir != opposite || i == 0)
            {
                switch (dir)
                {
                    case 0:
                        if (pos.x + 1 < map.GetLength(0)) pos.x++;
                        else i--;
                        opposite = 2;
                        break;

                    case 1:
                        if (pos.y + 1 < map.GetLength(1)) pos.y++;
                        else i--;
                        opposite = 3;
                        break;

                    case 2:
                        if (pos.x - 1 >= 0) pos.x--;
                        else i--;
                        opposite = 0;
                        break;

                    case 3:
                        if (pos.y - 1 >= 0) pos.y--;
                        else i--;
                        opposite = 1;
                        break;
                }
            }
            else i--;
            
            Debug.Log(pos);
            if (currentRoom != map[(int) pos.x, (int) pos.y])
            {
                currentRoom = map[(int) pos.x, (int) pos.y];
                currentRoom.GetComponent<SpriteRenderer>().color = Color.green;
            }
            //else Debug.Log("try again");

        }
    }
}
