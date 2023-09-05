using _Project.Scripts.ScriptableObjects.SoEventVector3;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform cameraObjectTransform;

        [SerializeField] private float speed;

        [SerializeField] private Collider boundingArea;
        [Space]
        
        [SerializeField] private SoEventVector3 onInitializeNewRoomPlaceholder;
        
        
        private float minX, maxX, minY, maxY, minZ, maxZ;
        
        private Vector3 velocity = Vector3.zero;
        

        void Awake()
        {
            onInitializeNewRoomPlaceholder.RegisterToEvent(SetPosition);
        }

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
            float deltaTime = Time.unscaledDeltaTime;
            
            var currentPos = cameraObjectTransform.position;
            Vector3 targetPos = currentPos;

            targetPos.x += Input.GetAxis("Horizontal") * speed * deltaTime;
            targetPos.y += Input.GetAxis("Vertical") * speed * deltaTime;
            targetPos.z += Input.mouseScrollDelta.y * speed / 4;

            targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
            targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);
            targetPos.z = Mathf.Clamp(targetPos.z, minZ, maxZ);

            currentPos = Vector3.SmoothDamp(currentPos, targetPos, ref velocity, deltaTime);
            cameraObjectTransform.position = currentPos;
        }

        private void SetPosition(Vector3 newPos)
        {
            newPos.z = cameraObjectTransform.position.z;
            
            cameraObjectTransform.position = newPos;
        }

        void OnDestroy()
        {
            onInitializeNewRoomPlaceholder.DeregisterFromEvent(SetPosition);
        }
    }
}
