using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UnityEditor;
using UnityEngine;

public static class ExcelLevelCreator
{
    [MenuItem("Tools/Create Levels.xlsx")]
    public static void CreateLevels()
    {
        string dir = Path.Combine(Application.streamingAssetsPath, "Levels");
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string filePath = Path.Combine(dir, "Levels.xlsx");

        IWorkbook workbook;

        if (File.Exists(filePath))
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                workbook = new XSSFWorkbook(fs);
            }
        }
        else
        {
            workbook = new XSSFWorkbook();
        }

        CreateOrReplaceSheet(workbook, "Level1", GetLevel1Data());
        CreateOrReplaceSheet(workbook, "Level2", GetLevel2Data());

        using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            workbook.Write(fs);
        }

        Debug.Log($"[ExcelLevelCreator] Created/Updated: {filePath}");
        AssetDatabase.Refresh();
    }

    private static void CreateOrReplaceSheet(IWorkbook workbook, string sheetName, object[][] data)
    {
        int existingIndex = workbook.GetSheetIndex(sheetName);
        if (existingIndex >= 0)
        {
            workbook.RemoveSheetAt(existingIndex);
        }

        ISheet sheet = workbook.CreateSheet(sheetName);

        IRow headerRow = sheet.CreateRow(0);
        headerRow.CreateCell(0).SetCellValue("Distance");
        headerRow.CreateCell(1).SetCellValue("ObjectType");
        headerRow.CreateCell(2).SetCellValue("Value");
        headerRow.CreateCell(3).SetCellValue("SubValue");

        for (int i = 0; i < data.Length; i++)
        {
            IRow row = sheet.CreateRow(i + 1);
            row.CreateCell(0).SetCellValue((double)data[i][0]);
            row.CreateCell(1).SetCellValue((string)data[i][1]);
            row.CreateCell(2).SetCellValue((string)data[i][2]);
            row.CreateCell(3).SetCellValue((string)data[i][3]);
        }
    }

    private static object[][] GetLevel1Data()
    {
        return new object[][]
        {
            new object[] { 5.0, "Gate", "+5", "x2" },
            new object[] { 40.0, "Gate", "+10", "-3" },
            new object[] { 75.0, "Gate", "x3", "+20" },
            new object[] { 110.0, "Obstacle", "Saw", "1" },
            new object[] { 115.0, "Hole", "", "" },
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
    }

    private static object[][] GetLevel2Data()
    {
        return new object[][]
        {
            new object[] { 5.0, "Gate", "+10", "x2" },
            new object[] { 50.0, "Gate", "-5", "-5" },
            new object[] { 90.0, "Obstacle", "Saw", "1" },
            new object[] { 95.0, "Hole", "", "" },
            new object[] { 120.0, "Enemy", "Flame", "5" },
            new object[] { 150.0, "Gate", "x3", "+30" },
            new object[] { 190.0, "Obstacle", "Saw", "1" },
            new object[] { 220.0, "Enemy", "Flame", "8" },
            new object[] { 260.0, "Gate", "+50", "÷2" },
            new object[] { 300.0, "Obstacle", "Saw", "1" },
            new object[] { 340.0, "Enemy", "Flame", "10" },
            new object[] { 380.0, "Gate", "x5", "+100" },
            new object[] { 420.0, "Obstacle", "Saw", "1" },
            new object[] { 460.0, "Enemy", "Flame", "12" },
            new object[] { 500.0, "Gate", "x10", "+200" },
        };
    }
}
