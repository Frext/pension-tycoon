using System;
using System.Collections.Generic;
using _Project.Scripts.ScriptableObjects.RoomType;
using _Project.Scripts.ScriptableObjects.SOEvent;
using _Project.Scripts.ScriptableObjects.SoEventTransform;
using UnityEngine;
using static _Project.Scripts.Gameplay.Building.Room;

namespace _Project.Scripts.Gameplay.Building
{
    public class FloorManager : MonoBehaviour
    {
        [Header("Building Parts Prefabs")]
        [SerializeField] private GameObject floorPrefab;
        [SerializeField] private GameObject customerSingleRoomPrefab;
        [SerializeField] private GameObject customerDoubleRoomPrefab;
        [SerializeField] private GameObject wcPrefab;
        [SerializeField] private GameObject diningRoomPrefab;
        
        [Header("Floor Positioning")] 
        [SerializeField] private Transform floorsParentTransform;
        [SerializeField] private Vector3 floorBasePosition;
        [SerializeField] private Vector3 floorOffsetPerFloor;

        [Header("Roof Positioning")]
        [SerializeField] private Transform roofTransform;
        [SerializeField] private Vector3 roofBasePosition;
        [SerializeField] private Vector3 roofOffsetPerFloor;

        [Header("So Events")] 
        [SerializeField] private SoEvent OnAppendFloor;
        [SerializeField] private SoEvent OnHideSlotUI;
        [SerializeField] private SoEvent OnShowRemoveSigns;
        [SerializeField] private SoEventTransform OnRemoveRoom;
        [Space]
        
        [SerializeField] private SoRoomType selectedRoomTypeSo;

        public int FloorCount => floorsParentTransform.childCount;
        public readonly int RoomCountPerFloor = 6;
        
        private readonly int RoomsParentIndex = 2;
        
        private List<List<Room>> roomsList = new();
        
        
        void Awake()
        {
            FetchRooms();
            
            RegisterEvents();
        }
        
        private void FetchRooms()
        {
            roomsList.Clear();

            for (int floorIndex = 0; floorIndex < FloorCount; floorIndex++)
            {
                roomsList.Add(new List<Room>());
                
                for (int roomIndex = 0; roomIndex < RoomCountPerFloor; roomIndex++)
                {
                    Room room = floorsParentTransform.GetChild(floorIndex).
                        GetChild(RoomsParentIndex).GetChild(roomIndex).GetComponent<Room>();
                    
                    roomsList[floorIndex].Add(room);
                }
            }
        }
        
        private void RegisterEvents()
        {
            OnAppendFloor.RegisterToEvent(AppendFloor);
            OnShowRemoveSigns.RegisterToEvent(ShowAllRemoveSigns);
            OnHideSlotUI.RegisterToEvent(HideAllSlots);
            OnRemoveRoom.RegisterToEvent(RemoveRoom);
        }
        
        private void AppendFloor()
        {
            Vector3 newFloorPos = floorBasePosition + floorOffsetPerFloor * FloorCount;
            Instantiate(floorPrefab, newFloorPos, Quaternion.identity, floorsParentTransform);
            
            PlaceRoof();
            
            FetchRooms();
        }
        
        private void PlaceRoof()
        {
            Vector3 roofPos = roofBasePosition + roofOffsetPerFloor * FloorCount;

            roofTransform.position = roofPos;
        }
        
        private void ShowAllRemoveSigns()
        {
            for (int floorIndex = 0; floorIndex < FloorCount; floorIndex++)
            {
                for (int roomIndex = 0; roomIndex < RoomCountPerFloor; roomIndex++)
                {
                    Room room = roomsList[floorIndex][roomIndex];
                    
                    if (!IsRoomConstructed(room) || room.slot.roomType == RoomTypeEnum.Reception)
                    {
                        continue;
                    }

                    if (GetRoomWidth(room.slot.roomType) == 2)
                    {
                        room.SetRemoveRoomWidth2Button(true);
                    
                        // We increment the room index because we don't want the other room to show its removal button of width 2.
                        roomIndex++;
                    }
                    else
                    {
                        room.SetRemoveRoomWidth1Button(true);
                    }
                }
            }
        }
        
        private bool IsRoomConstructed(Room room)
        {
            return room.slot.roomType != RoomTypeEnum.None;
        }
        
        private void HideAllSlots()
        {
            foreach (List<Room> floor in roomsList)
            {
                foreach (Room room in floor)
                {
                    room.SetRemoveRoomWidth1Button(false);
                    room.SetRemoveRoomWidth2Button(false);
                }
            }
        }
        
        private void RemoveRoom(Transform roomTransform)
        {
            Vector2Int index = GetRoomByPosition(roomTransform.position);
            Room room = roomsList[index.y][index.x];

            if (!IsRoomConstructed(room))
            {
                return;
            }
            
            // We need to check the room type of the current room before we change it so we can operate on the next room.
            if (GetRoomWidth(room.slot.roomType) == 2)
            {
                SetRoomSlotProperties(roomsList[index.y][index.x + 1], false, RoomTypeEnum.None);
            }
            SetRoomSlotProperties(room, false, RoomTypeEnum.None);
            
            DestroyRoomGameObject(room);
        }
        
        private Vector2Int GetRoomByPosition(Vector3 pos)
        {
            for (int floorIndex = 0; floorIndex < FloorCount; floorIndex++)
            {
                for (int roomIndex = 0; roomIndex < RoomCountPerFloor; roomIndex++)
                {
                    Room room = roomsList[floorIndex][roomIndex];
                
                    if (Mathf.Approximately(Vector3.Distance(room.gameObject.transform.position, pos), 0f))
                    {
                        return new Vector2Int(roomIndex, floorIndex);
                    }
                }
            }
            
            throw new Exception("No room was found at '" + pos + "'.");
        }
        
        private void SetRoomSlotProperties(Room room, bool isOccupied, RoomTypeEnum roomType)
        {
            room.slot.isOccupied = isOccupied;
            room.slot.roomType = roomType;
        }

        private void DestroyRoomGameObject(Room room)
        {
            Destroy(room.slot.roomObject);
        }

        void Start()
        {
            PlaceRoof();
        }
        
        void OnDestroy()
        {
            DeregisterEvents();
        }

        private void DeregisterEvents()
        {
            OnAppendFloor.DeregisterFromEvent(AppendFloor);
            OnShowRemoveSigns.DeregisterFromEvent(ShowAllRemoveSigns);
            OnHideSlotUI.DeregisterFromEvent(HideAllSlots);
            OnRemoveRoom.DeregisterFromEvent(RemoveRoom);
        }
        
        #region Methods used by other classes
        
        public bool IsRoomConstructedAt(Vector2Int index)
        {
            if (IsOutOfBoundaries(index.y, 0, FloorCount - 1) || 
                IsOutOfBoundaries(index.x, 0, RoomCountPerFloor - 1))
            {
                return true;
            }
            
            return IsRoomConstructed(roomsList[index.y][index.x]);
        }

        private bool IsOutOfBoundaries(int number, int min, int max)
        {
            return number < min || number > max;
        }

        public void CreateRoomAt(Vector2Int index)
        {
            Room room = roomsList[index.y][index.x];
            
            if (IsRoomConstructed(room))
            {
                return;
            }

            RoomTypeEnum selectedRoomType = selectedRoomTypeSo.SelectedRoomType;
            
            SetRoomSlotProperties(room, true, selectedRoomType);
            GameObject instantiatedRoomObject = InstantiateRoomGameObject(room);

            if (GetRoomWidth(selectedRoomType) == 2)
            {
                Room nextRoom = roomsList[index.y][index.x + 1]; 
                
                // Also create a new room in the next slot if it's a 2 block wide room.
                SetRoomSlotProperties(nextRoom, true, selectedRoomType);
                InstantiateRoomGameObject(nextRoom, instantiatedRoomObject);
            }
        }

        private GameObject InstantiateRoomGameObject(Room room, GameObject roomObject = null)
        {
            RoomTypeEnum selectedRoomType = selectedRoomTypeSo.SelectedRoomType;

            room.slot.roomObject = roomObject != null
                ? roomObject
                : Instantiate(GetPrefabByRoomType(selectedRoomType),
                    GetPositionByRoomType(room.transform.position, selectedRoomType), Quaternion.identity,
                    room.transform);

            return room.slot.roomObject;
        }
        
        private GameObject GetPrefabByRoomType(RoomTypeEnum roomType)
        {
            return roomType switch
            {
                RoomTypeEnum.CustomerSingle => customerSingleRoomPrefab,
                RoomTypeEnum.CustomerDouble => customerDoubleRoomPrefab,
                RoomTypeEnum.Wc => wcPrefab,
                RoomTypeEnum.Dining => diningRoomPrefab,
                _ => null
            };
        }

        private Vector3 GetPositionByRoomType(Vector3 pos, RoomTypeEnum roomType)
        {
            if(GetRoomWidth(roomType) == 2)
            {
                return pos + new Vector3(0.5f, 0f, 0f);
            }

            return pos;
        }

        #endregion
    }
}
