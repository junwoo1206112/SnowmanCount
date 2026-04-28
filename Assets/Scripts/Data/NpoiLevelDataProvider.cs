using System.IO;
using UnityEngine;

using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

using SnowmanCount.Data.Models;

namespace SnowmanCount.Data
{
    public class NpoiLevelDataProvider : ILevelDataProvider
    {
        private readonly string filePath;

        public NpoiLevelDataProvider(string filePath = null)
        {
            this.filePath = filePath ?? Path.Combine(Application.streamingAssetsPath, "Levels", "Levels.xlsx");
        }

        public LevelData LoadLevel(int levelNumber)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"[NpoiLevelDataProvider] File not found: {filePath}");
                return null;
            }

            string sheetName = $"Level{levelNumber}";

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(fs);
                ISheet sheet = workbook.GetSheet(sheetName);

                if (sheet == null)
                {
                    Debug.Log($"[NpoiLevelDataProvider] Sheet '{sheetName}' not found in {filePath}");
                    return null;
                }

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
