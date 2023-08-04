using _Project.Scripts.Gameplay.Building;
using UnityEngine;

namespace _Project.Scripts.Gameplay.NPC
{
    public class Customer : NPC
    {
        [Space]
        [Header(nameof(Customer) + " Properties")]
        [SerializeField] private Vector3 receptionPosition;
        [Space]

        [Space]
        [SerializeField] private Room.RoomTypeEnum diningRoomType;
        [SerializeField] private Room.RoomTypeEnum bathroomType;

        protected override void OnEnable()
        {
            base.OnEnable();
            
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
                OnReachDestination = InsertTargetRoomToWayPoint
            });
            wayPointsList.Add(new WayPoint{ position = GetRandomStartPoint() });
        }

        protected override void InsertTargetRoomToWayPoint()
        {
            SearchForTargetRoom();
            
            base.InsertTargetRoomToWayPoint();

            AddExtraWayPoints();
        }

        private void AddExtraWayPoints()
        {
            var noOfRoomsIncludingCustomerRoom = 3;
            
            // If the customer didn't get a room.
            if (wayPointsList.Count < noOfRoomsIncludingCustomerRoom)
            {
                return;
            }
            
            float randomVal = Random.value;
            
            if (randomVal > .6f)
            {
                AddExtraSingleWayPoint(diningRoomType);
                
            }
            else if (randomVal > .3f)
            {
                AddExtraSingleWayPoint(bathroomType);
            }
        }

        private void AddExtraSingleWayPoint(Room.RoomTypeEnum roomType)
        {
            Room extraRoom = floorManagerScript.GetRoom(roomType);

            if (extraRoom != null)
            {
                wayPointsList.Insert(currentWayPointIndex + 2, 
                    CreateWayPoint(extraRoom.slot.roomObject.transform.position));
            }
        }

        protected override void LeaveTargetRoom()
        {
            base.LeaveTargetRoom();
            
            floorManagerScript.MakeRoomNotUsable(selectedRoom);
        }
    }
}
