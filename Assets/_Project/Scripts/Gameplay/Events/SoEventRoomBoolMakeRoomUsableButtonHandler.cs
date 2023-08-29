using _Project.Scripts.Gameplay.Building;
using _Project.Scripts.ScriptableObjects.SoEventRoomBool;
using _Project.Scripts.ScriptableObjects.SoEventRoomString;
using UnityEngine;
using UnityEngine.Events;
using static _Project.Scripts.Gameplay.Building.Room;

namespace _Project.Scripts.Gameplay.Events
{
    public class SoEventRoomBoolMakeRoomUsableButtonHandler : MonoBehaviour
    {
        [SerializeField] private SoEventRoomBool soEventRoomBool;
        [SerializeField] private SoEventString onNoAvailableRoomOrEmployee;
        [Space]
        [SerializeField] private UnityEvent onEmployeeNotFound;

        public void Invoke(Room room)
        {
            if (!soEventRoomBool.Invoke(room))
            {
                onEmployeeNotFound.Invoke();
                
                onNoAvailableRoomOrEmployee.Invoke( GetLegibleEmployeeName(GetEmployeeTypeForRoom(room.slot.roomType)));
            }
        }
    }
}
