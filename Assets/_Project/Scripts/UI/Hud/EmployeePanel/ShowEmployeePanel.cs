using System;
using _Project.Scripts.ScriptableObjects.EmployeeDict;
using UnityEngine;
using static _Project.Scripts.ScriptableObjects.EmployeeDict.EmployeeDictSo;

namespace _Project.Scripts.UI.Hud.EmployeePanel
{
    public class ShowEmployeePanel : MonoBehaviour
    {
        [SerializeField] private EmployeeDictSo employeeDictSo;
        [Space]
        
        [SerializeField] private EmployeeViewMode employeeViewMode;
        [Space]
        
        [SerializeField] private EmployeeElement employeeElementCleaner;
        [SerializeField] private EmployeeElement employeeElementCook;
        [SerializeField] private EmployeeElement employeeElementGameTechnician;
        [SerializeField] private EmployeeElement employeeElementGymCoach;

        private enum EmployeeViewMode
        {
            AvailableAndTotal,
            Total
        }
        
        void OnEnable()
        {
            UpdateEmployeeElements();
        }
        
        public void UpdateEmployeeElements()
        {
            for (int index = 0; index < employeeDictSo.employeeCountDict.Keys.Count; index++)
            {
                EmployeeTypesEnum currentEmployeeType = (EmployeeTypesEnum)index;
                int totalEmployeeCount = employeeDictSo.employeeCountDict[currentEmployeeType];

                EmployeeElement employeeElement = GetCorrespondingEmployeeElement(currentEmployeeType);
                
                if (totalEmployeeCount > 0)
                {
                    if (employeeViewMode == EmployeeViewMode.AvailableAndTotal)
                    {
                        employeeElement.SetEmployeeElement(
                            GetAvailableEmployeeCount(currentEmployeeType), totalEmployeeCount);
                    }
                    else
                    {
                        employeeElement.SetEmployeeElement(totalEmployeeCount);
                    }
                    
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
