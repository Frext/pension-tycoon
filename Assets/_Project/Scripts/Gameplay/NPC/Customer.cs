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
        }
        
        protected override void LeaveSelectedRoom()
        {
            base.LeaveSelectedRoom();
            floorManagerScript.MakeRoomNotUsable(selectedRoom);
            
            AddExtraWayPointsRandomly();
        }

        private void AddExtraWayPointsRandomly()
        {
            float randomVal = Random.value;

            for (int divider = extraRoomTypes.Length; divider > 0; divider--)
            {
                if (randomVal >= 1 - extraRoomChance / divider)
                {
                    AddExtraSingleWayPoint(extraRoomTypes[divider - 1]);
                    break;
                }
            }
        }

        private void AddExtraSingleWayPoint(Room.RoomTypeEnum roomType)
        {
            extraRoom = floorManagerScript.GetRoom(roomType);

            if (extraRoom != null)
            {
                floorManagerScript.EnterRoom(extraRoom);
                
                wayPointsList.Insert(currentWayPointIndex + 1, 
                    CreateWayPoint(extraRoom.slot.roomObject.transform.position, LeaveExtraRoom));
            }
        }

        private void LeaveExtraRoom()
        {
            floorManagerScript.LeaveRoom(extraRoom);

            if (Random.value > 1 - makeExtraRoomNotUsableChance)
                floorManagerScript.MakeRoomNotUsable(extraRoom);
        }


    }
}
