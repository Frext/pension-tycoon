using System.Collections;
using _Project.Scripts.ScriptableObjects.SoEventRoomString;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Hud
{
    public class ShowErrorBlink : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMesh;
        
        [Tooltip("Show the text if this game object is active, if not wait until it is.")]
        [SerializeField] private GameObject gameObjectToSync;
        
        [SerializeField] private float showDuration;
        [Space]

        [SerializeField] private string errorText;

        [Header("Events")] 
        [SerializeField] private SoEventString onNoAvailableRoomOrEmployee;

        
        void Awake()
        {
            onNoAvailableRoomOrEmployee.RegisterToEvent(ShowText);
        }
        
        private void ShowText(string nameOfUnit)
        {
            StopAllCoroutines();
            
            StartCoroutine(ShowTextCoroutine(nameOfUnit));
        }
        
        private IEnumerator ShowTextCoroutine(string text)
        {
            while (!gameObjectToSync.activeInHierarchy)
            {
                // Wait for one frame.
                yield return null; 
            }
    
            SetText(errorText + " " + text);
            yield return StartCoroutine(ShowAndHide());
        }
        
        private void SetText(string text)
        {
            textMesh.text = text;
        }

        private IEnumerator ShowAndHide()
        {
            SetTextVisibility(true);
            
            yield return new WaitForSeconds(showDuration);
            
            SetTextVisibility(false);
        }
        
        private void SetTextVisibility(bool state)
        {
            textMesh.enabled = state;
        }
        
        void Start()
        {
            SetTextVisibility(false);
        }
        
        void OnDestroy()
        {
            onNoAvailableRoomOrEmployee.DeregisterFromEvent(ShowText);
        }
    }
}
