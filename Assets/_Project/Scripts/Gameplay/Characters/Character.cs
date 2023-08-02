using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Gameplay.Building;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Gameplay.Characters
{
    public class Character : MonoBehaviour
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
        
        protected List<WayPoint> wayPointsList = new();
        private int currentWayPointIndex;

        [SerializeField] Room.RoomTypeEnum targetRoomType;
        
        [Space]
        [SerializeField] protected FloorManager floorManagerScript;
        
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
                while (Vector3.Distance(transform.position, currentWayPoint.position) > 0.01f)
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

        #region Customer Room Methods
        
        protected void AddCustomerRoomWayPoint()
        {
            WayPoint customerRoomWayPoint = GetCustomerRoomWayPoint(targetRoomType);

            if (customerRoomWayPoint != null)
            {
                wayPointsList.Insert(currentWayPointIndex + 1, customerRoomWayPoint);
            }
        }

        private WayPoint GetCustomerRoomWayPoint(Room.RoomTypeEnum customerRoomType)
        {
            if (customerRoomType != Room.RoomTypeEnum.CustomerSingle &&
                customerRoomType != Room.RoomTypeEnum.CustomerDouble)
            {
                throw new Exception("Invalid customer room type.");
            }
            
            Room room = floorManagerScript.GetTypeOfRoom(customerRoomType);

            if (room != null)
            {
                room.slot.isOccupied = true;
                
                return new WayPoint {
                    position = room.slot.roomObject.transform.position,
                    waitTime = Random.Range(4, 5)
                };
            }
            
            return null;
        }
        
        protected WayPoint GetCustomerDisappearWayPoint()
        {
            int randomVal = Random.Range(0, 2);

            return randomVal == 0 ? new WayPoint { position = new Vector3(-4.5f, .25f, -1.75f), waitTime = 0f }
                : new WayPoint { position = new Vector3(4.5f, .25f, -1.75f), waitTime = 0f };
        }
        
        #endregion
    }
}
