using System;
using System.Collections.Generic;
using UnityEngine;

namespace SnowmanCount.Data.Models
{
    [Serializable]
    public class LevelRowData
    {
        public float distance;
        public string objectType;
        public string value;
        public string subValue;
    }

    [CreateAssetMenu(fileName = "LevelData", menuName = "SnowmanCount/LevelData")]
    public class LevelDataSO : ScriptableObject
    {
        public int levelNumber;
        public List<LevelRowData> rows = new List<LevelRowData>();
    }
}
