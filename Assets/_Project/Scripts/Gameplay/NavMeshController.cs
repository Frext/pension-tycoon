using System;
using UnityEditor.AI;
using UnityEngine;

namespace _Project.Scripts.Gameplay
{
    public class NavMeshController : MonoBehaviour
    {
        public static NavMeshController Instance { get; private set; }
        
        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                throw new Exception("More than 1 singleton found @" + gameObject.name);
            }

            Instance = this;
            
            UpdateNavMesh();
        }
        
        public void UpdateNavMesh()
        {
            NavMeshBuilder.BuildNavMesh();
        }
    }
}
