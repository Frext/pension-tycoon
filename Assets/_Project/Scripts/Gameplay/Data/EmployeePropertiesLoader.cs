using System.Collections.Generic;
using _Project.Scripts.ScriptableObjects.Employee;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Data
{
    public class EmployeePropertiesLoader : MonoBehaviour
    {
        [SerializeField] private List<EmployeeSo> employeeObjectSoList;

        void Start()
        {
            foreach (var employeeObjectSo in employeeObjectSoList)
            {
                employeeObjectSo.UpdateFloatRangeObjects();
            }
        }
    }
}
