using System;
using System.Collections.Generic;
using _Project.Scripts.ScriptableObjects.FloatRange;
using _Project.Scripts.ScriptableObjects.Int;
using UnityEngine;

namespace _Project.Scripts.ScriptableObjects.Employee
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(EmployeeSo))]
    public class EmployeeSo : ScriptableObject
    {
        [Serializable]
        public class Level
        {
            public int purchaseAmount;
            public int upgradeAmount;
            public FloatRangeSo.FloatRange makeRoomUsableTimeRange;
            public FloatRangeSo.FloatRange availabilityRange;
        }

        [SerializeField] private List<Level> levelsList;
        [SerializeField] private IntSo currentLevelIndex;

        [SerializeField] private FloatRangeSo timeRangeObjectSo;
        [SerializeField] private FloatRangeSo availabilityRangeObjectSo;
        
        public void UpdateFloatRangeObjects()
        {
            timeRangeObjectSo.SetRange(levelsList[currentLevelIndex.Value].makeRoomUsableTimeRange);
            availabilityRangeObjectSo.SetRange(levelsList[currentLevelIndex.Value].availabilityRange);
        }

        public void IncrementLevel()
        {
            if (currentLevelIndex.Value + 1 < levelsList.Count)
            {
                currentLevelIndex.IncrementValue();

                UpdateFloatRangeObjects();
            }
        }
        
        public Level GetCurrentLevel()
        {
            return levelsList[currentLevelIndex.Value];
        }

        public float GetUpdateProgressRatio()
        {
            return (float)(currentLevelIndex.Value + 1) / levelsList.Count;
        }
    }
}
