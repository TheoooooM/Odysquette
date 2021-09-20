using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion
    
    public enum Effect {none,bounce, pierce, explosion, poison, ice}
    public enum Straw {basic, bubble, snipaille, eightPaille, fourDir, tripaille, mitra}
    
    
    
    public class StrawClass
    {
        public GameObject StrawParent;
        public List<Transform> spawnerTransform;
    }
    
    //Juices
    [SerializeField] Effect firstEffect;
    [SerializeField] Effect secondEffect;
    
    //Straw
    private Straw actualStraw = Straw.basic;
    private GameObject actualStrawGam;
    List<StrawClass> straws = new List<StrawClass>();

    private float shootCooldown;
    
    //Bullet
    private float ShootRate;
    
    //Player
    public GameObject Player;

    //
    private float damageModifier;
    private float firerateModifier;
    private float speedModifier;

    private void Start()
    {
        actualStrawGam = Player.transform.GetChild((int) actualStraw).gameObject;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (shootCooldown <= 0f)
            {
               PoolManager.Instance.SpawnFromPool("basic");
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


