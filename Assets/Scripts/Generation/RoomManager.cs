using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour {
    public Rect cameraRect;

    public List<RoomContainer> partList = new List<RoomContainer>();

    public List<GameObject> ennemiesList = new List<GameObject>();
    public bool runningRoom;
    [HideInInspector] public bool roomFinish = false;

    public GameObject exitGO;
    public GameObject enterGO;

    public Transform exit;
    [Space]
    public GameObject chest;
    private int chestProcentDrop = 80;
    
    private bool done = false;
    private bool sendanim = false;

    private void Start() {
        if (exit != null) exit.gameObject.SetActive(false);
    }

    private void Update() {
        if (runningRoom) {
            foreach (RoomContainer RC in partList) {
                RC.gameObject.SetActive(true);
            }

            if (!done) {
                CameraControllers.Instance.currentRectLimitation = cameraRect;
                if (exitGO != null) exitGO.SetActive(true);
                Generation.Instance.runningroom = this;
                //if (enterGO != null) enterGO.SetActive(true);
                //exitClose.SetActive(true);
                done = true;
            }

            if (ennemiesList.Count == 0) {
                if (!sendanim) {
                    GameManager.Instance.endRoom();
                    int random = Random.Range(0, 100);
                    if (chest != null && chestProcentDrop > random) Instantiate(chest, Playercontroller.Instance.transform.position, Quaternion.identity);
                    sendanim = true;
                }

                //Debug.Log("finish Room");
                roomFinish = true;
                if (exitGO != null) exitGO.SetActive(false);
                if (exit != null) exit.gameObject.SetActive(true);
                //exitClose.SetActive(false);
            }
        }
    }
}