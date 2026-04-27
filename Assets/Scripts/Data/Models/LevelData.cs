using System;
using System.Collections.Generic;

namespace SnowmanCount.Data.Models
{
    [Serializable]
    public class LevelRow
    {
        public float distance;
        public string objectType;
        public string value;
        public string subValue;
    }

    [Serializable]
    public class LevelData
    {
        public int levelNumber;
        public List<LevelRow> rows = new List<LevelRow>();
    }
}
