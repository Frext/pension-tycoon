using System;
using _Project.Scripts.ScriptableObjects.EmployeeDictObject;
using UnityEngine;
using static _Project.Scripts.ScriptableObjects.EmployeeDictObject.EmployeeDictObject;

namespace _Project.Scripts.UI.HUDPanel
{
    public class ShowEmployeePanel : MonoBehaviour
    {
        [SerializeField] private EmployeeDictObject employeeDictSo;
        [Space]
        
        [SerializeField] private EmployeeElement employeeElementCleaner;
        [SerializeField] private EmployeeElement employeeElementCook;
        [SerializeField] private EmployeeElement employeeElementGameTechnician;
        [SerializeField] private EmployeeElement employeeElementGymCoach;

        
        void Start()
        {
            UpdateEmployeeElements();
        }
        
        public void UpdateEmployeeElements()
        {
            gameObject.SetActive(true);
            
            for (int index = 0; index < employeeDictSo.employeeCountDict.Keys.Count; index++)
            {
                EmployeeTypesEnum currentEmployeeType = (EmployeeTypesEnum)index;
                int totalEmployeeCount = employeeDictSo.employeeCountDict[currentEmployeeType];

                EmployeeElement employeeElement = GetCorrespondingEmployeeElement(currentEmployeeType);
                
                if (totalEmployeeCount > 0)
                {
                    employeeElement.SetEmployeeElement(
                        GetAvailableEmployeeCount(currentEmployeeType), totalEmployeeCount);
                    
                    employeeElement.gameObject.SetActive(true);
                }
                else
                {
                    employeeElement.gameObject.SetActive(false);
                }
            }
        }

        private EmployeeElement GetCorrespondingEmployeeElement(EmployeeTypesEnum employeeType)
        {
            return employeeType switch
            {
                EmployeeTypesEnum.Cleaner => employeeElementCleaner,
                EmployeeTypesEnum.Cook => employeeElementCook,
                EmployeeTypesEnum.GameTechnician => employeeElementGameTechnician,
                EmployeeTypesEnum.GymCoach => employeeElementGymCoach,
                _ => throw new ArgumentOutOfRangeException(nameof(employeeType), employeeType, null)
            };
        }

        private int GetAvailableEmployeeCount(EmployeeTypesEnum employeeType)
        {
            int count = 0;

            foreach (var employeeScript in employeeDictSo.employeeScriptsDict[employeeType])
            {
                if (employeeScript.IsAvailable())
                {
                    count++;
                }
            }

            return count;
        }
    }
}
