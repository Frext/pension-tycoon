using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.ScriptableObjects.TimeRangeObject
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(TimeRangeObject))]
    public class TimeRangeObject : ScriptableObject
    {
        [Serializable]
        public struct FloatRange
        {
            public float min;
            public float max;

            public float Randomize()
            {
                return Random.Range(min, max);
            }
        }

        [SerializeField] private FloatRange timeRange;

        public float GetRandomTime()
        {
            return timeRange.Randomize();
        }

        public void SetTimeRange(FloatRange newTimeRange)
        {
            timeRange = newTimeRange;
        }
    }
}
