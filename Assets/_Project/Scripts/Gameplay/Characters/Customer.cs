using UnityEngine;

namespace _Project.Scripts.Gameplay.Characters
{
    public class Customer : NPC
    {
        [Space]
        [Header(nameof(Customer) + " Properties")]
        [SerializeField] private Vector3 receptionPosition;
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
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
            
            wayPointsList.Add(new WayPoint
            {
                position = receptionPosition,
                waitTime = 2f,
                OnReachDestination = AddRoomToWayPoint
            });
            wayPointsList.Add(GetCharacterDisappearWayPoint());
        }
    }
}
