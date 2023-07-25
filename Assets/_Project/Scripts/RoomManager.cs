using System.Collections.Generic;
using _Project.Scripts.ScriptableObjects.RoomType;
using UnityEngine;

namespace _Project.Scripts
{
    public class RoomManager : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject roomPrefab;
        [SerializeField] private GameObject wcPrefab;
        [SerializeField] private GameObject diningRoomPrefab;
        [Space]
        
        [SerializeField] private List<Room> roomsList;
        
        [SerializeField] private RoomType selectedRoomTypeSO;
        
        void Awake()
        {
            RegisterEvents();
        }
        
        private void RegisterEvents()
        {
            SlotEvents.OnShowRoomSlots += ShowRoomSlotButtons;
            SlotEvents.OnShowWcSlots += ShowWcSlotButtons;
            SlotEvents.OnShowDiningRoomSlots += ShowDiningRoomSlotButtons;
            SlotEvents.OnHideAll += HideAll;
        }
        
        void OnDestroy()
        {
            DeregisterEvents();
        }
        
        private void DeregisterEvents()
        {
            SlotEvents.OnShowRoomSlots -= ShowRoomSlotButtons;
            SlotEvents.OnShowWcSlots -= ShowWcSlotButtons;
            SlotEvents.OnShowDiningRoomSlots -= ShowDiningRoomSlotButtons;
            SlotEvents.OnHideAll -= HideAll;
        }

        private void ShowRoomSlotButtons()
        {
            foreach (Room room in roomsList)
            {
                if (!room.roomSlot.isOccupied)
                {
                    room.SetRoomSlotButton(true);
                }
            }
        }
        
        private void ShowWcSlotButtons()
        {
            foreach (Room room in roomsList)
            {
                if (!room.roomSlot.isOccupied)
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
                if (!roomsList[i].roomSlot.isOccupied && 
                    !roomsList[i + 1].roomSlot.isOccupied)
                {
                    roomsList[i].SetDiningRoomSlotButton(true);
                }
            }
        }

        private void HideAll()
        {
            foreach (Room room in roomsList)
            {
                room.SetRoomSlotButton(false);
                room.SetWcSlotButton(false);
                room.SetDiningRoomSlotButton(false);
            }
        }

        public void CreateRoom(Transform transform)
        {
            for (int index = 0; index < roomsList.Count; index++)
            {
                Room room = roomsList[index];
                
                if (room.gameObject.transform.position == transform.position)
                {
                    Room.RoomTypes selectedRoomType = selectedRoomTypeSO.SelectedRoomType;
                    
                    OccupyRoomByRoomType(selectedRoomType, index);
                    
                    Instantiate(GetRoomPrefab(selectedRoomType), 
                        GetPositionByRoomType(transform.position, selectedRoomType), Quaternion.identity, transform);
                    
                    break;
                }
            }
        }

        private void OccupyRoomByRoomType(Room.RoomTypes roomType, int index)
        {
            Room currentRoom = roomsList[index];
            
            currentRoom.roomSlot.isOccupied = true;
            currentRoom.roomSlot.roomType = roomType;
            
            if (roomType == Room.RoomTypes.DiningRoom)
            {
                Room nextRoom = roomsList[Mathf.Clamp(index + 1, 0, roomsList.Count - 1)];
                
                nextRoom.roomSlot.isOccupied = true;
                nextRoom.roomSlot.roomType = roomType;
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

        private GameObject GetRoomPrefab(Room.RoomTypes roomType)
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
    }
}
