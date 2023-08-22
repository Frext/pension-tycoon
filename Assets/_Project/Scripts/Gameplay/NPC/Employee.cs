using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Gameplay.Building;
using _Project.Scripts.ScriptableObjects.TimeRangeObject;
using UnityEngine;
using UnityEngine.Events;
using static _Project.Scripts.Gameplay.Building.Room;

namespace _Project.Scripts.Gameplay.NPC
{
    public class Employee : Npc
    {
        [Space]
        [Header(nameof(Employee) + " Properties")]
        [Space]
        [Tooltip("The amount of time to make the employee available")]
        [SerializeField] private FloatRangeObject availabilityRangeSo;
        [Space]
        [SerializeField] private List<RoomTypeEnum> extraRoomTypes;
        [Space]
        [SerializeField] private UnityEvent onMakeRoomUsable;
        
        
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
            yield return new WaitForSeconds(availabilityRangeSo.Randomize());
            
            isNpcMoving = false;
        }

        private void InvokeOnMakeRoomUsable()
        {
            onMakeRoomUsable.Invoke();
        }
    }
}
