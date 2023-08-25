using _Project.Scripts.ScriptableObjects.EmployeeDictObject;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class ShowEmployeePanel : MonoBehaviour
    {
        [SerializeField] private EmployeeDictObject employeeCountDictSo;

        [SerializeField] private GameObject elementEmployeePrefab;
    }
}
