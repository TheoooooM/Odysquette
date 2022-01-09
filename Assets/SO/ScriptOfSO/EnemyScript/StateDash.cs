using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "StateDashSO", menuName = "EnnemyState/StateDashSO", order = 0)]
public class StateDash : StateEnemySO {
    public AnimationCurve animationCurve;
    public float maxspeed;
    public float rangeDetection;
    public float distanceDash;

    public float timeKnockback;
    public float speedKnockback;
    public AnimationCurve curveKnockback;

    public Vector3 extentsRangeDetection;
    public LayerMask layerMask;

    public override bool CheckCondition(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary) {
        Rigidbody2D rb = (Rigidbody2D) objectDictionary[ExtensionMethods.ObjectInStateManager.RigidBodyEnemy];
        Rigidbody2D rbPlayer = (Rigidbody2D) objectDictionary[ExtensionMethods.ObjectInStateManager.RigidBodyPlayer];
        if (Vector2.Distance(rb.position, rbPlayer.position) < rangeDetection) {
            Vector2 direction = (rbPlayer.position - rb.position);
            RaycastHit2D hit = Physics2D.BoxCast(rb.position, extentsRangeDetection, 0, direction.normalized, direction.magnitude, layerMask);
            ExtDebug.DrawBoxCastBox(rb.position, extentsRangeDetection / 2, Quaternion.identity, direction.normalized, direction.magnitude, Color.red);

            if (hit.collider.gameObject.layer == 9) {
                Transform target = (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.AimDash];
                target.position = rb.position + (direction.normalized) * distanceDash;
                target.gameObject.SetActive(true);


                Debug.DrawRay(rb.position, direction * rangeDetection);
                return true;
            }
        }

        return false;
    }

    public override void StartState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep, EnemyFeedBack enemyFeedBack) {
        Rigidbody2D rb = (Rigidbody2D) objectDictionary[ExtensionMethods.ObjectInStateManager.RigidBodyEnemy];

        CheckFeedBackEvent(enemyFeedBack, ExtensionMethods.EventFeedBackEnum.DuringStartState);
        rb.velocity = Vector2.zero;
        endStep = false;
    }

    public override void PlayState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep, EnemyFeedBack enemyFeedBack) {
        Transform transformDash = (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.AimDash];
        Rigidbody2D rb =
            (Rigidbody2D) objectDictionary[ExtensionMethods.ObjectInStateManager.RigidBodyEnemy];
        EnemyDashCollision enemyDashCollision = (EnemyDashCollision) objectDictionary[ExtensionMethods.ObjectInStateManager.EnemyDashCollision];
        Vector2 direction = ((Vector2) transformDash.position - rb.position);
        float factorSpeed =  (distanceDash-direction.magnitude)/distanceDash ;
        
        float speed = animationCurve.Evaluate(factorSpeed) * maxspeed;
        

        CheckFeedBackEvent(enemyFeedBack, ExtensionMethods.EventFeedBackEnum.BeginPlayState); 
     
        rb.velocity = direction.normalized * speed;

        enemyDashCollision.inDash = true;
        if (enemyDashCollision.isTrigger) {
            Playercontroller.Instance.KnockBack(timeKnockback, enemyDashCollision.direction, curveKnockback, speedKnockback);
            CheckFeedBackEvent(enemyFeedBack, ExtensionMethods.EventFeedBackEnum.CollideDuringPlayState);
            enemyDashCollision.direction = Vector2.zero;
            enemyDashCollision.contact = Vector2.zero;
            enemyDashCollision.isTrigger = false;
            Debug.Log("je suis lu là");
        }

  

        if (Vector2.Distance(rb.position, transformDash.position) < 0.2f || enemyDashCollision.contactWall) {
            CheckFeedBackEvent(enemyFeedBack, ExtensionMethods.EventFeedBackEnum.EndPlayState);
            enemyDashCollision.inDash = false;
            transformDash.gameObject.SetActive(false);
            Debug.Log(Vector2.Distance(rb.position, transformDash.position) < 0.2f);
           Debug.Log(enemyDashCollision.contactWall); 
            rb.velocity = Vector2.zero;
            endStep = true;
            Debug.Log("je suis lu là 2 ");
            enemyDashCollision.contactWall = false;

            return;
        }


        endStep = false;
    }
}