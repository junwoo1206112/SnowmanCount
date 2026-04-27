using SnowmanCount.Data.Models;

namespace SnowmanCount.Data
{
    public interface ILevelDataProvider
    {
        LevelData LoadLevel(int levelNumber);
    }
}
