using System.Collections.Generic;
using System.IO;
using UnityEngine;

using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

using SnowmanCount.Data.Models;

namespace SnowmanCount.Data
{
    public class NpoiLevelDataProvider : ILevelDataProvider, ILevelDataRepository
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

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
                    try { return cell.StringCellValue; }
                    catch { return string.Empty; }
                case CellType.Numeric:
                    return cell.NumericCellValue.ToString();
                default:
                    return string.Empty;
            }
        }

        public bool SaveLevel(LevelData levelData)
        {
            return WriteWorkbook(workbook =>
            {
                string sheetName = $"Level{levelData.levelNumber}";
                ISheet sheet = workbook.GetSheet(sheetName);
                if (sheet == null)
                {
                    sheet = workbook.CreateSheet(sheetName);
                }

                IRow header = sheet.GetRow(0);
                if (header == null)
                {
                    header = sheet.CreateRow(0);
                    header.CreateCell(0).SetCellValue("Distance");
                    header.CreateCell(1).SetCellValue("ObjectType");
                    header.CreateCell(2).SetCellValue("Value");
                    header.CreateCell(3).SetCellValue("SubValue");
                }

                int rowIndex = 1;
                foreach (LevelRow row in levelData.rows)
                {
                    IRow excelRow = sheet.GetRow(rowIndex);
                    if (excelRow == null) excelRow = sheet.CreateRow(rowIndex);
                    excelRow.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(row.distance);
                    excelRow.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(row.objectType);
                    excelRow.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(row.value);
                    excelRow.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(row.subValue);
                    rowIndex++;
                }

                return true;
            });
        }

        public bool AddRow(int levelNumber, LevelRow row)
        {
            return WriteWorkbook(workbook =>
            {
                string sheetName = $"Level{levelNumber}";
                ISheet sheet = workbook.GetSheet(sheetName);
                if (sheet == null)
                {
                    sheet = workbook.CreateSheet(sheetName);
                    IRow header = sheet.CreateRow(0);
                    header.CreateCell(0).SetCellValue("Distance");
                    header.CreateCell(1).SetCellValue("ObjectType");
                    header.CreateCell(2).SetCellValue("Value");
                    header.CreateCell(3).SetCellValue("SubValue");
                }

                int newRow = sheet.LastRowNum + 1;
                IRow excelRow = sheet.CreateRow(newRow);
                excelRow.CreateCell(0).SetCellValue(row.distance);
                excelRow.CreateCell(1).SetCellValue(row.objectType);
                excelRow.CreateCell(2).SetCellValue(row.value);
                excelRow.CreateCell(3).SetCellValue(row.subValue);

                return true;
            });
        }

        public bool UpdateRow(int levelNumber, int rowIndex, LevelRow row)
        {
            return WriteWorkbook(workbook =>
            {
                string sheetName = $"Level{levelNumber}";
                ISheet sheet = workbook.GetSheet(sheetName);
                if (sheet == null) return false;

                IRow excelRow = sheet.GetRow(rowIndex);
                if (excelRow == null) excelRow = sheet.CreateRow(rowIndex);

                excelRow.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(row.distance);
                excelRow.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(row.objectType);
                excelRow.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(row.value);
                excelRow.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(row.subValue);

                return true;
            });
        }

        public bool DeleteRow(int levelNumber, int rowIndex)
        {
            return WriteWorkbook(workbook =>
            {
                string sheetName = $"Level{levelNumber}";
                ISheet sheet = workbook.GetSheet(sheetName);
                if (sheet == null) return false;

                IRow excelRow = sheet.GetRow(rowIndex);
                if (excelRow == null) return false;

                sheet.RemoveRow(excelRow);

                return true;
            });
        }

        private delegate bool WorkbookAction(IWorkbook workbook);

        private bool WriteWorkbook(WorkbookAction action)
        {
            try
            {
                IWorkbook workbook;

                if (File.Exists(filePath))
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        workbook = new XSSFWorkbook(fs);
                    }
                }
                else
                {
                    workbook = new XSSFWorkbook();
                }

                bool result = action(workbook);

                if (result)
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        workbook.Write(fs);
                    }
                }

                workbook.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[NpoiLevelDataProvider] Write failed: {ex.Message}");
                return false;
            }
        }
    }
}
