using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
[CreateAssetMenu(fileName = "RandomShootSO", menuName = "ShootMode/RandomShootSO", order = 1)]
public class RandomShootSO : StrawSO
{
    public List<angleRandom> possibleDirections = new List<angleRandom>(2);
    public int drawNumber;
    public int  totalProbability;
    [SerializeField]
    private bool probabilityDiscount;

    public int[] possibleDirectionsParameter;
  public override void Shoot(Transform parentBulletTF,MonoBehaviour script, float currentTimeValue = 1 ) 
    {

        if (!isDelayBetweenShoot && !isDelayBetweenWaveShoot)
        {

            List<angleRandom> currentListProbability = new List<angleRandom>();
            currentListProbability.AddRange(possibleDirections);
            int currentTotalProbability = totalProbability;

            for (int i = 0; i < drawNumber; i++)
            {
                int rand = Random.Range(0, currentTotalProbability+1);

                int index = 0;
            
                while (rand > 0)
                {
                    Debug.Log(rand);
                        rand -= currentListProbability[index].probability;
                    index++;
                    if (index == currentListProbability.Count)
                    {
                        index--;
                        break;
                    }
                }
Debug.Log(index+ "index");
                GameObject bullet = PoolManager.Instance.SpawnFromPool(parentBulletTF, prefabBullet, rateMode);
                bullet.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                bullet.SetActive(true);
                Vector3 rotation = Vector3.zero;
        
            if (possibleDirectionsParameter != null)
            {
                 if (possibleDirectionsParameter.Length >= index + 1)
                                {
                                    rotation = Quaternion.Euler(0, 0,
                                                   possibleDirections[index].angle +
                                                   possibleDirectionsParameter[index] * currentTimeValue) *
                                               parentBulletTF.transform.right;
                                } 
                 else
                 { 
                     rotation = Quaternion.Euler(0, 0, possibleDirections[index].angle) * parentBulletTF.transform.right;
                 }
            }
            
               
               
                else
                {  
                    rotation = Quaternion.Euler(0, 0, possibleDirections[index].angle) * parentBulletTF.transform.right;
                }

            if (basePosition != null)
            {
                if (basePosition.Length != 0)
                {
                    bullet.transform.position += basePosition[index];
                    if (basePositionParameter.Length == 1 + index)
                    {
                        bullet.transform.position += basePositionParameter[i] * currentTimeValue;
                    }
                }
            }

        
                //save pool
             
                bullet.GetComponent<Rigidbody2D>().AddForce(rotation * (speedBullet + speedParameter * currentTimeValue), ForceMode2D.Force);
                SetParameter(bullet, currentTimeValue, null);
          
                if (probabilityDiscount)
                {
                    currentTotalProbability -= currentListProbability[index].probability;
                    currentListProbability.RemoveAt(index);
                }

            }
        }
        else
        {
           script.StartCoroutine(ShootDelay(parentBulletTF, currentTimeValue));
        }
      
    }

 

    public override IEnumerator ShootDelay(Transform parentBulletTF, float currentTimeValue)
    {
        for (int j = 0; j < numberWaveShoot; j++)
        {
            List<angleRandom> currentListProbability = new List<angleRandom>();
            currentListProbability.AddRange(possibleDirections);
            int currentTotalProbability = totalProbability;

            for (int i = 0; i < drawNumber; i++)
            {
                int rand = Random.Range(0, currentTotalProbability+1);

                int index = 0;
            
                while (rand > 0)
                {
                    Debug.Log(rand);
                        rand -= currentListProbability[index].probability;
                    index++;
                    if (index == currentListProbability.Count)
                    {
                        index--;
                        break;
                    }
                }
Debug.Log(index+ "index");
                GameObject bullet = PoolManager.Instance.SpawnFromPool(parentBulletTF, prefabBullet, rateMode);
                bullet.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                bullet.SetActive(true);
                Vector3 rotation = Vector3.zero;
        
            if (possibleDirectionsParameter != null)
            {
                 if (possibleDirectionsParameter.Length >= index + 1)
                                {
                                    rotation = Quaternion.Euler(0, 0,
                                                   possibleDirections[index].angle +
                                                   possibleDirectionsParameter[index] * currentTimeValue) *
                                               parentBulletTF.transform.right;
                                } 
                 else
                 { 
                     rotation = Quaternion.Euler(0, 0, possibleDirections[index].angle) * parentBulletTF.transform.right;
                 }
            }
            
               
               
                else
                {  
                    rotation = Quaternion.Euler(0, 0, possibleDirections[index].angle) * parentBulletTF.transform.right;
                }

            if (basePosition != null)
            {
                if (basePosition.Length != 0)
                {
                    bullet.transform.position += basePosition[index];
                    if (basePositionParameter.Length == 1 + index)
                    {
                        bullet.transform.position += basePositionParameter[i] * currentTimeValue;
                    }
                }
            }

        
                //save pool
             
                bullet.GetComponent<Rigidbody2D>().AddForce(rotation * (speedBullet + speedParameter * currentTimeValue), ForceMode2D.Force);
                SetParameter(bullet, currentTimeValue, null);
          
                if (probabilityDiscount)
                {
                    currentTotalProbability -= currentListProbability[index].probability;
                    currentListProbability.RemoveAt(index);
                }
                                        if(isDelayBetweenShoot) 
                                            yield return new WaitForSeconds(delayBetweenShoot + delayParameter * currentTimeValue);
            }
            if(isDelayBetweenWaveShoot)
                                    yield return new WaitForSeconds(delayBetweenShoot);
                    }
            
        
       
    }




[Serializable]
    public class angleRandom
    {
        public float angle;
        public int probability;
    }
}
