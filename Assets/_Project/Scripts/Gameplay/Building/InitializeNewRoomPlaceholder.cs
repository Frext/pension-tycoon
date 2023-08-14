using _Project.Scripts.ScriptableObjects.IntObject;
using _Project.Scripts.ScriptableObjects.RoomType;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Building
{
    public class InitializeNewRoomPlaceholder : MonoBehaviour
    {
        [SerializeField] private SoRoomType selectedRoomTypeSo;
        
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
            GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }
    }
}
