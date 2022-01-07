using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Pathfinding;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


public class EnemyStateManager : MonoBehaviour {
    public int scorePoint = 3;
    public EnemyFeedBack enemyFeedBack;
    public bool isActivate = false;
    public bool isContactWall;
    public EMainStatsSO EMainStatsSo;
    private PlayerDetector playerDetector;
    // Variable for Set Value and Object in States
    public Vector2 forceApply;
    public List<BaseObject> baseObjectListCondition = new List<BaseObject>();
    public List<BaseObject> baseObjectListState = new List<BaseObject>();

    public Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionaryCondition =
        new Dictionary<ExtensionMethods.ObjectInStateManager, Object>();

    public Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionaryState =
        new Dictionary<ExtensionMethods.ObjectInStateManager, Object>();

    public BoxCollider2D collider2D;

    public bool isInWind;
    public Vector2 windDirection;
    public float windSpeed;
    public float health;
    public bool isDragKnockUp;
    public bool isConvey;
    public Vector2 conveyBeltSpeed;
    public bool isDead;

    //Main Stat
    [SerializeField] private Transform playerTransform;
    private bool knockUpInState = true;


    public Vector3 spawnPosition;

    //State and Stat Condition
    public int indexCurrentState;


    bool check;
    public bool IsCurrentStartPlayed;
    public bool IsCurrentStatePlayed;
    [SerializeField] private bool IsFirstStartPlayed;

    public RoomManager roomParent;

    //Delegate
    public delegate void CurrentState(Dictionary<ExtensionMethods.ObjectInStateManager, Object>
        objectValue, out bool endStep, EnemyFeedBack enemyFeedBack);

    public CurrentState CurrentFixedState;

    public CurrentState CurrentUpdateState;
    //Timer

    public Dictionary<int, float> timerCondition = new Dictionary<int, float>();
    public float timerCurrentStartState;
    public float timerCurrentState;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Dictionary<int, bool> healthUse = new Dictionary<int, bool>();
    private int counthealh;

    private void OnValidate() {
        //   
        // spriteRenderer.sprite = EMainStatsSo.sprite;
    }

    public virtual void Start()
    {
        
            playerDetector = GetComponent<PlayerDetector>();
            collider2D = GetComponent<BoxCollider2D>();
        enemyFeedBack = GetComponent<EnemyFeedBack>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spawnPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        health = EMainStatsSo.maxHealth;

        for (int i = 0; i < EMainStatsSo.stateEnnemList.Count; i++) {
            if (EMainStatsSo.stateEnnemList[i].useTimeCondition) {
                timerCondition.Add(i, 0);
            }
        }

        for (int i = 0; i < baseObjectListCondition.Count; i++) {
            switch (baseObjectListCondition[i].objectInStateManager) {
                case ExtensionMethods.ObjectInStateManager.TransformPlayer:{
                    baseObjectListCondition[i]._object = HealthPlayer.Instance.transform;
                    break;
                }
                case ExtensionMethods.ObjectInStateManager.RigidBodyPlayer:{
                    baseObjectListCondition[i]._object = HealthPlayer.Instance.rb;

                    break;
                }
                case ExtensionMethods.ObjectInStateManager.PlayerController:{
                    baseObjectListState[i]._object = HealthPlayer.Instance.playerController;
                    break;
                }
            }
        }

        for (int i = 0; i < EMainStatsSo.stateEnnemList.Count; i++) {
            if (EMainStatsSo.stateEnnemList[i].objectInStateManagersCondition.Count > 0) {
                foreach (ExtensionMethods.ObjectInStateManager objectInStateManager in EMainStatsSo.stateEnnemList[i].objectInStateManagersCondition) {
                    if (!objectDictionaryCondition.ContainsKey(objectInStateManager)) {
                        for (int j = 0; j < baseObjectListCondition.Count; j++) {
                            if (baseObjectListCondition[j].objectInStateManager == objectInStateManager) {
                                objectDictionaryCondition.Add(objectInStateManager, baseObjectListCondition[j]._object);
                            }
                        }
                    }
                }
            }
        }


        for (int i = 0; i < baseObjectListState.Count; i++) {
            switch (baseObjectListState[i].objectInStateManager) {
                case ExtensionMethods.ObjectInStateManager.TransformPlayer:{
                    baseObjectListState[i]._object = HealthPlayer.Instance.transform;
                    break;
                }
                case ExtensionMethods.ObjectInStateManager.RigidBodyPlayer:{
                    baseObjectListState[i]._object = HealthPlayer.Instance.rb;
                    break;
                }
                case ExtensionMethods.ObjectInStateManager.PlayerController:{
                    baseObjectListState[i]._object = HealthPlayer.Instance.playerController;
                    break;
                }
            }
        }

        if (EMainStatsSo.baseState != null) UpdateDictionaries(EMainStatsSo.baseState);
    }

    public virtual void Update() {
        if (isActivate && !isDead) {
            #region CheckStates

            if (!IsCurrentStatePlayed && !IsCurrentStartPlayed && EMainStatsSo.stateEnnemList.Count != 0) {
                for (int i = 0; i < EMainStatsSo.stateEnnemList.Count; i++) {
                    if (EMainStatsSo.stateEnnemList[i].useTimeCondition) {
                        if (EMainStatsSo.stateEnnemList[i].timeCondition > timerCondition[i]) {
                            continue;
                        }
                    }


                    if (EMainStatsSo.stateEnnemList[i].CheckCondition(objectDictionaryCondition)) {
                        if (!EMainStatsSo.stateEnnemList[i].isFixedUpdate) {
                            if (EMainStatsSo.stateEnnemList[i].haveStartState) {
                                CurrentUpdateState += EMainStatsSo.stateEnnemList[i].StartState;


                                IsCurrentStartPlayed = true;
                                if (EMainStatsSo.stateEnnemList[i].oneStartState)
                                    IsFirstStartPlayed = true;
                      
                            }
                            else {
                                CurrentUpdateState += EMainStatsSo.stateEnnemList[i].PlayState;
                                IsCurrentStatePlayed = true;
                            }
                        }
                        else {
                            if (EMainStatsSo.stateEnnemList[i].haveStartState) {
                                CurrentFixedState += EMainStatsSo.stateEnnemList[i].StartState;
                                IsCurrentStartPlayed = true;
                                if (EMainStatsSo.stateEnnemList[i].oneStartState)
                                    IsFirstStartPlayed = true;
                            }
                            else {
                                CurrentFixedState += EMainStatsSo.stateEnnemList[i].PlayState;
                                IsCurrentStatePlayed = true;
                            }

                            objectDictionaryState.Clear();
                            ;

                            knockUpInState = EMainStatsSo.stateEnnemList[i].isKnockUpInState;
                            indexCurrentState = i;


                            UpdateDictionaries(EMainStatsSo.stateEnnemList[indexCurrentState]);
                        }

                        objectDictionaryState.Clear();

                        knockUpInState = EMainStatsSo.stateEnnemList[i].isKnockUpInState;
                        indexCurrentState = i;

                        UpdateDictionaries(EMainStatsSo.stateEnnemList[indexCurrentState]);
                    }
                }
            }

            #endregion

            #region UpdateTimer

            for (int i = 0; i < EMainStatsSo.stateEnnemList.Count; i++) {
                if (EMainStatsSo.stateEnnemList[i].useTimeCondition) {
                    timerCondition[i] += Time.deltaTime;
                    timerCondition[i] = Mathf.Min(timerCondition[i], EMainStatsSo.stateEnnemList[i].timeCondition);
                }
            }

            if (IsCurrentStatePlayed && EMainStatsSo.stateEnnemList[indexCurrentState].playStateTime != 0) timerCurrentState += Time.deltaTime;
            if (IsCurrentStartPlayed && EMainStatsSo.stateEnnemList[indexCurrentState].startTime != 0) {
                timerCurrentStartState += Time.deltaTime;
            }

            #endregion

            ApplyState();
        }
    }


    public virtual void FixedUpdate() {
        if (isActivate && !isDead) {
            ApplyState();

            if (EMainStatsSo.baseState != null) {
                if (!IsCurrentStartPlayed && !IsCurrentStatePlayed) {
                    enemyFeedBack = GetComponent<EnemyFeedBack>();
                    EMainStatsSo.baseState.PlayState(objectDictionaryState, out bool endStep, enemyFeedBack);
                    knockUpInState = EMainStatsSo.baseState.isKnockUpInState;
                }
                else if (((IsCurrentStartPlayed || IsCurrentStatePlayed) && EMainStatsSo.stateEnnemList[indexCurrentState].duringDefaultState)) {
                    UpdateDictionaries(EMainStatsSo.baseState);
                    enemyFeedBack = GetComponent<EnemyFeedBack>();
                    Debug.Log(enemyFeedBack);
                    EMainStatsSo.baseState.PlayState(objectDictionaryState, out bool endStep, enemyFeedBack);
                    knockUpInState = EMainStatsSo.baseState.isKnockUpInState;
                }
            }


            if (EMainStatsSo.isKnockUp) {
                if (isDragKnockUp) {
                    rb.drag = EMainStatsSo.dragForKnockUp;


                    if (rb.velocity.magnitude <= 0.1f || isContactWall) {
                        isDragKnockUp = false;
                        rb.drag = 0;
                        rb.velocity = Vector2.zero;
                        isContactWall = false;
                    }
                }
            }
        }
    }

    bool CheckTimer(float timer, float time) {
        if (timer >= time)
            return true;
        return false;
    }

    void ApplyState() {
        if (IsCurrentStartPlayed || IsCurrentStatePlayed) {
            bool _endstep = false;
            if (CurrentFixedState != null) {
                if (EMainStatsSo.stateEnnemList[indexCurrentState].isFixedUpdate) {
                    CurrentFixedState(objectDictionaryState, out bool endStep, enemyFeedBack);
                    _endstep = endStep;
                }
            }

            if (CurrentUpdateState != null) {
                if (!EMainStatsSo.stateEnnemList[indexCurrentState].isFixedUpdate) {
                    CurrentUpdateState(objectDictionaryState, out bool endStep, enemyFeedBack);
                    _endstep = endStep;
                }
            }


            if (IsCurrentStartPlayed) {
                if (IsFirstStartPlayed) {
                    if (EMainStatsSo.stateEnnemList[indexCurrentState].isFixedUpdate)
                        CurrentFixedState -= EMainStatsSo.stateEnnemList[indexCurrentState].StartState;
                    else
                        CurrentUpdateState -= EMainStatsSo.stateEnnemList[indexCurrentState].StartState;
                    IsFirstStartPlayed = false;
                    ;
                }


                if (_endstep || CheckTimer(timerCurrentStartState, EMainStatsSo.stateEnnemList[indexCurrentState].startTime)) {
                    if (EMainStatsSo.stateEnnemList[indexCurrentState].isFixedUpdate) {
                        if (!EMainStatsSo.stateEnnemList[indexCurrentState].oneStartState)
                            CurrentFixedState -= EMainStatsSo.stateEnnemList[indexCurrentState].StartState;

                        CurrentFixedState += EMainStatsSo.stateEnnemList[indexCurrentState].PlayState;
                    }
                    else {
                        if (!EMainStatsSo.stateEnnemList[indexCurrentState].oneStartState)
                            CurrentUpdateState -= EMainStatsSo.stateEnnemList[indexCurrentState].StartState;

                        CurrentUpdateState += EMainStatsSo.stateEnnemList[indexCurrentState].PlayState;
                    }


                    IsCurrentStartPlayed = false;
                    IsCurrentStatePlayed = true;
                    timerCurrentStartState = 0;
                }
            }

            else if (IsCurrentStatePlayed) {
                check = EMainStatsSo.stateEnnemList[indexCurrentState].playStateTime == 0
                    ? _endstep
                    : CheckTimer(timerCurrentState, EMainStatsSo.stateEnnemList[indexCurrentState].playStateTime);
                if (check) {
                    IsCurrentStatePlayed = false;

                    if (EMainStatsSo.stateEnnemList[indexCurrentState].isFixedUpdate)
                        CurrentFixedState -= EMainStatsSo.stateEnnemList[indexCurrentState].PlayState;


                    else
                        CurrentUpdateState -= EMainStatsSo.stateEnnemList[indexCurrentState].PlayState;
                    if (timerCondition.ContainsKey(indexCurrentState)) {
                        timerCondition[indexCurrentState] = 0;
                    }


                    objectDictionaryState.Clear();
                    if (EMainStatsSo.baseState != null)
                        UpdateDictionaries(EMainStatsSo.baseState);
                    timerCurrentState = 0;
                    timerCondition[indexCurrentState] = 0;
                    indexCurrentState = 0;
                }
            }
        }
    }

    public void UpdateDictionaries(StateEnemySO stateEnemySo) {
        foreach (ExtensionMethods.ObjectInStateManager objectInStateManager in stateEnemySo.objectInStateManagersState) {
            if (!objectDictionaryState.ContainsKey(objectInStateManager)) {
                for (int i = 0; i < baseObjectListState.Count; i++) {
                    if (baseObjectListState[i].objectInStateManager == objectInStateManager) {
                        objectDictionaryState.Add(objectInStateManager, baseObjectListState[i]._object);
                    }
                }
            }
        }
    }

    [Serializable]
    public class BaseObject {
        public ExtensionMethods.ObjectInStateManager objectInStateManager;
        public Object _object;
    }

    public virtual void OnDeath(bool byFall = false) {
        if (!isDead) {
            isDead = true;
            
            GameObject GO = Resources.Load<GameObject>("ressource");
            GameManager.Instance.AddScore(scorePoint);
            int rdm = Random.Range(0, 5);
            for (int i = 0; i < rdm; i++) {
                Instantiate(GO, transform.position, Quaternion.identity);
            }
            
            if (byFall) {
                collider2D.enabled = false;

                rb.constraints = RigidbodyConstraints2D.FreezePosition;
                roomParent.ennemiesList.Remove(transform.parent.gameObject);
                this.enabled = false;
                
                Animator animator = GetComponent<Animator>();
                animator.Play("FallAnim");
                
                StartCoroutine(ShowCurrentClipLength(gameObject.transform.parent.gameObject, animator));
            }
            else {
                AudioManager.Instance.PlayEnemySound(AudioManager.EnemySoundEnum.Death, gameObject);

                collider2D.enabled = false;

                rb.constraints = RigidbodyConstraints2D.FreezePosition;
                roomParent.ennemiesList.Remove(transform.parent.gameObject);
                this.enabled = false;


                Animator animator = GetComponent<Animator>();
                if (TryGetComponent(out EnemyFeedBackDeath eventDeath)) {
                    eventDeath.deathEvent.Invoke();
                    return;
                }

                try {
                    roomParent.ennemiesList.Remove(gameObject.transform.parent.gameObject);
                    if (enemyFeedBack.stateDeathName != "") {
                        animator.Play(enemyFeedBack.stateDeathName);


                        StartCoroutine(ShowCurrentClipLength(gameObject.transform.parent.gameObject, animator));
                    }
                    else {
                        Destroy(gameObject.transform.parent.gameObject);
                    }
                }
                catch (Exception e) {
                    roomParent.ennemiesList.Remove(gameObject);

                    if (enemyFeedBack != null && enemyFeedBack.stateDeathName != "") {
                        animator.Play(enemyFeedBack.stateDeathName);
                        StartCoroutine(ShowCurrentClipLength(gameObject, animator));
                    }
                    else {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    IEnumerator ShowCurrentClipLength(GameObject gameObjectToDestroy, Animator animator) {
        yield return new WaitForEndOfFrame();
        Destroy(gameObjectToDestroy, animator.GetCurrentAnimatorStateInfo(0).length);
    }

    public virtual void TakeDamage(float damage, Vector2 position, float knockUpValue, bool knockup, bool isExplosion) {
        if(playerDetector != null) playerDetector.EndDetection();
        if (health - damage <= 0) {
            OnDeath();
        }
        else {
            AudioManager.Instance.PlayEnemySound(AudioManager.EnemySoundEnum.Damage, gameObject);
            Knockup(position, knockUpValue, knockup, isExplosion);
            health -= damage;
        }

        spriteRenderer.material.SetFloat("_HitTime", Time.time);
    }

    void Knockup(Vector2 position, float knockUpValue, bool knockUp, bool isExplosion) {
        if (!knockUp) {
            return;
        }

        if (EMainStatsSo.isKnockUp && knockUpInState && EMainStatsSo.baseState != null && (!isDragKnockUp || isExplosion)) {
            Vector2 direction = new Vector2();

            direction = (rb.position - position).normalized;

            rb.velocity += direction * knockUpValue;


            isDragKnockUp = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!HealthPlayer.Instance.playerController.InDash)
            if (other.gameObject.CompareTag("Player"))
                HealthPlayer.Instance.TakeDamagePlayer(1);
        if (other.CompareTag("Wind")) {
            StateWind stateWind = other.GetComponent<WindParticleManager>().StateWind;
            windDirection += stateWind.direction * stateWind.speedWind;

            isInWind = true;
        }

        if (other.CompareTag("Convey")) {
            conveyBeltSpeed += other.GetComponent<LDConveyorBelt>().direction;
            isConvey = true;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Wind")) {
            isInWind = true;
        }

        if (other.CompareTag("Convey")) {
            isConvey = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Convey")) {
            conveyBeltSpeed -= other.GetComponent<LDConveyorBelt>().direction;
            isConvey = false;
        }

        if (other.CompareTag("Wind")) {
            Debug.Log("exist");
            StateWind stateWind = other.GetComponent<WindParticleManager>().StateWind;
            windDirection -= stateWind.direction * stateWind.speedWind;
            isInWind = false;
        }
    }
}