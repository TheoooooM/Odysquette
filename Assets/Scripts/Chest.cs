using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chest : MonoBehaviour
{
    private bool canOpen;
    
    [SerializeField] private ChestDrop[] Drops;
    private float rate;
    private int index;
    private GameObject finalItem;

    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canOpen) Generate();
    }

    void Generate()
    {
        rate =Random.Range(0, 100);
        //Debug.Log("rate : " + rate);
        while (rate>=0)
        {
            //Debug.Log("index : " +index);
            finalItem = Drops[index].ennemy;
            rate -= Drops[index].spawnRate;
            index++;
            if (index == Drops.Length)
            {
                break;
            }
        }

        Instantiate(finalItem, transform.position, Quaternion.identity);
        Destroy(gameObject);
        
    }
    */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canOpen = true;
            sprite.color = Color.red;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canOpen = true;
            sprite.color = Color.white;
        }
    }
}

[System.Serializable]
public class ChestDrop {
    [SerializeField] private List<Items> itemsToDrop = new List<Items>();
}
