using _Project.Scripts.Gameplay.Building;
using UnityEngine;

namespace _Project.Scripts.Gameplay.NPC
{
    public class Customer : NPC
    {
        [Space]
        [Header(nameof(Customer) + " Properties")]
        [Space]
        [SerializeField] private Vector3 receptionPosition;
        
        [Header("Extra Rooms")]
        [Space]
        [SerializeField] private Room.RoomTypeEnum[] extraRoomTypes;
        [Range(0, 1)] 
        [SerializeField] private float extraRoomChance;
        [Range(0, 1)] 
        [SerializeField] private float makeExtraRoomNotUsableChance;

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
                OnReachDestination = InsertSelectedRoomToWayPoints
            });
            wayPointsList.Add(new WayPoint
            {
                position = GetRandomStartPoint(),
                OnReachDestination = () => { Destroy(gameObject);}
            });
        }

        protected override void InsertSelectedRoomToWayPoints()
        {
            AssignSelectedRoomToBaseTargetRoom();
            
            base.InsertSelectedRoomToWayPoints();

            AddExtraWayPointsRandomly();
        }

        private void AddExtraWayPointsRandomly()
        {
            const int noOfRoomsIncludingCustomerRoom = 3;
            
            // If the customer didn't get a room, don't add an extra room.
            if (wayPointsList.Count < noOfRoomsIncludingCustomerRoom)
                return;
            
            
            float randomVal = Random.value;

            for (int divider = extraRoomTypes.Length; divider > 0; divider--)
            {
                if (randomVal > 1 - extraRoomChance / divider)
                    AddExtraSingleWayPoint(extraRoomTypes[divider - 1]);
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

            if (Random.value > 1 - makeExtraRoomNotUsableChance)
                floorManagerScript.MakeRoomNotUsable(extraRoom);
        }

        protected override void LeaveSelectedRoom()
        {
            base.LeaveSelectedRoom();
            
            floorManagerScript.MakeRoomNotUsable(selectedRoom);
            
            if (extraRoom != null)
                floorManagerScript.EnterRoom(extraRoom);
        }
    }
}
