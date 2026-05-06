using SnowmanCount.Data.Models;

namespace SnowmanCount.Data
{
    public interface ILevelDataRepository : ILevelDataProvider
    {
        bool SaveLevel(LevelData levelData);

        bool AddRow(int levelNumber, LevelRow row);

        bool UpdateRow(int levelNumber, int rowIndex, LevelRow row);

        bool DeleteRow(int levelNumber, int rowIndex);
    }
}
