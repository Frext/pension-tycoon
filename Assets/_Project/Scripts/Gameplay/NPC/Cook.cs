using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Gameplay.NPC
{
    public class Cook : NPC
    {
        [Space]
        [Header(nameof(Cook) + " Properties")]
        [Space]
        [SerializeField] private UnityEvent onCookFood;

        protected override void OnEnable()
        {
            base.OnEnable();

            StartCoroutine(SearchForTargetRoomsForever(null, .6f));
        }
        
        protected override void AddWayPoints()
        {
            wayPointsList.Clear();

            var initialPosition = transform.position;
            
            wayPointsList.Add(new WayPoint{ position = initialPosition,
                OnReachDestination = InsertSelectedRoomToWayPoints});
            wayPointsList.Add(new WayPoint{ position = initialPosition });
        }

        protected override void LeaveSelectedRoom()
        {
            base.LeaveSelectedRoom();
            
            floorManagerScript.MakeRoomUsable(selectedRoom);
            CookFood();
            
            isNpcMoving = false;
        }
        
        private void CookFood()
        {
            onCookFood.Invoke();
        }
    }
}
