using _Project.Scripts.ScriptableObjects.IntObject;
using _Project.Scripts.ScriptableObjects.SOEvent;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class ShowDayCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMeshPro;
        [Space]
        
        [SerializeField] private string precedingText;
        [SerializeField] private IntObject dayIntObjectSo;

        [SerializeField] private SoEvent OnChangeDay;

        void Awake()
        {
            OnChangeDay.RegisterToEvent(UpdateText);
        }

        void Start()
        {
            UpdateText();
        }

        void OnDestroy()
        {
            OnChangeDay.DeregisterFromEvent(UpdateText);
        }

        private void UpdateText()
        {
            textMeshPro.text = precedingText + " " + dayIntObjectSo.Value;
        }
    }
}
