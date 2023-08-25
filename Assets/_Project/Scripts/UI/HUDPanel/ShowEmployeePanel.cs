using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.ScriptableObjects.EmployeeDictObject;
using _Project.Scripts.ScriptableObjects.SoEventGameObject;
using UnityEngine;
using static _Project.Scripts.ScriptableObjects.EmployeeDictObject.EmployeeDictObject;

namespace _Project.Scripts.UI.HUDPanel
{
    public class ShowEmployeePanel : MonoBehaviour
    {
        [SerializeField] private EmployeeDictObject employeeDictSo;
        [SerializeField] private List<EmployeeElement> employeeElementsList;
        [Space]

        private const float EventDelay = 0.1f;
        
        private void UpdateEmployeeElementsAfterDelay(GameObject go)
        {
            // The delay is needed to wait the employees to get assigned to a room and change their availability status.

            StartCoroutine(RunAfterSeconds(UpdateEmployeeElements, EventDelay));
        }

        private IEnumerator RunAfterSeconds(Action action, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            
            action.Invoke();
        }

        public void UpdateEmployeeElements()
        {
            gameObject.SetActive(true);
            
            for (int index = 0; index < employeeDictSo.employeeCountDict.Keys.Count; index++)
            {
                EmployeeTypesEnum currentEmployeeType = (EmployeeTypesEnum)index;
                int totalEmployeeCount = employeeDictSo.employeeCountDict[currentEmployeeType];
                
                if (totalEmployeeCount > 0)
                {
                    employeeElementsList[index].SetEmployeeElement(
                        GetAvailableEmployeeCount(currentEmployeeType), totalEmployeeCount);
                    
                    employeeElementsList[index].gameObject.SetActive(true);
                }
                else
                {
                    employeeElementsList[index].gameObject.SetActive(false);
                }
            }
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
