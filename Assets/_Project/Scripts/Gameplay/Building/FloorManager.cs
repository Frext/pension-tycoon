using System;
using System.Collections.Generic;
using _Project.Scripts.Gameplay.Data;
using _Project.Scripts.ScriptableObjects.RoomType;
using _Project.Scripts.ScriptableObjects.SoEventRoom;
using UnityEngine;
using UnityEngine.Events;
using static _Project.Scripts.Gameplay.Building.Room;
using static _Project.Scripts.Gameplay.Building.Room.RoomTypeEnum;

namespace _Project.Scripts.Gameplay.Building
{
    public class FloorManager : MonoBehaviour
    {
        [Serializable]
        public class Floor
        {
            public List<Room> roomsList = new();
        }
        
        [Serializable]
        public class FloorSlot
        {
            public List<RoomSlot> slotsList = new();
        }
        
        [Header("Building Parts Prefabs")]
        [SerializeField] private GameObject floorPrefab;
        [SerializeField] private GameObject customerSingleRoomPrefab;
        [SerializeField] private GameObject customerDoubleRoomPrefab;
        [SerializeField] private GameObject bathroomPrefab;
        [SerializeField] private GameObject diningRoomPrefab;
        [SerializeField] private GameObject arcadePrefab;
        [SerializeField] private GameObject gymPrefab;
        
        [Header("Floor Positioning")] 
        [SerializeField] private Transform floorsParentTransform;
        [SerializeField] private Vector3 floorBasePosition;
        [SerializeField] private Vector3 floorOffsetPerFloor;

        [Header("Roof Positioning")]
        [SerializeField] private Transform roofTransform;
        [SerializeField] private Vector3 roofBasePosition;
        [SerializeField] private Vector3 roofOffsetPerFloor;

        [Header("Events")]
        [SerializeField] private SoEventRoom OnRemoveRoom;
        [Space] 
        [SerializeField] private UnityEvent OnDecrementPensionRating;
        [Space]
        
        [SerializeField] private SoRoomType selectedRoomTypeSo;
        [Space]
        
        [Header("Data Saving")] 
        [SerializeField] private string dataKey = "floorSlotsList";

        public int FloorCount => floorsList.Count;
        public const int RoomCountPerFloor = 6;
        private const int RoomsParentIndex = 2;

        private readonly List<Floor> floorsList = new();
        
        
        void Awake()
        {
            LoadRooms();
            
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            OnRemoveRoom.RegisterToEvent(RemoveRoom);
        }

        private void LoadRooms()
        {
            floorsList.Clear();
            
            // Add the room scripts of the rooms in the first floor to the floors list because they already exist in the scene.
            AddRoomsInFloorToFloorsList(floorsParentTransform.GetChild(0).GetChild(RoomsParentIndex));

            // Load the state of the slots if there are any.
            if (DataManager.Load<List<FloorSlot>>(dataKey, out var floorSlotsList))
            {
                // Build the floors and add them to floors list
                BuildFloors(floorSlotsList.Count - 1);
            
                // Then assign the loaded slots to the floors list
                AssignSlots(floorSlotsList);

                // Finally create the game objects because they weren't stored inside the data
                CreateSlotGameObjects();
            }
        }
        
        private void AddRoomsInFloorToFloorsList(Transform roomsTransform)
        {
            floorsList.Add(new Floor());
            
            for (int roomIndex = 0; roomIndex < RoomCountPerFloor; roomIndex++)
            {
                Room room = roomsTransform.GetChild(roomIndex).GetComponent<Room>();
                
                floorsList[^1].roomsList.Add(room);
            }
        }
        
        private void BuildFloors(int floorCount)
        {
            // Then build the other floors
            for (int floorSlotIndex = 0; floorSlotIndex < floorCount; floorSlotIndex++)
            {
                AppendFloor();
            }
        }
        
        public void AppendFloor()
        {
            GameObject instantiatedFloor = Instantiate(floorPrefab,
                floorBasePosition + floorOffsetPerFloor * FloorCount,
                Quaternion.identity, floorsParentTransform);
            
            AddRoomsInFloorToFloorsList(instantiatedFloor.transform.GetChild(RoomsParentIndex));
            
            PlaceRoof();
        }
        
        private void PlaceRoof()
        {
            Vector3 roofPos = roofBasePosition + roofOffsetPerFloor * (FloorCount - 1);

            roofTransform.position = roofPos;
        }
        
        private void AssignSlots(List<FloorSlot> floorSlotsList)
        {
            for (int floorIndex = 0; floorIndex < FloorCount; floorIndex++)
            {
                for (int roomIndex = 0; roomIndex < RoomCountPerFloor; roomIndex++)
                {
                    // Assign the slot properties
                    floorsList[floorIndex].roomsList[roomIndex].slot = floorSlotsList[floorIndex].slotsList[roomIndex];
                }
            }
        }
        
        private void CreateSlotGameObjects()
        {
            for (int floorIndex = 0; floorIndex < FloorCount; floorIndex++)
            {
                for (int roomIndex = 0; roomIndex < RoomCountPerFloor; roomIndex++)
                {
                    RoomTypeEnum roomType = floorsList[floorIndex].roomsList[roomIndex].slot.roomType;
                    
                    if (GetPrefabByRoomType(roomType) == null)
                    {
                        continue;
                    }
                    
                    selectedRoomTypeSo.SetSelectedRoomTypeTo(roomType);
                    InstantiateRoomGameObject(new Vector2Int(roomIndex, floorIndex));

                    // If it's a 2 slot wide room, don't instantiate it in the next slot.
                    if (GetRoomWidth(roomType) == 2)
                    {
                        roomIndex++;
                    }
                }
            }
        }
        
        public void ShowAllRemoveSigns()
        {
            for (int floorIndex = 0; floorIndex < FloorCount; floorIndex++)
            {
                for (int roomIndex = 0; roomIndex < RoomCountPerFloor; roomIndex++)
                {
                    Room room = floorsList[floorIndex].roomsList[roomIndex];
                    
                    if (!IsRoomConstructed(room) || room.slot.roomType == Reception)
                    {
                        continue;
                    }

                    if (GetRoomWidth(room.slot.roomType) == 2)
                    {
                        room.SetMakeUsableButton(true, 2);
                    
                        // We increment the room index because we don't want the other room to show its removal button of width 2.
                        roomIndex++;
                    }
                    else
                    {
                        room.SetMakeUsableButton(true);
                    }
                }
            }
        }
        
        private bool IsRoomConstructed(Room room)
        {
            return room.slot.roomType != None;
        }
        
        public void HideAllRemoveSlotsButtons()
        {
            foreach (Floor floor in floorsList)
            {
                foreach (Room room in floor.roomsList)
                {
                    room.SetRemoveRoomButton(false);
                }
            }
        }
        
        private void SetMakeRoomUsableButton(Room room, bool activeState)
        {
            room.SetMakeUsableButton(activeState, GetRoomWidth(room.slot.roomType));
        }
        
        private void RemoveRoom(Room room)
        {
            Vector2Int index = GetIndexByRoom(room);

            if (!IsRoomConstructed(room))
            {
                return;
            }
            
            SetRoomSlotProperties(index, None);
            
            DestroyRoomGameObject(room);
        }
        
        private Vector2Int GetIndexByRoom(Room room)
        {
            for (int floorIndex = 0; floorIndex < FloorCount; floorIndex++)
            {
                for (int roomIndex = 0; roomIndex < RoomCountPerFloor; roomIndex++)
                {
                    if (floorsList[floorIndex].roomsList[roomIndex] == room)
                    {
                        return new Vector2Int(roomIndex, floorIndex);
                    }
                }
            }

            throw new Exception("No room was found with room '" + room.gameObject + "'.");
        }
        
        private void SetRoomSlotProperties(Vector2Int index, RoomTypeEnum roomType, bool isOccupied = false, bool isUsable = true)
        {
            Room room = floorsList[index.y].roomsList[index.x];
            
            // If it's a 2-wide room is getting created or removed, set the next room slot too before changing the type.
            if (GetRoomWidth(room.slot.roomType) == 2 || GetRoomWidth(roomType) == 2)
            {
                SetSingleSlot(floorsList[index.y].roomsList[index.x + 1], roomType, isOccupied, isUsable);
            }
            
            SetSingleSlot(room, roomType, isOccupied, isUsable);
            
            SetMakeRoomUsableButton(room, !isUsable);
        }
        
        private void SetRoomSlotProperties(Vector2Int index, bool isOccupied, bool isUsable)
        {
            Room room = floorsList[index.y].roomsList[index.x];
            
            if (GetRoomWidth(room.slot.roomType) == 2)
            {
                SetSingleSlot(floorsList[index.y].roomsList[index.x + 1], isOccupied, isUsable);
            }
            
            SetSingleSlot(room, isOccupied, isUsable);
            
            SetMakeRoomUsableButton(room, !isUsable);
        }

        private bool IsIndexValid(Vector2Int index)
        {
            if (IsOutOfBoundaries(index.y, 0, FloorCount - 1) || 
                IsOutOfBoundaries(index.x, 0, RoomCountPerFloor - 1))
            {
                return false;
            }

            return true;
        }
        
        private bool IsOutOfBoundaries(int number, int min, int max)
        {
            return number < min || number > max;
        }

        private void SetSingleSlot(Room room, RoomTypeEnum roomType, bool isOccupied = false, bool isUsable = true)
        {
            room.slot.isOccupied = isOccupied;
            room.slot.isUsable = isUsable;
            room.slot.roomType = roomType;
        }
        
        private void SetSingleSlot(Room room, bool isOccupied, bool isUsable)
        {
            room.slot.isOccupied = isOccupied;
            room.slot.isUsable = isUsable;
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
            OnRemoveRoom.DeregisterFromEvent(RemoveRoom);
        }
        
        #region Methods used by other classes
        
        public bool IsRoomConstructedAt(Vector2Int index)
        {
            if (!IsIndexValid(index))
            {
                return true;
            }
            
            return IsRoomConstructed(floorsList[index.y].roomsList[index.x]);
        }

        public void CreateRoomAt(Vector2Int index)
        {
            if (IsRoomConstructed(floorsList[index.y].roomsList[index.x]) && !IsIndexValid(index))
            {
                return;
            }

            RoomTypeEnum selectedRoomType = selectedRoomTypeSo.SelectedRoomType;
            
            SetRoomSlotProperties(index, selectedRoomType);
            InstantiateRoomGameObject(index);
        }

        private void InstantiateRoomGameObject(Vector2Int index)
        {
            Room room = floorsList[index.y].roomsList[index.x];
            
            
            RoomTypeEnum selectedRoomType = selectedRoomTypeSo.SelectedRoomType;
            GameObject instantiatedRoomGameObject = Instantiate(GetPrefabByRoomType(selectedRoomType),
                GetPositionByRoomType(room.transform.position, selectedRoomType), Quaternion.identity, room.transform);
            
            SetRoomGameObject(room, instantiatedRoomGameObject);

            if (GetRoomWidth(room.slot.roomType) == 2)
            {
                SetRoomGameObject(floorsList[index.y].roomsList[index.x + 1], instantiatedRoomGameObject);
            }
        }

        private void SetRoomGameObject(Room room, GameObject roomObject)
        {
            room.slot.roomObject = roomObject;
        }
        
        private GameObject GetPrefabByRoomType(RoomTypeEnum roomType)
        {
            return roomType switch
            {
                CustomerSingle => customerSingleRoomPrefab,
                CustomerDouble => customerDoubleRoomPrefab,
                Bathroom => bathroomPrefab,
                Dining => diningRoomPrefab,
                Arcade => arcadePrefab,
                Gym => gymPrefab,
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
        
        public Room GetRoom(RoomTypeEnum roomType, bool isOccupied = false, bool isUsable = true)
        {
            foreach (Floor floor in floorsList)
            {
                foreach (Room room in floor.roomsList)
                {
                    if (room.slot.roomType == roomType && 
                        room.slot.isOccupied == isOccupied && room.slot.isUsable == isUsable)
                    {
                        return room;
                    }
                }
            }

            if (isOccupied == false && isUsable)
            {
                OnDecrementPensionRating.Invoke();
            }
            
            return null;
        }
        
        public void EnterRoom(Room room)
        {
            SetRoomSlotProperties(GetIndexByRoom(room), true, room.slot.isUsable);
        }
        
        public void LeaveRoom(Room room)
        {
            SetRoomSlotProperties(GetIndexByRoom(room), false, room.slot.isUsable);
        }
        
        public void MakeRoomUsable(Room room)
        {
            SetRoomSlotProperties(GetIndexByRoom(room), room.slot.isOccupied, true);
        }

        public void MakeRoomNotUsable(Room room)
        {
            Vector2Int index = GetIndexByRoom(room);
            
            SetRoomSlotProperties(index, room.slot.isOccupied, false);
        }

        public void ResetEveryRoom()
        {
            for (int floorIndex = 0; floorIndex < FloorCount; floorIndex++)
            {
                for (int roomIndex = 0; roomIndex < RoomCountPerFloor; roomIndex++)
                {
                    int roomWidth = GetRoomWidth(floorsList[floorIndex].roomsList[roomIndex].slot.roomType);
                    
                    SetRoomSlotProperties(new Vector2Int(roomIndex, floorIndex), false, true);

                    if (roomWidth == 2)
                    {
                        roomIndex++;
                    }
                }
            }
        }
        
        #endregion
        
        void OnApplicationQuit()
        {
            SaveRooms();
        }

        private void SaveRooms()
        {
            List<FloorSlot> floorSlots = new();
            
            for (int floorIndex = 0; floorIndex < FloorCount; floorIndex++)
            {
                floorSlots.Add(new FloorSlot());

                for (int roomIndex = 0; roomIndex < RoomCountPerFloor; roomIndex++)
                {
                    floorsList[floorIndex].roomsList[roomIndex].slot.ResetForSave();
                    
                    floorSlots[floorIndex].slotsList.Add(floorsList[floorIndex].roomsList[roomIndex].slot);
                }
            }
            
            DataManager.Save(dataKey, floorSlots);
        }
    }
}
