using System;
using _Project.Scripts.ScriptableObjects.SoEventGameObject;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Gameplay.Events
{
    public class SoEventGameObjectHandler : MonoBehaviour
    {
        [SerializeField] private SoEventGameObject soEventGameObject;
        [SerializeField] private UnityEvent onSuccess;

        void Awake()
        {
            soEventGameObject.RegisterToEvent(HandleEvent);
        }

        private void HandleEvent(GameObject go)
        {
            if (go == gameObject)
            {
                onSuccess.Invoke();
            }
        }

        void OnDestroy()
        {
            soEventGameObject.DeregisterFromEvent(HandleEvent);
        }
    }
}
