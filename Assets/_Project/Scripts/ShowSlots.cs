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

        void Awake()
        {
            instance = this;
        }

        public void ShowSlots1()
        {
            // We trigger or fire the event here.
            if (OnShow1 != null) 
                OnShow1.Invoke();
        }
        
        public void ShowSlots2()
        {
            // We trigger or fire the event here.
            if (OnShow2 != null) 
                OnShow2.Invoke();
        }
    }
}
