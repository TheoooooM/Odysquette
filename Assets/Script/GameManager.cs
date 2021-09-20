using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    
    enum Effect {bounce, pierce, explosion, poison, ice}
    enum Straw {basic, bubble, snipaille, eightPaille, fourDir, tripaille, mitra}
    
    
    
    public class StrawClass
    {
        public GameObject StrawParent;
        public List<Transform> spawnerTransform;
    }
    
    //Juices
    [SerializeField] Effect firstEffect;
    [SerializeField] Effect secondEffect;
    
    //Straw
    private Straw actualStraw;
    //GameObject actualStrawGam = (int) actualStraw;
    List<StrawClass> straws = new List<StrawClass>();

    //Player
    public GameObject Player;

    //
    private float damageModifier;
    private float firerateModifier;
    private float speedModifier;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (shootCooldown <= 0f)
            {
                SpawnFromPool("basic");
                shootCooldown = 1.5f;
            }
            else
            {
                Debug.Log("nope");
            }
        }
        shootCooldown -= Time.deltaTime*ShootRate;
    }
}


