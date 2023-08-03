using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Gameplay.NPC
{
    public class Cleaner : NPC
    {
        [Space] 
        [SerializeField] private UnityEvent onCleanRoom;
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                StartMoving();
            }
        }
        
        private void StartMoving()
        {
            AddWayPoints();
            Move();
        }

        protected override void AddWayPoints()
        {
            wayPointsList.Clear();
            
            wayPointsList.Add(new WayPoint{ position = startPosition,
                OnReachDestination = InsertTargetRoomToWayPoint});
            wayPointsList.Add(new WayPoint{ position = startPosition });
        }

        protected override void LeaveTargetRoom()
        {
            base.LeaveTargetRoom();
            
            CleanBathroom();
        }

        private void CleanBathroom()
        {
            onCleanRoom.Invoke();
        }
    }
}
