using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Gameplay.Building;
using _Project.Scripts.ScriptableObjects.FloatRange;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
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

        [SerializeField] protected NavMeshAgent navMeshAgent;
        
        [SerializeField] protected List<Vector3> startPositionsList;
        [Space]
        
        [SerializeField] protected FloatRangeSo timeRangeObjectSo;
        
        [Header("NavMesh Events")]
        [SerializeField] protected int navMeshSelectedAreaId;
        [SerializeField] protected UnityEvent onCrossSelectedOffMeshLink;
        [Space]
        
        [SerializeField] protected FloorManager floorManagerScript;
        
        
        protected readonly List<WayPoint> wayPointsList = new();
        protected int currentWayPointIndex;
        protected bool isNpcMoving;

        protected Room selectedRoom;
        
        private readonly Vector3 characterOffset = new(0,-0.25f,0);
        private readonly float humanMovementStimulus = 0.1f;
        
        protected enum RandomStartPointOverrideTypesEnum
        {
            None,
            RoomYPosition,
            IncrementYPosition
        }
        
        
        protected virtual void OnEnable()
        {
            navMeshAgent.Warp(GetRandomStartPoint());
        }

        protected Vector3 GetRandomStartPoint(RandomStartPointOverrideTypesEnum randomStartPointOverrideTypesEnum = RandomStartPointOverrideTypesEnum.None, float yPos = 0)
        {
            Vector3 randomStartPoint = startPositionsList[Random.Range(0, startPositionsList.Count)];

            if (randomStartPointOverrideTypesEnum == RandomStartPointOverrideTypesEnum.RoomYPosition)
            {
                randomStartPoint.y = yPos + characterOffset.y;
            }
            else if (randomStartPointOverrideTypesEnum == RandomStartPointOverrideTypesEnum.IncrementYPosition)
            {
                randomStartPoint.y += yPos;
            }

            return randomStartPoint;
        }
        
        protected virtual void OnDisable()
        {
            StopAllCoroutines();
        }
        
        protected void EnterSelectedRoom()
        {
            floorManagerScript.EnterRoom(selectedRoom);
        }
        
        protected abstract void AddWayPoints();

        protected void Move(int index = 0)
        {
            StopCoroutine(nameof(IMove));
            
            currentWayPointIndex = index;
            
            StartCoroutine(nameof(IMove));
        }
        
        private IEnumerator IMove()
        {
            isNpcMoving = true;
                
            while (currentWayPointIndex < wayPointsList.Count)
            {
                WayPoint currentWayPoint = wayPointsList[currentWayPointIndex];
                currentWayPoint.InvokeAction(currentWayPoint.OnStartMoving);
                
                bool didUseOffMeshLink = false;
                navMeshAgent.SetDestination(currentWayPoint.position);
                
                // Wait while the agent gets to the destination.
                while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
                {
                    if (!didUseOffMeshLink && navMeshAgent.isOnOffMeshLink)
                    {
                        OffMeshLinkData data = navMeshAgent.currentOffMeshLinkData;

                        if (navMeshSelectedAreaId == data.offMeshLink.area)
                        {
                            onCrossSelectedOffMeshLink.Invoke();
                            
                            didUseOffMeshLink = true;
                        }
                    }
                    
                    yield return new WaitForSeconds(humanMovementStimulus);
                }
                
                currentWayPoint.InvokeAction(currentWayPoint.OnReachDestination);
                
                yield return new WaitForSeconds(currentWayPoint.waitTime);
                
                currentWayPoint.InvokeAction(currentWayPoint.OnLeaveDestination);

                
                currentWayPointIndex++;
            }

            isNpcMoving = false;
        }

        protected virtual void InsertSelectedRoomToWayPoints()
        {
            WayPoint roomWayPoint = GetSelectedRoomWayPoint(LeaveSelectedRoom);

            // We make the null comparison for the customer scripts if they don't get a room.
            if (roomWayPoint != null)
            {
                // Insert the new waypoint in between the start and end or in the first place.
                wayPointsList.Insert(Mathf.Clamp(wayPointsList.Count - 1, 0 , int.MaxValue), roomWayPoint);
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
                waitTime = timeRangeObjectSo.Randomize(),
                OnLeaveDestination = OnLeaveDestination
            };
        }
        
        protected virtual void LeaveSelectedRoom()
        {
            floorManagerScript.LeaveRoom(selectedRoom);
        }
    }
}
