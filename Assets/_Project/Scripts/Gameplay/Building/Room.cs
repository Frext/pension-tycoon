using System;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Building
{
    public class Room : MonoBehaviour
    {
        public enum RoomTypes
        {
            None,
            Customer,
            Dining,
            Wc,
            Reception
        }
        
        [Serializable]
        public class Slot
        {
            public bool isOccupied;

            [HideInInspector] public GameObject roomObject;
            
            public RoomTypes roomType;
        }

        public Slot slot;
        [Space]
        
        [Header("UI Slots")]
        [SerializeField] private GameObject removeRoomWidth1Button;
        [SerializeField] private GameObject removeRoomWidth2Button;

        
        void Awake()
        {
            SetRemoveRoomWidth1Button(false);
            SetRemoveRoomWidth2Button(false);
        }

        #region Button Methods
        
        public void SetRemoveRoomWidth1Button(bool activeState)
        {
            removeRoomWidth1Button.SetActive(activeState);
        }
        
        public void SetRemoveRoomWidth2Button(bool activeState)
        {
            removeRoomWidth2Button.SetActive(activeState);
        }

        #endregion
    }
}
