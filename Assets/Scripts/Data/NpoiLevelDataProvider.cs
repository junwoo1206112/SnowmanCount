using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SnowmanCount.Data.Models;
using UnityEngine;

namespace SnowmanCount.Data
{
    public class NpoiLevelDataProvider : ILevelDataProvider
    {
        private readonly string basePath;

        public NpoiLevelDataProvider(string basePath = null)
        {
            this.basePath = basePath ?? Path.Combine(Application.streamingAssetsPath, "Levels");
        }

        public LevelData LoadLevel(int levelNumber)
        {
            string filePath = Path.Combine(basePath, $"Level_{levelNumber:D2}.xlsx");

            if (!File.Exists(filePath))
            {
                Debug.LogError($"[NpoiLevelDataProvider] File not found: {filePath}");
                return null;
            }

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(fs);
                ISheet sheet = workbook.GetSheetAt(0);

                LevelData data = new LevelData
                {
                    levelNumber = levelNumber
                };

                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;

                    LevelRow levelRow = new LevelRow
                    {
                        distance = GetCellFloatValue(row.GetCell(0)),
                        objectType = GetCellStringValue(row.GetCell(1)),
                        value = GetCellStringValue(row.GetCell(2)),
                        subValue = GetCellStringValue(row.GetCell(3))
                    };

                    data.rows.Add(levelRow);
                }

                return data;
            }
        }

        private float GetCellFloatValue(ICell cell)
        {
            if (cell == null) return 0f;

            switch (cell.CellType)
            {
                case CellType.Numeric:
                    return (float)cell.NumericCellValue;
                case CellType.String:
                    return float.TryParse(cell.StringCellValue, out float result) ? result : 0f;
                default:
                    return 0f;
            }
        }

        private int GetCellIntValue(ICell cell)
        {
            if (cell == null) return 0;

            switch (cell.CellType)
            {
                case CellType.Numeric:
                    return (int)cell.NumericCellValue;
                case CellType.String:
                    return int.TryParse(cell.StringCellValue, out int result) ? result : 0;
                default:
                    return 0;
            }
        }

        private string GetCellStringValue(ICell cell)
        {
            if (cell == null) return string.Empty;

            switch (cell.CellType)
            {
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Numeric:
                    return cell.NumericCellValue.ToString();
                default:
                    return string.Empty;
            }
        }
    }
}
