using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts
{
    public class RoomManager : MonoBehaviour
    {
        enum RoomTypes
        {
            None,
            Room,
            DiningRoom,
            Wc
        }
        
        [Serializable]
        class Room
        {
            public GameObject go;
            
            public bool isOccupied;
            
            public RoomTypes roomType;
        }
    }
}
