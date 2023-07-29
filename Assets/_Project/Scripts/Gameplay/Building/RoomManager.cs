using System;
using System.Collections.Generic;
using _Project.Scripts.ScriptableObjects.RoomType;
using _Project.Scripts.ScriptableObjects.SOEvent;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace _Project.Scripts.Gameplay.Building
{
    public class RoomManager : MonoBehaviour
    {
        [Header("Room Prefabs")]
        [SerializeField] private GameObject roomPrefab;
        [SerializeField] private GameObject wcPrefab;
        [SerializeField] private GameObject diningRoomPrefab;
        [Space]
        
        [SerializeField] private List<Room> roomsList;
        
        [SerializeField] private RoomType selectedRoomTypeSO;

        [Header("Events")] 
        [SerializeField] private SOEvent OnHideAllSlots;
        [SerializeField] private SOEvent OnShowRemoveSigns;
        
        void Awake()
        {
            RegisterEvents();
        }
        
        private void RegisterEvents()
        {
            OnShowRemoveSigns.RegisterToEvent(ShowAllRemoveSigns);
            OnHideAllSlots.RegisterToEvent(HideAllSlots);
        }
        
        void OnDestroy()
        {
            DeregisterEvents();
        }
        
        private void DeregisterEvents()
        {
            OnShowRemoveSigns.DeregisterFromEvent(ShowAllRemoveSigns);
            OnHideAllSlots.DeregisterFromEvent(HideAllSlots);
        }

        private void HideAllSlots()
        {
            foreach (Room room in roomsList)
            {
                room.SetRemoveRoomWidth1Button(false);
                room.SetRemoveRoomWidth2Button(false);
            }
        }

        private void ShowAllRemoveSigns()
        {
            for (int index = 0; index < roomsList.Count; index++)
            {
                Room room = roomsList[index];
                
                if (!room.slot.isOccupied || room.slot.roomType == Room.RoomTypes.Reception)
                {
                    continue;
                }

                if (room.slot.roomType == Room.RoomTypes.DiningRoom)
                {
                    room.SetRemoveRoomWidth2Button(true);
                    
                    // We increment the index because we don't want the other room to show its removal button 2.
                    index++;
                }
                else
                {
                    room.SetRemoveRoomWidth1Button(true);
                }
            }
        }

        #region Public Methods

        public void CreateRoom(int index)
        {
            if (roomsList[index].slot.isOccupied)
            {
                return;
            }

            Room.RoomTypes selectedRoomType = selectedRoomTypeSO.SelectedRoomType;
            
            SetRoomSlotProperties(index, true, selectedRoomType);
            GameObject roomObject = InstantiateRoom(index, selectedRoomType);

            if (selectedRoomType == Room.RoomTypes.DiningRoom)
            {
                // Also create a new room in the next slot if it's a 2 block wide room.
                SetRoomSlotProperties(index + 1, true, selectedRoomType);
                InstantiateRoom(index + 1, selectedRoomType, roomObject);
            }
        }
        
        private int SearchRoomByPosition(Vector3 pos)
        {
            for (int index = 0; index < roomsList.Count; index++)
            {
                Room room = roomsList[index];
                
                if (Mathf.Approximately(Vector3.Distance(room.gameObject.transform.position, pos), 0))
                {
                    return index;
                }
            }

            throw new Exception("No room was found!");
        }

        private void SetRoomSlotProperties(int index, bool isOccupied, Room.RoomTypes roomType)
        {
            Room currentRoom = roomsList[index];
            
            currentRoom.slot.isOccupied = isOccupied;
            currentRoom.slot.roomType = roomType;
        }
        
        private GameObject InstantiateRoom(int index, Room.RoomTypes selectedRoomType, GameObject roomObject = null)
        {
            Room room = roomsList[index];

            room.slot.roomObject = roomObject != null
                ? roomObject
                : Instantiate(GetPrefabByRoomType(selectedRoomType),
                    GetPositionByRoomType(room.transform.position, selectedRoomType), Quaternion.identity,
                    room.transform);

            return room.slot.roomObject;
        }
        
        private GameObject GetPrefabByRoomType(Room.RoomTypes roomType)
        {
            switch (roomType)
            {
                case Room.RoomTypes.Room:
                    return roomPrefab;
                case Room.RoomTypes.Wc:
                    return wcPrefab;
                case Room.RoomTypes.DiningRoom:
                    return diningRoomPrefab;
                default:
                    return null;
            }
        }

        private Vector3 GetPositionByRoomType(Vector3 pos, Room.RoomTypes roomType)
        {
            if(roomType == Room.RoomTypes.DiningRoom)
            {
                return pos + new Vector3(0.5f, 0f, 0f);
            }

            return pos;
        }
        
        public void RemoveRoom(Transform roomTransform)
        {
            int index = SearchRoomByPosition(roomTransform.position);
            Room room = roomsList[index];

            if (!room.slot.isOccupied || room.slot.roomType == Room.RoomTypes.Reception)
            {
                return;
            }
            
            // We need to check the room type of the current room before we change it so we can operate on the next room.
            if (room.slot.roomType == Room.RoomTypes.DiningRoom)
            {
                SetRoomSlotProperties(index + 1, false, Room.RoomTypes.None);
            }
            SetRoomSlotProperties(index, false, Room.RoomTypes.None);
            
            DestroyRoomObject(room);
        }

        private void DestroyRoomObject(Room room)
        {
            Destroy(room.slot.roomObject);
        }

        public bool IsRoomOccupied(int index)
        {
            if (index > roomsList.Count - 1 || index < 0)
            {
                return true;
            }
            
            return roomsList[index].slot.isOccupied;
        }
        
        #endregion
    }
}
