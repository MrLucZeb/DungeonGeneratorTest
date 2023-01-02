using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [Header("Settings")]
    public int splitAmount = 10;
    public int maxSplits = 7;
    public int rWidth;
    public int rHeight;
    [Range(0, 1)]
    public float minScale = 0.1f;
    [Range(0, 1)]
    public float maxScale = 0.9f;
    [Range(0, 1)]
    public float maxDifference = 0.25f;
    [Range(0, 0.5f)]
    public float splitBorder = 0.1f;

    public int maxSequenctialSplits = 2;
    public float minSize = 0.1f;
    

    public GameObject roomPrefab;

   
    List<GameObject> rObjects;

    List<Room> rList;


    
    struct Room
    {
        public Vector2 basePosition;
        public Vector2 worldPosition;
        public Vector2 size;
        public Vector2 worldScale;
        public int lastSplit;
        public bool horizontal;
        public int horizontalSplits;
        public int verticalSplits;

        public Room(Vector2 position, Vector2 size)
        {
            this.basePosition = position;
            this.worldPosition = new Vector2();
            this.worldScale = new Vector2();
            this.size = size;
            this.horizontal = true;
            this.horizontalSplits = 0;
            this.verticalSplits = 0;
            this.lastSplit = 0;
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            rList = new List<Room>() { new Room(new Vector2(0, 0), new Vector2(rWidth, rHeight)) };
            //DrawRoom();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Room[] rooms = SplitRoom(rList[0], true);

            rList.Clear();
            rList.Add(rooms[0]);
            rList.Add(rooms[1]);

            Debug.Log("Room 0: position" + rooms[0].basePosition + " size: " + rooms[0].size);
            Debug.Log("Room 1: position" + rooms[1].basePosition + " size: " + rooms[1].size);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Room[] rooms = SplitRoom(rList[0], false);

            rList.Clear();
            rList.Add(rooms[0]);
            rList.Add(rooms[1]);
            // DrawRoom();

            Debug.Log("Room 0: position" + rooms[0].basePosition + " size: " + rooms[0].size);
            Debug.Log("Room 1: position" + rooms[1].basePosition + " size: " + rooms[1].size);
        }

        if (Input.GetKeyDown(KeyCode.G)) {
            
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Generate(rWidth, rHeight);
           // DrawRoom();
        }
    }

    void DrawRoom()
    {
        //foreach(GameObject roomObject in rObjects)
        //{
        //    Destroy(roomObject);
        //}

        foreach (Room room in rList)
        {
            Debug.Log(rList.Count);
            GameObject roomObject = Instantiate(roomPrefab, new Vector3(room.basePosition.x, 0, room.basePosition.y) * 10, Quaternion.identity);

            roomObject.transform.localScale = new Vector3(room.size.x, 1, room.size.y);
            roomObject.GetComponentInChildren<Renderer>().material.color = Random.ColorHSV();
        }
    }

    void Generate(int width, int height)
    {
        foreach (GameObject room in GameObject.FindGameObjectsWithTag("Room"))
        {
            Destroy(room);
        }

        rList = new List<Room>() { new Room(new Vector2(0, 0), new Vector2(width, height)) };
        List<Room> test = new List<Room>();

        int hLevel = 1;
        for (int i = 0; i < splitAmount; i++)
        {
            int index = rList.Count;
            for (int j = 1; j <= hLevel; j++)
            {
                Room[] rooms = SplitRoom(rList[index - j], Random.Range(0, 2) == 0);

                rList.Add(rooms[0]);
                rList.Add(rooms[1]);

                if (i == splitAmount - 1)
                {
                    test.Add(rooms[0]);
                    test.Add(rooms[1]);
                }
            }

            hLevel *= 2;
        }

        //for (int i = 1; i <= hLevel; i++)
        //{
        //    Room room = rList[rList.Count - i];
        //    room.worldScale = new Vector2(Mathf.Clamp(Random.Range(0, room.size.x), room.size.x * minScale, room.size.x), Mathf.Clamp(Random.Range(0, room.size.y), room.size.y * minScale, room.size.y));
        //    room.worldPosition = room.basePosition + new Vector2(Random.Range(0, room.size.x - room.worldScale.x), Random.Range(0, room.size.y - room.worldScale.y));

        //    GameObject roomObject = Instantiate(roomPrefab, new Vector3(room.worldPosition.x, 0, room.worldPosition.y) * 10, Quaternion.identity);
        //    roomObject.transform.localScale = new Vector3(room.worldScale.x, 1, room.worldScale.y);
        //    roomObject.GetComponentInChildren<Renderer>().material.color = Random.ColorHSV();

        //    Debug.Log(room.horizontal);

        //}
        for (int i = 0; i < test.Count; i++) {
            Room room = test[i];
            room.worldScale = new Vector2(Random.Range(room.size.x * minScale, room.size.x * maxScale), Random.Range(room.size.y * minScale, room.size.y * maxScale));
            Vector2 d = new Vector2(Random.Range(0, room.size.x - room.worldScale.x), -Random.Range(0, room.size.y - room.worldScale.y));

            room.worldPosition = room.basePosition + d;// + new Vector2(Random.Range(0, room.size.x - room.worldScale.x), Random.Range(0, room.size.y - room.worldScale.y));

            GameObject roomObject = Instantiate(roomPrefab, new Vector3(room.worldPosition.x, 0, room.worldPosition.y) * 10, Quaternion.identity);
            roomObject.transform.localScale = new Vector3(room.worldScale.x, 1, room.worldScale.y);
            roomObject.GetComponentInChildren<Renderer>().material.color = Random.ColorHSV();
            roomObject.tag = "Room";
        }
    }

    //void Generate(int width, int height)
    //{
    //    rList = new List<Room>() { new Room(new Vector2(0, 0), new Vector2(width, height)) };
    //    List<Room> newList = new List<Room>();

    //    int hLevel = 0;
    //    for (int i = 0; i < splitAmount; i++)
    //    {
    //        bool splitHorizontal = Random.Range(0, 2) == 1;
    //        for (int j = 1; j < hLevel * 2; j++)
    //        {

    //            int roomIndex = Random.Range(0, rList.Count);

    //            Room[] rooms = SplitRoom(rList[rList.Count - j], splitHorizontal);

    //            //newList.RemoveAt(roomIndex);
    //            newList.Add(rooms[0]);
    //            newList.Add(rooms[1]);
    //        }

    //    }
    //}

    Room[] SplitRoom(Room room, bool horizontal)
    {
        Room[] rooms = new Room[2];

        Debug.Log("De: " + room.size.x * splitBorder);
        if (horizontal && room.size.x * splitBorder <= minSize) { horizontal = true; }
        else if (!horizontal && room.size.y * splitBorder <= minSize) { horizontal = false; }
        
        if (room.horizontalSplits != 0 && room.verticalSplits != 0)
        {
            if (horizontal && (room.horizontalSplits + 1) / room.verticalSplits < maxDifference)
                horizontal = false;
            else if (!horizontal && (room.verticalSplits + 1) / room.horizontalSplits < maxDifference)
                horizontal = true;
        }

        if (horizontal && room.horizontalSplits >= maxSplits)
        {
            horizontal = false;
        }
        else if (!horizontal && room.verticalSplits >= maxSplits)
        {
            horizontal = true;
        }



        if (horizontal && room.lastSplit >= maxSequenctialSplits)
        {
            horizontal = false;
        } else if (!horizontal && room.lastSplit <= -maxSequenctialSplits)
        {
            horizontal = true;
        }

        if (horizontal)
        {

            rooms[0].horizontalSplits = room.horizontalSplits + 1;
            rooms[1].horizontalSplits = room.horizontalSplits + 1;
            rooms[0].lastSplit++;
            rooms[1].lastSplit++;

        } 
        else
        {
            rooms[0].verticalSplits = room.verticalSplits + 1;
            rooms[1].verticalSplits = room.verticalSplits + 1;
            rooms[0].lastSplit--;
            rooms[1].lastSplit--;
        }


        Vector2 size;
        Vector2 size2;
        Vector2 positionOffset;
        float division = Random.Range(splitBorder, 1 - splitBorder);
        if (horizontal)
        {
            size = new Vector2(room.size.x, room.size.y * division);
            size2 = new Vector2(room.size.x, room.size.y * (1 - division));
            positionOffset = new Vector2(0, -size.y);
       
        }
        else
        {
            size = new Vector2(room.size.x * division, room.size.y);
            size2 = new Vector2(room.size.x * (1 - division), room.size.y);
            positionOffset = new Vector2(size.x, 0);
        }

        rooms[0] = new Room(room.basePosition, size);
        rooms[1] = new Room(room.basePosition + positionOffset, size2);

        if (horizontal)
        {
            rooms[0].horizontalSplits = room.horizontalSplits + 1;
            rooms[1].horizontalSplits = room.horizontalSplits + 1;
        }
        else
        {
            rooms[0].verticalSplits = room.verticalSplits + 1;
            rooms[1].verticalSplits = room.verticalSplits + 1;
        }


        return rooms;
    }
}

struct Node
{
    List<Node> children;
}

struct Tree
{
    Node root;
}
