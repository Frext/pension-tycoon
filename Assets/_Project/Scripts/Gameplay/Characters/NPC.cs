using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Gameplay.Building;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Gameplay.Characters
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
    
    public abstract class NPC : MonoBehaviour
    {
        [Serializable]
        public class WayPoint
        {
            public Vector3 position;
            public float waitTime;
            public Action OnReachDestination;

            public void InvokeOnReachDestination()
            {
                if (OnReachDestination != null)
                {
                    OnReachDestination.Invoke();
                }
            }
        }
        
        [SerializeField] protected Vector3 startPosition;
        [Space]
        
        [SerializeField] private Room.RoomTypeEnum targetRoomType;
        [SerializeField] private FloatRange stayRange;
        [Space]
        
        [SerializeField] protected FloorManager floorManagerScript;

        
        protected List<WayPoint> wayPointsList = new();
        private int currentWayPointIndex;
        
        private readonly Vector3 characterOffset = new Vector3(0,-0.25f,0);
        
        
        protected void OnEnable()
        {
            transform.position = startPosition;
        }

        protected abstract void AddWayPoints();

        protected void Move()
        {
            currentWayPointIndex = 0;
            StartCoroutine(IMove());
        }
        
        private IEnumerator IMove()
        {
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

                currentWayPointIndex++;
            }
        }

        protected void AddRoomToWayPoint()
        {
            WayPoint roomWayPoint = GetRoomWayPoint();

            if (roomWayPoint != null)
            {
                wayPointsList.Insert(currentWayPointIndex + 1, roomWayPoint);
            }
        }

        private WayPoint GetRoomWayPoint()
        {
            Room room = floorManagerScript.GetTypeOfRoom(targetRoomType);

            if (room == null)
            {
                // Decrease the pension rating.
                return null;
            }

            room.slot.isOccupied = true;
            
            return new WayPoint {
                position = room.slot.roomObject.transform.position + characterOffset,
                waitTime = stayRange.Randomize()
            };
        }
        
        protected WayPoint GetCharacterDisappearWayPoint()
        {
            int randomVal = Random.Range(0, 2);

            return randomVal == 0 ? new WayPoint { position = new Vector3(-4.5f, .25f, -1.75f), waitTime = 0f }
                : new WayPoint { position = new Vector3(4.5f, .25f, -1.75f), waitTime = 0f };
        }
    }
}
