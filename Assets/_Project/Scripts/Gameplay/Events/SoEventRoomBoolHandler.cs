using _Project.Scripts.Gameplay.Building;
using _Project.Scripts.ScriptableObjects.SoEventRoomBool;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Gameplay.Events
{
    public class SoEventRoomBoolHandler : MonoBehaviour
    {
        [SerializeField] private SoEventRoomBool soEventRoomBool;
        [Space]
        [SerializeField] private UnityEvent onEmployeeNotFound;

        public void Invoke(Room room)
        {
            if (!soEventRoomBool.Invoke(room))
            {
                onEmployeeNotFound.Invoke();
            }
        }
    }
}
