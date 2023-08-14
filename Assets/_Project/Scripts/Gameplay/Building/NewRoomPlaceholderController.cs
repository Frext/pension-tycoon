using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Gameplay.Building
{
    public class NewRoomPlaceholderController : MonoBehaviour
    {
        [Header("Materials")]
        [SerializeField] private Material canPlaceMaterial;
        [SerializeField] private Material cannotPlaceMaterial;

        [Header("Position Constraints")]
        [SerializeField] private float minX;
        [SerializeField] private float maxX;
        [SerializeField] private float minY;
        [SerializeField] private float posZ;
        [Range(1, 2)] [SerializeField] private int length = 1; 
        [Space]
        
        [SerializeField] private List<MeshRenderer> childMeshRenderersList;
        
        [SerializeField] private FloorManager floorManagerScript;
        [Space]
        
        [SerializeField] private UnityEvent OnCancelPlacing;

        private float passedTime;

        private Vector2Int CurrentGridPosition
        {
            get
            {
                var position = transform.position;
                float x = position.x - minX;
                float y = position.y - minY;
                return new Vector2Int((int)x, (int)y);
            }
        }

        
        void OnEnable()
        {
            GoToFirstUnconstructedRoom();
        }

        private void GoToFirstUnconstructedRoom()
        {
            for (int floor = 0; floor < floorManagerScript.FloorCount; floor++)
            {
                for (int room = 0; room < FloorManager.RoomCountPerFloor; room++)
                {
                    if (CanPlaceRoomAt(new Vector2Int(room, floor)))
                    {
                        gameObject.transform.position = new Vector3(minX + room, minY + floor, posZ);
                        return;
                    }
                }
            }

            // If there is not enough place.
            CancelPlacingRoom();
        }
        
        private bool CanPlaceRoomAt(Vector2Int gridPosition)
        {
            if (length == 1)
            {
                return !floorManagerScript.IsRoomConstructedAt(gridPosition);
            }

            for (int i = 0; i < length; i++)
            {
                if (floorManagerScript.IsRoomConstructedAt(gridPosition + new Vector2Int(i, 0)))
                {
                    return false;
                }
            }
            return true;
        }

        void Update()
        {
            if (!(Time.time > passedTime)) 
                return;
            
            Move(new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
            UpdateAccuracyColor();
            
            passedTime = Time.time + 0.1f;
        }

        private void Move(Vector3 rawInput)
        {
            Vector3 newPos = transform.position + new Vector3(rawInput.x, rawInput.y);
            newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
            newPos.y = Mathf.Clamp(newPos.y, minY, minY + (floorManagerScript.FloorCount - 1));

            transform.position = newPos;
        }

        private void UpdateAccuracyColor()
        {
            SetMaterial(CanPlaceRoomAt(CurrentGridPosition) ? canPlaceMaterial : cannotPlaceMaterial);
        }
        
        private void SetMaterial(Material material)
        {
            foreach (var meshRenderer in childMeshRenderersList)
            { 
                meshRenderer.sharedMaterial = material;
            }
        }

        #region Button Methods

        public void PlaceRoom()
        {
            if (!CanPlaceRoomAt(CurrentGridPosition))
                return;

            floorManagerScript.CreateRoomAt(CurrentGridPosition);
            DestroyObject();
        }
        
        private void DestroyObject()
        {
            Destroy(gameObject);
        }
        
        public void CancelPlacingRoom()
        {
            OnCancelPlacing.Invoke();
            
            DestroyObject();
        }

        #endregion
    }
}
