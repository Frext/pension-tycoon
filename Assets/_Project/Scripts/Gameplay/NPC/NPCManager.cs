using UnityEngine;

namespace _Project.Scripts.Gameplay.NPC
{
    public class NPCManager : MonoBehaviour
    {
        [SerializeField] private GameObject singleCustomerPrefab;
        [SerializeField] private GameObject doubleCustomerPrefab;
        [SerializeField] private GameObject cookPrefab;
        [SerializeField] private GameObject cleanerPrefab;
        
        public enum CharactersTypeEnum
        {
            SingleCustomer,
            DoubleCustomer,
            Cook,
            Cleaner
        }

        public void SpawnSingleCustomer() => SpawnCharacter(CharactersTypeEnum.SingleCustomer);
        public void SpawnDoubleCustomer() => SpawnCharacter(CharactersTypeEnum.DoubleCustomer);
        public void SpawnCook() => SpawnCharacter(CharactersTypeEnum.Cook);
        public void SpawnCleaner() => SpawnCharacter(CharactersTypeEnum.Cleaner);

        private void SpawnCharacter(CharactersTypeEnum characterType)
        {
            Instantiate(GetPrefabByCharacterType(characterType), Vector3.zero, Quaternion.identity, transform);
        }
        
        private GameObject GetPrefabByCharacterType(CharactersTypeEnum roomType)
        {
            return roomType switch
            {
                CharactersTypeEnum.SingleCustomer => singleCustomerPrefab,
                CharactersTypeEnum.DoubleCustomer => doubleCustomerPrefab,
                CharactersTypeEnum.Cook => cookPrefab,
                CharactersTypeEnum.Cleaner => cleanerPrefab,
                _ => null
            };
        }
    }
}
