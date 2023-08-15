using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Gameplay.Building;
using _Project.Scripts.ScriptableObjects.TimeRangeObject;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Gameplay.NPC
{
    public abstract class Npc : MonoBehaviour
    {
        [Serializable]
        public class WayPoint
        {
            public Vector3 position;
            public float waitTime;
            public Action OnStartMoving;
            public Action OnReachDestination;
            public Action OnLeaveDestination;

            public void InvokeAction(Action action)
            {
                if (action != null)
                {
                    action.Invoke();
                }
            }
        }
        
        [SerializeField] protected Room.RoomTypeEnum baseTargetRoomType;
        
        [SerializeField] protected List<Vector3> startPositionsList;
        [Space]
        
        [SerializeField] protected TimeRangeObject timeRangeObjectSo;
        [Space]
        
        [SerializeField] protected FloorManager floorManagerScript;
        
        protected readonly List<WayPoint> wayPointsList = new();
        protected int currentWayPointIndex;
        protected bool isNpcMoving;

        protected Room selectedRoom;
        
        private readonly Vector3 characterOffset = new(0,-0.25f,0);
        
        
        protected virtual void OnEnable()
        {
            transform.position = GetRandomStartPoint();
        }

        protected Vector3 GetRandomStartPoint()
        {
            return startPositionsList[Random.Range(0, startPositionsList.Count)];
        }
        
        protected virtual void OnDisable()
        {
            StopAllCoroutines();
        }

        protected IEnumerator SearchForTargetRoomsForever(List<Room.RoomTypeEnum> extraTargetRoomTypes = null, float searchInterval = .4f, bool isOccupied = false, bool isUsable = false)
        {
            extraTargetRoomTypes ??= new List<Room.RoomTypeEnum>();
            extraTargetRoomTypes.Add(baseTargetRoomType);
            
            while (true)
            {
                if (!isNpcMoving)
                {
                    foreach (var roomType in extraTargetRoomTypes)
                    {
                        Room foundRoom = floorManagerScript.GetRoom(roomType, isOccupied, isUsable);

                        if (foundRoom != null)
                        {
                            selectedRoom = foundRoom;

                            // We occupy the room before we get here because 2 npc could go to the same room otherwise.
                            EnterSelectedRoom();

                            AddWayPoints();
                            Move();
                            
                            break;
                        }
                    }
                }
                
                yield return new WaitForSeconds(searchInterval);
            }
        }
        
        protected virtual void EnterSelectedRoom()
        {
            floorManagerScript.EnterRoom(selectedRoom);
        }
        
        protected abstract void AddWayPoints();

        protected void Move()
        {
            currentWayPointIndex = 0;
            
            StopCoroutine(nameof(IMove));
            StartCoroutine(nameof(IMove));
        }
        
        private IEnumerator IMove()
        {
            isNpcMoving = true;
                
            while (currentWayPointIndex < wayPointsList.Count)
            {
                WayPoint currentWayPoint = wayPointsList[currentWayPointIndex];
                
                currentWayPoint.InvokeAction(currentWayPoint.OnStartMoving);
                while (Vector3.Distance(transform.position, currentWayPoint.position) > 0.001f)
                {
                    transform.position = Vector3.Lerp(transform.position, 
                        wayPointsList[currentWayPointIndex].position, Time.deltaTime * 2);
                        
                    yield return null;
                }
                
                currentWayPoint.InvokeAction(currentWayPoint.OnReachDestination);
                yield return new WaitForSeconds(currentWayPoint.waitTime);
                currentWayPoint.InvokeAction(currentWayPoint.OnLeaveDestination);

                currentWayPointIndex++;
            }

            isNpcMoving = false;
        }
        
        protected void AssignSelectedRoomToBaseTargetRoom()
        {
            selectedRoom = floorManagerScript.GetRoom(baseTargetRoomType);

            if (selectedRoom != null)
            {
                EnterSelectedRoom();
            }
        }

        protected virtual void InsertSelectedRoomToWayPoints()
        {
            WayPoint roomWayPoint = GetSelectedRoomWayPoint(LeaveSelectedRoom);

            // We make the null comparison for the customer scripts if they don't get a room.
            if (roomWayPoint != null)
            {
                wayPointsList.Insert(currentWayPointIndex + 1, roomWayPoint);
            }
        }

        private WayPoint GetSelectedRoomWayPoint(Action OnLeaveDestination)
        {
            return selectedRoom == null ? null : CreateWayPoint(selectedRoom.slot.roomObject.transform.position, OnLeaveDestination: OnLeaveDestination);
        }

        protected WayPoint CreateWayPoint(Vector3 roomPosition, Action OnLeaveDestination = null)
        {
            return new WayPoint {
                position = roomPosition + characterOffset,
                waitTime = timeRangeObjectSo.GetRandomTime(),
                OnLeaveDestination = OnLeaveDestination
            };
        }
        
        protected virtual void LeaveSelectedRoom()
        {
            floorManagerScript.LeaveRoom(selectedRoom);
        }
    }
}
