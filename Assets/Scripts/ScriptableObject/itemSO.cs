using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Item Scriptable",menuName = "Item Script")]
public class itemSO : ScriptableObject
{
    [Header("----------- Straw -----------")]
    [SerializeField] private GameObject _basicStraw;
    public GameObject basicStraw => _basicStraw;
    
    [SerializeField] private GameObject _bubbleStraw;
    public GameObject bubbleStraw => _bubbleStraw;
    
    [SerializeField] private GameObject _snipStraw;
    public GameObject snipStraw => _snipStraw;

    [SerializeField] private GameObject _eightStraw;
    public GameObject eightStraw => _eightStraw;

    [SerializeField] private GameObject _triStraw;
    public GameObject triStraw => _triStraw;
    
    [SerializeField] private GameObject _mitraStraw;
    public GameObject mitraStraw => _mitraStraw;
    
    
    [Header("----------- Juice -----------")]
    [SerializeField] private GameObject _bounceJuice;
    public GameObject bounceJuice => _bounceJuice;
    
    [SerializeField] private GameObject _pierceJuice;
    public GameObject pierceJuice => _pierceJuice;

    [SerializeField] private GameObject _explosionJuice;
    public GameObject explosionJuice => _explosionJuice;

    [SerializeField] private GameObject _poisonJuice;
    public GameObject poisonJuice => _poisonJuice;

    [Header("----------- Juice Sprite -----------")] 
    [SerializeField] private Sprite _bounceSprite;
    public Sprite bounceSprite => _bounceSprite;

    [SerializeField] private Sprite _pierceSprite;
    public Sprite pierceSprite => _pierceSprite;
    
    [SerializeField] private Sprite _explosionSprite;
    public Sprite explosionSprite => _explosionSprite;
    
    [SerializeField] private Sprite _poisonSprite;
    public Sprite poisonSprite => _poisonSprite;
    
    [Header("----------- Other -----------")]
    
    [SerializeField] private GameObject _life;
    public GameObject life => _life;
    
    [SerializeField] private GameObject _doubleLife;
    public GameObject doubleLife => _doubleLife;
}
