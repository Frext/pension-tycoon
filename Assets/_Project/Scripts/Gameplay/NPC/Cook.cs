using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Gameplay.NPC
{
    public class Cook : NPC
    {
        [Space] 
        [SerializeField] private UnityEvent onCookFood;
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
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
            
            CookFood();
        }
        
        private void CookFood()
        {
            onCookFood.Invoke();
        }
    }
}
