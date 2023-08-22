using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Gameplay.Building;
using _Project.Scripts.Gameplay.Data;
using _Project.Scripts.ScriptableObjects.IntObject;
using _Project.Scripts.ScriptableObjects.SoEventGameObject;
using _Project.Scripts.ScriptableObjects.SoEventRoomBool;
using _Project.Scripts.ScriptableObjects.TimeRangeObject;
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
        public class CustomerObject
        {
            public GameObject prefab;
            public int firstLevel;
            public int initialValue;
            public int incrementAmount;
            public int incrementEveryRoundOf;
        }
        
        [SerializeField] private List<CustomerObject> customerObjectsList;
        [Space]
        [SerializeField] private IntObject dayCountSo;
        [SerializeField] private FloatRangeObject.FloatRange timeBetweenEachSpawn;
        [Space]
        
        [Header("Employee Prefabs")]
        [SerializeField] private GameObject cleanerPrefab;
        [SerializeField] private GameObject cookPrefab;
        [SerializeField] private GameObject gameTechnicianPrefab;
        [SerializeField] private GameObject gymCoachPrefab;

        [Header("Events")] 
        [SerializeField] private SoEventGameObject OnCustomerLeave;
        [Space]
        [SerializeField] private SoEventRoomBool OnAssignEmployeToRoom;
        [Space]
        [SerializeField] private UnityEvent OnEnemyWaveRestart;
        [SerializeField] private UnityEvent OnEnemyWaveDecreased;
        [SerializeField] private UnityEvent OnEnemyWaveFinished;
        
        [Header("Data Saving")]
        [SerializeField] private string dataKey = "employeeCountDict";
        
        public enum EmployeeTypesEnum
        {
            Cook,
            Cleaner,
            GameTechnician,
            GymCoach
        }
        private readonly Dictionary<EmployeeTypesEnum, int> employeeCountDict = new();
        private readonly Dictionary<EmployeeTypesEnum, List<Employee>> employeeScriptsListDict = new();
        
        private readonly List<GameObject> enemyWaveList = new();
        private int lastSpawnedEnemyWaveSize;

        
        void Awake()
        {
            RegisterEvents();
            
            LoadFriendNpcCountDict();
        }

        private void RegisterEvents()
        {
            OnCustomerLeave.RegisterToEvent(UpdateEnemyWaveList);
            OnAssignEmployeToRoom.RegisterToEvent(AssignEmployeeToRoom);
        }

        private void LoadFriendNpcCountDict()
        {
            GetDefaultEmployeeNpcCountDict();
            GetDefaultEmployeeScriptsListDict();

            // Change the values if the deserialized dictionary is loaded.
            if (DataManager.Load(dataKey, out Dictionary<string, int> deserialized))
            {
                LoadValuesFromDeserializedDict(deserialized);

                SpawnByFriendNpcCountDict();
            }
        }
        
        private void GetDefaultEmployeeNpcCountDict()
        {
            employeeCountDict.Clear();
            
            // Add each type in enum to the dictionary before changing values.
            for (int index = 0; index < Enum.GetNames(typeof(EmployeeTypesEnum)).Length; index++)
            {
                employeeCountDict.Add((EmployeeTypesEnum)index, 0);
            }
        }
        
        private void GetDefaultEmployeeScriptsListDict()
        {
            employeeScriptsListDict.Clear();
            
            // Add each type in enum to the dictionary before changing values.
            for (int index = 0; index < Enum.GetNames(typeof(EmployeeTypesEnum)).Length; index++)
            {
                employeeScriptsListDict.Add((EmployeeTypesEnum)index, new List<Employee>());
            }
        }
        
        private void LoadValuesFromDeserializedDict(Dictionary<string, int> deserializedDict)
        {
            foreach (var keyValuePair in deserializedDict)
            {
                if (Enum.TryParse(keyValuePair.Key, out EmployeeTypesEnum keyEnum))
                {
                    employeeCountDict[keyEnum] = keyValuePair.Value;
                }
            }
        }
        
        private void SpawnByFriendNpcCountDict()
        {
            foreach (var keyValuePair in employeeCountDict)
            {
                // Convert the enum key to string
                for (int count = 0; count < keyValuePair.Value; count++)
                {
                    SpawnEmployee(keyValuePair.Key, false);
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

            int waveCount = dayCountSo.Value;

            foreach (var spawnableObject in customerObjectsList)
            {
                if (waveCount < spawnableObject.firstLevel)
                {
                    continue;
                }
                
                for (int count = 0;
                     count < spawnableObject.initialValue +
                     (waveCount - 1) / spawnableObject.incrementEveryRoundOf * spawnableObject.incrementAmount;
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
        
        private bool AssignEmployeeToRoom(Room room)
        {
            for (int typeIndex = 0; typeIndex < employeeScriptsListDict.Keys.Count; typeIndex++)
            {
                EmployeeTypesEnum employeeType = (EmployeeTypesEnum)typeIndex;
                
                List<Employee> employeeScriptsList = employeeScriptsListDict[employeeType];
                
                if (Equals(Room.GetEmployeeTypeForRoom(room.slot.roomType), employeeType))
                {
                    foreach (var employee in employeeScriptsList)
                    {
                        if (employee.IsAvailable())
                        {
                            employee.AssignToRoom(room);
                            return true;
                        }
                    }
                }
            }
            
            // If the program comes here, that means no employees were available.
            return false;
        }
        
        void OnDestroy()
        {
            DeregisterEvents();
        }

        private void DeregisterEvents()
        {
            OnCustomerLeave.DeregisterFromEvent(UpdateEnemyWaveList);
            OnAssignEmployeToRoom.DeregisterFromEvent(AssignEmployeeToRoom);
        }

        public void SpawnCook() => SpawnEmployee(EmployeeTypesEnum.Cook);
        public void SpawnCleaner() => SpawnEmployee(EmployeeTypesEnum.Cleaner);
        public void SpawnGameTechnician() => SpawnEmployee(EmployeeTypesEnum.GameTechnician);
        public void SpawnGymCoach() => SpawnEmployee(EmployeeTypesEnum.GymCoach);

        private void SpawnEmployee(EmployeeTypesEnum characterType, bool incrementInDictionary = true)
        {
            if (incrementInDictionary)
            {
                employeeCountDict[characterType]++;
            }
            
            GameObject employeeGo = InstantiateObject(GetPrefabByCharacterType(characterType));
            
            employeeScriptsListDict[characterType].Add(employeeGo.GetComponent<Employee>());
        }

        private GameObject GetPrefabByCharacterType(EmployeeTypesEnum roomType)
        {
            return roomType switch
            {
                EmployeeTypesEnum.Cleaner => cleanerPrefab,
                EmployeeTypesEnum.Cook => cookPrefab,
                EmployeeTypesEnum.GameTechnician => gameTechnicianPrefab,
                EmployeeTypesEnum.GymCoach => gymCoachPrefab,
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

            foreach (var keyValuePair in employeeCountDict)
            {
                // Convert the enum key to string
                serializedDict.Add(keyValuePair.Key.ToString(), keyValuePair.Value);
            }
            
            DataManager.Save(dataKey, serializedDict);
        }
    }
}
