using System.Collections.Generic;
using _Project.Scripts.ScriptableObjects.EmployeeObject;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Data
{
    public class EmployeeTimeLoader : MonoBehaviour
    {
        [SerializeField] private List<EmployeeObject> employeeObjectSoList;

        void Start()
        {
            foreach (var employeeObjectSo in employeeObjectSoList)
            {
                employeeObjectSo.UpdateTimeRange();
            }
        }
    }
}
