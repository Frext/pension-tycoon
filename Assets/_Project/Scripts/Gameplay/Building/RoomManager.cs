using System;
using System.Collections.Generic;
using _Project.Scripts.ScriptableObjects.RoomType;
using _Project.Scripts.ScriptableObjects.SOEvent;
using UnityEngine;
using UnityEngine.Serialization;
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
        [SerializeField] private SOEvent OnShowRoomSlots;
        [SerializeField] private SOEvent OnShowWcSlots;
        [SerializeField] private SOEvent OnShowDiningRoomSlots;
        [SerializeField] private SOEvent OnShowRemoveSigns;
        [SerializeField] private SOEvent OnHideSlots;

        void Awake()
        {
            RegisterEvents();
        }
        
        private void RegisterEvents()
        {
            OnShowRoomSlots.RegisterToEvent(ShowRoomSlotButtons);
            OnShowWcSlots.RegisterToEvent(ShowWcSlotButtons);
            OnShowDiningRoomSlots.RegisterToEvent(ShowDiningRoomSlotButtons);
            OnHideSlots.RegisterToEvent(HideAllSlots);
            OnShowRemoveSigns.RegisterToEvent(ShowAllRemoveSigns);
        }
        
        void OnDestroy()
        {
            DeregisterEvents();
        }
        
        private void DeregisterEvents()
        {
            OnShowRoomSlots.DeregisterFromEvent(ShowRoomSlotButtons);
            OnShowWcSlots.DeregisterFromEvent(ShowWcSlotButtons);
            OnShowDiningRoomSlots.DeregisterFromEvent(ShowDiningRoomSlotButtons);
            OnHideSlots.DeregisterFromEvent(HideAllSlots);
            OnShowRemoveSigns.DeregisterFromEvent(ShowAllRemoveSigns);
        }

        private void ShowRoomSlotButtons()
        {
            foreach (Room room in roomsList)
            {
                if (!room.slot.isOccupied)
                {
                    room.SetRoomSlotButton(true);
                }
            }
        }
        
        private void ShowWcSlotButtons()
        {
            foreach (Room room in roomsList)
            {
                if (!room.slot.isOccupied)
                {
                    room.SetWcSlotButton(true);
                }
            }
        }

        private void ShowDiningRoomSlotButtons()
        {
            for (int i = 0; i < roomsList.Count - 1; i++)
            {
                // If the slot width of 2 is active for the previous room, don't enable it.
                if (i > 0 && roomsList[i - 1].IsDiningRoomSlotButtonActive())
                {
                    continue;
                }
                
                // If the current room and the next one is not occupied, show a slot width of 2.
                if (!roomsList[i].slot.isOccupied && 
                    !roomsList[i + 1].slot.isOccupied)
                {
                    roomsList[i].SetDiningRoomSlotButton(true);
                }
            }
        }

        private void HideAllSlots()
        {
            foreach (Room room in roomsList)
            {
                room.SetRoomSlotButton(false);
                room.SetWcSlotButton(false);
                room.SetDiningRoomSlotButton(false);
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

        #region Button Methods

        public void CreateRoom(Transform roomTransform)
        {
            int index = SearchRoomByPosition(roomTransform.position);

            if (roomsList[index].slot.isOccupied)
            {
                return;
            }

            Room.RoomTypes selectedRoomType = selectedRoomTypeSO.SelectedRoomType;
                    
            SetRoomSlotProperties(index, true, selectedRoomType);

            if (selectedRoomType == Room.RoomTypes.DiningRoom)
            {
                // Set the next room if it's a 2 block wide room.
                SetRoomSlotProperties(Mathf.Clamp(index + 1, 0, roomsList.Count - 1), true, selectedRoomType);
            }
                    
            Instantiate(GetPrefabByRoomType(selectedRoomType), 
                        GetPositionByRoomType(roomTransform.position, selectedRoomType), Quaternion.identity, roomTransform);
        }

        private int SearchRoomByPosition(Vector3 pos)
        {
            for (int index = 0; index < roomsList.Count; index++)
            {
                Room room = roomsList[index];
                
                if (room.gameObject.transform.position == pos)
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
            Destroy(room.transform.GetChild(1).gameObject);
        }
        
        #endregion
    }
}
