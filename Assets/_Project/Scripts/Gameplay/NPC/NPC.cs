using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Gameplay.Building;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Gameplay.NPC
{
    public abstract class NPC : MonoBehaviour
    {
        [Serializable]
        public class FloatRange
        {
            public float min;
            public float max;

            public float Randomize()
            {
                return Random.Range(min, max);
            }
        }
        
        [Serializable]
        public class WayPoint
        {
            public Vector3 position;
            public float waitTime;
            public Action OnReachDestination;
            public Action OnLeaveDestination;

            public void InvokeOnReachDestination()
            {
                if (OnReachDestination != null)
                {
                    OnReachDestination.Invoke();
                }
            }

            public void InvokeOnLeaveDestination()
            {
                if (OnLeaveDestination != null)
                {
                    OnLeaveDestination.Invoke();
                }
            }
        }
        
        [SerializeField] protected List<Vector3> startPositionsList;
        [Space]
        
        [SerializeField] protected Room.RoomTypeEnum targetRoomType;
        [SerializeField] protected FloatRange stayRange;
        [Space]
        
        [SerializeField] protected FloorManager floorManagerScript;

        
        [SerializeField] protected List<WayPoint> wayPointsList = new();
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

        protected IEnumerator SearchForTargetRooms(List<Room.RoomTypeEnum> targetRoomTypes, float searchInterval = .4f, bool isOccupied = false, bool isUsable = false)
        {
            Room foundRoom = null;
            
            while (true)
            {
                if (!isNpcMoving)
                {
                    for (int index = 0; index < targetRoomTypes.Count; index++)
                    {
                        foundRoom = floorManagerScript.GetRoom(targetRoomTypes[index], isOccupied, isUsable);

                        if (foundRoom != null)
                            index = targetRoomTypes.Count;
                    }

                    if (foundRoom != null)
                    {
                        selectedRoom = foundRoom;

                        // We occupy the room before we get here because 2 npc can go to the same room otherwise.
                        EnterSelectedRoom();

                        AddWayPoints();
                        Move();
                    }
                }
                
                yield return new WaitForSeconds(searchInterval);
            }
        }

        protected void SearchForTargetRooms()
        {
            selectedRoom = floorManagerScript.GetRoom(targetRoomType);

            if (selectedRoom != null)
            {
                EnterSelectedRoom();
            }
        }
        
        protected abstract void AddWayPoints();

        protected void Move()
        {
            currentWayPointIndex = 0;
            StartCoroutine(IMove());
        }
        
        private IEnumerator IMove()
        {
            isNpcMoving = true;
                
            while (currentWayPointIndex < wayPointsList.Count)
            {
                WayPoint currentWayPoint = wayPointsList[currentWayPointIndex];
                while (Vector3.Distance(transform.position, currentWayPoint.position) > 0.001f)
                {
                    transform.position = Vector3.Lerp(transform.position, 
                        wayPointsList[currentWayPointIndex].position, Time.deltaTime * 2);
                        
                    yield return null;
                }
                
                currentWayPoint.InvokeOnReachDestination();
                yield return new WaitForSeconds(currentWayPoint.waitTime);
                currentWayPoint.InvokeOnLeaveDestination();

                currentWayPointIndex++;
            }

            isNpcMoving = false;
        }

        protected virtual void InsertSelectedRoomToWayPoint()
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
            if (selectedRoom == null)
            {
                return null;
            }

            return CreateWayPoint(selectedRoom.slot.roomObject.transform.position, OnLeaveDestination);
        }

        protected WayPoint CreateWayPoint(Vector3 roomPosition, Action OnLeaveDestination = null)
        {
            return new WayPoint {
                position = roomPosition + characterOffset,
                waitTime = stayRange.Randomize(),
                OnLeaveDestination = OnLeaveDestination
            };
        }

        private void EnterSelectedRoom()
        {
            floorManagerScript.EnterRoom(selectedRoom);
        }

        protected virtual void LeaveSelectedRoom()
        {
            floorManagerScript.LeaveRoom(selectedRoom);
        }
    }
}
