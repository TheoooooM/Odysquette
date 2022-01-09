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
        if (Input.GetKeyDown(KeyCode.E) && canOpen)
        {
            AudioManager.Instance.PlayPlayerSound(AudioManager.PlayerSoundEnum.OpenChest);
            Generate();
        }
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
        GameManager.Straw strawSelect = GameManager.Straw.basic;

        
        while (item == null || strawSelect == GameManager.Straw.sniper) {
            index = Random.Range(1, 6);


            strawSelect = index switch
            {
                0 => GameManager.Straw.basic,
                1 => GameManager.Straw.sniper,
                2 => GameManager.Straw.riffle,
                3 => GameManager.Straw.tri,
                4 => GameManager.Straw.bubble,
                5 => GameManager.Straw.helix,
                _ => GameManager.Straw.basic,
            };

            if (strawSelect == GameManager.Instance.actualStraw)
            {
                RdmStraw();
                return ;
            }
            
            item = strawSelect switch {
                GameManager.Straw.basic=> SO.basicStraw,
                GameManager.Straw.bubble => SO.bubbleStraw,
                GameManager.Straw.sniper => SO.snipStraw,
                GameManager.Straw.helix => SO.eightStraw,
                GameManager.Straw.tri => SO.triStraw,
                GameManager.Straw.riffle => SO.mitraStraw,
                _ => null
            };

            item.transform.GetChild(0).GetComponent<SetStrawUI>().setData(strawSelect, !itemFromChest, true);
        }
        
        InstantiateItem(item);
    }

    /// <summary>
    /// Create a random juice
    /// </summary>
    protected virtual void RdmJuice() {
        GameObject item = null;
        GameManager.Effect effectSelect;
        
        int index = Random.Range(0, 4);
        effectSelect = index switch
        {
            0 => GameManager.Effect.bouncing,
            1 => GameManager.Effect.piercing,
            2 => GameManager.Effect.explosive,
            3 => GameManager.Effect.poison,
        };

        if (effectSelect == GameManager.Instance.firstEffect || effectSelect == GameManager.Instance.secondEffect)
        {
            RdmJuice();
            return;
        }
        
        item = effectSelect switch {
            GameManager.Effect.bouncing => SO.bounceJuice,
            GameManager.Effect.piercing => SO.pierceJuice,
            GameManager.Effect.explosive => SO.explosionJuice,
            GameManager.Effect.poison => SO.poisonJuice,
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
            canOpen = false;
            sprite.color = Color.white;
        }
    }
}

[System.Serializable]
public class ChestDrop {
    public Items.type item;
    public float dropRate;
}