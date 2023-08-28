using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Gameplay.Building;
using _Project.Scripts.ScriptableObjects.EmployeeDict;
using _Project.Scripts.ScriptableObjects.FloatRange;
using _Project.Scripts.ScriptableObjects.Int;
using _Project.Scripts.ScriptableObjects.SoEventGameObject;
using _Project.Scripts.ScriptableObjects.SoEventRoomBool;
using UnityEngine;
using UnityEngine.Events;
using static _Project.Scripts.ScriptableObjects.EmployeeDict.EmployeeDictSo;

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
        [SerializeField] private IntSo dayCountSo;
        [SerializeField] private FloatRangeSo.FloatRange timeBetweenEachSpawn;
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
        
        [Space] 
        [SerializeField] private EmployeeDictSo employeeDictSo;
        
        private readonly List<GameObject> enemyWaveList = new();
        private int lastSpawnedEnemyWaveSize;

        
        void Awake()
        {
            RegisterEvents();
            
            SpawnByFriendNpcCountDict();
        }

        private void RegisterEvents()
        {
            OnCustomerLeave.RegisterToEvent(UpdateEnemyWaveList);
            OnAssignEmployeToRoom.RegisterToEvent(AssignEmployeeToRoom);
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
        
        private void SpawnByFriendNpcCountDict()
        {
            foreach (var keyValuePair in employeeDictSo.employeeCountDict)
            {
                // Convert the enum key to string
                for (int count = 0; count < keyValuePair.Value; count++)
                {
                    SpawnEmployee(keyValuePair.Key, false);
                }
            }
        }
        
        private bool AssignEmployeeToRoom(Room room)
        {
            for (int typeIndex = 0; typeIndex < employeeDictSo.employeeScriptsDict.Keys.Count; typeIndex++)
            {
                EmployeeTypesEnum employeeType = (EmployeeTypesEnum)typeIndex;
                
                if (Equals(Room.GetEmployeeTypeForRoom(room.slot.roomType), employeeType))
                {
                    foreach (var employee in employeeDictSo.employeeScriptsDict[employeeType])
                    {
                        if (employee.IsAvailable())
                        {
                            employee.AssignToRoom(room);
                            return true;
                        }
                    }
                }
            }
            
            // TODO : If the program comes here, that means no employees were available.
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
                employeeDictSo.employeeCountDict[characterType]++;
            }
            
            GameObject employeeGo = InstantiateObject(GetPrefabByCharacterType(characterType));
            
            employeeDictSo.employeeScriptsDict[characterType].Add(employeeGo.GetComponent<Employee>());
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
        
        #region Public Methods
        
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

        #endregion
    }
}
