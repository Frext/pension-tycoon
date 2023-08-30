using _Project.Scripts.ScriptableObjects.SOEvent;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.UI.RestartDay
{
    public class ShowRestartDayMenu : MonoBehaviour
    {
        [SerializeField] private SoEvent OnRestartDay;
        [Space]
        
        [SerializeField] private UnityEvent OnShow;
        [SerializeField] private UnityEvent OnHide;
        
        void Awake()
        {
            OnRestartDay.RegisterToEvent(InvokeOnShow);
        }

        void OnDestroy()
        {
            OnRestartDay.DeregisterFromEvent(InvokeOnShow);
        }

        public void InvokeOnShow()
        {
            OnShow.Invoke();
        }
        
        public void InvokeOnHide()
        {
            OnHide.Invoke();
        }
    }
}
