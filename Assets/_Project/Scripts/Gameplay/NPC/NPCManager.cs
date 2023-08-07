using System;
using System.Collections;
using System.Collections.Generic;
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
    
    public class NPCManager : MonoBehaviour
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
        [SerializeField] private IntObject waveCountSo;
        [SerializeField] private FloatRange timeBetweenEachSpawn;
        [Space]
        
        [Header("Friend Spawnable Objects")]
        [SerializeField] private GameObject cleanerPrefab;
        [SerializeField] private GameObject cookPrefab;

        [Header("Events")] 
        [SerializeField] private SoEventGameObject OnCustomerLeave;
        [SerializeField] private UnityEvent OnWaveFinished;
        
        private enum FriendSpawnableObjectTypesEnum
        {
            Cook,
            Cleaner
        }

        private List<GameObject> enemyWaveList = new();

        
        void Awake()
        {
            OnCustomerLeave.RegisterToEvent(UpdateEnemyWaveList);
        }

        void OnDestroy()
        {
            OnCustomerLeave.DeregisterFromEvent(UpdateEnemyWaveList);
        }
        
        private void UpdateEnemyWaveList(GameObject gameObject)
        {
            enemyWaveList.Remove(gameObject);
            
            if (enemyWaveList.Count == 0)
            {
                OnWaveFinished.Invoke();
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SpawnEnemyWave();
            }
        }
        
        public void SpawnEnemyWave()
        {
            enemyWaveList.Clear();

            int waveCount = waveCountSo.Value;

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

            StartCoroutine(IEnableEachEnemyWaveObject());
        }
        
        private GameObject InstantiateObject(GameObject prefab)
        {
            return Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
        }

        private IEnumerator IEnableEachEnemyWaveObject()
        {
            enemyWaveList.Shuffle();

            foreach (var randomGameObject in enemyWaveList)
            {
                randomGameObject.SetActive(true);

                yield return new WaitForSeconds(timeBetweenEachSpawn.Randomize());
            }
        }

        public void SpawnCook() => SpawnCharacter(FriendSpawnableObjectTypesEnum.Cook);
        public void SpawnCleaner() => SpawnCharacter(FriendSpawnableObjectTypesEnum.Cleaner);

        private void SpawnCharacter(FriendSpawnableObjectTypesEnum characterType)
        {
            InstantiateObject(GetPrefabByCharacterType(characterType));
        }

        private GameObject GetPrefabByCharacterType(FriendSpawnableObjectTypesEnum roomType)
        {
            return roomType switch
            {
                FriendSpawnableObjectTypesEnum.Cleaner => cleanerPrefab,
                FriendSpawnableObjectTypesEnum.Cook => cookPrefab,
                _ => null
            };
        }
    }
}
