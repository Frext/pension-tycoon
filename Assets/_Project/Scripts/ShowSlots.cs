using System;
using UnityEngine;

namespace _Project.Scripts
{
    public class ShowSlots : MonoBehaviour
    {
        static ShowSlots instance;
		
        // The “action” keyword defines a delegate function that returns nothing. A delegate makes a function to be storable like a variable.
        // The “event” keyword is needed to create an event handler instance of the Action type.
        public static event Action OnShow1;
        public static event Action OnShow2;

        public static event Action OnHideAll;

        void Awake()
        {
            instance = this;
        }

        public void ShowSlotsWidth1()
        {
            if (OnShow1 != null) 
                OnShow1.Invoke();
        }
        
        public void ShowSlotsWidth2()
        {
            if (OnShow2 != null) 
                OnShow2.Invoke();
        }

        public void HideAllSlots()
        {
            if (OnHideAll != null) 
                OnHideAll.Invoke();
        }
    }
}
