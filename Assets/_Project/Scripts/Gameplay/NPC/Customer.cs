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
        [Range(0, 1)] 
        [SerializeField] private float extraRoomChance;

        [Space]
        [Range(0, 1)] 
        [SerializeField] private float makeRoomNotUsableChance;

        Room extraRoom;
        
        
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
                OnReachDestination = InsertSelectedRoomToWayPoint
            });
            wayPointsList.Add(new WayPoint{ position = GetRandomStartPoint(),
                OnReachDestination = () => { Destroy(gameObject); }});
        }

        protected override void InsertSelectedRoomToWayPoint()
        {
            SearchForTargetRooms();
            
            base.InsertSelectedRoomToWayPoint();

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
            
            if (randomVal > 1 - extraRoomChance / 2)
            {
                AddExtraSingleWayPoint(diningRoomType);
            }
            else if (randomVal > 1 - extraRoomChance)
            {
                AddExtraSingleWayPoint(bathroomType);
            }
        }

        private void AddExtraSingleWayPoint(Room.RoomTypeEnum roomType)
        {
            extraRoom = floorManagerScript.GetRoom(roomType);

            if (extraRoom != null)
            {
                wayPointsList.Insert(currentWayPointIndex + 2, 
                    CreateWayPoint(extraRoom.slot.roomObject.transform.position, LeaveExtraRoom));
            }
        }

        private void LeaveExtraRoom()
        {
            floorManagerScript.LeaveRoom(extraRoom);

            if (Random.value > 1 - makeRoomNotUsableChance)
                floorManagerScript.MakeRoomNotUsable(extraRoom);
        }

        protected override void LeaveSelectedRoom()
        {
            base.LeaveSelectedRoom();
            
            floorManagerScript.MakeRoomNotUsable(selectedRoom);
            
            if (extraRoom != null)
            {
                floorManagerScript.EnterRoom(extraRoom);
            }
        }
    }
}
