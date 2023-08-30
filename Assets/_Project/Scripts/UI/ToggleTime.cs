using System;
using _Project.Scripts.ScriptableObjects.Int;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class ToggleTime : MonoBehaviour
    {
        public static ToggleTime Instance { get; private set; }

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                throw new Exception("More than 1 singleton found @" + gameObject.name);
            }

            Instance = this;
        }

        public void SetTimeScaleTo(float scale)
        {
            Time.timeScale = scale;
        }
        
        public void SetTimeScaleTo(IntSo intSo)
        {
            Time.timeScale = intSo.Value;
        }
    }
}
