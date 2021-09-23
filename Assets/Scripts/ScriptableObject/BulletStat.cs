using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Bullet Scriptable",menuName = "Bullet Script")]
public class BulletStat : ScriptableObject
{
    [Header("Settings")]
    [SerializeField] int _ShootRate;
    public int ShootRate => _ShootRate;
    
    [SerializeField] private float _bulletSize;
    public float bulletSize => _bulletSize;
    
    [SerializeField] private float _bulletSpeed;
    public float bulletSpeed => _bulletSpeed;

    [SerializeField] private float _bulletSpray;
    public float bulletSpray => _bulletSpray;
    
    [SerializeField] private float _damage;
    public float damage => _damage;
    
    [SerializeField] private float _Range;
    public float range => _Range;
}
