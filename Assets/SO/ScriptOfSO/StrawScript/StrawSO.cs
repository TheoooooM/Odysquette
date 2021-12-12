using System.Collections;
using UnityEngine;

public class StrawSO : ScriptableObject {
    public bool hasRange;
  
    public string strawName;
    public Sprite strawRenderer;
    public float damage = 1;
    public float timeValue = 0;
    public int effectAllNumberShoot = 0;
    public float dragRB = 0;
    public float range = 1;
    public RateMode rateMode;
    public float knockUp;
    public bool rateMainParameter = false;
    public bool rateSecondParameter = false;
    public float damageParameter = 0;
    public float rangeParameter = 0;
    public float dragRBParameter = 0;
    public float speedParameter = 0;
    public bool isDelayBetweenShoot;
    public bool isDelayBetweenWaveShoot;
    public int numberWaveShoot = 1;
    public float delayBetweenShoot = 0;
    public float delayBetweenWaveShoot = 0;
    public float ultimatePoints;
    public float delayParameter = 0;
    
    [NamedArray("vector3", true)] public Vector3[] basePositionParameter = new Vector3[0];

    [NamedArray("vector3", true)] public Vector3[] basePosition = new Vector3[0];

    public float speedBullet = 0;
    public GameObject prefabBullet;

    #if UNITY_EDITOR
    public virtual void OnValidate() {
        damage = Mathf.Max(damage, 0);
        timeValue = Mathf.Max(timeValue, 0);
        dragRB = Mathf.Max(dragRB, 0);
        range = Mathf.Max(range, 0);
        delayBetweenShoot = Mathf.Max(delayBetweenShoot, 0);
        speedBullet = Mathf.Max(speedBullet, 0);
        effectAllNumberShoot = Mathf.Max(effectAllNumberShoot, 0);
    }
    #endif
    
    public virtual void Shoot(Transform parentBulletTF, MonoBehaviour script, float currentTimeValue = 1) {
    }

    public virtual IEnumerator ShootDelay(Transform parentBulletTF, float currentTimeValue = 1) {
        yield return null;
    }

    public virtual void SetParameter(GameObject bullet, float currentTimeValue, Transform transform = null) {
        Bullet scriptBullet = bullet.GetComponent<Bullet>();
        if (rateMainParameter == true) {
            scriptBullet.hasRange = hasRange;
            scriptBullet.damage = damage + damageParameter * currentTimeValue;
            if (hasRange)
                scriptBullet.range = range + rangeParameter * currentTimeValue;
            scriptBullet.rb.drag = dragRB + dragRBParameter * currentTimeValue;
            if(rateMode != RateMode.Ultimate)
                scriptBullet.ammountUltimate = ultimatePoints;
            scriptBullet.rateMode = rateMode;
        }
        else if (rateMainParameter == false) {
            scriptBullet.damage = damage;
            scriptBullet.hasRange = hasRange;
            scriptBullet.range = range;
            if(rateMode != RateMode.Ultimate)
            scriptBullet.ammountUltimate =  ultimatePoints;

            scriptBullet.rb.drag = dragRB;
            scriptBullet.rateMode = rateMode;
        }

        scriptBullet.knockUpValue = knockUp;
    }

    public enum RateMode {
        Ultimate,
        FireRate,
        FireLoading
    }
}