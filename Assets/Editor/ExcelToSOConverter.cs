using System.IO;
using UnityEditor;
using UnityEngine;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SnowmanCount.Data.Models;

namespace SnowmanCount.Editor
{
    public static class ExcelToSOConverter
    {
        private static readonly string ExcelPath = "Assets/StreamingAssets/Levels/Levels.xlsx";
        private static readonly string ExportDir = "Assets/Resources/Data/Levels";

        [MenuItem("Tools/Convert Excel to ScriptableObjects")]
        public static void Convert()
        {
            if (!File.Exists(ExcelPath))
            {
                Debug.LogError($"[Converter] Excel file not found at: {ExcelPath}");
                return;
            }

            if (!Directory.Exists(ExportDir))
            {
                Directory.CreateDirectory(ExportDir);
            }

            try
            {
                using (FileStream fs = new FileStream(ExcelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    IWorkbook workbook = new XSSFWorkbook(fs);
                    
                    for (int i = 0; i < workbook.NumberOfSheets; i++)
                    {
                        ISheet sheet = workbook.GetSheetAt(i);
                        string sheetName = sheet.SheetName;

                        // "Level"로 시작하는 시트만 처리
                        if (!sheetName.ToLower().StartsWith("level")) continue;

                        int levelNum = ExtractLevelNumber(sheetName);
                        ProcessSheet(sheet, levelNum);
                    }
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log("[Converter] Conversion completed successfully!");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[Converter] Error during conversion: {e.Message}");
            }
        }

        private static void ProcessSheet(ISheet sheet, int levelNum)
        {
            string assetPath = $"{ExportDir}/LevelData_{levelNum}.asset";
            LevelDataSO levelSO = AssetDatabase.LoadAssetAtPath<LevelDataSO>(assetPath);

            if (levelSO == null)
            {
                levelSO = ScriptableObject.CreateInstance<LevelDataSO>();
                AssetDatabase.CreateAsset(levelSO, assetPath);
            }

            levelSO.levelNumber = levelNum;
            levelSO.rows.Clear();

            // 첫 줄은 헤더이므로 index 1부터 시작
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null || row.GetCell(0) == null) continue;

                LevelRowData rowData = new LevelRowData
                {
                    distance = GetCellFloatValue(row.GetCell(0)),
                    objectType = GetCellStringValue(row.GetCell(1)),
                    value = GetCellStringValue(row.GetCell(2)),
                    subValue = GetCellStringValue(row.GetCell(3))
                };

                levelSO.rows.Add(rowData);
            }

            EditorUtility.SetDirty(levelSO);
            Debug.Log($"[Converter] Exported Level {levelNum} to {assetPath}");
        }

        private static int ExtractLevelNumber(string name)
        {
            string numPart = System.Text.RegularExpressions.Regex.Replace(name, @"[^\d]", "");
            return int.TryParse(numPart, out int result) ? result : 0;
        }

        private static float GetCellFloatValue(ICell cell)
        {
            if (cell == null) return 0f;
            if (cell.CellType == CellType.Numeric) return (float)cell.NumericCellValue;
            if (cell.CellType == CellType.String)
                return float.TryParse(cell.StringCellValue, out float res) ? res : 0f;
            return 0f;
        }

        private static string GetCellStringValue(ICell cell)
        {
            if (cell == null) return string.Empty;
            if (cell.CellType == CellType.String) return cell.StringCellValue;
            if (cell.CellType == CellType.Numeric) return cell.NumericCellValue.ToString();
            return string.Empty;
        }
    }
}
