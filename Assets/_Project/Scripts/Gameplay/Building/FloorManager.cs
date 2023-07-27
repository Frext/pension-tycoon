using UnityEngine;

namespace _Project.Scripts.Gameplay.Building
{
    public class FloorManager : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject floorPrefab;

        [Header("Floor Positioning")] 
        [SerializeField] private Vector3 floorBasePosition;
        [SerializeField] private Vector3 floorOffsetPerFloor;
        [Space]
        
        [Header("GameObjects")]
        [SerializeField] private Transform floorsParentTransform;
        [SerializeField] private Transform roofTransform;

        [Header("Roof Positioning")]
        [SerializeField] private Vector3 roofBasePosition;
        [SerializeField] private Vector3 roofOffsetPerFloor;
        

        void Start()
        {
            PlaceRoof();
        }
        
        private void PlaceRoof()
        {
            Vector3 roofPos = roofBasePosition + roofOffsetPerFloor * floorsParentTransform.childCount;

            roofTransform.position = roofPos;
        }

        private void AddNewFloor()
        {
            Vector3 newFloorPos = floorBasePosition + floorOffsetPerFloor * floorsParentTransform.childCount;

            Instantiate(floorPrefab, newFloorPos, Quaternion.identity, floorsParentTransform);
            
            PlaceRoof();
        }
    }
}
