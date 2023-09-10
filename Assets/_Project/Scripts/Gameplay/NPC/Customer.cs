using System;
using _Project.Scripts.Gameplay.Building;
using _Project.Scripts.ScriptableObjects.Int;
using _Project.Scripts.ScriptableObjects.SoEventGameObject;
using _Project.Scripts.ScriptableObjects.SoEventRoomString;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Gameplay.NPC
{
    public class Customer : Npc
    {
        [Serializable]
        public class ExtraTargetRoom
        {
            public Room.RoomTypeEnum roomType;
            public UnityEvent onReachExtraRoom;
            public UnityEvent onLeaveExtraRoom;
        }
        
        [Space]
        [Header(nameof(Customer) + " Properties")]
        [Space]
        [SerializeField] private Vector3 receptionPosition;
        [Space]
        
        
        [Header("Rooms")]
        [SerializeField] private Room.RoomTypeEnum baseTargetRoomType;
        [Space]
        [SerializeField] private ExtraTargetRoom[] extraRoomTypes;
        [Range(0, 1)] 
        [SerializeField] private float extraRoomChance;
        [Range(0, 1)] 
        [SerializeField] private float makeExtraRoomNotUsableChance;

        [Header("Money")] 
        [SerializeField] private int payAmount;

        [Header("Events")]
        [SerializeField] private SoEventString onNoAvailableRoom;
        [SerializeField] private SoEventGameObject onOpenRoomDoor;
        [Space]
        
        [SerializeField] private UnityEvent onGetBaseRoom;
        [SerializeField] private UnityEvent onNotGetBaseRoom;
        [SerializeField] private UnityEvent onNotGetExtraRoom;
        [SerializeField] private UnityEvent onCustomerLeave;
        
            
        Room extraRoom;
        
        const int NoOfWayPointsIfSuccessful = 3;

        private bool didLeaveRoom;
        
        
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
                    onCustomerLeave.Invoke();
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
                onGetBaseRoom.Invoke();
                
                EnterSelectedRoom();
            }
            else
            {
                FindNoRoom(baseTargetRoomType, onNotGetBaseRoom);
            }
        }

        private void FindNoRoom(Room.RoomTypeEnum roomType, UnityEvent unityEvent)
        {
            onNoAvailableRoom.Invoke(Room.GetLegibleRoomName(roomType));
            
            unityEvent.Invoke();
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

        private void AddExtraSingleWayPoint(ExtraTargetRoom extraTargetRoom)
        {
            extraRoom = floorManagerScript.GetRoom(extraTargetRoom.roomType);

            if (extraRoom != null)
            {
                floorManagerScript.EnterRoom(extraRoom);
                
                wayPointsList.Insert(currentWayPointIndex + 1, 
                    CreateWayPoint(extraRoom.slot.roomObject.transform.position, 
                        () => extraTargetRoom.onReachExtraRoom.Invoke(),
                        () =>
                        {
                            extraTargetRoom.onLeaveExtraRoom.Invoke();
                            LeaveExtraRoom();
                        }
                        ));
            }
            else
            {
                FindNoRoom(extraTargetRoom.roomType, onNotGetExtraRoom);
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

        public void OpenRoomDoor()
        {
            // We need to identify the room we'll open the door of.
            // If the waypoints are less than or equal to the number, that means we didn't go to an extra room.

            // We also open the door again when we are leaving the room.
            if (!didLeaveRoom && wayPointsList.Count > NoOfWayPointsIfSuccessful)
            {
                onOpenRoomDoor.Invoke(selectedRoom.slot.roomObject);
                
                didLeaveRoom = true;
            }
            else if (wayPointsList.Count <= NoOfWayPointsIfSuccessful)
            {
                onOpenRoomDoor.Invoke(selectedRoom.slot.roomObject);
            }
            else
            {
                onOpenRoomDoor.Invoke(extraRoom.slot.roomObject);
            }
        }
    }
}
