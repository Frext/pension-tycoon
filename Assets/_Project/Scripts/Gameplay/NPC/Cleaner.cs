using System.Collections.Generic;
using _Project.Scripts.Gameplay.Building;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Gameplay.NPC
{
    public class Cleaner : Npc
    {
        [Space]
        [Header(nameof(Cleaner) + " Properties")]
        [Space]
        [SerializeField] private List<Room.RoomTypeEnum> extraTargetRoomTypes;
        [Space]
        [SerializeField] private UnityEvent onCleanRoom;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            StartCoroutine(SearchForTargetRoomsForever(extraTargetRoomTypes, .6f));
        }
        
        protected override void AddWayPoints()
        {
            wayPointsList.Clear();
            
            wayPointsList.Add(new WayPoint{ position = transform.position,
                OnReachDestination = InsertSelectedRoomToWayPoints});
            wayPointsList.Add(new WayPoint{ position = GetRandomStartPoint() });
        }
        
        protected override void LeaveSelectedRoom()
        {
            base.LeaveSelectedRoom();
            
            floorManagerScript.MakeRoomUsable(selectedRoom);
            CleanRoom();

            isNpcMoving = false;
        }

        private void CleanRoom()
        {
            onCleanRoom.Invoke();
        }
    }
}
