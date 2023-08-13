using _Project.Scripts.ScriptableObjects.IntObject;
using _Project.Scripts.ScriptableObjects.SOEvent;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class ShowIntObject : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMeshPro;
        [Space]
        
        [SerializeField] private string precedingText;
        [SerializeField] private IntObject intObjectSo;

        [Tooltip("This is required to update the TMP text when the event gets triggered.")]
        [SerializeField] private SoEvent OnChangeValue;

        void Awake()
        {
            OnChangeValue.RegisterToEvent(UpdateText);
        }

        void Start()
        {
            UpdateText();
        }

        void OnDestroy()
        {
            OnChangeValue.DeregisterFromEvent(UpdateText);
        }

        private void UpdateText()
        {
            textMeshPro.text = precedingText + intObjectSo.Value;
        }
    }
}
