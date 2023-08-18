using _Project.Scripts.ScriptableObjects.IntObject;
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

        void Start()
        {
            UpdateText();
        }

        public void UpdateText()
        {
            textMeshPro.text = precedingText + intObjectSo.Value;
        }
    }
}
