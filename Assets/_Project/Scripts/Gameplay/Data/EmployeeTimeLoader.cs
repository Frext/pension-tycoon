using _Project.Scripts.ScriptableObjects.EmployeeObject;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Data
{
    public class EmployeeTimeLoader : MonoBehaviour
    {
        [SerializeField] private EmployeeObject employeeObjectSo;

        void Start()
        {
            employeeObjectSo.UpdateTimeRange();
        }
    }
}
