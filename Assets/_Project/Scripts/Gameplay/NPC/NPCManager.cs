using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Gameplay.Data;
using _Project.Scripts.ScriptableObjects.IntObject;
using _Project.Scripts.ScriptableObjects.SoEventGameObject;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Gameplay.NPC
{
    public static class ListExtensions {
        /// <summary>
        /// Shuffles the element order of the specified list.
        /// </summary>
        public static void Shuffle<T>(this IList<T> ts) {
            var count = ts.Count;
            var last = count - 1;
            
            for (var i = 0; i < last; ++i) {
                var r = UnityEngine.Random.Range(i, count);
                (ts[i], ts[r]) = (ts[r], ts[i]);
            }
        }
    }
    
    public class NpcManager : MonoBehaviour
    {
        [Serializable]
        public class SpawnableObject
        {
            public GameObject prefab;
            public int initialValue;
            public int incrementAmount;
            public int incrementEveryRoundOf;
        }
        
        [SerializeField] private List<SpawnableObject> enemySpawnableObjects;
        [Space]
        [SerializeField] private IntObject dayCountSo;
        [SerializeField] private FloatRange timeBetweenEachSpawn;
        [Space]
        
        [Header("Friend Spawnable Objects")]
        [SerializeField] private GameObject cleanerPrefab;
        [SerializeField] private GameObject cookPrefab;

        [Header("Events")] 
        [SerializeField] private SoEventGameObject OnCustomerLeave;
        [SerializeField] private UnityEvent OnEnemyWaveRestart;
        [SerializeField] private UnityEvent OnEnemyWaveDecreased;
        [SerializeField] private UnityEvent OnEnemyWaveFinished;
        
        [Header("Data Saving")]
        [SerializeField] private string dataKey = "friendNpcCountDict";
        
        private enum FriendNpcTypesEnum
        {
            Cook,
            Cleaner
        }
        private readonly Dictionary<FriendNpcTypesEnum, int> friendNpcCountDict = new();
        
        private readonly List<GameObject> enemyWaveList = new();
        private int lastSpawnedEnemyWaveSize;

        
        void Awake()
        {
            OnCustomerLeave.RegisterToEvent(UpdateEnemyWaveList);
            
            LoadFriendNpcCountDict();
        }
        
        private void LoadFriendNpcCountDict()
        {
            GetDefaultFriendNpcCountDict();

            // Change the values if the deserialized dictionary is loaded.
            if (DataManager.Load(dataKey, out Dictionary<string, int> deserialized))
            {
                LoadValuesFromDeserializedDict(deserialized);

                SpawnByFriendNpcCountDict();
            }
        }

        private void GetDefaultFriendNpcCountDict()
        {
            friendNpcCountDict.Clear();
            
            // Add each type in enum to the dictionary before changing values.
            for (int index = 0; index < Enum.GetNames(typeof(FriendNpcTypesEnum)).Length; index++)
            {
                friendNpcCountDict.Add((FriendNpcTypesEnum)index, 0);
            }
        }
        
        private void LoadValuesFromDeserializedDict(Dictionary<string, int> deserializedDict)
        {
            foreach (var keyValuePair in deserializedDict)
            {
                if (Enum.TryParse(keyValuePair.Key, out FriendNpcTypesEnum keyEnum))
                {
                    friendNpcCountDict[keyEnum] = keyValuePair.Value;
                }
            }
        }
        
        private void SpawnByFriendNpcCountDict()
        {
            foreach (var keyValuePair in friendNpcCountDict)
            {
                // Convert the enum key to string
                for (int count = 0; count < keyValuePair.Value; count++)
                {
                    SpawnFriendCharacter(keyValuePair.Key, false);
                }
            }
        }

        private void UpdateEnemyWaveList(GameObject enemyGameObject)
        {
            if (!enemyWaveList.Remove(enemyGameObject))
            {
                return;
            }
            
            OnEnemyWaveDecreased.Invoke();
            
            if (enemyWaveList.Count == 0)
            {
                OnEnemyWaveFinished.Invoke();
            }
        }
        
        public void SpawnEnemyWave()
        {
            enemyWaveList.Clear();

            int waveCount = dayCountSo.Value - 1;

            foreach (var spawnableObject in enemySpawnableObjects)
            {
                for (int count = 0;
                     count < spawnableObject.initialValue +
                     waveCount / spawnableObject.incrementEveryRoundOf * spawnableObject.incrementAmount;
                     count++)
                {
                    enemyWaveList.Add(InstantiateObject(spawnableObject.prefab));
                }
            }

            lastSpawnedEnemyWaveSize = enemyWaveList.Count;
            StartCoroutine(IEnableEveryEnemyWaveObject());
        }
        
        private GameObject InstantiateObject(GameObject prefab)
        {
            return Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
        }

        private IEnumerator IEnableEveryEnemyWaveObject()
        {
            enemyWaveList.Shuffle();
            
            int i = 0;
            
            while (GetCurrentEnemyWaveSize() > 0)
            {
                enemyWaveList[Mathf.Clamp(i, 0, GetCurrentEnemyWaveSize() - 1)].SetActive(true);
                i++;
                
                yield return new WaitForSeconds(timeBetweenEachSpawn.Randomize());
            }
        }
        
        void OnDestroy()
        {
            OnCustomerLeave.DeregisterFromEvent(UpdateEnemyWaveList);
        }

        public void SpawnCook() => SpawnFriendCharacter(FriendNpcTypesEnum.Cook);
        public void SpawnCleaner() => SpawnFriendCharacter(FriendNpcTypesEnum.Cleaner);

        private void SpawnFriendCharacter(FriendNpcTypesEnum characterType, bool incrementInDictionary = true)
        {
            if (incrementInDictionary)
            {
                friendNpcCountDict[characterType]++;
            }
            
            InstantiateObject(GetPrefabByCharacterType(characterType));
        }

        private GameObject GetPrefabByCharacterType(FriendNpcTypesEnum roomType)
        {
            return roomType switch
            {
                FriendNpcTypesEnum.Cleaner => cleanerPrefab,
                FriendNpcTypesEnum.Cook => cookPrefab,
                _ => null
            };
        }

        public int GetTotalEnemyWaveSize()
        {
            return lastSpawnedEnemyWaveSize;
        }

        public int GetCurrentEnemyWaveSize()
        {
            return enemyWaveList.Count;
        }

        public void RestartEnemyWave()
        {
            OnEnemyWaveRestart.Invoke();
            
            StopAllCoroutines();

            foreach (var enemyGameObject in enemyWaveList)
            {
                Destroy(enemyGameObject);
            }
        }

        private void OnApplicationQuit()
        {
            SaveFriendNpcCountDict();
        }
        
        private void SaveFriendNpcCountDict()
        {
            Dictionary<string, int> serializedDict = new Dictionary<string, int>();

            foreach (var keyValuePair in friendNpcCountDict)
            {
                // Convert the enum key to string
                serializedDict.Add(keyValuePair.Key.ToString(), keyValuePair.Value);
            }
            
            DataManager.Save(dataKey, serializedDict);
        }
    }
}
