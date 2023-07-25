using System;
using UnityEngine;

namespace _Project.Scripts
{
    public class SlotEvents : MonoBehaviour
    {
        static SlotEvents instance;
		
        // The “action” keyword defines a delegate function that returns nothing. A delegate makes a function to be storable like a variable.
        // The “event” keyword is needed to create an event handler instance of the Action type.
        public static event Action OnShowRoomSlots;
        public static event Action OnShowWcSlots;
        public static event Action OnShowDiningRoomSlots;
        public static event Action OnHideAll;

        void Awake()
        {
            instance = this;
        }

        public void ShowRoomSlots()
        {
            if (OnShowRoomSlots != null) 
                OnShowRoomSlots.Invoke();
        }
        
        public void ShowWcSlots()
        {
            if (OnShowWcSlots != null) 
                OnShowWcSlots.Invoke();
        }
        
        public void ShowDiningRoomSlots()
        {
            if (OnShowDiningRoomSlots != null) 
                OnShowDiningRoomSlots.Invoke();
        }

        public void HideAllSlots()
        {
            if (OnHideAll != null) 
                OnHideAll.Invoke();
        }
    }
}
