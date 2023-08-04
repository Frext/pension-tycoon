using System;
using System.Collections.Generic;
using _Project.Scripts.Gameplay.Building;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Gameplay.NPC
{
    public class Cleaner : NPC
    {
        [Space] 
        [SerializeField] private List<Room.RoomTypeEnum> extraTargets;
        
        [SerializeField] private UnityEvent onCleanRoom;

        void Awake()
        {
            extraTargets.Add(targetRoomType);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            StartCoroutine(SearchForTargetRooms(extraTargets, .6f));
        }
        
        protected override void AddWayPoints()
        {
            wayPointsList.Clear();
            
            wayPointsList.Add(new WayPoint{ position = transform.position,
                OnReachDestination = InsertSelectedRoomToWayPoint});
            wayPointsList.Add(new WayPoint{ position = GetRandomStartPoint() });
        }
        
        private void OnDisable()
        {
            StopAllCoroutines();
        }

        protected override void LeaveSelectedRoom()
        {
            base.LeaveSelectedRoom();
            
            floorManagerScript.MakeRoomUsable(selectedRoom);
            CleanRoom();

            isNpcMoving = false;
        }

        private void CleanRoom()
        {
            onCleanRoom.Invoke();
        }
    }
}
