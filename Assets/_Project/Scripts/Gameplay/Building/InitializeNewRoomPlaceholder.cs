using UnityEngine;

namespace _Project.Scripts.Gameplay.Building
{
    public class InitializeNewRoomPlaceholder : MonoBehaviour
    {
        [SerializeField] private GameObject newRoomPlaceholderWidth1Prefab;
        [SerializeField] private GameObject newRoomPlaceholderWidth2Prefab;

        public void InitializeWidth1()
        {
            InstantiatePlaceholder(newRoomPlaceholderWidth1Prefab);
        }

        private void InstantiatePlaceholder(GameObject prefab)
        {
            GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }

        public void InitializeWidth2()
        {
            InstantiatePlaceholder(newRoomPlaceholderWidth2Prefab);
        }
    }
}
