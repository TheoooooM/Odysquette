using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chest : MonoBehaviour {
    [SerializeField] public itemSO SO;
    [SerializeField] private ChestDrop[] Drops;
    [SerializeField] private bool itemFromChest = true;

    private bool canOpen;
    private float rate;
    private int index;
    private Items.type finalItem;

    private SpriteRenderer sprite;

    public virtual void Start() {
        sprite = GetComponent<SpriteRenderer>();
    }

    public virtual void Update() {
        if (Input.GetKeyDown(KeyCode.E) && canOpen) Generate();
    }

    protected virtual void Generate() {
        float totalProb = 0;
        
        foreach (ChestDrop CD in Drops) {
            totalProb += CD.dropRate;
        }
        rate = Random.Range(0, totalProb);
        
        while (rate >= 0) {
            finalItem = Drops[index].item;
            rate -= Drops[index].dropRate;
            index++;
            if (index == Drops.Length) {
                break;
            }
        }

        switch (finalItem) {
            case Items.type.straw:
                RdmStraw();
                break;

            case Items.type.juice:
                RdmJuice();
                break;

            case Items.type.life:
                InstantiateItem(SO.life);
                break;

            case Items.type.doubleLife:
                InstantiateItem(SO.doubleLife);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        Destroy(gameObject);
    }
    
    /// <summary>
    /// Create a random Straw
    /// </summary>
    protected virtual void RdmStraw() {
        GameObject item = null;

        
        while (item == null) {
            index = Random.Range(1, 6);
            item = index switch {
                0 => /*SO.basicStraw*/ null,
                1 => SO.bounceJuice,
                2 => SO.bubbleStraw,
                3 => SO.snipStraw,
                4 => /*SO.eightStraw*/ null,
                5 => SO.triStraw,
                6 => SO.mitraStraw,
                _ => null
            };
        }
        
        InstantiateItem(item);
    }

    /// <summary>
    /// Create a random juice
    /// </summary>
    protected virtual void RdmJuice() {
        GameObject item = null;

        int index = Random.Range(0, 3);
        item = index switch {
            0 => SO.bounceJuice,
            1 => SO.pierceJuice,
            2 => SO.explosionJuice,
            3 => SO.poisonJuice,
            _ => item
        };

        InstantiateItem(item);
    }
   
    /// <summary>
    /// Instantiate the item
    /// </summary>
    /// <param name="GO"></param>
    protected virtual void InstantiateItem(GameObject GO) {
        GameObject gam = Instantiate(GO, transform.position, Quaternion.identity, transform.parent);
        gam.GetComponent<Items>().SpawnObject(itemFromChest);
    }

    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            canOpen = true;
            sprite.color = Color.red;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            canOpen = true;
            sprite.color = Color.white;
        }
    }
}

[System.Serializable]
public class ChestDrop {
    public Items.type item;
    public float dropRate;
}