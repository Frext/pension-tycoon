using _Project.Scripts.Gameplay.Building;
using _Project.Scripts.ScriptableObjects.Int;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Shop
{
    public class ShowFloorProgress : MonoBehaviour
    {
        [SerializeField] private IntSo constructedRoomsCountSo;
        [SerializeField] private IntSo floorCountSo;
        
        [SerializeField] private Image image;

        void OnEnable()
        {
            UpdateProgress();
        }

        public void UpdateProgress()
        {
            image.fillAmount = constructedRoomsCountSo.Value / (float)floorCountSo.GetMax() / FloorManager.RoomCountPerFloor;
        }
    }
}
