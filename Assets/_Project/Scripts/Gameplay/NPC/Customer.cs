using _Project.Scripts.Gameplay.Building;
using _Project.Scripts.ScriptableObjects.Int;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Gameplay.NPC
{
    public class Customer : Npc
    {
        [Space]
        [Header(nameof(Customer) + " Properties")]
        [Space]
        [SerializeField] private Vector3 receptionPosition;
        [Space] 
        [SerializeField] private UnityEvent OnCustomerLeave;
        
        [Header("Rooms")]
        [SerializeField] private Room.RoomTypeEnum baseTargetRoomType;
        [Space]
        [SerializeField] private Room.RoomTypeEnum[] extraRoomTypes;
        [Range(0, 1)] 
        [SerializeField] private float extraRoomChance;
        [Range(0, 1)] 
        [SerializeField] private float makeExtraRoomNotUsableChance;

        [Header("Money")] 
        [SerializeField] private int payAmount;

        Room extraRoom;
        
        const int NoOfWayPointsIfSuccessful = 3;
        
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
                OnStartMoving = () =>
                {
                    OnCustomerLeave.Invoke();
                },
                OnReachDestination = () => { Destroy(gameObject); }
            });
        }

        protected override void InsertSelectedRoomToWayPoints()
        {
            GetOneTargetRoom();
            
            base.InsertSelectedRoomToWayPoints();
        }
        
        private void GetOneTargetRoom()
        {
            selectedRoom = floorManagerScript.GetRoom(baseTargetRoomType);

            if (selectedRoom != null)
            {
                EnterSelectedRoom();
            }
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
                    return;
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

        public void PayRoom(IntSo coinCountSo)
        {
            if (wayPointsList.Count < NoOfWayPointsIfSuccessful)
            {
                return;
            }
            
            coinCountSo.IncrementValue(payAmount);
        }
    }
}
