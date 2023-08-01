using _Project.Scripts.Gameplay.Building;
using UnityEngine;

namespace _Project.Scripts.ScriptableObjects.RoomType
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(SoRoomType))]
    public class SoRoomType : ScriptableObject
    {
        public Room.RoomTypeEnum SelectedRoomType { get; private set; }

        public void SetSelectedRoomTypeToCustomerSingle() => SetSelectedRoomTypeTo(Room.RoomTypeEnum.CustomerSingle);
        public void SetSelectedRoomTypeToCustomerDouble() => SetSelectedRoomTypeTo(Room.RoomTypeEnum.CustomerDouble);
        public void SetSelectedRoomTypeToBathroom() => SetSelectedRoomTypeTo(Room.RoomTypeEnum.Bathroom);
        public void SetSelectedRoomTypeToDiningRoom() => SetSelectedRoomTypeTo(Room.RoomTypeEnum.Dining);

        private void SetSelectedRoomTypeTo(Room.RoomTypeEnum roomType)
        {
            SelectedRoomType = roomType;
        }
    }
}
