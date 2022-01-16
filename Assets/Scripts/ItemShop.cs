using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class ItemShop : Chest {
    public override void Start() {
        base.Start();
        Generate();
    }

    public override void Update() {
    }

    protected override void RdmJuice() {
        GameObject item = null;
        GameManager.Effect effect = GameManager.Effect.none;
        GameManager.Instance.GetND();
        
        while (effect == GameManager.Instance.firstEffect || effect == GameManager.Instance.secondEffect || effect == GameManager.Effect.none) {
            int index = Random.Range(0, 4);
            switch (index) {
                case 0:
                    item = SO.bounceJuice;
                    effect = GameManager.Effect.bouncing;
                    break;

                case 1:
                    item = SO.pierceJuice;
                    effect = GameManager.Effect.piercing;
                    break;

                case 2:
                    item = SO.explosionJuice;
                    effect = GameManager.Effect.explosive;
                    break;

                case 3:
                    item = SO.poisonJuice;
                    effect = GameManager.Effect.poison;
                    break;
                default: effect = GameManager.Effect.none;
                    break;
            }
            Debug.Log("set " + effect);
        }

        InstantiateItem(item);
    }

    protected override void RdmStraw() {
        GameObject item = null;
        GameManager.Straw straw = GameManager.Straw.basic;

        int index = 0;

        List<GameManager.Straw> strawPossibleList = new List<GameManager.Straw>();
        for (int i = 1; i < 5; i++) {
            strawPossibleList.Add((GameManager.Straw)i);
        }

        strawPossibleList.Remove(GameManager.Instance.actualStraw);
        strawPossibleList.Remove(GameManager.Straw.sniper);

        index = Random.Range(0, strawPossibleList.Count);
        straw = strawPossibleList[index];
        item = straw switch {
            GameManager.Straw.basic => SO.basicStraw,
            GameManager.Straw.bubble => SO.bubbleStraw,
            GameManager.Straw.sniper => SO.snipStraw,
            GameManager.Straw.helix => SO.eightStraw,
            GameManager.Straw.tri => SO.triStraw,
            GameManager.Straw.riffle => SO.mitraStraw,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        item.transform.GetChild(0).GetComponent<SetStrawUI>().setData(straw, true, true);
        InstantiateItem(item);
    }

    protected override void InstantiateItem(GameObject GO) {
        GameObject gam = Instantiate(GO, transform.position, Quaternion.identity, transform.parent);
        gam.GetComponent<Items>().shop = true;
        gam.GetComponent<Items>().SpawnObject();
    }
}