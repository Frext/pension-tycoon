using _Project.Scripts.ScriptableObjects.RoomType;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Building
{
    public class InitializeNewRoomPlaceholder : MonoBehaviour
    {
        [SerializeField] private GameObject newRoomPlaceholderWidth1Prefab;
        [SerializeField] private GameObject newRoomPlaceholderWidth2Prefab;
        
        public void InitializePlaceholder(SoRoomType roomTypeSo)
        {
            InstantiatePlaceholder(Room.GetRoomWidth(roomTypeSo.SelectedRoomType) == 2
                ? newRoomPlaceholderWidth2Prefab
                : newRoomPlaceholderWidth1Prefab);
        }

        private void InstantiatePlaceholder(GameObject prefab)
        {
            GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }
    }
}
