using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.HUDPanel
{
    public class EmployeeElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI availableEmployeeCountText;
        [SerializeField] private TextMeshProUGUI totalEmployeeCountText;

        public void SetEmployeeElement(int availableEmployeeCount, int totalEmployeeCount)
        {
            availableEmployeeCountText.text = availableEmployeeCount.ToString();
            totalEmployeeCountText.text = totalEmployeeCount.ToString();
        }
    }
}
