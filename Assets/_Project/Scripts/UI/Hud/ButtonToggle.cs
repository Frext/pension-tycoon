using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.UI.Hud
{
    public class ButtonToggle : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnToggleOn;
        [SerializeField] private UnityEvent OnToggleOff;

        private bool isToggledOn;

        public void Toggle()
        {
            SetTo(!isToggledOn);
        }

        public void SetTo(bool toggleState = false)
        {
            isToggledOn = toggleState;

            if (isToggledOn)
            {
                OnToggleOn.Invoke();
            }
            else
            {
                OnToggleOff.Invoke();
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
            
            SetTo();
        }

        public void Hide()
        {
            SetTo();
            
            gameObject.SetActive(false);
        }
    }
}
