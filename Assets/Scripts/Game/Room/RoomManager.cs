using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header("Room List")]
    [SerializeField] private List<RoomController> roomsList = new List<RoomController>();

    [Header("Spawner")]
    [SerializeField] private Vector2 minMaxSpawnRange = new Vector2(0, 3);
    [SerializeField] private List<RoomController> currentRooms = new List<RoomController>();
    private int currentRoom;

    [Header("CACHE")]
    [SerializeField] private ProceduralGridMover proceduralGrid;
    [SerializeField] private MMF_Player spawnRoomFEEDBACK;

    private Queue<RoomController> lastRoomsQueue = new Queue<RoomController>();

    public int GetCurrentRoom()
    {
        return currentRoom;
    }

    public void SpawnNewRoom(Vector3 endPosition)
    {
        AddRoomCount();
        spawnRoomFEEDBACK.PlayFeedbacks();
        RoomController room = Instantiate(GetRandomRoom(), endPosition, Quaternion.identity, transform);
        currentRooms.Add(room);
    }

    public void SetGridTarget(Transform roomCenter)
    {
        proceduralGrid.target = roomCenter;
    }

    public void RemoveFirstRoomFromList()
    {
        StartCoroutine(RemoveFirstRoomFromList_COROUTINE());
    }

    private IEnumerator RemoveFirstRoomFromList_COROUTINE()
    {
        yield return new WaitForSeconds(1f);
        currentRooms[0].RemoveEnemies();
        Destroy(currentRooms[0].gameObject);
        currentRooms.RemoveAt(0);
    }

    private RoomController GetRandomRoom()
    {
        RoomController selectedRoom;
        int attempts = 0;

        do
        {
            selectedRoom = roomsList[(int)Random.Range(minMaxSpawnRange.x, minMaxSpawnRange.y)];
            attempts++;
        }
        while (lastRoomsQueue.Contains(selectedRoom) && attempts < 10);

        lastRoomsQueue.Enqueue(selectedRoom);
        if (lastRoomsQueue.Count > 2)
        {
            lastRoomsQueue.Dequeue();
        }

        return selectedRoom;
    }

    private void AddRoomCount()
    {
        currentRoom++;
        //if (currentRoom % 3 == 0)
        AddMinMaxSpawn();
    }

    private void AddMinMaxSpawn()
    {
        if (minMaxSpawnRange.y < roomsList.Count)
            minMaxSpawnRange.y++;
    }

    public void AddEnemy(GameObject enemy)
    {
        currentRooms[0].AddEnemy(enemy);
    }

    public void RemoveEnemiesLastRoom()
    {
        currentRooms[0].RemoveEnemies();
    }
}
