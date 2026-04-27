using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UnityEditor;
using UnityEngine;

public static class ExcelLevelCreator
{
    [MenuItem("Tools/Create Sample Level .xlsx")]
    public static void CreateSampleLevel()
    {
        string dir = Path.Combine(Application.streamingAssetsPath, "Levels");
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string filePath = Path.Combine(dir, "Level_01.xlsx");

        using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Level1");

            IRow headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("Distance");
            headerRow.CreateCell(1).SetCellValue("ObjectType");
            headerRow.CreateCell(2).SetCellValue("Value");
            headerRow.CreateCell(3).SetCellValue("SubValue");

            object[][] data = new object[][]
            {
                new object[] { 5.0, "Gate", "+5", "x2" },
                new object[] { 40.0, "Gate", "+10", "-3" },
                new object[] { 75.0, "Gate", "x3", "+20" },
                new object[] { 110.0, "Obstacle", "Saw", "1" },
                new object[] { 130.0, "Gate", "+15", "÷2" },
                new object[] { 170.0, "Enemy", "Flame", "5" },
                new object[] { 200.0, "Gate", "-10", "x2" },
                new object[] { 240.0, "Obstacle", "Saw", "1" },
                new object[] { 270.0, "Gate", "+30", "÷3" },
                new object[] { 310.0, "Enemy", "Flame", "8" },
                new object[] { 350.0, "Gate", "x2", "+50" },
                new object[] { 390.0, "Obstacle", "Saw", "1" },
                new object[] { 430.0, "Gate", "+25", "-5" },
                new object[] { 470.0, "Enemy", "Flame", "10" },
                new object[] { 510.0, "Gate", "x4", "+100" },
            };

            for (int i = 0; i < data.Length; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue((double)data[i][0]);
                row.CreateCell(1).SetCellValue((string)data[i][1]);
                row.CreateCell(2).SetCellValue((string)data[i][2]);
                row.CreateCell(3).SetCellValue((string)data[i][3]);
            }

            workbook.Write(fs);
        }

        Debug.Log($"[ExcelLevelCreator] Created: {filePath}");
        AssetDatabase.Refresh();
    }
}
