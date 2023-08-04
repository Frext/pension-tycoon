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

            StartCoroutine(SearchForTargetRoom(.4f, false, false));
        }
        
        protected override void AddWayPoints()
        {
            wayPointsList.Clear();
            
            wayPointsList.Add(new WayPoint{ position = startPosition,
                OnReachDestination = InsertTargetRoomToWayPoint});
            wayPointsList.Add(new WayPoint{ position = startPosition });
        }
        
        private void OnDisable()
        {
            StopAllCoroutines();
        }

        protected override void LeaveTargetRoom()
        {
            base.LeaveTargetRoom();
            
            floorManagerScript.MakeRoomUsable(selectedRoom);
            CleanBathroom();
        }

        private void CleanBathroom()
        {
            onCleanRoom.Invoke();
        }
    }
}
