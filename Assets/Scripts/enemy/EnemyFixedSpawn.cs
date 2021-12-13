using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFixedSpawn : MonoBehaviour
{
    [SerializeField] private RoomContainer part;
    // Start is called before the first frame update
    void Start()
    {
        part.room.ennemiesList.Add(this.gameObject);
        GetComponent<EnemyStateManager>().roomParent = part.room;
    }
}
