using _Project.Scripts.Gameplay.Building;
using _Project.Scripts.ScriptableObjects.SoEventRoomBool;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Gameplay.Events
{
    public class SoEventRoomBoolManager : MonoBehaviour
    {
        [SerializeField] private SoEventRoomBool soEventRoomBool;
        [Space]
        [SerializeField] private UnityEvent OnEmployeeNotFound;

        public void Invoke(Room room)
        {
            if (!soEventRoomBool.Invoke(room))
            {
                OnEmployeeNotFound.Invoke();
            }
        }
    }
}
