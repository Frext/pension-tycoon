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
        
        protected override void OnEnable()
        {
            base.OnEnable();

            extraTargets.Add(targetRoomType);
            StartCoroutine(SearchForTargetRooms(extraTargets, .6f));
        }
        
        protected override void AddWayPoints()
        {
            wayPointsList.Clear();

            var initialPosition = transform.position;
            
            wayPointsList.Add(new WayPoint{ position = initialPosition,
                OnReachDestination = InsertSelectedRoomToWayPoint});
            wayPointsList.Add(new WayPoint{ position = initialPosition });
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
        }

        private void CleanRoom()
        {
            onCleanRoom.Invoke();
        }
    }
}
