using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


//Salle Dim 18.4 x 10.4

[Serializable]
public class Room {
    public RoomContainer RoomGO;
    public Vector2 ArrayPosition;


    public bool exitTop = true;
    public bool exitLeft = true;
    public bool exitRight = true;
    public bool exitBot = true;
}


[ExecuteInEditMode]
public class RoomCreator : MonoBehaviour {
    public bool Exist = false;
    public Room[] partList;

    [SerializeField] private List<GameObject> _ennemiiList;
    public List<GameObject> ennemiList => _ennemiiList;

    public Transform exit;

    public GameObject[,] partMap = new GameObject[5, 5];

    public GameObject roomPref;
    public GameObject chest;

    public bool update = false;

    public Dictionary<Generation.open, List<RoomContainer>> exitDicitonnary = new Dictionary<Generation.open, List<RoomContainer>>();
    [HideInInspector] public List<RoomContainer> topExits;
    [HideInInspector] public List<RoomContainer> botExits;
    [HideInInspector] public List<RoomContainer> leftExits;
    [HideInInspector] public List<RoomContainer> rightExits;


    private void Awake() {
        if (!Exist) {
            transform.position = Vector3.zero;
            partList = new Room[2];
            partList[0] = new Room();
            roomPref = Resources.Load<GameObject>("Room/StartRoom");

            partList[0].RoomGO = Instantiate(roomPref, transform).GetComponent<RoomContainer>();
            partList[0].RoomGO.transform.position = new Vector2((partList[0].ArrayPosition.x - 2) * 62, (partList[0].ArrayPosition.y - 2) * 40);
            partList[0].RoomGO.transform.rotation = Quaternion.identity;

            partList[0].ArrayPosition = new Vector2(2, 2);
            partMap[2, 2] = partList[0].RoomGO.gameObject;
            Exist = true;
        }
    }

    private void Update() {
        if (update) {
            PartUpdate();
            Debug.Log("Update");
            update = false;
        }

        if (exitDicitonnary.Count == 0) {
            DictionaryUpdate();
        }
    }

    public void PartUpdate() {
        foreach (Room rom in partList) {
            if (rom.RoomGO == null) {
                rom.RoomGO = Instantiate(roomPref, transform).GetComponent<RoomContainer>();
                rom.RoomGO.transform.position = new Vector2((rom.ArrayPosition.x - 2) * 9.921f, (rom.ArrayPosition.y - 2) * 6.4f);
                rom.RoomGO.transform.rotation = Quaternion.identity;
            }

            partMap[(int) rom.ArrayPosition.x, (int) rom.ArrayPosition.y] = rom.RoomGO.gameObject;
            rom.RoomGO.roomRef = this;
            rom.RoomGO.roomPos = rom.ArrayPosition;
            rom.RoomGO.name = rom.ArrayPosition.ToString();
        }

        foreach (Room rom in partList) {
            if ((rom.ArrayPosition.x - 1) >= 0) {
                rom.RoomGO.partLeft = partMap[(int) rom.ArrayPosition.x - 1, (int) rom.ArrayPosition.y] != null ? partMap[(int) rom.ArrayPosition.x - 1, (int) rom.ArrayPosition.y].GetComponent<RoomContainer>() : null;
            }
            else rom.RoomGO.partLeft = null;

            if (rom.ArrayPosition.x + 1 < partMap.GetLength(0)) {
                rom.RoomGO.partRight = partMap[(int) rom.ArrayPosition.x + 1, (int) rom.ArrayPosition.y] != null ? partMap[(int) rom.ArrayPosition.x + 1, (int) rom.ArrayPosition.y].GetComponent<RoomContainer>() : null;
            }
            else rom.RoomGO.partRight = null;

            if ((rom.ArrayPosition.y - 1) >= 0) {
                rom.RoomGO.partBot = partMap[(int) rom.ArrayPosition.x, (int) rom.ArrayPosition.y - 1] != null ? partMap[(int) rom.ArrayPosition.x, (int) rom.ArrayPosition.y - 1].GetComponent<RoomContainer>() : null;
            }
            else rom.RoomGO.partBot = null;


            if (rom.ArrayPosition.y + 1 < partMap.GetLength(1)) {
                rom.RoomGO.partTop = partMap[(int) rom.ArrayPosition.x, (int) rom.ArrayPosition.y + 1] != null ? partMap[(int) rom.ArrayPosition.x, (int) rom.ArrayPosition.y + 1].GetComponent<RoomContainer>() : null;
            }
            else rom.RoomGO.partTop = null;

            rom.RoomGO.exitTop = rom.exitTop;
            if (rom.RoomGO.exitTop) topExits.Add(rom.RoomGO);
            else if (topExits.Contains(rom.RoomGO)) topExits.Remove(rom.RoomGO);

            rom.RoomGO.exitLeft = rom.exitLeft;
            if (rom.RoomGO.exitLeft) leftExits.Add(rom.RoomGO);
            else if (leftExits.Contains(rom.RoomGO)) leftExits.Remove(rom.RoomGO);

            rom.RoomGO.exitRight = rom.exitRight;
            if (rom.RoomGO.exitRight) rightExits.Add(rom.RoomGO);
            else if (rightExits.Contains(rom.RoomGO)) rightExits.Remove(rom.RoomGO);

            rom.RoomGO.exitBot = rom.exitBot;
            if (rom.RoomGO.exitBot) botExits.Add(rom.RoomGO);
            else if (botExits.Contains(rom.RoomGO)) botExits.Remove(rom.RoomGO);


            rom.RoomGO.UpdatePart();
        }

        if (exit != null) ;
    }

    public void DictionaryUpdate() {
        topExits = new List<RoomContainer>();
        exitDicitonnary.Add(Generation.open.top, topExits);
        leftExits = new List<RoomContainer>();
        exitDicitonnary.Add(Generation.open.left, leftExits);
        rightExits = new List<RoomContainer>();
        exitDicitonnary.Add(Generation.open.right, rightExits);
        botExits = new List<RoomContainer>();
        exitDicitonnary.Add(Generation.open.bot, botExits);
    }
}