using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public Dictionary<GameManager.Straw, Queue<GameObject>> poolDictionary;
    
    
    #region  Singleton
    public static PoolManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion
    
    private void Start()
    {
        poolDictionary = new Dictionary<GameManager.Straw, Queue<GameObject>>(); //Créer un dictionnaire regroupant chaque pool

        foreach (GameManager.StrawClass pol in GameManager.Instance.strawsClass)
        {
            //---------------------Génère les pool et les bullets de base------------------------- 
            Queue<GameObject> objectPool = new Queue<GameObject>();
            
            for (int i = 0; i < pol.sizePool; i++)
            {
                GameObject obj = Instantiate(pol.strawSO.prefabBullet, transform);
                obj.name = GameManager.Instance.actualStraw.ToString();
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            //---------------------------------------------------------------------------------------
            
            poolDictionary.Add(pol.StrawType, objectPool);
        }
    }
    
     public GameObject SpawnFromPool(Transform parentBulletTF, GameObject prefabBullet) //Active ou instancie une balle sur le spawn bullet
    {
       
        GameObject obj;
        
            if (poolDictionary[GameManager.Instance.actualStraw].Count == 0) // Instancie une balle si il n'y en a plus dans la queue
            {
                obj = Instantiate(prefabBullet, parentBulletTF.position, parentBulletTF.rotation );
                obj.name = GameManager.Instance.actualStrawClass.strawSO.name;
             
                //poolDictionary[GameManager.Instance.actualStraw].Enqueue(obj);
                return obj;
            }
            else // Sinon active la première balle se trouvant dans la queue
            {
                
                //Debug.Log("Count : "+count);    
                obj = poolDictionary[GameManager.Instance.actualStraw].Dequeue();
                
                obj.transform.position = parentBulletTF.position;
             
                obj.transform.rotation = parentBulletTF.rotation; 
                return obj;
            }
        
    }
    
}
