using UnityEngine;

namespace _Project.Scripts
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private GameObject width1Slot;
        [SerializeField] private GameObject width2Slot;

        void Awake()
        {
            ShowSlots.OnShow1 += ToggleWidth1Slot;
        }

        void OnDestroy()
        {
            ShowSlots.OnShow1 -= ToggleWidth1Slot;
        }

        void ToggleWidth1Slot()
        {
            width1Slot.SetActive(!width1Slot.activeInHierarchy);
        }

        void ToggleWidth2Slot()
        {
            
        }
    }
}
