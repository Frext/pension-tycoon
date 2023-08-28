using _Project.Scripts.Gameplay.Building;
using _Project.Scripts.ScriptableObjects.Int;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Shop
{
    public class ShowFloorProgress : MonoBehaviour
    {
        [SerializeField] private IntSo maxFloorCount;
        [Space]
        
        [SerializeField] private FloorManager floorManagerScript;

        [SerializeField] private Image image;

        void OnEnable()
        {
            UpdateProgress();
        }

        public void UpdateProgress()
        {
            image.fillAmount = (float)floorManagerScript.FloorCount / maxFloorCount.Value;
        }
    }
}
