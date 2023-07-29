using System.Collections.Generic;
using _Project.Scripts.ScriptableObjects.SOEvent;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Building
{
    public class FloorManager : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject floorPrefab;

        [Header("Floor Positioning")] 
        [SerializeField] private Vector3 floorBasePosition;
        [SerializeField] private Vector3 floorOffsetPerFloor;
        [Space]
        
        [Header("GameObjects")]
        [SerializeField] private Transform floorsParentTransform;
        [SerializeField] private Transform roofTransform;

        [Header("Roof Positioning")]
        [SerializeField] private Vector3 roofBasePosition;
        [SerializeField] private Vector3 roofOffsetPerFloor;

        [Header("Events")] 
        [SerializeField] private SOEvent OnAppendFloor;

        public int FloorCount => floorsParentTransform. childCount;

        private List<RoomManager> roomManagersList = new();

        void Awake()
        {
            GetRoomManagers();
            
            OnAppendFloor.RegisterToEvent(AppendFloor);
        }

        private void GetRoomManagers()
        {
            roomManagersList.Clear();

            for (int index = 0; index < FloorCount; index++)
            {
                roomManagersList.Add(floorsParentTransform.GetChild(index).GetComponentInChildren<RoomManager>());
            }
        }

        void Start()
        {
            PlaceRoof();
        }

        private void OnDestroy()
        {
            OnAppendFloor.DeregisterFromEvent(AppendFloor);
        }

        private void PlaceRoof()
        {
            Vector3 roofPos = roofBasePosition + roofOffsetPerFloor * FloorCount;

            roofTransform.position = roofPos;
        }

        private void AppendFloor()
        {
            Vector3 newFloorPos = floorBasePosition + floorOffsetPerFloor * FloorCount;

            Instantiate(floorPrefab, newFloorPos, Quaternion.identity, floorsParentTransform);
            
            PlaceRoof();
            
            GetRoomManagers();
        }

        public bool IsRoomOccupiedAt(Vector2Int index)
        {
            if (index.y > roomManagersList.Count - 1 || index.y < 0)
            {
                return true;
            }
            
            return roomManagersList[index.y].IsRoomOccupied(index.x);
        }

        public void CreateRoomAt(Vector2Int index)
        {
            RoomManager roomManager = roomManagersList[Mathf.Clamp(index.y, 0, roomManagersList.Count - 1)];
            
            roomManager.CreateRoom(index.x);
        }
    }
}
