using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chest : MonoBehaviour
{
    [SerializeField] public itemSO SO;
    [SerializeField] private ChestDrop[] Drops;
    
    
    
    private bool canOpen;
    private float rate;
    private int index;
    private Items.type finalItem;

    private SpriteRenderer sprite;

    public virtual void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canOpen) Generate();
    }

    virtual public void Generate()
    {
        Debug.Log("generate");
        float totalProb = 0;
        foreach (ChestDrop CD in Drops)
        {
            totalProb += CD.dropRate;
        }
        rate =Random.Range(0, totalProb);
        while (rate>=0)
        {
            finalItem = Drops[index].item;
            rate -= Drops[index].dropRate;
            index++;
            if (index == Drops.Length)
            {
                break;
            }
        }

        switch (finalItem)
        {
            case Items.type.straw:
                RdmStraw();
                break;
            
            case Items.type.juice:
                RdmJuice();
                break;
            
            case Items.type.life:
                InstantiateItem(SO.life);
                break;
            
            case Items.type.doubleLife :
                InstantiateItem(SO.doubleLife);
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        Destroy(gameObject);
        
    }
    

    virtual public void RdmStraw()
    {
        GameObject item = null;
        
        int index = Random.Range(1, 6);
        switch (index)
        {
            case 0 :
                item = SO.basicStraw;
                break;
            
            case 1 :
                item = SO.bounceJuice;
                break;

            case 2 :
                item = SO.bubbleStraw;
                break;

            case 3 :
                item = SO.snipStraw;
                break;

            case 4 :
                item = SO.eightStraw;
                break;

            case 5 :
                item = SO.triStraw;
                break;
            
            case 6 :
                item = SO.mitraStraw;
                break;
        }

        InstantiateItem(item);
    }
    
    virtual public void RdmJuice()
    {
        GameObject item = null;
        
        int index = Random.Range(0, 3);
        switch (index)
        {
            case 0 :
                item = SO.bounceJuice;
                break;
            
            case 1 :
                item = SO.pierceJuice;
                break;

            case 2 :
                item = SO.explosionJuice;
                break;

            case 3 :
                item = SO.poisonJuice;
                break;
        }
        InstantiateItem(item);
    }
    
    virtual public void InstantiateItem(GameObject GO)
    {
        Instantiate(GO, transform.position, Quaternion.identity, transform.parent);
        //gameObject.name = GO.name;
    }

    
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
public class ChestDrop
{
    public Items.type item;
    public float dropRate;
}
