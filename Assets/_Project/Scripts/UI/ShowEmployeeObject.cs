using _Project.Scripts.ScriptableObjects.EmployeeObject;
using _Project.Scripts.ScriptableObjects.SOEvent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static _Project.Scripts.ScriptableObjects.EmployeeObject.EmployeeObject;

namespace _Project.Scripts.UI
{
   public class ShowEmployeeObject : MonoBehaviour
   {
      [SerializeField] private EmployeeObject employeeObject;
      [SerializeField] private string maxText = "Max";

      [SerializeField] private SoEvent OnChangeLevel;

      [Header("UI Elements")] 
      [SerializeField] private TextMeshProUGUI purchaseText;
      [SerializeField] private Button upgradeButton;
      [SerializeField] private TextMeshProUGUI upgradeText;
      [SerializeField] private Image upgradeImage;

      
      void Awake()
      {
         OnChangeLevel.RegisterToEvent(UpdateUI);
      }

      void Start()
      {
         UpdateUI();
      }
      
      private void UpdateUI()
      {
         // If the update progress is at max, don't let the user press it again by making it not interactable.
         if (Mathf.Approximately(upgradeImage.fillAmount, 1f))
         {
            upgradeButton.interactable = false;
            upgradeText.text = maxText;
            return;
         }
         
         Level currentLevel = employeeObject.GetCurrentLevel();

         purchaseText.text = currentLevel.purchaseAmount.ToString();
         upgradeText.text = currentLevel.upgradeAmount.ToString();
         upgradeImage.fillAmount = employeeObject.GetUpdateProgressRatio();
      }

      void OnDestroy()
      {
         OnChangeLevel.DeregisterFromEvent(UpdateUI);
      }
   }
}
