using System;
using _Project.Scripts.ScriptableObjects.SOEvent;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Gameplay.Events
{
    public class SoEventListener : MonoBehaviour
    {
        [SerializeField] private SoEvent soEvent;
        [Space]
        [SerializeField] private UnityEvent responseEvent;

        void Awake()
        {
            soEvent.RegisterToEvent(FireResponseEvent);
        }

        private void FireResponseEvent()
        {
            responseEvent.Invoke();
        }

        void OnDestroy()
        {
            soEvent.DeregisterFromEvent(FireResponseEvent);
        }
    }
}
