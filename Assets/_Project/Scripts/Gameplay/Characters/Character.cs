using _Project.Scripts.Gameplay.Building;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Characters
{
    public class Character : MonoBehaviour
    {
        [SerializeField] protected Vector3 startPos;
        [Space]
        
        [SerializeField] protected FloorManager floorManagerScript;

        void OnEnable()
        {
            transform.position = startPos;
        }
    }
}
