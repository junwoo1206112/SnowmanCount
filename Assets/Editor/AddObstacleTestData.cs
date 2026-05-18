using UnityEditor;
using UnityEngine;

using SnowmanCount.Data;
using SnowmanCount.Data.Models;

public static class AddObstacleTestData
{
    [MenuItem("Tools/Add Obstacle Test Data")]
    private static void AddData()
    {
        NpoiLevelDataProvider provider = new NpoiLevelDataProvider();

        // Level1: 기존 데이터 사이 빈 공간에 배치 (중복 방지)
        AddObstacle(provider, 1, 18f, "saw", "1");
        AddObstacle(provider, 1, 55f, "wall", "left,1");
        AddObstacle(provider, 1, 90f, "spinner", "1");
        AddObstacle(provider, 1, 150f, "wall", "right,2");
        AddObstacle(provider, 1, 185f, "saw", "1");
        AddObstacle(provider, 1, 260f, "spinner", "2");
        AddObstacle(provider, 1, 330f, "wall", "center,1");
        AddObstacle(provider, 1, 370f, "saw", "2");
        AddObstacle(provider, 1, 450f, "spinner", "1");
        AddObstacle(provider, 1, 490f, "wall", "left,2");

        AssetDatabase.Refresh();
        Debug.Log("[AddObstacleTestData] Done! Added 10 test obstacles to Levels.xlsx");
    }

    private static void AddObstacle(NpoiLevelDataProvider provider, int level, float distance, string value, string subValue)
    {
        LevelRow row = new LevelRow
        {
            distance = distance,
            objectType = "obstacle",
            value = value,
            subValue = subValue
        };
        provider.AddRow(level, row);
        Debug.Log($"[AddObstacleTestData] Added {value} at distance {distance} (Level {level})");
    }
}
