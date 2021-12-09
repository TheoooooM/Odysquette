using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShop : Chest
{
    [SerializeField] bool shop; 
    
    public override void Start()
    {
        base.Start();
        Generate();
    }

    public override void Update() {}

    public override void RdmJuice()
    {
        GameObject item = null;
        Effect effect = Effect.none;
        
        while (effect == GameManager.Instance.firstEffect || effect == GameManager.Instance.secondEffect || effect == Effect.none)
        {
            int index = Random.Range(0, 3);
            switch (index)
            {
                case 0 :
                    item = SO.bounceJuice;
                    effect = Effect.bounce;
                    break;
                
                case 1 :
                    item = SO.pierceJuice;
                    effect = Effect.pierce;
                    break;

                case 2 :
                    item = SO.explosionJuice;
                    effect = Effect.explosion;
                    break;

                case 3 :
                    item = SO.poisonJuice;
                    effect = Effect.poison;
                    break;
            }
        }
        InstantiateItem(item);
    }

    public override void RdmStraw()
    {
        GameObject item = null;
        Straw straw = Straw.basic;
        
        int index = 0;
        while (index == 0 || straw == GameManager.Instance.actualStraw)
        {
            index = Random.Range(1, 5);
            switch (index)
            {
                case 0 :
                    item = SO.basicStraw;
                    straw = Straw.basic;
                    break;

                case 1 :
                    item = SO.mitraStraw;
                    straw = Straw.mitra;
                    break;
                
                case 2 :
                    item = SO.bubbleStraw;
                    straw = Straw.bubble;
                    break;

                case 3 :
                    item = SO.snipStraw;
                    straw = Straw.snipaille;
                    break;

                case 4 :
                    item = SO.eightStraw;
                    straw = Straw.eightPaille;
                    break;

                case 5 :
                    item = SO.triStraw;
                    straw = Straw.tripaille;
                    break;
            
            }
        }

        InstantiateItem(item);
    }

    public override void InstantiateItem(GameObject GO)
    {
        GameObject Gobj = Instantiate(GO, transform.position, Quaternion.identity, transform.parent);
        Gobj.GetComponent<Items>().shop = true;
    }
}
