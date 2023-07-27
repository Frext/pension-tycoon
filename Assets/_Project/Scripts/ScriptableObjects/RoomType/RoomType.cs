using _Project.Scripts.Gameplay.Building;
using UnityEngine;

namespace _Project.Scripts.ScriptableObjects.RoomType
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(RoomType))]
    public class RoomType : ScriptableObject
    {
        public Room.RoomTypes SelectedRoomType { get; private set; }

        public void SetSelectedRoomTypeToRoom() => SetSelectedRoomTypeTo(Room.RoomTypes.Room);
        public void SetSelectedRoomTypeToWc() => SetSelectedRoomTypeTo(Room.RoomTypes.Wc);
        public void SetSelectedRoomTypeToDiningRoom() => SetSelectedRoomTypeTo(Room.RoomTypes.DiningRoom);

        private void SetSelectedRoomTypeTo(Room.RoomTypes roomType)
        {
            SelectedRoomType = roomType;
        }
    }
}
