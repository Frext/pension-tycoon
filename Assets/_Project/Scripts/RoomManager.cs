using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts
{
    public class RoomManager : MonoBehaviour
    {
        [SerializeField] private List<Room> roomsList;

        void Awake()
        {
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            ShowSlots.OnShow1 += ShowSlotsWidth1;
            ShowSlots.OnShow2 += ShowSlotsWidth2;
            ShowSlots.OnHideAll += HideAll;
        }

        void OnDestroy()
        {
            DeregisterEvents();
        }

        private void DeregisterEvents()
        {
            ShowSlots.OnShow1 -= ShowSlotsWidth2;
            ShowSlots.OnShow2 -= ShowSlotsWidth2;
            ShowSlots.OnHideAll -= HideAll;
        }

        void ShowSlotsWidth1()
        {
            foreach (Room room in roomsList)
            {
                if (!room.roomSlot.isOccupied)
                {
                    room.SetSlotWidth1(true);
                }
            }
        }

        void ShowSlotsWidth2()
        {
            for (int i = 0; i < roomsList.Count - 1; i++)
            {
                // If the slot width of 2 is active for the previous room, don't enable it.
                if (i > 0 && roomsList[i - 1].IsSlotWidth2Active())
                {
                    continue;
                }
                
                // If the current room and the next one is not occupied, show a slot width of 2.
                if (!roomsList[i].roomSlot.isOccupied && 
                    !roomsList[i + 1].roomSlot.isOccupied)
                {
                    roomsList[i].SetSlotWidth2(true);
                }
            }
        }

        void HideAll()
        {
            foreach (Room room in roomsList)
            {
                room.SetSlotWidth1(false);
                room.SetSlotWidth2(false);
            }
        }
    }
}
