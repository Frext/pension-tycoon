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
        
        [SerializeField] protected Vector3 startPosition;
        [Space]
        
        [SerializeField] protected Room.RoomTypeEnum targetRoomType;
        [SerializeField] protected FloatRange stayRange;
        [Space]
        
        [SerializeField] protected FloorManager floorManagerScript;

        
        protected readonly List<WayPoint> wayPointsList = new();
        protected int currentWayPointIndex;
        private bool isNpcMoving;

        protected Room selectedRoom;
        
        private readonly Vector3 characterOffset = new(0,-0.25f,0);
        
        
        protected virtual void OnEnable()
        {
            transform.position = startPosition;
        }
        
        protected IEnumerator SearchForTargetRoom(float searchInterval = .4f, bool isOccupied = false, bool isUsable = true)
        {
            Room foundRoom;
            
            while (true)
            {
                if (isNpcMoving)
                    yield return new WaitForSeconds(searchInterval);
                
                foundRoom = floorManagerScript.GetRoom(targetRoomType, false, false);

                if (foundRoom != null)
                {
                    selectedRoom = foundRoom;
                    
                    // We occupy the room before we get here because 2 npc can go to the same room otherwise.
                    EnterTargetRoom();
                    
                    AddWayPoints();
                    Move();
                }

                yield return new WaitForSeconds(searchInterval);
            }
        }

        protected void SearchForTargetRoom()
        {
            selectedRoom = floorManagerScript.GetRoom(targetRoomType);

            if (selectedRoom != null)
            {
                EnterTargetRoom();
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

        protected virtual void InsertTargetRoomToWayPoint()
        {
            WayPoint roomWayPoint = GetTargetRoomWayPoint(LeaveTargetRoom);

            // We make the null comparison for the customer scripts if they don't get a room.
            if (roomWayPoint != null)
            {
                wayPointsList.Insert(currentWayPointIndex + 1, roomWayPoint);
            }
        }

        private WayPoint GetTargetRoomWayPoint(Action OnLeaveDestination)
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

        private void EnterTargetRoom()
        {
            floorManagerScript.EnterRoom(selectedRoom);
        }

        protected virtual void LeaveTargetRoom()
        {
            floorManagerScript.LeaveRoom(selectedRoom);
        }
        
        protected WayPoint GetCharacterDisappearWayPoint()
        {
            int randomVal = Random.Range(0, 2);

            return randomVal == 0 ? new WayPoint { position = new Vector3(-4.5f, .25f, -1.75f), waitTime = 0f }
                : new WayPoint { position = new Vector3(4.5f, .25f, -1.75f), waitTime = 0f };
        }
    }
}
