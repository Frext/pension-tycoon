using System.Collections.Generic;
using _Project.Scripts.Gameplay.Building;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Gameplay.NPC
{
    public class Employee : Npc
    {
        [Space]
        [Header(nameof(Employee) + " Properties")]
        [Space]
        [SerializeField] private List<Room.RoomTypeEnum> extraTargetRoomTypes;
        [Space]
        [SerializeField] private float searchInterval = .6f;
        [Space]
        [SerializeField] private UnityEvent onMakeRoomUsable;

        
        protected override void OnEnable()
        {
            base.OnEnable();

            StartCoroutine(extraTargetRoomTypes.Count > 0
                ? SearchForTargetRoomsForever(extraTargetRoomTypes, searchInterval)
                : SearchForTargetRoomsForever(null, searchInterval));
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
            InvokeOnMakeRoomUsable();

            isNpcMoving = false;
        }

        private void InvokeOnMakeRoomUsable()
        {
            onMakeRoomUsable.Invoke();
        }
    }
}
