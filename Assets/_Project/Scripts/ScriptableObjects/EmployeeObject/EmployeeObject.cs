using System;
using System.Collections.Generic;
using UnityEngine;
using static _Project.Scripts.ScriptableObjects.TimeRangeObject.FloatRangeObject;

namespace _Project.Scripts.ScriptableObjects.EmployeeObject
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(EmployeeObject))]
    public class EmployeeObject : ScriptableObject
    {
        [Serializable]
        public class Level
        {
            public int purchaseAmount;
            public int upgradeAmount;
            public FloatRange makeRoomUsableTimeRange;
            public FloatRange availabilityRange;
        }

        [SerializeField] private List<Level> levelsList;
        [SerializeField] private IntObject.IntObject currentLevelIndex;

        [SerializeField] private TimeRangeObject.FloatRangeObject timeRangeObjectSo;
        [SerializeField] private TimeRangeObject.FloatRangeObject availabilityRangeObjectSo;
        
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
