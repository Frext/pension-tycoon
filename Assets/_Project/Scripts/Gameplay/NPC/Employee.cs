using System.Collections;
using _Project.Scripts.Gameplay.Building;
using _Project.Scripts.ScriptableObjects.FloatRange;
using _Project.Scripts.ScriptableObjects.Int;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Gameplay.NPC
{
    public class Employee : Npc
    {
        [Space] 
        [Header(nameof(Employee) + " Properties")] 
        [Space] 
        
        [SerializeField] private IntSo floorCountSo;
        [Space]
        
        [Tooltip("The amount of time to make the employee available")]
        [SerializeField] private FloatRangeSo availabilityRangeSo;

        [Space] 
        [SerializeField] private UnityEvent onEmployeeAvailabilityChange;


        protected override void OnEnable()
        {
            navMeshAgent.Warp(GetRandomStartPoint(RandomStartPointOverrideTypesEnum.IncrementYPosition,
                Random.Range(0, floorCountSo.Value)));
        }

        public bool IsAvailable()
        {
            return !isNpcMoving;
        }

        public void AssignToRoom(Room room)
        {
            // First, set the selected room.
            selectedRoom = room;
            
            // Don't enter the room because it reactivates the make room usable button if it was disabled before.
            // Also the make room usable button doesn't look if the room is occupied, we don't have to set room as occupied.
            
            // Then add the waypoints and start moving.
            AddWayPoints();
            Move();
            
            // Also update the employee panel UI about how many employees are available.
            onEmployeeAvailabilityChange.Invoke();
        }

        protected override void AddWayPoints()
        {
            wayPointsList.Clear();

            InsertSelectedRoomToWayPoints();
            wayPointsList.Add(new WayPoint{ position = GetRandomStartPoint(RandomStartPointOverrideTypesEnum.RoomYPosition, selectedRoom.transform.position.y) });
        }
        
        protected override void LeaveSelectedRoom()
        {
            base.LeaveSelectedRoom();
            
            floorManagerScript.MakeRoomUsable(selectedRoom);

            StartCoroutine(MakeEmployeeAvailable(availabilityRangeSo.Randomize()));
        }

        private IEnumerator MakeEmployeeAvailable(float seconds = 0f)
        {
            yield return new WaitForSeconds(seconds);
            
            isNpcMoving = false;
            
            onEmployeeAvailabilityChange.Invoke();
        }
        
        public void StopTask()
        {
            if (isNpcMoving)
            {
                // Go to the last waypoint immediately.
                Move(wayPointsList.Count - 1);

                // This is for the employee panel UI to count these employees as available when they are moving towards their last waypoint.
                StartCoroutine(MakeEmployeeAvailable());
            }
        }
    }
}
