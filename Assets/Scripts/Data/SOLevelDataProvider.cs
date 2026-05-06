using UnityEngine;
using SnowmanCount.Data.Models;

namespace SnowmanCount.Data
{
    public class SOLevelDataProvider : ILevelDataProvider
    {
        public LevelData LoadLevel(int levelNumber)
        {
            string assetPath = $"Data/Levels/LevelData_{levelNumber}";
            LevelDataSO so = Resources.Load<LevelDataSO>(assetPath);

            if (so == null)
            {
                Debug.LogWarning($"[SOLevelDataProvider] LevelData asset not found for Level {levelNumber} at Resources/{assetPath}");
                return null;
            }

            // 기존 LevelData 클래스 형식으로 변환하여 반환 (하위 호환성)
            LevelData data = new LevelData
            {
                levelNumber = so.levelNumber
            };

            foreach (var rowSO in so.rows)
            {
                data.rows.Add(new LevelRow
                {
                    distance = rowSO.distance,
                    objectType = rowSO.objectType,
                    value = rowSO.value,
                    subValue = rowSO.subValue
                });
            }

            Debug.Log($"[SOLevelDataProvider] Level {levelNumber} loaded from ScriptableObject.");
            return data;
        }
    }
}
