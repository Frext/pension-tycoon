using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Gameplay.Characters
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

            WayPoint startWayPoint = new WayPoint
            {
                position = startPosition,
                OnReachDestination = AddRoomToWayPoint
            };
            
            wayPointsList.Add(startWayPoint);
            wayPointsList.Add(startWayPoint);
        }
        
        
        public void CleanBathroom()
        {
            onCleanRoom.Invoke();
        }
    }
}
