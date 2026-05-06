## 1. Repository Contract

- [x] 1.1 Add `ILevelDataRepository`
- [x] 1.2 Keep `ILevelDataProvider` read-only compatibility

## 2. NPOI Read/Write Implementation

- [x] 2.1 Implement `SaveLevel`
- [x] 2.2 Implement `AddRow`
- [x] 2.3 Implement `UpdateRow`
- [x] 2.4 Implement `DeleteRow`
- [x] 2.5 Create missing workbook/sheet/header rows when saving

## 3. DI Registration

- [x] 3.1 Expose `LevelDataRepository` from `GameManager`
- [x] 3.2 Register NPOI repository as the editable data source
- [x] 3.3 Keep `LevelDataProvider` callers working

## 4. Verification

- [x] 4.1 Static search confirms repository methods exist
- [x] 4.2 Static search confirms `GameManager` registers repository
- [x] 4.3 Unity log check shows no C# compile errors
