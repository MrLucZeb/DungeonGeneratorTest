using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator2 : MonoBehaviour
{
    public struct Room
    {
        public Vector2 position;
        public Vector2 size;
    }

    public float minRooms = 5;
    public float maxRooms = 10;
    public float maxTries = 1000;
    public float minSize;
    public float maxSize;
    [Range(0, 1)]
    public float maxRatio;
    public Vector2 boundary;

    public GameObject roomPrefab;
    List<Room> rooms = new List<Room>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            Generate();
    }

    public bool IsValid(Room room)
    {
       // return true;
        //if (!AABB(room.position, room.size, Vector2.zero, boundary))
        //    return false;

        foreach (Room room2 in rooms)
        {
            if (AABB(room.position, room.size, room2.position, room2.size))
            {
                return false;
            }
        }

        return true;
    }

    public bool AABB(Vector2 aPos, Vector2 aSize, Vector2 bPos, Vector2 bSize) 
    {
        return (aPos.x < bPos.x + bSize.x &&
                aPos.x + aSize.x > bPos.x &&
                aPos.y < bPos.y + bSize.y &&
                aPos.y + aSize.y > bPos.y);
    }
    void Generate()
    {
        rooms.Clear();

        foreach (GameObject room in GameObject.FindGameObjectsWithTag("Room"))
        {
            Destroy(room);
        }

        for (int i = 0; i < maxTries && rooms.Count < maxRooms; i++)
        {
            Room room = new Room();
            room.size.x = Random.Range(minSize, maxSize + 1);
            room.size.y = Random.Range(minSize, maxSize + 1);
            room.size.y = Mathf.Clamp(room.size.y, room.size.y * maxRatio, room.size.y * (1 + maxRatio));
            room.position.x = Random.Range(0, boundary.x);
            room.position.y = Random.Range(0, boundary.y);

            if (IsValid(room))
            {
                rooms.Add(room);
                Debug.Log("Valid");
            } else
            {
                Debug.Log("Invalid");
            }
               
        }

        foreach (Room room in rooms)
        {
            GameObject roomObject = Instantiate(roomPrefab, new Vector3(room.position.x, 0, room.position.y) * 10, Quaternion.identity);
            roomObject.transform.localScale = new Vector3(room.size.x, 1, room.size.y);
            roomObject.GetComponentInChildren<Renderer>().material.color = Random.ColorHSV();
            roomObject.tag = "Room";
        }
    }
}
