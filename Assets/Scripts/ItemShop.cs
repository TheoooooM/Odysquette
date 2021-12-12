using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        while (effect == GameManager.Instance.firstEffect || effect == GameManager.Instance.secondEffect || effect == GameManager.Effect.none) {
            int index = Random.Range(0, 3);
            switch (index) {
                case 0:
                    item = SO.bounceJuice;
                    effect = GameManager.Effect.bounce;
                    break;

                case 1:
                    item = SO.pierceJuice;
                    effect = GameManager.Effect.pierce;
                    break;

                case 2:
                    item = SO.explosionJuice;
                    effect = GameManager.Effect.explosion;
                    break;

                case 3:
                    item = SO.poisonJuice;
                    effect = GameManager.Effect.poison;
                    break;
            }
        }

        InstantiateItem(item);
    }

    protected override void RdmStraw() {
        GameObject item = null;
        GameManager.Straw straw = GameManager.Straw.basic;

        int index = 0;
        while (index == 0 || straw == GameManager.Instance.actualStraw) {
            index = Random.Range(1, 5);
            switch (index) {
                case 0:
                    item = SO.basicStraw;
                    straw = GameManager.Straw.basic;
                    break;

                case 1:
                    item = SO.mitraStraw;
                    straw = GameManager.Straw.mitra;
                    break;

                case 2:
                    item = SO.bubbleStraw;
                    straw = GameManager.Straw.bubble;
                    break;

                case 3:
                    item = SO.snipStraw;
                    straw = GameManager.Straw.snipaille;
                    break;

                case 4:
                    item = SO.eightStraw;
                    straw = GameManager.Straw.eightPaille;
                    break;

                case 5:
                    item = SO.triStraw;
                    straw = GameManager.Straw.tripaille;
                    break;
            }
        }

        InstantiateItem(item);
    }

    protected override void InstantiateItem(GameObject GO) {
        GameObject gam = Instantiate(GO, transform.position, Quaternion.identity, transform.parent);
        gam.GetComponent<Items>().shop = true;
        gam.GetComponent<Items>().SpawnObject();
    }
}