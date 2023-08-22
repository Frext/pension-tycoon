using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Gameplay.Building;
using UnityEngine;
using UnityEngine.Events;
using static _Project.Scripts.Gameplay.Building.Room;
using static _Project.Scripts.ScriptableObjects.TimeRangeObject.TimeRangeObject;

namespace _Project.Scripts.Gameplay.NPC
{
    public class Employee : Npc
    {
        [Space]
        [Header(nameof(Employee) + " Properties")]
        [SerializeField] private List<RoomTypeEnum> extraRoomTypes;
        [Space]
        [SerializeField] private UnityEvent onMakeRoomUsable;
        [Space]
        [Tooltip("The amount of time to make the employee available")]
        [SerializeField] private FloatRange availabilityRange;
        
        public bool IsAvailable()
        {
            return !isNpcMoving;
        }

        public void AssignToRoom(Room room)
        {
            selectedRoom = room;
            
            AddWayPoints();
            Move();
        }
        
        protected override void AddWayPoints()
        {
            wayPointsList.Clear();

            InsertSelectedRoomToWayPoints();
            wayPointsList.Add(new WayPoint{ position = GetRandomStartPoint() });
        }
        
        protected override void LeaveSelectedRoom()
        {
            base.LeaveSelectedRoom();
            
            floorManagerScript.MakeRoomUsable(selectedRoom);
            InvokeOnMakeRoomUsable();

            StartCoroutine(MakeEmployeeAvailable());
        }

        private IEnumerator MakeEmployeeAvailable()
        {
            yield return new WaitForSeconds(availabilityRange.Randomize());
            
            isNpcMoving = false;
        }

        private void InvokeOnMakeRoomUsable()
        {
            onMakeRoomUsable.Invoke();
        }
    }
}
