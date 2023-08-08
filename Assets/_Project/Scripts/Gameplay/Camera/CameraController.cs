using UnityEngine;

namespace _Project.Scripts.Gameplay.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform cameraObjectTransform;

        [SerializeField] private float speed;

        [SerializeField] 
        private Collider boundingArea;
        
        private float minX, maxX, minY, maxY, minZ, maxZ;
        
        private Vector3 velocity = Vector3.zero;

        
        void Start()
        {
            GetCameraBoundaries();
        }

        private void GetCameraBoundaries()
        {
            Vector3 extents = boundingArea.bounds.extents;
            Vector3 position = boundingArea.transform.position;

            minX = position.x - extents.x;
            maxX = position.x + extents.x;

            minY = position.y - extents.y;
            maxY = position.y + extents.y;
            
            minZ = position.z - extents.z;
            maxZ = position.z + extents.z;
        }
        
        void Update()
        {
            HandleCameraMovement();
        }

        private void HandleCameraMovement()
        {
            var currentPos = cameraObjectTransform.position;
            Vector3 targetPos = currentPos;

            targetPos.x += Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            targetPos.y += Input.GetAxis("Vertical") * speed * Time.deltaTime;
            targetPos.z += Input.mouseScrollDelta.y * speed / 4;

            targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
            targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);
            targetPos.z = Mathf.Clamp(targetPos.z, minZ, maxZ);

            currentPos = Vector3.SmoothDamp(currentPos, targetPos, ref velocity, Time.deltaTime);
            cameraObjectTransform.position = currentPos;
        }
    }
}
