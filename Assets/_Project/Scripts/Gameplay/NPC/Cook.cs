using System.Collections.Generic;
using _Project.Scripts.Gameplay.Building;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Gameplay.NPC
{
    public class Cook : NPC
    {
        [Space] 
        [SerializeField] private UnityEvent onCookFood;

        protected override void OnEnable()
        {
            base.OnEnable();

            StartCoroutine(SearchForTargetRooms(new List<Room.RoomTypeEnum>{ targetRoomType }));
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
            CookFood();
        }
        
        private void CookFood()
        {
            onCookFood.Invoke();
        }
    }
}
