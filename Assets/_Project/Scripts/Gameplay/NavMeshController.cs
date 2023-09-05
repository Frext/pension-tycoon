using Unity.AI.Navigation;
using UnityEngine;

namespace _Project.Scripts.Gameplay
{
    public class NavMeshController : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface navMeshSurface;
        
        void Start()
        {
            UpdateNavMesh();
        }

        public void UpdateNavMesh()
        {
            navMeshSurface.BuildNavMesh();
        }
    }
}
