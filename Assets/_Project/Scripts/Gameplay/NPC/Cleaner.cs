using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Gameplay.NPC
{
    public class Cleaner : NPC
    {
        [Space] 
        [SerializeField] private UnityEvent onCleanRoom;
        
        protected override void OnEnable()
        {
            base.OnEnable();

            StartCoroutine(SearchForTargetRoom(.4f));
        }
        
        protected override void AddWayPoints()
        {
            wayPointsList.Clear();

            var initialPosition = transform.position;
            
            wayPointsList.Add(new WayPoint{ position = initialPosition,
                OnReachDestination = InsertTargetRoomToWayPoint});
            wayPointsList.Add(new WayPoint{ position = initialPosition });
        }
        
        private void OnDisable()
        {
            StopAllCoroutines();
        }

        protected override void LeaveTargetRoom()
        {
            base.LeaveTargetRoom();
            
            floorManagerScript.MakeRoomUsable(selectedRoom);
            CleanRoom();
        }

        private void CleanRoom()
        {
            onCleanRoom.Invoke();
        }
    }
}
