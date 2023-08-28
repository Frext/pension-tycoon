using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.ScriptableObjects.FloatRange
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(FloatRangeSo))]
    public class FloatRangeSo : ScriptableObject
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

        public float Randomize()
        {
            return timeRange.Randomize();
        }

        public void SetRange(FloatRange newTimeRange)
        {
            timeRange = newTimeRange;
        }
    }
}
