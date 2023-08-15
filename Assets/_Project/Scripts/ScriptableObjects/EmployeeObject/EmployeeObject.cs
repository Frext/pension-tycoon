using System;
using System.Collections.Generic;
using UnityEngine;
using static _Project.Scripts.ScriptableObjects.TimeRangeObject.TimeRangeObject;

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
        }

        [SerializeField] private List<Level> levelsList;
        [SerializeField] private IntObject.IntObject currentLevelIndex;

        [SerializeField] private TimeRangeObject.TimeRangeObject timeRangeObjectSo;
        
        public void UpdateTimeRange()
        {
            timeRangeObjectSo.SetTimeRange(levelsList[currentLevelIndex.Value].makeRoomUsableTimeRange);
        }

        public void IncrementLevel()
        {
            if (currentLevelIndex.Value + 1 < levelsList.Count)
            {
                currentLevelIndex.IncrementValue();

                UpdateTimeRange();
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
