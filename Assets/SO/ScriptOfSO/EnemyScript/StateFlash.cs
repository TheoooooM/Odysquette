using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "StateFlashSO", menuName = "EnnemyState/StateFlashSO", order = 0)]
public class StateFlash : StateEnemySO
{
    public float rangeForShoot;

    public LayerMask layerMaskRay; 
 
    public Vector3 extentsRangeDetection;
    public float timeEffectFlash;
    public float lowSpeed;
    public float offSetDistance;
   
 

    public override bool CheckCondition(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary)
    {  Rigidbody2D rbPlayer = (Rigidbody2D) objectDictionary[ExtensionMethods.ObjectInStateManager.RigidBodyPlayer];
             Rigidbody2D rbEnemy = (Rigidbody2D) objectDictionary[ExtensionMethods.ObjectInStateManager.RigidBodyEnemy];
             if (Vector2.Distance(rbPlayer.position, rbEnemy.position) <= rangeForShoot)
             {
                 Vector2 direction = (rbPlayer.position - rbEnemy.position);
                
                 RaycastHit2D hit = Physics2D.BoxCast(rbEnemy.position, extentsRangeDetection, 0,
                     direction.normalized, direction.magnitude, layerMaskRay);
               
                 ExtDebug.DrawBoxCastBox(rbEnemy.position, extentsRangeDetection/2, Quaternion.identity, direction.normalized, direction.magnitude, Color.red);
                 if(hit.collider.gameObject.layer == 9 )
                 {
                     return true;
                 }
             }
             return false;
    }

    public override void StartState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep, EnemyFeedBack enemyFeedBack)
    {
        Transform transformPlayer = (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.TransformPlayer];
        GameObject flashObject = (GameObject) objectDictionary[ExtensionMethods.ObjectInStateManager.FlashObject];
        Transform transformEnemy = (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.TransformEnemy];
      
        Vector3 direction =  (transformPlayer.position-transformEnemy.position).normalized;
  
        flashObject.transform.position = transformEnemy.position + direction * offSetDistance;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        CheckFeedBackEvent(enemyFeedBack, ExtensionMethods.EventFeedBackEnum.DuringStartState);

       flashObject.transform.rotation = Quaternion.Euler(0,0,angle);
        flashObject.SetActive(true);

        endStep = false;
    }

    public override void PlayState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep, EnemyFeedBack enemyFeedBack)
    {
       
        Playercontroller Pcontroller =(Playercontroller) objectDictionary[ExtensionMethods.ObjectInStateManager.PlayerController];
        GameObject flashObject = (GameObject) objectDictionary[ExtensionMethods.ObjectInStateManager.FlashObject];
        if (Pcontroller.isInFlash)
        {
            Pcontroller.SetEffectFlash(timeEffectFlash, lowSpeed );
        }
        CheckFeedBackEvent(enemyFeedBack, ExtensionMethods.EventFeedBackEnum.DuringPlayState);
        flashObject.SetActive(false);
        endStep = true;
    
    }
}
