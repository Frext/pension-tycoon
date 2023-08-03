using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Gameplay.Characters
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

            WayPoint startWayPoint = new WayPoint
            {
                position = startPosition,
                OnReachDestination = AddRoomToWayPoint
            };
            
            wayPointsList.Add(startWayPoint);
            wayPointsList.Add(startWayPoint);
        }
        
        public void CookFood()
        {
            onCookFood.Invoke();
        }
    }
}
