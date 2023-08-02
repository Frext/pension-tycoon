using UnityEngine;

namespace _Project.Scripts.Gameplay.Characters
{
    public class Customer : Character
    {
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

        private void AddWayPoints()
        {
            wayPointsList.Clear();
            
            wayPointsList.Add(new WayPoint{position = new Vector3(.25f, .25f, -.75f), waitTime = 2f, 
                OnReachDestination = AddCustomerRoomWayPoint});
            wayPointsList.Add(GetCustomerDisappearWayPoint());
        }
    }
}
