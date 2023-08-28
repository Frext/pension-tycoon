using _Project.Scripts.ScriptableObjects.RoomType;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Building
{
    public class InitializeNewRoomPlaceholder : MonoBehaviour
    {
        [SerializeField] private RoomTypeSo selectedRoomTypeSo;
        
        [SerializeField] private GameObject newRoomPlaceholderWidth1Prefab;
        [SerializeField] private GameObject newRoomPlaceholderWidth2Prefab;
        
        public void InitializePlaceholder()
        {
            InstantiatePlaceholder(Room.GetRoomWidth(selectedRoomTypeSo.SelectedRoomType) == 2
                ? newRoomPlaceholderWidth2Prefab
                : newRoomPlaceholderWidth1Prefab);
        }

        private void InstantiatePlaceholder(GameObject prefab)
        {
            GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
        }

        public void CancelAllRoomPlacing()
        {
            for (int index = 0; index < transform.childCount; index++)
            {
                transform.GetChild(index).GetComponent<NewRoomPlaceholderController>().CancelPlacingRoom();
            }
        }
    }
}
